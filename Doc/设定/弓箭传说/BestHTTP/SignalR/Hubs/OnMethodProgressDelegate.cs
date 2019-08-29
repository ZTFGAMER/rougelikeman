namespace BestHTTP.SignalR.Hubs
{
    using BestHTTP.SignalR.Messages;
    using System;
    using System.Runtime.CompilerServices;

    public delegate void OnMethodProgressDelegate(Hub hub, ClientMessage originialMessage, ProgressMessage progress);
}

