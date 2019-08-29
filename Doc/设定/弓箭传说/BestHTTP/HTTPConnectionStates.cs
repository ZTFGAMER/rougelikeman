namespace BestHTTP
{
    using System;

    internal enum HTTPConnectionStates
    {
        Initial,
        Processing,
        Redirected,
        Upgraded,
        WaitForProtocolShutdown,
        WaitForRecycle,
        Free,
        AbortRequested,
        TimedOut,
        Closed
    }
}

