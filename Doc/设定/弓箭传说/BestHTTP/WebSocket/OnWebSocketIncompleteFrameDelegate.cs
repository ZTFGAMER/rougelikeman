namespace BestHTTP.WebSocket
{
    using BestHTTP.WebSocket.Frames;
    using System;
    using System.Runtime.CompilerServices;

    public delegate void OnWebSocketIncompleteFrameDelegate(BestHTTP.WebSocket.WebSocket webSocket, WebSocketFrameReader frame);
}

