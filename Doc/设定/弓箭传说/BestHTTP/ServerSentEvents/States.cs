namespace BestHTTP.ServerSentEvents
{
    using System;

    public enum States
    {
        Initial,
        Connecting,
        Open,
        Retrying,
        Closing,
        Closed
    }
}

