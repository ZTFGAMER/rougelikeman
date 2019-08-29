namespace BestHTTP.ServerSentEvents
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate void OnMessageDelegate(EventSource eventSource, Message message);
}

