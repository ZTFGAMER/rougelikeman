namespace BestHTTP.ServerSentEvents
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate void OnStateChangedDelegate(EventSource eventSource, States oldState, States newState);
}

