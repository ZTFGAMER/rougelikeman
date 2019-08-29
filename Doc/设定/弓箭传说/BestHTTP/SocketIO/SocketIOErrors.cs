namespace BestHTTP.SocketIO
{
    using System;

    public enum SocketIOErrors
    {
        UnknownTransport,
        UnknownSid,
        BadHandshakeMethod,
        BadRequest,
        Internal,
        User,
        Custom
    }
}

