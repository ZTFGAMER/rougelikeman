namespace BestHTTP.ServerSentEvents
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public sealed class Message
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Id>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Event>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Data>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan <Retry>k__BackingField;

        public override string ToString() => 
            $""{this.Event}": "{this.Data}"";

        public string Id { get; internal set; }

        public string Event { get; internal set; }

        public string Data { get; internal set; }

        public TimeSpan Retry { get; internal set; }
    }
}

