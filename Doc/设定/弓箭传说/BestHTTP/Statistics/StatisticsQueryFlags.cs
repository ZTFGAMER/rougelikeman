namespace BestHTTP.Statistics
{
    using System;

    [Flags]
    public enum StatisticsQueryFlags : byte
    {
        Connections = 1,
        Cache = 2,
        Cookies = 4,
        All = 0xff
    }
}

