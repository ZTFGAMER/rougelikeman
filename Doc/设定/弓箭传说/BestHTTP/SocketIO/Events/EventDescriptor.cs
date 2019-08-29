namespace BestHTTP.SocketIO.Events
{
    using BestHTTP;
    using BestHTTP.SocketIO;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    internal sealed class EventDescriptor
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private List<SocketIOCallback> <Callbacks>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <OnlyOnce>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <AutoDecodePayload>k__BackingField;
        private SocketIOCallback[] CallbackArray;

        public EventDescriptor(bool onlyOnce, bool autoDecodePayload, SocketIOCallback callback)
        {
            this.OnlyOnce = onlyOnce;
            this.AutoDecodePayload = autoDecodePayload;
            this.Callbacks = new List<SocketIOCallback>(1);
            if (callback != null)
            {
                this.Callbacks.Add(callback);
            }
        }

        public void Call(Socket socket, Packet packet, params object[] args)
        {
            if ((this.CallbackArray == null) || (this.CallbackArray.Length < this.Callbacks.Count))
            {
                Array.Resize<SocketIOCallback>(ref this.CallbackArray, this.Callbacks.Count);
            }
            this.Callbacks.CopyTo(this.CallbackArray);
            for (int i = 0; i < this.CallbackArray.Length; i++)
            {
                try
                {
                    SocketIOCallback callback = this.CallbackArray[i];
                    if (callback != null)
                    {
                        callback(socket, packet, args);
                    }
                }
                catch (Exception exception)
                {
                    ((ISocket) socket).EmitError(SocketIOErrors.User, exception.Message + " " + exception.StackTrace);
                    HTTPManager.Logger.Exception("EventDescriptor", "Call", exception);
                }
                if (this.OnlyOnce)
                {
                    this.Callbacks.Remove(this.CallbackArray[i]);
                }
                this.CallbackArray[i] = null;
            }
        }

        public List<SocketIOCallback> Callbacks { get; private set; }

        public bool OnlyOnce { get; private set; }

        public bool AutoDecodePayload { get; private set; }
    }
}

