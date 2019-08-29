namespace BestHTTP.SignalR.Hubs
{
    using BestHTTP.SignalR.Messages;
    using System;
    using System.Runtime.CompilerServices;

    public delegate void OnMethodCallCallbackDelegate(Hub hub, MethodCallMessage methodCall);
}

