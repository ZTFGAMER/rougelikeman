namespace BestHTTP.SocketIO.Transports
{
    using System;

    public enum TransportStates
    {
        Connecting,
        Opening,
        Open,
        Closed,
        Paused
    }
}

