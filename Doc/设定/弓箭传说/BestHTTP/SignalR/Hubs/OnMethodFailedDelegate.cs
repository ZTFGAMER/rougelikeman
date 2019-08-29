namespace BestHTTP.SignalR.Hubs
{
    using BestHTTP.SignalR.Messages;
    using System;
    using System.Runtime.CompilerServices;

    public delegate void OnMethodFailedDelegate(Hub hub, ClientMessage originalMessage, FailureMessage error);
}

