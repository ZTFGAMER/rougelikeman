namespace BestHTTP.SignalR.Transports
{
    using BestHTTP.SignalR;
    using System;
    using System.Runtime.CompilerServices;

    public delegate void OnTransportStateChangedDelegate(TransportBase transport, TransportStates oldState, TransportStates newState);
}

