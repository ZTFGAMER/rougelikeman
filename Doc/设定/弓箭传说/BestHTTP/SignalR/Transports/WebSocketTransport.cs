namespace BestHTTP.SignalR.Transports
{
    using BestHTTP;
    using BestHTTP.SignalR;
    using BestHTTP.SignalR.Messages;
    using BestHTTP.WebSocket;
    using System;

    public sealed class WebSocketTransport : TransportBase
    {
        private BestHTTP.WebSocket.WebSocket wSocket;

        public WebSocketTransport(Connection connection) : base("webSockets", connection)
        {
        }

        protected override void Aborted()
        {
            if ((this.wSocket != null) && this.wSocket.IsOpen)
            {
                this.wSocket.Close();
                this.wSocket = null;
            }
        }

        public override void Connect()
        {
            if (this.wSocket != null)
            {
                HTTPManager.Logger.Warning("WebSocketTransport", "Start - WebSocket already created!");
            }
            else
            {
                if (base.State != TransportStates.Reconnecting)
                {
                    base.State = TransportStates.Connecting;
                }
                RequestTypes type = (base.State != TransportStates.Reconnecting) ? RequestTypes.Connect : RequestTypes.Reconnect;
                Uri uri = base.Connection.BuildUri(type, this);
                this.wSocket = new BestHTTP.WebSocket.WebSocket(uri);
                this.wSocket.OnOpen = (OnWebSocketOpenDelegate) Delegate.Combine(this.wSocket.OnOpen, new OnWebSocketOpenDelegate(this.WSocket_OnOpen));
                this.wSocket.OnMessage = (OnWebSocketMessageDelegate) Delegate.Combine(this.wSocket.OnMessage, new OnWebSocketMessageDelegate(this.WSocket_OnMessage));
                this.wSocket.OnClosed = (OnWebSocketClosedDelegate) Delegate.Combine(this.wSocket.OnClosed, new OnWebSocketClosedDelegate(this.WSocket_OnClosed));
                this.wSocket.OnErrorDesc = (OnWebSocketErrorDescriptionDelegate) Delegate.Combine(this.wSocket.OnErrorDesc, new OnWebSocketErrorDescriptionDelegate(this.WSocket_OnError));
                base.Connection.PrepareRequest(this.wSocket.InternalRequest, type);
                this.wSocket.Open();
            }
        }

        protected override void SendImpl(string json)
        {
            if ((this.wSocket != null) && this.wSocket.IsOpen)
            {
                this.wSocket.Send(json);
            }
        }

        protected override void Started()
        {
        }

        public override void Stop()
        {
            if (this.wSocket != null)
            {
                this.wSocket.OnOpen = null;
                this.wSocket.OnMessage = null;
                this.wSocket.OnClosed = null;
                this.wSocket.OnErrorDesc = null;
                this.wSocket.Close();
                this.wSocket = null;
            }
        }

        private void WSocket_OnClosed(BestHTTP.WebSocket.WebSocket webSocket, ushort code, string message)
        {
            if (webSocket == this.wSocket)
            {
                string reason = code.ToString() + " : " + message;
                HTTPManager.Logger.Information("WebSocketTransport", "WSocket_OnClosed " + reason);
                if (base.State == TransportStates.Closing)
                {
                    base.State = TransportStates.Closed;
                }
                else
                {
                    base.Connection.Error(reason);
                }
            }
        }

        private void WSocket_OnError(BestHTTP.WebSocket.WebSocket webSocket, string reason)
        {
            if (webSocket == this.wSocket)
            {
                if ((base.State == TransportStates.Closing) || (base.State == TransportStates.Closed))
                {
                    base.AbortFinished();
                }
                else
                {
                    HTTPManager.Logger.Error("WebSocketTransport", "WSocket_OnError " + reason);
                    base.Connection.Error(reason);
                }
            }
        }

        private void WSocket_OnMessage(BestHTTP.WebSocket.WebSocket webSocket, string message)
        {
            if (webSocket == this.wSocket)
            {
                IServerMessage msg = TransportBase.Parse(base.Connection.JsonEncoder, message);
                if (msg != null)
                {
                    base.Connection.OnMessage(msg);
                }
            }
        }

        private void WSocket_OnOpen(BestHTTP.WebSocket.WebSocket webSocket)
        {
            if (webSocket == this.wSocket)
            {
                HTTPManager.Logger.Information("WebSocketTransport", "WSocket_OnOpen");
                base.OnConnected();
            }
        }

        public override bool SupportsKeepAlive =>
            true;

        public override TransportTypes Type =>
            TransportTypes.WebSocket;
    }
}

