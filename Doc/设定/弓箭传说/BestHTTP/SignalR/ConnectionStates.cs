namespace BestHTTP.SignalR
{
    using System;

    public enum ConnectionStates
    {
        Initial,
        Authenticating,
        Negotiating,
        Connecting,
        Connected,
        Reconnecting,
        Closed
    }
}

