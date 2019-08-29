namespace BestHTTP
{
    using BestHTTP.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    internal sealed class KeepAliveHeader
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan <TimeOut>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int <MaxRequests>k__BackingField;

        public void Parse(List<string> headerValues)
        {
            HeaderParser parser = new HeaderParser(headerValues[0]);
            if (parser.TryGet("timeout", out HeaderValue value2) && value2.HasValue)
            {
                int result = 0;
                if (int.TryParse(value2.Value, out result))
                {
                    this.TimeOut = TimeSpan.FromSeconds((double) result);
                }
                else
                {
                    this.TimeOut = TimeSpan.MaxValue;
                }
            }
            if (parser.TryGet("max", out value2) && value2.HasValue)
            {
                int result = 0;
                if (int.TryParse("max", out result))
                {
                    this.MaxRequests = result;
                }
                else
                {
                    this.MaxRequests = 0x7fffffff;
                }
            }
        }

        public TimeSpan TimeOut { get; private set; }

        public int MaxRequests { get; private set; }
    }
}

