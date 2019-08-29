namespace BestHTTP.SignalR
{
    using System;

    public enum RequestTypes
    {
        Negotiate,
        Connect,
        Start,
        Poll,
        Send,
        Reconnect,
        Abort,
        Ping
    }
}

