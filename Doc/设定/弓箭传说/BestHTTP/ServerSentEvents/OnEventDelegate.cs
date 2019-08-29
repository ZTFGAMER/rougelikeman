namespace BestHTTP.ServerSentEvents
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate void OnEventDelegate(EventSource eventSource, Message message);
}

