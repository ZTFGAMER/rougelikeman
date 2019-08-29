namespace BestHTTP
{
    using System;

    public enum HTTPRequestStates
    {
        Initial,
        Queued,
        Processing,
        Finished,
        Error,
        Aborted,
        ConnectionTimedOut,
        TimedOut
    }
}

