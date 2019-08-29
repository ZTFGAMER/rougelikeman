namespace BestHTTP.WebSocket
{
    using System;
    using System.Runtime.CompilerServices;

    public delegate void OnWebSocketClosedDelegate(BestHTTP.WebSocket.WebSocket webSocket, ushort code, string message);
}

