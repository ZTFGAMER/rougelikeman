namespace BestHTTP.SignalR
{
    using System;

    public enum MessageTypes
    {
        KeepAlive,
        Data,
        Multiple,
        Result,
        Failure,
        MethodCall,
        Progress
    }
}

