namespace BestHTTP.Caching
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public sealed class HTTPCacheMaintananceParams
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan <DeleteOlder>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ulong <MaxCacheSize>k__BackingField;

        public HTTPCacheMaintananceParams(TimeSpan deleteOlder, ulong maxCacheSize)
        {
            this.DeleteOlder = deleteOlder;
            this.MaxCacheSize = maxCacheSize;
        }

        public TimeSpan DeleteOlder { get; private set; }

        public ulong MaxCacheSize { get; private set; }
    }
}

