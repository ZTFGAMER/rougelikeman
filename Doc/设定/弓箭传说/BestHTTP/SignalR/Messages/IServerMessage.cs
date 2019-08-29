namespace BestHTTP.SignalR.Messages
{
    using BestHTTP.SignalR;
    using System;

    public interface IServerMessage
    {
        void Parse(object data);

        MessageTypes Type { get; }
    }
}

