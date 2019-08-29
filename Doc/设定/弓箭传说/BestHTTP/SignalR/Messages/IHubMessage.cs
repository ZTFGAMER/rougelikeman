namespace BestHTTP.SignalR.Messages
{
    using System;

    public interface IHubMessage
    {
        ulong InvocationId { get; }
    }
}

