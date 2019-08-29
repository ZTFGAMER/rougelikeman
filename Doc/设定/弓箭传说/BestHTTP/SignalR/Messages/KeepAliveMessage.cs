namespace BestHTTP.SignalR.Messages
{
    using BestHTTP.SignalR;
    using System;

    public sealed class KeepAliveMessage : IServerMessage
    {
        void IServerMessage.Parse(object data)
        {
        }

        MessageTypes IServerMessage.Type =>
            MessageTypes.KeepAlive;
    }
}

