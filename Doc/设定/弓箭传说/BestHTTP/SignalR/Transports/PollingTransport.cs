namespace BestHTTP.SignalR.Transports
{
    using BestHTTP;
    using BestHTTP.Extensions;
    using BestHTTP.SignalR;
    using BestHTTP.SignalR.Messages;
    using System;

    public sealed class PollingTransport : PostSendTransportBase, IHeartbeat
    {
        private DateTime LastPoll;
        private TimeSpan PollDelay;
        private TimeSpan PollTimeout;
        private HTTPRequest pollRequest;

        public PollingTransport(Connection connection) : base("longPolling", connection)
        {
            this.LastPoll = DateTime.MinValue;
            this.PollTimeout = connection.NegotiationResult.ConnectionTimeout + TimeSpan.FromSeconds(10.0);
        }

        protected override void Aborted()
        {
            HTTPManager.Heartbeats.Unsubscribe(this);
        }

        void IHeartbeat.OnHeartbeatUpdate(TimeSpan dif)
        {
            if ((base.State == TransportStates.Started) && ((this.pollRequest == null) && (DateTime.UtcNow >= ((this.LastPoll + this.PollDelay) + base.Connection.NegotiationResult.LongPollDelay))))
            {
                this.Poll();
            }
        }

        public override void Connect()
        {
            HTTPManager.Logger.Information("Transport - " + base.Name, "Sending Open Request");
            if (base.State != TransportStates.Reconnecting)
            {
                base.State = TransportStates.Connecting;
            }
            RequestTypes type = (base.State != TransportStates.Reconnecting) ? RequestTypes.Connect : RequestTypes.Reconnect;
            HTTPRequest req = new HTTPRequest(base.Connection.BuildUri(type, this), HTTPMethods.Get, true, true, new OnRequestFinishedDelegate(this.OnConnectRequestFinished));
            base.Connection.PrepareRequest(req, type);
            req.Send();
        }

        private void OnConnectRequestFinished(HTTPRequest req, HTTPResponse resp)
        {
            string str = string.Empty;
            switch (req.State)
            {
                case HTTPRequestStates.Finished:
                {
                    if (!resp.IsSuccess)
                    {
                        str = $"Connect - Request Finished Successfully, but the server sent an error. Status Code: {resp.StatusCode}-{resp.Message} Message: {resp.DataAsText}";
                        break;
                    }
                    HTTPManager.Logger.Information("Transport - " + base.Name, "Connect - Request Finished Successfully! " + resp.DataAsText);
                    base.OnConnected();
                    IServerMessage msg = TransportBase.Parse(base.Connection.JsonEncoder, resp.DataAsText);
                    if (msg != null)
                    {
                        base.Connection.OnMessage(msg);
                        MultiMessage message2 = msg as MultiMessage;
                        if ((message2 != null) && message2.PollDelay.HasValue)
                        {
                            this.PollDelay = message2.PollDelay.Value;
                        }
                    }
                    break;
                }
                case HTTPRequestStates.Error:
                    str = "Connect - Request Finished with Error! " + ((req.Exception == null) ? "No Exception" : (req.Exception.Message + "\n" + req.Exception.StackTrace));
                    break;

                case HTTPRequestStates.Aborted:
                    str = "Connect - Request Aborted!";
                    break;

                case HTTPRequestStates.ConnectionTimedOut:
                    str = "Connect - Connection Timed Out!";
                    break;

                case HTTPRequestStates.TimedOut:
                    str = "Connect - Processing the request Timed Out!";
                    break;
            }
            if (!string.IsNullOrEmpty(str))
            {
                base.Connection.Error(str);
            }
        }

        private void OnPollRequestFinished(HTTPRequest req, HTTPResponse resp)
        {
            if (req.State == HTTPRequestStates.Aborted)
            {
                HTTPManager.Logger.Warning("Transport - " + base.Name, "Poll - Request Aborted!");
            }
            else
            {
                this.pollRequest = null;
                string str = string.Empty;
                switch (req.State)
                {
                    case HTTPRequestStates.Finished:
                    {
                        if (!resp.IsSuccess)
                        {
                            str = $"Poll - Request Finished Successfully, but the server sent an error. Status Code: {resp.StatusCode}-{resp.Message} Message: {resp.DataAsText}";
                            break;
                        }
                        HTTPManager.Logger.Information("Transport - " + base.Name, "Poll - Request Finished Successfully! " + resp.DataAsText);
                        IServerMessage msg = TransportBase.Parse(base.Connection.JsonEncoder, resp.DataAsText);
                        if (msg != null)
                        {
                            base.Connection.OnMessage(msg);
                            MultiMessage message2 = msg as MultiMessage;
                            if ((message2 != null) && message2.PollDelay.HasValue)
                            {
                                this.PollDelay = message2.PollDelay.Value;
                            }
                            this.LastPoll = DateTime.UtcNow;
                        }
                        break;
                    }
                    case HTTPRequestStates.Error:
                        str = "Poll - Request Finished with Error! " + ((req.Exception == null) ? "No Exception" : (req.Exception.Message + "\n" + req.Exception.StackTrace));
                        break;

                    case HTTPRequestStates.ConnectionTimedOut:
                        str = "Poll - Connection Timed Out!";
                        break;

                    case HTTPRequestStates.TimedOut:
                        str = "Poll - Processing the request Timed Out!";
                        break;
                }
                if (!string.IsNullOrEmpty(str))
                {
                    base.Connection.Error(str);
                }
            }
        }

        private void Poll()
        {
            this.pollRequest = new HTTPRequest(base.Connection.BuildUri(RequestTypes.Poll, this), HTTPMethods.Get, true, true, new OnRequestFinishedDelegate(this.OnPollRequestFinished));
            base.Connection.PrepareRequest(this.pollRequest, RequestTypes.Poll);
            this.pollRequest.Timeout = this.PollTimeout;
            this.pollRequest.Send();
        }

        protected override void Started()
        {
            this.LastPoll = DateTime.UtcNow;
            HTTPManager.Heartbeats.Subscribe(this);
        }

        public override void Stop()
        {
            HTTPManager.Heartbeats.Unsubscribe(this);
            if (this.pollRequest != null)
            {
                this.pollRequest.Abort();
                this.pollRequest = null;
            }
        }

        public override bool SupportsKeepAlive =>
            false;

        public override TransportTypes Type =>
            TransportTypes.LongPoll;
    }
}

