namespace GooglePlayGames.BasicApi.Nearby
{
    using System;

    public interface IDiscoveryListener
    {
        void OnEndpointFound(EndpointDetails discoveredEndpoint);
        void OnEndpointLost(string lostEndpointId);
    }
}

