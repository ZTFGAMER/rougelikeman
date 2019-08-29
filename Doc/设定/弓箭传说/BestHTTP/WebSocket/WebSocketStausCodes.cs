namespace BestHTTP.WebSocket
{
    using System;

    public enum WebSocketStausCodes : uint
    {
        NormalClosure = 0x3e8,
        GoingAway = 0x3e9,
        ProtocolError = 0x3ea,
        WrongDataType = 0x3eb,
        Reserved = 0x3ec,
        NoStatusCode = 0x3ed,
        ClosedAbnormally = 0x3ee,
        DataError = 0x3ef,
        PolicyError = 0x3f0,
        TooBigMessage = 0x3f1,
        ExtensionExpected = 0x3f2,
        WrongRequest = 0x3f3,
        TLSHandshakeError = 0x3f7
    }
}

