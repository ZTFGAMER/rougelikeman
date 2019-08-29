namespace BestHTTP.SignalR.Hubs
{
    using BestHTTP.SignalR.Messages;
    using System;
    using System.Runtime.CompilerServices;

    public delegate void OnMethodResultDelegate(Hub hub, ClientMessage originalMessage, ResultMessage result);
}

