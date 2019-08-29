namespace BestHTTP.SignalR
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate void OnStateChanged(Connection connection, ConnectionStates oldState, ConnectionStates newState);
}

