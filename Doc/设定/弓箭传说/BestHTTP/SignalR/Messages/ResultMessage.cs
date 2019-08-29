namespace BestHTTP.SignalR.Messages
{
    using BestHTTP.SignalR;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public sealed class ResultMessage : IServerMessage, IHubMessage
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ulong <InvocationId>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private object <ReturnValue>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IDictionary<string, object> <State>k__BackingField;

        void IServerMessage.Parse(object data)
        {
            IDictionary<string, object> dictionary = data as IDictionary<string, object>;
            this.InvocationId = ulong.Parse(dictionary["I"].ToString());
            if (dictionary.TryGetValue("R", out object obj2))
            {
                this.ReturnValue = obj2;
            }
            if (dictionary.TryGetValue("S", out obj2))
            {
                this.State = obj2 as IDictionary<string, object>;
            }
        }

        MessageTypes IServerMessage.Type =>
            MessageTypes.Result;

        public ulong InvocationId { get; private set; }

        public object ReturnValue { get; private set; }

        public IDictionary<string, object> State { get; private set; }
    }
}

