namespace GooglePlayGames.BasicApi.Nearby
{
    using System;
    using System.Collections.Generic;

    public interface INearbyConnectionClient
    {
        void AcceptConnectionRequest(string remoteEndpointId, byte[] payload, IMessageListener listener);
        void DisconnectFromEndpoint(string remoteEndpointId);
        string GetAppBundleId();
        string GetServiceId();
        int MaxReliableMessagePayloadLength();
        int MaxUnreliableMessagePayloadLength();
        void RejectConnectionRequest(string requestingEndpointId);
        void SendConnectionRequest(string name, string remoteEndpointId, byte[] payload, Action<ConnectionResponse> responseCallback, IMessageListener listener);
        void SendReliable(List<string> recipientEndpointIds, byte[] payload);
        void SendUnreliable(List<string> recipientEndpointIds, byte[] payload);
        void StartAdvertising(string name, List<string> appIdentifiers, TimeSpan? advertisingDuration, Action<AdvertisingResult> resultCallback, Action<ConnectionRequest> connectionRequestCallback);
        void StartDiscovery(string serviceId, TimeSpan? advertisingTimeout, IDiscoveryListener listener);
        void StopAdvertising();
        void StopAllConnections();
        void StopDiscovery(string serviceId);
    }
}

