namespace BestHTTP.SignalR.Messages
{
    using BestHTTP.SignalR;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public sealed class FailureMessage : IServerMessage, IHubMessage
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ulong <InvocationId>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool <IsHubError>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <ErrorMessage>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IDictionary<string, object> <AdditionalData>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <StackTrace>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IDictionary<string, object> <State>k__BackingField;

        void IServerMessage.Parse(object data)
        {
            IDictionary<string, object> dictionary = data as IDictionary<string, object>;
            this.InvocationId = ulong.Parse(dictionary["I"].ToString());
            if (dictionary.TryGetValue("E", out object obj2))
            {
                this.ErrorMessage = obj2.ToString();
            }
            if (dictionary.TryGetValue("H", out obj2))
            {
                this.IsHubError = int.Parse(obj2.ToString()) == 1;
            }
            if (dictionary.TryGetValue("D", out obj2))
            {
                this.AdditionalData = obj2 as IDictionary<string, object>;
            }
            if (dictionary.TryGetValue("T", out obj2))
            {
                this.StackTrace = obj2.ToString();
            }
            if (dictionary.TryGetValue("S", out obj2))
            {
                this.State = obj2 as IDictionary<string, object>;
            }
        }

        MessageTypes IServerMessage.Type =>
            MessageTypes.Failure;

        public ulong InvocationId { get; private set; }

        public bool IsHubError { get; private set; }

        public string ErrorMessage { get; private set; }

        public IDictionary<string, object> AdditionalData { get; private set; }

        public string StackTrace { get; private set; }

        public IDictionary<string, object> State { get; private set; }
    }
}

