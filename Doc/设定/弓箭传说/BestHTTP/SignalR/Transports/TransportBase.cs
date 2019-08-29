namespace BestHTTP.SignalR.Transports
{
    using BestHTTP;
    using BestHTTP.SignalR;
    using BestHTTP.SignalR.JsonEncoders;
    using BestHTTP.SignalR.Messages;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public abstract class TransportBase
    {
        private const int MaxRetryCount = 5;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Name>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IConnection <Connection>k__BackingField;
        public TransportStates _state;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event OnTransportStateChangedDelegate OnStateChanged;

        public TransportBase(string name, BestHTTP.SignalR.Connection connection)
        {
            this.Name = name;
            this.Connection = connection;
            this.State = TransportStates.Initial;
        }

        public virtual void Abort()
        {
            if (this.State == TransportStates.Started)
            {
                this.State = TransportStates.Closing;
                HTTPRequest req = new HTTPRequest(this.Connection.BuildUri(RequestTypes.Abort, this), HTTPMethods.Get, true, true, new OnRequestFinishedDelegate(this.OnAbortRequestFinished)) {
                    Tag = 0,
                    DisableRetry = true
                };
                this.Connection.PrepareRequest(req, RequestTypes.Abort);
                req.Send();
            }
        }

        protected abstract void Aborted();
        protected void AbortFinished()
        {
            this.State = TransportStates.Closed;
            this.Connection.TransportAborted();
            this.Aborted();
        }

        public abstract void Connect();
        private void OnAbortRequestFinished(HTTPRequest req, HTTPResponse resp)
        {
            if (req.State == HTTPRequestStates.Finished)
            {
                if (resp.IsSuccess)
                {
                    HTTPManager.Logger.Information("Transport - " + this.Name, "Abort - Returned: " + resp.DataAsText);
                    if (this.State == TransportStates.Closing)
                    {
                        this.AbortFinished();
                    }
                    return;
                }
                HTTPManager.Logger.Warning("Transport - " + this.Name, $"Abort - Handshake request finished Successfully, but the server sent an error. Status Code: {resp.StatusCode}-{resp.Message} Message: {resp.DataAsText} Uri: {req.CurrentUri}");
            }
            HTTPManager.Logger.Information("Transport - " + this.Name, "Abort request state: " + req.State.ToString());
            int tag = (int) req.Tag;
            if (tag++ < 5)
            {
                req.Tag = tag;
                req.Send();
            }
            else
            {
                this.Connection.Error("Failed to send Abort request!");
            }
        }

        protected void OnConnected()
        {
            if (this.State != TransportStates.Reconnecting)
            {
                this.Start();
            }
            else
            {
                this.Connection.TransportReconnected();
                this.Started();
                this.State = TransportStates.Started;
            }
        }

        private void OnStartRequestFinished(HTTPRequest req, HTTPResponse resp)
        {
            if (req.State == HTTPRequestStates.Finished)
            {
                if (resp.IsSuccess)
                {
                    HTTPManager.Logger.Information("Transport - " + this.Name, "Start - Returned: " + resp.DataAsText);
                    string str = this.Connection.ParseResponse(resp.DataAsText);
                    if (str != "started")
                    {
                        this.Connection.Error($"Expected 'started' response, but '{str}' found!");
                        return;
                    }
                    this.State = TransportStates.Started;
                    this.Started();
                    this.Connection.TransportStarted();
                    return;
                }
                HTTPManager.Logger.Warning("Transport - " + this.Name, $"Start - request finished Successfully, but the server sent an error. Status Code: {resp.StatusCode}-{resp.Message} Message: {resp.DataAsText} Uri: {req.CurrentUri}");
            }
            HTTPManager.Logger.Information("Transport - " + this.Name, "Start request state: " + req.State.ToString());
            int tag = (int) req.Tag;
            if (tag++ < 5)
            {
                req.Tag = tag;
                req.Send();
            }
            else
            {
                this.Connection.Error("Failed to send Start request.");
            }
        }

        public static IServerMessage Parse(IJsonEncoder encoder, string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                HTTPManager.Logger.Error("MessageFactory", "Parse - called with empty or null string!");
                return null;
            }
            if ((json.Length == 2) && (json == "{}"))
            {
                return new KeepAliveMessage();
            }
            IDictionary<string, object> data = null;
            try
            {
                data = encoder.DecodeMessage(json);
            }
            catch (Exception exception)
            {
                HTTPManager.Logger.Exception("MessageFactory", "Parse - encoder.DecodeMessage", exception);
                return null;
            }
            if (data == null)
            {
                HTTPManager.Logger.Error("MessageFactory", "Parse - Json Decode failed for json string: \"" + json + "\"");
                return null;
            }
            IServerMessage message2 = null;
            if (!data.ContainsKey("C"))
            {
                if (!data.ContainsKey("E"))
                {
                    message2 = new ResultMessage();
                }
                else
                {
                    message2 = new FailureMessage();
                }
            }
            else
            {
                message2 = new MultiMessage();
            }
            message2.Parse(data);
            return message2;
        }

        public void Reconnect()
        {
            HTTPManager.Logger.Information("Transport - " + this.Name, "Reconnecting");
            this.Stop();
            this.State = TransportStates.Reconnecting;
            this.Connect();
        }

        public void Send(string jsonStr)
        {
            try
            {
                HTTPManager.Logger.Information("Transport - " + this.Name, "Sending: " + jsonStr);
                this.SendImpl(jsonStr);
            }
            catch (Exception exception)
            {
                HTTPManager.Logger.Exception("Transport - " + this.Name, "Send", exception);
            }
        }

        protected abstract void SendImpl(string json);
        protected void Start()
        {
            HTTPManager.Logger.Information("Transport - " + this.Name, "Sending Start Request");
            this.State = TransportStates.Starting;
            if (this.Connection.Protocol > ProtocolVersions.Protocol_2_0)
            {
                HTTPRequest req = new HTTPRequest(this.Connection.BuildUri(RequestTypes.Start, this), HTTPMethods.Get, true, true, new OnRequestFinishedDelegate(this.OnStartRequestFinished)) {
                    Tag = 0,
                    DisableRetry = true,
                    Timeout = this.Connection.NegotiationResult.ConnectionTimeout + TimeSpan.FromSeconds(10.0)
                };
                this.Connection.PrepareRequest(req, RequestTypes.Start);
                req.Send();
            }
            else
            {
                this.State = TransportStates.Started;
                this.Started();
                this.Connection.TransportStarted();
            }
        }

        protected abstract void Started();
        public abstract void Stop();

        public string Name { get; protected set; }

        public abstract bool SupportsKeepAlive { get; }

        public abstract TransportTypes Type { get; }

        public IConnection Connection { get; protected set; }

        public TransportStates State
        {
            get => 
                this._state;
            protected set
            {
                TransportStates oldState = this._state;
                this._state = value;
                if (this.OnStateChanged != null)
                {
                    this.OnStateChanged(this, oldState, this._state);
                }
            }
        }
    }
}

