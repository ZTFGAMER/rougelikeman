namespace BestHTTP.SocketIO
{
    using BestHTTP;
    using BestHTTP.JSON;
    using BestHTTP.SocketIO.Events;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public sealed class Socket : ISocket
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SocketManager <Manager>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Namespace>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Id>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsOpen>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <AutoDecodePayload>k__BackingField;
        private Dictionary<int, SocketIOAckCallback> AckCallbacks;
        private EventTable EventCallbacks;
        private List<object> arguments = new List<object>();

        internal Socket(string nsp, SocketManager manager)
        {
            this.Namespace = nsp;
            this.Manager = manager;
            this.IsOpen = false;
            this.AutoDecodePayload = true;
            this.EventCallbacks = new EventTable(this);
        }

        void ISocket.Disconnect(bool remove)
        {
            if (this.IsOpen)
            {
                Packet packet = new Packet(TransportEventTypes.Message, SocketIOEventTypes.Disconnect, this.Namespace, string.Empty, 0, 0);
                ((IManager) this.Manager).SendPacket(packet);
                this.IsOpen = false;
                ((ISocket) this).OnPacket(packet);
            }
            if (this.AckCallbacks != null)
            {
                this.AckCallbacks.Clear();
            }
            if (remove)
            {
                this.EventCallbacks.Clear();
                ((IManager) this.Manager).Remove(this);
            }
        }

        void ISocket.EmitError(SocketIOErrors errCode, string msg)
        {
            object[] args = new object[] { new Error(errCode, msg) };
            ((ISocket) this).EmitEvent(SocketIOEventTypes.Error, args);
        }

        void ISocket.EmitEvent(SocketIOEventTypes type, params object[] args)
        {
            ((ISocket) this).EmitEvent(EventNames.GetNameFor(type), args);
        }

        void ISocket.EmitEvent(string eventName, params object[] args)
        {
            if (!string.IsNullOrEmpty(eventName))
            {
                this.EventCallbacks.Call(eventName, null, args);
            }
        }

        void ISocket.OnPacket(Packet packet)
        {
            switch (packet.SocketIOEvent)
            {
                case SocketIOEventTypes.Connect:
                    this.Id = (this.Namespace == "/") ? this.Manager.Handshake.Sid : (this.Namespace + "#" + this.Manager.Handshake.Sid);
                    break;

                case SocketIOEventTypes.Disconnect:
                    if (this.IsOpen)
                    {
                        this.IsOpen = false;
                        this.EventCallbacks.Call(EventNames.GetNameFor(SocketIOEventTypes.Disconnect), packet, Array.Empty<object>());
                        this.Disconnect();
                    }
                    break;

                case SocketIOEventTypes.Error:
                {
                    bool success = false;
                    object obj2 = Json.Decode(packet.Payload, ref success);
                    if (success)
                    {
                        Error error;
                        Dictionary<string, object> dictionary = obj2 as Dictionary<string, object>;
                        if ((dictionary != null) && dictionary.ContainsKey("code"))
                        {
                            error = new Error((SocketIOErrors) Convert.ToInt32(dictionary["code"]), dictionary["message"] as string);
                        }
                        else
                        {
                            error = new Error(SocketIOErrors.Custom, packet.Payload);
                        }
                        object[] args = new object[] { error };
                        this.EventCallbacks.Call(EventNames.GetNameFor(SocketIOEventTypes.Error), packet, args);
                        return;
                    }
                    break;
                }
            }
            this.EventCallbacks.Call(packet);
            if (((packet.SocketIOEvent == SocketIOEventTypes.Ack) || (packet.SocketIOEvent == SocketIOEventTypes.BinaryAck)) && (this.AckCallbacks != null))
            {
                SocketIOAckCallback callback = null;
                if (this.AckCallbacks.TryGetValue(packet.Id, out callback) && (callback != null))
                {
                    try
                    {
                        callback(this, packet, !this.AutoDecodePayload ? null : packet.Decode(this.Manager.Encoder));
                    }
                    catch (Exception exception)
                    {
                        HTTPManager.Logger.Exception("Socket", "ackCallback", exception);
                    }
                }
                this.AckCallbacks.Remove(packet.Id);
            }
        }

        void ISocket.Open()
        {
            if (this.Manager.State == SocketManager.States.Open)
            {
                this.OnTransportOpen(this.Manager.Socket, null, Array.Empty<object>());
            }
            else
            {
                this.Manager.Socket.Off("connect", new SocketIOCallback(this.OnTransportOpen));
                this.Manager.Socket.On("connect", new SocketIOCallback(this.OnTransportOpen));
                if (this.Manager.Options.AutoConnect && (this.Manager.State == SocketManager.States.Initial))
                {
                    this.Manager.Open();
                }
            }
        }

        public void Disconnect()
        {
            ((ISocket) this).Disconnect(true);
        }

        public Socket Emit(string eventName, params object[] args) => 
            this.Emit(eventName, null, args);

        public Socket Emit(string eventName, SocketIOAckCallback callback, params object[] args)
        {
            if (EventNames.IsBlacklisted(eventName))
            {
                throw new ArgumentException("Blacklisted event: " + eventName);
            }
            this.arguments.Clear();
            this.arguments.Add(eventName);
            List<byte[]> list = null;
            if ((args != null) && (args.Length > 0))
            {
                int num = 0;
                for (int i = 0; i < args.Length; i++)
                {
                    byte[] item = args[i] as byte[];
                    if (item != null)
                    {
                        if (list == null)
                        {
                            list = new List<byte[]>();
                        }
                        Dictionary<string, object> dictionary = new Dictionary<string, object>(2) {
                            { 
                                "_placeholder",
                                true
                            },
                            { 
                                "num",
                                num++
                            }
                        };
                        this.arguments.Add(dictionary);
                        list.Add(item);
                    }
                    else
                    {
                        this.arguments.Add(args[i]);
                    }
                }
            }
            string payload = null;
            try
            {
                payload = this.Manager.Encoder.Encode(this.arguments);
            }
            catch (Exception exception)
            {
                ((ISocket) this).EmitError(SocketIOErrors.Internal, "Error while encoding payload: " + exception.Message + " " + exception.StackTrace);
                return this;
            }
            this.arguments.Clear();
            if (payload == null)
            {
                throw new ArgumentException("Encoding the arguments to JSON failed!");
            }
            int id = 0;
            if (callback != null)
            {
                id = this.Manager.NextAckId;
                if (this.AckCallbacks == null)
                {
                    this.AckCallbacks = new Dictionary<int, SocketIOAckCallback>();
                }
                this.AckCallbacks[id] = callback;
            }
            Packet packet = new Packet(TransportEventTypes.Message, (list != null) ? SocketIOEventTypes.BinaryEvent : SocketIOEventTypes.Event, this.Namespace, payload, 0, id);
            if (list != null)
            {
                packet.Attachments = list;
            }
            ((IManager) this.Manager).SendPacket(packet);
            return this;
        }

        public Socket EmitAck(Packet originalPacket, params object[] args)
        {
            if (originalPacket == null)
            {
                throw new ArgumentNullException("originalPacket == null!");
            }
            if ((originalPacket.SocketIOEvent != SocketIOEventTypes.Event) && (originalPacket.SocketIOEvent != SocketIOEventTypes.BinaryEvent))
            {
                throw new ArgumentException("Wrong packet - you can't send an Ack for a packet with id == 0 and SocketIOEvent != Event or SocketIOEvent != BinaryEvent!");
            }
            this.arguments.Clear();
            if ((args != null) && (args.Length > 0))
            {
                this.arguments.AddRange(args);
            }
            string payload = null;
            try
            {
                payload = this.Manager.Encoder.Encode(this.arguments);
            }
            catch (Exception exception)
            {
                ((ISocket) this).EmitError(SocketIOErrors.Internal, "Error while encoding payload: " + exception.Message + " " + exception.StackTrace);
                return this;
            }
            if (payload == null)
            {
                throw new ArgumentException("Encoding the arguments to JSON failed!");
            }
            Packet packet = new Packet(TransportEventTypes.Message, (originalPacket.SocketIOEvent != SocketIOEventTypes.Event) ? SocketIOEventTypes.BinaryAck : SocketIOEventTypes.Ack, this.Namespace, payload, 0, originalPacket.Id);
            ((IManager) this.Manager).SendPacket(packet);
            return this;
        }

        public void Off()
        {
            this.EventCallbacks.Clear();
        }

        public void Off(SocketIOEventTypes type)
        {
            this.Off(EventNames.GetNameFor(type));
        }

        public void Off(string eventName)
        {
            this.EventCallbacks.Unregister(eventName);
        }

        public void Off(SocketIOEventTypes type, SocketIOCallback callback)
        {
            this.EventCallbacks.Unregister(EventNames.GetNameFor(type), callback);
        }

        public void Off(string eventName, SocketIOCallback callback)
        {
            this.EventCallbacks.Unregister(eventName, callback);
        }

        public void On(SocketIOEventTypes type, SocketIOCallback callback)
        {
            string nameFor = EventNames.GetNameFor(type);
            this.EventCallbacks.Register(nameFor, callback, false, this.AutoDecodePayload);
        }

        public void On(string eventName, SocketIOCallback callback)
        {
            this.EventCallbacks.Register(eventName, callback, false, this.AutoDecodePayload);
        }

        public void On(SocketIOEventTypes type, SocketIOCallback callback, bool autoDecodePayload)
        {
            string nameFor = EventNames.GetNameFor(type);
            this.EventCallbacks.Register(nameFor, callback, false, autoDecodePayload);
        }

        public void On(string eventName, SocketIOCallback callback, bool autoDecodePayload)
        {
            this.EventCallbacks.Register(eventName, callback, false, autoDecodePayload);
        }

        public void Once(SocketIOEventTypes type, SocketIOCallback callback)
        {
            this.EventCallbacks.Register(EventNames.GetNameFor(type), callback, true, this.AutoDecodePayload);
        }

        public void Once(string eventName, SocketIOCallback callback)
        {
            this.EventCallbacks.Register(eventName, callback, true, this.AutoDecodePayload);
        }

        public void Once(SocketIOEventTypes type, SocketIOCallback callback, bool autoDecodePayload)
        {
            this.EventCallbacks.Register(EventNames.GetNameFor(type), callback, true, autoDecodePayload);
        }

        public void Once(string eventName, SocketIOCallback callback, bool autoDecodePayload)
        {
            this.EventCallbacks.Register(eventName, callback, true, autoDecodePayload);
        }

        private void OnTransportOpen(Socket socket, Packet packet, params object[] args)
        {
            if (this.Namespace != "/")
            {
                ((IManager) this.Manager).SendPacket(new Packet(TransportEventTypes.Message, SocketIOEventTypes.Connect, this.Namespace, string.Empty, 0, 0));
            }
            this.IsOpen = true;
        }

        public SocketManager Manager { get; private set; }

        public string Namespace { get; private set; }

        public string Id { get; private set; }

        public bool IsOpen { get; private set; }

        public bool AutoDecodePayload { get; set; }
    }
}

