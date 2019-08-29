namespace BestHTTP.SocketIO.Transports
{
    using BestHTTP;
    using BestHTTP.Extensions;
    using BestHTTP.Logger;
    using BestHTTP.SocketIO;
    using BestHTTP.WebSocket;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    internal sealed class WebSocketTransport : ITransport
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TransportStates <State>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SocketManager <Manager>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private BestHTTP.WebSocket.WebSocket <Implementation>k__BackingField;
        private Packet PacketWithAttachment;
        private byte[] Buffer;

        public WebSocketTransport(SocketManager manager)
        {
            this.State = TransportStates.Closed;
            this.Manager = manager;
        }

        public void Close()
        {
            if (this.State != TransportStates.Closed)
            {
                this.State = TransportStates.Closed;
                if (this.Implementation != null)
                {
                    this.Implementation.Close();
                }
                else
                {
                    HTTPManager.Logger.Warning("WebSocketTransport", "Close - WebSocket Implementation already null!");
                }
                this.Implementation = null;
            }
        }

        private void OnBinary(BestHTTP.WebSocket.WebSocket ws, byte[] data)
        {
            if (ws == this.Implementation)
            {
                if (HTTPManager.Logger.Level <= Loglevels.All)
                {
                    HTTPManager.Logger.Verbose("WebSocketTransport", "OnBinary");
                }
                if (this.PacketWithAttachment != null)
                {
                    this.PacketWithAttachment.AddAttachmentFromServer(data, false);
                    if (this.PacketWithAttachment.HasAllAttachment)
                    {
                        try
                        {
                            this.OnPacket(this.PacketWithAttachment);
                        }
                        catch (Exception exception)
                        {
                            HTTPManager.Logger.Exception("WebSocketTransport", "OnBinary", exception);
                        }
                        finally
                        {
                            this.PacketWithAttachment = null;
                        }
                    }
                }
            }
        }

        private void OnClosed(BestHTTP.WebSocket.WebSocket ws, ushort code, string message)
        {
            if (ws == this.Implementation)
            {
                HTTPManager.Logger.Information("WebSocketTransport", "OnClosed");
                this.Close();
                if (this.Manager.UpgradingTransport != this)
                {
                    ((IManager) this.Manager).TryToReconnect();
                }
                else
                {
                    this.Manager.UpgradingTransport = null;
                }
            }
        }

        private void OnError(BestHTTP.WebSocket.WebSocket ws, Exception ex)
        {
            if (ws == this.Implementation)
            {
                string err = string.Empty;
                if (ex != null)
                {
                    err = ex.Message + " " + ex.StackTrace;
                }
                else
                {
                    switch (ws.InternalRequest.State)
                    {
                        case HTTPRequestStates.Finished:
                            if (!ws.InternalRequest.Response.IsSuccess && (ws.InternalRequest.Response.StatusCode != 0x65))
                            {
                                err = $"Request Finished Successfully, but the server sent an error. Status Code: {ws.InternalRequest.Response.StatusCode}-{ws.InternalRequest.Response.Message} Message: {ws.InternalRequest.Response.DataAsText}";
                                break;
                            }
                            err = $"Request finished. Status Code: {ws.InternalRequest.Response.StatusCode.ToString()} Message: {ws.InternalRequest.Response.Message}";
                            break;

                        case HTTPRequestStates.Error:
                            err = (("Request Finished with Error! : " + ws.InternalRequest.Exception) == null) ? string.Empty : (ws.InternalRequest.Exception.Message + " " + ws.InternalRequest.Exception.StackTrace);
                            break;

                        case HTTPRequestStates.Aborted:
                            err = "Request Aborted!";
                            break;

                        case HTTPRequestStates.ConnectionTimedOut:
                            err = "Connection Timed Out!";
                            break;

                        case HTTPRequestStates.TimedOut:
                            err = "Processing the request Timed Out!";
                            break;
                    }
                }
                if (this.Manager.UpgradingTransport != this)
                {
                    ((IManager) this.Manager).OnTransportError(this, err);
                }
                else
                {
                    this.Manager.UpgradingTransport = null;
                }
            }
        }

        private void OnMessage(BestHTTP.WebSocket.WebSocket ws, string message)
        {
            if (ws == this.Implementation)
            {
                if (HTTPManager.Logger.Level <= Loglevels.All)
                {
                    HTTPManager.Logger.Verbose("WebSocketTransport", "OnMessage: " + message);
                }
                try
                {
                    Packet packet = new Packet(message);
                    if (packet.AttachmentCount == 0)
                    {
                        this.OnPacket(packet);
                    }
                    else
                    {
                        this.PacketWithAttachment = packet;
                    }
                }
                catch (Exception exception)
                {
                    HTTPManager.Logger.Exception("WebSocketTransport", "OnMessage", exception);
                }
            }
        }

        private void OnOpen(BestHTTP.WebSocket.WebSocket ws)
        {
            if (ws == this.Implementation)
            {
                HTTPManager.Logger.Information("WebSocketTransport", "OnOpen");
                this.State = TransportStates.Opening;
                if (this.Manager.UpgradingTransport == this)
                {
                    this.Send(new Packet(TransportEventTypes.Ping, SocketIOEventTypes.Unknown, "/", "probe", 0, 0));
                }
            }
        }

        private void OnPacket(Packet packet)
        {
            switch (packet.TransportEvent)
            {
                case TransportEventTypes.Open:
                    if (this.State != TransportStates.Opening)
                    {
                        HTTPManager.Logger.Warning("PollingTransport", "Received 'Open' packet while state is '" + this.State.ToString() + "'");
                    }
                    else
                    {
                        this.State = TransportStates.Open;
                    }
                    break;

                case TransportEventTypes.Pong:
                    if (packet.Payload == "probe")
                    {
                        this.State = TransportStates.Open;
                        ((IManager) this.Manager).OnTransportProbed(this);
                    }
                    break;
            }
            if (this.Manager.UpgradingTransport != this)
            {
                ((IManager) this.Manager).OnPacket(packet);
            }
        }

        public void Open()
        {
            if (this.State == TransportStates.Closed)
            {
                Uri uri = null;
                string str = new UriBuilder(!HTTPProtocolFactory.IsSecureProtocol(this.Manager.Uri) ? "ws" : "wss", this.Manager.Uri.Host, this.Manager.Uri.Port, this.Manager.Uri.GetRequestPathAndQueryURL()).Uri.ToString();
                string format = "{0}?EIO={1}&transport=websocket{3}";
                if (this.Manager.Handshake != null)
                {
                    format = format + "&sid={2}";
                }
                bool flag = !this.Manager.Options.QueryParamsOnlyForHandshake || (this.Manager.Options.QueryParamsOnlyForHandshake && (this.Manager.Handshake == null));
                uri = new Uri(string.Format(format, new object[] { str, 4, (this.Manager.Handshake == null) ? string.Empty : this.Manager.Handshake.Sid, !flag ? string.Empty : this.Manager.Options.BuildQueryParams() }));
                this.Implementation = new BestHTTP.WebSocket.WebSocket(uri);
                this.Implementation.OnOpen = new OnWebSocketOpenDelegate(this.OnOpen);
                this.Implementation.OnMessage = new OnWebSocketMessageDelegate(this.OnMessage);
                this.Implementation.OnBinary = new OnWebSocketBinaryDelegate(this.OnBinary);
                this.Implementation.OnError = new OnWebSocketErrorDelegate(this.OnError);
                this.Implementation.OnClosed = new OnWebSocketClosedDelegate(this.OnClosed);
                this.Implementation.Open();
                this.State = TransportStates.Connecting;
            }
        }

        public void Poll()
        {
        }

        public void Send(Packet packet)
        {
            if ((this.State != TransportStates.Closed) && (this.State != TransportStates.Paused))
            {
                string message = packet.Encode();
                if (HTTPManager.Logger.Level <= Loglevels.All)
                {
                    HTTPManager.Logger.Verbose("WebSocketTransport", "Send: " + message);
                }
                if ((packet.AttachmentCount != 0) || ((packet.Attachments != null) && (packet.Attachments.Count != 0)))
                {
                    if (packet.Attachments == null)
                    {
                        throw new ArgumentException("packet.Attachments are null!");
                    }
                    if (packet.AttachmentCount != packet.Attachments.Count)
                    {
                        throw new ArgumentException("packet.AttachmentCount != packet.Attachments.Count. Use the packet.AddAttachment function to add data to a packet!");
                    }
                }
                this.Implementation.Send(message);
                if (packet.AttachmentCount != 0)
                {
                    int newSize = packet.Attachments[0].Length + 1;
                    for (int i = 1; i < packet.Attachments.Count; i++)
                    {
                        if ((packet.Attachments[i].Length + 1) > newSize)
                        {
                            newSize = packet.Attachments[i].Length + 1;
                        }
                    }
                    if ((this.Buffer == null) || (this.Buffer.Length < newSize))
                    {
                        Array.Resize<byte>(ref this.Buffer, newSize);
                    }
                    for (int j = 0; j < packet.AttachmentCount; j++)
                    {
                        this.Buffer[0] = 4;
                        Array.Copy(packet.Attachments[j], 0, this.Buffer, 1, packet.Attachments[j].Length);
                        this.Implementation.Send(this.Buffer, 0L, (ulong) (packet.Attachments[j].Length + 1L));
                    }
                }
            }
        }

        public void Send(List<Packet> packets)
        {
            for (int i = 0; i < packets.Count; i++)
            {
                this.Send(packets[i]);
            }
            packets.Clear();
        }

        public TransportTypes Type =>
            TransportTypes.WebSocket;

        public TransportStates State { get; private set; }

        public SocketManager Manager { get; private set; }

        public bool IsRequestInProgress =>
            false;

        public bool IsPollingInProgress =>
            false;

        public BestHTTP.WebSocket.WebSocket Implementation { get; private set; }
    }
}

