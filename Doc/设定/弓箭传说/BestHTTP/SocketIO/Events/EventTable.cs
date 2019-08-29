namespace BestHTTP.SocketIO.Events
{
    using BestHTTP;
    using BestHTTP.Logger;
    using BestHTTP.SocketIO;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    internal sealed class EventTable
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private BestHTTP.SocketIO.Socket <Socket>k__BackingField;
        private Dictionary<string, List<EventDescriptor>> Table = new Dictionary<string, List<EventDescriptor>>();

        public EventTable(BestHTTP.SocketIO.Socket socket)
        {
            this.Socket = socket;
        }

        public void Call(Packet packet)
        {
            string eventName = packet.DecodeEventName();
            string str2 = (packet.SocketIOEvent == SocketIOEventTypes.Unknown) ? EventNames.GetNameFor(packet.TransportEvent) : EventNames.GetNameFor(packet.SocketIOEvent);
            object[] args = null;
            if (this.HasSubsciber(eventName) || this.HasSubsciber(str2))
            {
                if (((packet.TransportEvent == TransportEventTypes.Message) && ((packet.SocketIOEvent == SocketIOEventTypes.Event) || (packet.SocketIOEvent == SocketIOEventTypes.BinaryEvent))) && this.ShouldDecodePayload(eventName))
                {
                    args = packet.Decode(this.Socket.Manager.Encoder);
                }
                if (!string.IsNullOrEmpty(eventName))
                {
                    this.Call(eventName, packet, args);
                }
                if (!packet.IsDecoded && this.ShouldDecodePayload(str2))
                {
                    args = packet.Decode(this.Socket.Manager.Encoder);
                }
                if (!string.IsNullOrEmpty(str2))
                {
                    this.Call(str2, packet, args);
                }
            }
        }

        public void Call(string eventName, Packet packet, params object[] args)
        {
            if (HTTPManager.Logger.Level <= Loglevels.All)
            {
                HTTPManager.Logger.Verbose("EventTable", "Call - " + eventName);
            }
            if (this.Table.TryGetValue(eventName, out List<EventDescriptor> list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Call(this.Socket, packet, args);
                }
            }
        }

        public void Clear()
        {
            this.Table.Clear();
        }

        private bool HasSubsciber(string eventName) => 
            this.Table.ContainsKey(eventName);

        public void Register(string eventName, SocketIOCallback callback, bool onlyOnce, bool autoDecodePayload)
        {
            <Register>c__AnonStorey0 storey = new <Register>c__AnonStorey0 {
                onlyOnce = onlyOnce,
                autoDecodePayload = autoDecodePayload
            };
            if (!this.Table.TryGetValue(eventName, out List<EventDescriptor> list))
            {
                this.Table.Add(eventName, list = new List<EventDescriptor>(1));
            }
            EventDescriptor descriptor = list.Find(new Predicate<EventDescriptor>(storey.<>m__0));
            if (descriptor == null)
            {
                list.Add(new EventDescriptor(storey.onlyOnce, storey.autoDecodePayload, callback));
            }
            else
            {
                descriptor.Callbacks.Add(callback);
            }
        }

        private bool ShouldDecodePayload(string eventName)
        {
            if (this.Table.TryGetValue(eventName, out List<EventDescriptor> list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].AutoDecodePayload && (list[i].Callbacks.Count > 0))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void Unregister(string eventName)
        {
            this.Table.Remove(eventName);
        }

        public void Unregister(string eventName, SocketIOCallback callback)
        {
            if (this.Table.TryGetValue(eventName, out List<EventDescriptor> list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Callbacks.Remove(callback);
                }
            }
        }

        private BestHTTP.SocketIO.Socket Socket { get; set; }

        [CompilerGenerated]
        private sealed class <Register>c__AnonStorey0
        {
            internal bool onlyOnce;
            internal bool autoDecodePayload;

            internal bool <>m__0(EventDescriptor d) => 
                ((d.OnlyOnce == this.onlyOnce) && (d.AutoDecodePayload == this.autoDecodePayload));
        }
    }
}

