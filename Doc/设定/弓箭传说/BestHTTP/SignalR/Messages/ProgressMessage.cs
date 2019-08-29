namespace BestHTTP.SignalR.Messages
{
    using BestHTTP.SignalR;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public sealed class ProgressMessage : IServerMessage, IHubMessage
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ulong <InvocationId>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double <Progress>k__BackingField;

        void IServerMessage.Parse(object data)
        {
            IDictionary<string, object> dictionary = data as IDictionary<string, object>;
            IDictionary<string, object> dictionary2 = dictionary["P"] as IDictionary<string, object>;
            this.InvocationId = ulong.Parse(dictionary2["I"].ToString());
            this.Progress = double.Parse(dictionary2["D"].ToString());
        }

        MessageTypes IServerMessage.Type =>
            MessageTypes.Progress;

        public ulong InvocationId { get; private set; }

        public double Progress { get; private set; }
    }
}

