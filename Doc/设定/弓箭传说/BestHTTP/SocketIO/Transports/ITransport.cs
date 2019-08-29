namespace BestHTTP.SocketIO.Transports
{
    using BestHTTP.SocketIO;
    using System;
    using System.Collections.Generic;

    public interface ITransport
    {
        void Close();
        void Open();
        void Poll();
        void Send(Packet packet);
        void Send(List<Packet> packets);

        TransportTypes Type { get; }

        TransportStates State { get; }

        SocketManager Manager { get; }

        bool IsRequestInProgress { get; }

        bool IsPollingInProgress { get; }
    }
}

