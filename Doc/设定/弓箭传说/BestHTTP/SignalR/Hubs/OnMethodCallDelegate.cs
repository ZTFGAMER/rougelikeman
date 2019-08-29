namespace BestHTTP.SignalR.Hubs
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate void OnMethodCallDelegate(Hub hub, string method, params object[] args);
}

