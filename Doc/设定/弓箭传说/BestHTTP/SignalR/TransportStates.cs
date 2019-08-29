namespace BestHTTP.SignalR
{
    using System;

    public enum TransportStates
    {
        Initial,
        Connecting,
        Reconnecting,
        Starting,
        Started,
        Closing,
        Closed
    }
}

