namespace BestHTTP.SignalR.Transports
{
    using BestHTTP;
    using BestHTTP.ServerSentEvents;
    using BestHTTP.SignalR;
    using BestHTTP.SignalR.Messages;
    using System;
    using System.Runtime.CompilerServices;

    public sealed class ServerSentEventsTransport : PostSendTransportBase
    {
        private BestHTTP.ServerSentEvents.EventSource EventSource;
        [CompilerGenerated]
        private static OnRetryDelegate <>f__am$cache0;

        public ServerSentEventsTransport(Connection con) : base("serverSentEvents", con)
        {
        }

        public override void Abort()
        {
            base.Abort();
            this.EventSource.Close();
        }

        protected override void Aborted()
        {
            if (base.State == TransportStates.Closing)
            {
                base.State = TransportStates.Closed;
            }
        }

        public override void Connect()
        {
            if (this.EventSource != null)
            {
                HTTPManager.Logger.Warning("ServerSentEventsTransport", "Start - EventSource already created!");
            }
            else
            {
                if (base.State != TransportStates.Reconnecting)
                {
                    base.State = TransportStates.Connecting;
                }
                RequestTypes type = (base.State != TransportStates.Reconnecting) ? RequestTypes.Connect : RequestTypes.Reconnect;
                Uri uri = base.Connection.BuildUri(type, this);
                this.EventSource = new BestHTTP.ServerSentEvents.EventSource(uri);
                this.EventSource.OnOpen += new OnGeneralEventDelegate(this.OnEventSourceOpen);
                this.EventSource.OnMessage += new OnMessageDelegate(this.OnEventSourceMessage);
                this.EventSource.OnError += new OnErrorDelegate(this.OnEventSourceError);
                this.EventSource.OnClosed += new OnGeneralEventDelegate(this.OnEventSourceClosed);
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = es => false;
                }
                this.EventSource.OnRetry += <>f__am$cache0;
                this.EventSource.Open();
            }
        }

        private void OnEventSourceClosed(BestHTTP.ServerSentEvents.EventSource eventSource)
        {
            HTTPManager.Logger.Information("Transport - " + base.Name, "OnEventSourceClosed");
            this.OnEventSourceError(eventSource, "EventSource Closed!");
        }

        private void OnEventSourceError(BestHTTP.ServerSentEvents.EventSource eventSource, string error)
        {
            HTTPManager.Logger.Information("Transport - " + base.Name, "OnEventSourceError");
            if (base.State == TransportStates.Reconnecting)
            {
                this.Connect();
            }
            else if (base.State != TransportStates.Closed)
            {
                if (base.State == TransportStates.Closing)
                {
                    base.State = TransportStates.Closed;
                }
                else
                {
                    base.Connection.Error(error);
                }
            }
        }

        private void OnEventSourceMessage(BestHTTP.ServerSentEvents.EventSource eventSource, Message message)
        {
            if (message.Data.Equals("initialized"))
            {
                base.OnConnected();
            }
            else
            {
                IServerMessage msg = TransportBase.Parse(base.Connection.JsonEncoder, message.Data);
                if (msg != null)
                {
                    base.Connection.OnMessage(msg);
                }
            }
        }

        private void OnEventSourceOpen(BestHTTP.ServerSentEvents.EventSource eventSource)
        {
            HTTPManager.Logger.Information("Transport - " + base.Name, "OnEventSourceOpen");
        }

        protected override void Started()
        {
        }

        public override void Stop()
        {
            this.EventSource.OnOpen -= new OnGeneralEventDelegate(this.OnEventSourceOpen);
            this.EventSource.OnMessage -= new OnMessageDelegate(this.OnEventSourceMessage);
            this.EventSource.OnError -= new OnErrorDelegate(this.OnEventSourceError);
            this.EventSource.OnClosed -= new OnGeneralEventDelegate(this.OnEventSourceClosed);
            this.EventSource.Close();
            this.EventSource = null;
        }

        public override bool SupportsKeepAlive =>
            true;

        public override TransportTypes Type =>
            TransportTypes.ServerSentEvents;
    }
}

