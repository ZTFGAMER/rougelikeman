namespace BestHTTP.SocketIO.Events
{
    using BestHTTP.SocketIO;
    using System;
    using System.Runtime.CompilerServices;

    public delegate void SocketIOCallback(Socket socket, Packet packet, params object[] args);
}

