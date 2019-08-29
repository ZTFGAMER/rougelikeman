namespace BestHTTP.SignalR.Hubs
{
    using BestHTTP.SignalR;
    using BestHTTP.SignalR.Messages;
    using System;

    public interface IHub
    {
        bool Call(ClientMessage msg);
        void Close();
        bool HasSentMessageId(ulong id);
        void OnMessage(IServerMessage msg);
        void OnMethod(MethodCallMessage msg);

        BestHTTP.SignalR.Connection Connection { get; set; }
    }
}

