namespace GooglePlayGames.BasicApi.Nearby
{
    using GooglePlayGames.BasicApi;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class DummyNearbyConnectionClient : INearbyConnectionClient
    {
        public void AcceptConnectionRequest(string remoteEndpointId, byte[] payload, IMessageListener listener)
        {
            Debug.LogError("AcceptConnectionRequest in dummy implementation called");
        }

        public void DisconnectFromEndpoint(string remoteEndpointId)
        {
            Debug.LogError("DisconnectFromEndpoint in dummy implementation called");
        }

        public string GetAppBundleId() => 
            "dummy.bundle.id";

        public string GetServiceId() => 
            "dummy.service.id";

        public string LocalDeviceId() => 
            "DummyDevice";

        public string LocalEndpointId() => 
            string.Empty;

        public int MaxReliableMessagePayloadLength() => 
            0x1000;

        public int MaxUnreliableMessagePayloadLength() => 
            0x490;

        public void RejectConnectionRequest(string requestingEndpointId)
        {
            Debug.LogError("RejectConnectionRequest in dummy implementation called");
        }

        public void SendConnectionRequest(string name, string remoteEndpointId, byte[] payload, Action<ConnectionResponse> responseCallback, IMessageListener listener)
        {
            Debug.LogError("SendConnectionRequest called from dummy implementation");
            if (responseCallback != null)
            {
                ConnectionResponse response = ConnectionResponse.Rejected(0L, string.Empty);
                responseCallback(response);
            }
        }

        public void SendReliable(List<string> recipientEndpointIds, byte[] payload)
        {
            Debug.LogError("SendReliable called from dummy implementation");
        }

        public void SendUnreliable(List<string> recipientEndpointIds, byte[] payload)
        {
            Debug.LogError("SendUnreliable called from dummy implementation");
        }

        public void StartAdvertising(string name, List<string> appIdentifiers, TimeSpan? advertisingDuration, Action<AdvertisingResult> resultCallback, Action<ConnectionRequest> connectionRequestCallback)
        {
            AdvertisingResult result = new AdvertisingResult(ResponseStatus.LicenseCheckFailed, string.Empty);
            resultCallback(result);
        }

        public void StartDiscovery(string serviceId, TimeSpan? advertisingTimeout, IDiscoveryListener listener)
        {
            Debug.LogError("StartDiscovery in dummy implementation called");
        }

        public void StopAdvertising()
        {
            Debug.LogError("StopAvertising in dummy implementation called");
        }

        public void StopAllConnections()
        {
            Debug.LogError("StopAllConnections in dummy implementation called");
        }

        public void StopDiscovery(string serviceId)
        {
            Debug.LogError("StopDiscovery in dummy implementation called");
        }
    }
}

