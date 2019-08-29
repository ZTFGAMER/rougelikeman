namespace BestHTTP.WebSocket.Extensions
{
    using BestHTTP;
    using BestHTTP.WebSocket;
    using BestHTTP.WebSocket.Frames;
    using System;

    public interface IExtension
    {
        void AddNegotiation(HTTPRequest request);
        byte[] Decode(byte header, byte[] data);
        byte[] Encode(WebSocketFrame writer);
        byte GetFrameHeader(WebSocketFrame writer, byte inFlag);
        bool ParseNegotiation(WebSocketResponse resp);
    }
}

