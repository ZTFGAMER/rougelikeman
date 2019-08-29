namespace BestHTTP.SocketIO
{
    using BestHTTP.SocketIO.Transports;
    using System;
    using System.Runtime.InteropServices;

    public interface IManager
    {
        void Close(bool removeSockets = true);
        void EmitAll(string eventName, params object[] args);
        void EmitError(SocketIOErrors errCode, string msg);
        void EmitEvent(SocketIOEventTypes type, params object[] args);
        void EmitEvent(string eventName, params object[] args);
        void OnPacket(Packet packet);
        bool OnTransportConnected(ITransport transport);
        void OnTransportError(ITransport trans, string err);
        void OnTransportProbed(ITransport trans);
        void Remove(Socket socket);
        void SendPacket(Packet packet);
        void TryToReconnect();
    }
}

