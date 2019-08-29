namespace BestHTTP.SignalR.Messages
{
    using BestHTTP.SignalR;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public sealed class DataMessage : IServerMessage
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object <Data>k__BackingField;

        void IServerMessage.Parse(object data)
        {
            this.Data = data;
        }

        MessageTypes IServerMessage.Type =>
            MessageTypes.Data;

        public object Data { get; private set; }
    }
}

