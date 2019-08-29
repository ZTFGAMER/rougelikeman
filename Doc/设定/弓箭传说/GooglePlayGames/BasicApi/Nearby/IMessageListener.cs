namespace GooglePlayGames.BasicApi.Nearby
{
    using System;

    public interface IMessageListener
    {
        void OnMessageReceived(string remoteEndpointId, byte[] data, bool isReliableMessage);
        void OnRemoteEndpointDisconnected(string remoteEndpointId);
    }
}

