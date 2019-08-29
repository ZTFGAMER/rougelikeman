namespace BestHTTP.SocketIO
{
    using BestHTTP;
    using BestHTTP.Extensions;
    using BestHTTP.SocketIO.Events;
    using BestHTTP.SocketIO.JsonEncoders;
    using BestHTTP.SocketIO.Transports;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public sealed class SocketManager : IHeartbeat, IManager
    {
        public static IJsonEncoder DefaultEncoder = new DefaultJSonEncoder();
        public const int MinProtocolVersion = 4;
        private States state;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SocketOptions <Options>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private System.Uri <Uri>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private HandshakeData <Handshake>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ITransport <Transport>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ulong <RequestCounter>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <ReconnectAttempts>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IJsonEncoder <Encoder>k__BackingField;
        private int nextAckId;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private States <PreviousState>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ITransport <UpgradingTransport>k__BackingField;
        private Dictionary<string, BestHTTP.SocketIO.Socket> Namespaces;
        private List<BestHTTP.SocketIO.Socket> Sockets;
        private List<Packet> OfflinePackets;
        private DateTime LastHeartbeat;
        private DateTime LastPongReceived;
        private DateTime ReconnectAt;
        private DateTime ConnectionStarted;
        private bool closing;

        public SocketManager(System.Uri uri) : this(uri, new SocketOptions())
        {
        }

        public SocketManager(System.Uri uri, SocketOptions options)
        {
            this.Namespaces = new Dictionary<string, BestHTTP.SocketIO.Socket>();
            this.Sockets = new List<BestHTTP.SocketIO.Socket>();
            this.LastHeartbeat = DateTime.MinValue;
            this.LastPongReceived = DateTime.MinValue;
            this.Uri = uri;
            this.Options = options;
            this.State = States.Initial;
            this.PreviousState = States.Initial;
            this.Encoder = DefaultEncoder;
        }

        void IHeartbeat.OnHeartbeatUpdate(TimeSpan dif)
        {
            switch (this.State)
            {
                case States.Opening:
                    if ((DateTime.UtcNow - this.ConnectionStarted) >= this.Options.Timeout)
                    {
                        ((IManager) this).EmitError(SocketIOErrors.Internal, "Connection timed out!");
                        ((IManager) this).EmitEvent("connect_error", Array.Empty<object>());
                        ((IManager) this).EmitEvent("connect_timeout", Array.Empty<object>());
                        ((IManager) this).TryToReconnect();
                    }
                    return;

                case States.Open:
                    break;

                case States.Paused:
                    if (this.Transport.IsRequestInProgress || this.Transport.IsPollingInProgress)
                    {
                        return;
                    }
                    this.State = States.Open;
                    this.Transport.Close();
                    this.Transport = this.UpgradingTransport;
                    this.UpgradingTransport = null;
                    this.Transport.Send(new Packet(TransportEventTypes.Upgrade, SocketIOEventTypes.Unknown, "/", string.Empty, 0, 0));
                    break;

                case States.Reconnecting:
                    if ((this.ReconnectAt != DateTime.MinValue) && (DateTime.UtcNow >= this.ReconnectAt))
                    {
                        ((IManager) this).EmitEvent("reconnect_attempt", Array.Empty<object>());
                        ((IManager) this).EmitEvent("reconnecting", Array.Empty<object>());
                        this.Open();
                    }
                    return;

                default:
                    return;
            }
            ITransport transport = null;
            if ((this.Transport != null) && (this.Transport.State == TransportStates.Open))
            {
                transport = this.Transport;
            }
            if ((transport != null) && (transport.State == TransportStates.Open))
            {
                transport.Poll();
                this.SendOfflinePackets();
                if (this.LastHeartbeat == DateTime.MinValue)
                {
                    this.LastHeartbeat = DateTime.UtcNow;
                }
                else
                {
                    if ((DateTime.UtcNow - this.LastHeartbeat) > this.Handshake.PingInterval)
                    {
                        ((IManager) this).SendPacket(new Packet(TransportEventTypes.Ping, SocketIOEventTypes.Unknown, "/", string.Empty, 0, 0));
                        this.LastHeartbeat = DateTime.UtcNow;
                    }
                    if ((DateTime.UtcNow - this.LastPongReceived) > this.Handshake.PingTimeout)
                    {
                        ((IManager) this).TryToReconnect();
                    }
                }
            }
        }

        void IManager.Close(bool removeSockets)
        {
            if ((this.State != States.Closed) && !this.closing)
            {
                this.closing = true;
                HTTPManager.Logger.Information("SocketManager", "Closing");
                HTTPManager.Heartbeats.Unsubscribe(this);
                if (removeSockets)
                {
                    while (this.Sockets.Count > 0)
                    {
                        this.Sockets[this.Sockets.Count - 1].Disconnect(removeSockets);
                    }
                }
                else
                {
                    for (int i = 0; i < this.Sockets.Count; i++)
                    {
                        this.Sockets[i].Disconnect(removeSockets);
                    }
                }
                this.State = States.Closed;
                this.LastHeartbeat = DateTime.MinValue;
                if (this.OfflinePackets != null)
                {
                    this.OfflinePackets.Clear();
                }
                if (removeSockets)
                {
                    this.Namespaces.Clear();
                }
                this.Handshake = null;
                if (this.Transport != null)
                {
                    this.Transport.Close();
                }
                this.Transport = null;
                this.closing = false;
            }
        }

        void IManager.EmitAll(string eventName, params object[] args)
        {
            for (int i = 0; i < this.Sockets.Count; i++)
            {
                this.Sockets[i].EmitEvent(eventName, args);
            }
        }

        void IManager.EmitError(SocketIOErrors errCode, string msg)
        {
            object[] args = new object[] { new Error(errCode, msg) };
            ((IManager) this).EmitEvent(SocketIOEventTypes.Error, args);
        }

        void IManager.EmitEvent(SocketIOEventTypes type, params object[] args)
        {
            ((IManager) this).EmitEvent(EventNames.GetNameFor(type), args);
        }

        void IManager.EmitEvent(string eventName, params object[] args)
        {
            BestHTTP.SocketIO.Socket socket = null;
            if (this.Namespaces.TryGetValue("/", out socket))
            {
                ((ISocket) socket).EmitEvent(eventName, args);
            }
        }

        void IManager.OnPacket(Packet packet)
        {
            BestHTTP.SocketIO.Socket socket;
            if (this.State != States.Closed)
            {
                TransportEventTypes transportEvent = packet.TransportEvent;
                if (transportEvent != TransportEventTypes.Open)
                {
                    if (transportEvent == TransportEventTypes.Ping)
                    {
                        ((IManager) this).SendPacket(new Packet(TransportEventTypes.Pong, SocketIOEventTypes.Unknown, "/", string.Empty, 0, 0));
                    }
                    else if (transportEvent == TransportEventTypes.Pong)
                    {
                        this.LastPongReceived = DateTime.UtcNow;
                    }
                    goto Label_00B9;
                }
                if (this.Handshake != null)
                {
                    goto Label_00B9;
                }
                this.Handshake = new HandshakeData();
                if (!this.Handshake.Parse(packet.Payload))
                {
                    HTTPManager.Logger.Warning("SocketManager", "Expected handshake data, but wasn't able to pars. Payload: " + packet.Payload);
                }
                ((IManager) this).OnTransportConnected(this.Transport);
            }
            return;
        Label_00B9:
            socket = null;
            if (this.Namespaces.TryGetValue(packet.Namespace, out socket))
            {
                ((ISocket) socket).OnPacket(packet);
            }
            else
            {
                HTTPManager.Logger.Warning("SocketManager", "Namespace \"" + packet.Namespace + "\" not found!");
            }
        }

        bool IManager.OnTransportConnected(ITransport trans)
        {
            if (this.State != States.Opening)
            {
                return false;
            }
            if (this.PreviousState == States.Reconnecting)
            {
                ((IManager) this).EmitEvent("reconnect", Array.Empty<object>());
            }
            this.State = States.Open;
            this.LastPongReceived = DateTime.UtcNow;
            this.ReconnectAttempts = 0;
            this.SendOfflinePackets();
            HTTPManager.Logger.Information("SocketManager", "Open");
            if ((this.Transport.Type != TransportTypes.WebSocket) && this.Handshake.Upgrades.Contains("websocket"))
            {
                this.UpgradingTransport = new WebSocketTransport(this);
                this.UpgradingTransport.Open();
            }
            return true;
        }

        void IManager.OnTransportError(ITransport trans, string err)
        {
            ((IManager) this).EmitError(SocketIOErrors.Internal, err);
            trans.Close();
            ((IManager) this).TryToReconnect();
        }

        void IManager.OnTransportProbed(ITransport trans)
        {
            HTTPManager.Logger.Information("SocketManager", "\"probe\" packet received");
            this.Options.ConnectWith = trans.Type;
            this.State = States.Paused;
        }

        void IManager.Remove(BestHTTP.SocketIO.Socket socket)
        {
            this.Namespaces.Remove(socket.Namespace);
            this.Sockets.Remove(socket);
            if (this.Sockets.Count == 0)
            {
                this.Close();
            }
        }

        void IManager.SendPacket(Packet packet)
        {
            ITransport transport = this.SelectTransport();
            if (transport != null)
            {
                try
                {
                    transport.Send(packet);
                }
                catch (Exception exception)
                {
                    ((IManager) this).EmitError(SocketIOErrors.Internal, exception.Message + " " + exception.StackTrace);
                }
            }
            else
            {
                if (this.OfflinePackets == null)
                {
                    this.OfflinePackets = new List<Packet>();
                }
                this.OfflinePackets.Add(packet.Clone());
            }
        }

        void IManager.TryToReconnect()
        {
            if ((this.State != States.Reconnecting) && (this.State != States.Closed))
            {
                if (!this.Options.Reconnection)
                {
                    this.Close();
                }
                else if (++this.ReconnectAttempts >= this.Options.ReconnectionAttempts)
                {
                    ((IManager) this).EmitEvent("reconnect_failed", Array.Empty<object>());
                    this.Close();
                }
                else
                {
                    Random random = new Random();
                    int num2 = ((int) this.Options.ReconnectionDelay.TotalMilliseconds) * this.ReconnectAttempts;
                    this.ReconnectAt = DateTime.UtcNow + TimeSpan.FromMilliseconds((double) Math.Min(random.Next(num2 - ((int) (num2 * this.Options.RandomizationFactor)), num2 + ((int) (num2 * this.Options.RandomizationFactor))), (int) this.Options.ReconnectionDelayMax.TotalMilliseconds));
                    ((IManager) this).Close(false);
                    this.State = States.Reconnecting;
                    for (int i = 0; i < this.Sockets.Count; i++)
                    {
                        this.Sockets[i].Open();
                    }
                    HTTPManager.Heartbeats.Subscribe(this);
                    HTTPManager.Logger.Information("SocketManager", "Reconnecting");
                }
            }
        }

        public void Close()
        {
            ((IManager) this).Close(true);
        }

        public void EmitAll(string eventName, params object[] args)
        {
            for (int i = 0; i < this.Sockets.Count; i++)
            {
                this.Sockets[i].Emit(eventName, args);
            }
        }

        public BestHTTP.SocketIO.Socket GetSocket() => 
            this.GetSocket("/");

        public BestHTTP.SocketIO.Socket GetSocket(string nsp)
        {
            if (string.IsNullOrEmpty(nsp))
            {
                throw new ArgumentNullException("Namespace parameter is null or empty!");
            }
            BestHTTP.SocketIO.Socket socket = null;
            if (!this.Namespaces.TryGetValue(nsp, out socket))
            {
                socket = new BestHTTP.SocketIO.Socket(nsp, this);
                this.Namespaces.Add(nsp, socket);
                this.Sockets.Add(socket);
                ((ISocket) socket).Open();
            }
            return socket;
        }

        public void Open()
        {
            if (((this.State == States.Initial) || (this.State == States.Closed)) || (this.State == States.Reconnecting))
            {
                HTTPManager.Logger.Information("SocketManager", "Opening");
                this.ReconnectAt = DateTime.MinValue;
                switch (this.Options.ConnectWith)
                {
                    case TransportTypes.Polling:
                        this.Transport = new PollingTransport(this);
                        break;

                    case TransportTypes.WebSocket:
                        this.Transport = new WebSocketTransport(this);
                        break;
                }
                this.Transport.Open();
                ((IManager) this).EmitEvent("connecting", Array.Empty<object>());
                this.State = States.Opening;
                this.ConnectionStarted = DateTime.UtcNow;
                HTTPManager.Heartbeats.Subscribe(this);
                this.GetSocket("/");
            }
        }

        private ITransport SelectTransport()
        {
            if (this.State != States.Open)
            {
                return null;
            }
            return (!this.Transport.IsRequestInProgress ? this.Transport : null);
        }

        private void SendOfflinePackets()
        {
            ITransport transport = this.SelectTransport();
            if (((this.OfflinePackets != null) && (this.OfflinePackets.Count > 0)) && (transport != null))
            {
                transport.Send(this.OfflinePackets);
                this.OfflinePackets.Clear();
            }
        }

        public States State
        {
            get => 
                this.state;
            private set
            {
                this.PreviousState = this.state;
                this.state = value;
            }
        }

        public SocketOptions Options { get; private set; }

        public System.Uri Uri { get; private set; }

        public HandshakeData Handshake { get; private set; }

        public ITransport Transport { get; private set; }

        public ulong RequestCounter { get; internal set; }

        public BestHTTP.SocketIO.Socket Socket =>
            this.GetSocket();

        public BestHTTP.SocketIO.Socket this[string nsp] =>
            this.GetSocket(nsp);

        public int ReconnectAttempts { get; private set; }

        public IJsonEncoder Encoder { get; set; }

        internal uint Timestamp =>
            ((uint) DateTime.UtcNow.Subtract(new DateTime(0x7b2, 1, 1)).TotalMilliseconds);

        internal int NextAckId =>
            Interlocked.Increment(ref this.nextAckId);

        internal States PreviousState { get; private set; }

        internal ITransport UpgradingTransport { get; set; }

        public enum States
        {
            Initial,
            Closed,
            Opening,
            Open,
            Paused,
            Reconnecting
        }
    }
}

