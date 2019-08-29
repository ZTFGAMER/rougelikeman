namespace BestHTTP.SignalR
{
    using BestHTTP;
    using BestHTTP.SignalR.JsonEncoders;
    using BestHTTP.SignalR.Messages;
    using BestHTTP.SignalR.Transports;
    using System;

    public interface IConnection
    {
        Uri BuildUri(RequestTypes type);
        Uri BuildUri(RequestTypes type, TransportBase transport);
        void Error(string reason);
        void OnMessage(IServerMessage msg);
        string ParseResponse(string responseStr);
        HTTPRequest PrepareRequest(HTTPRequest req, RequestTypes type);
        void TransportAborted();
        void TransportReconnected();
        void TransportStarted();

        ProtocolVersions Protocol { get; }

        NegotiationData NegotiationResult { get; }

        IJsonEncoder JsonEncoder { get; set; }
    }
}

