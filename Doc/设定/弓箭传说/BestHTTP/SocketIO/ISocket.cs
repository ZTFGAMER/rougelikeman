namespace BestHTTP.SocketIO
{
    using System;

    public interface ISocket
    {
        void Disconnect(bool remove);
        void EmitError(SocketIOErrors errCode, string msg);
        void EmitEvent(SocketIOEventTypes type, params object[] args);
        void EmitEvent(string eventName, params object[] args);
        void OnPacket(Packet packet);
        void Open();
    }
}

