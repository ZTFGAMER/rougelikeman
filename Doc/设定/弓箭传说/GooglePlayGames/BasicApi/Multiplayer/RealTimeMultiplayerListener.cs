namespace GooglePlayGames.BasicApi.Multiplayer
{
    using System;

    public interface RealTimeMultiplayerListener
    {
        void OnLeftRoom();
        void OnParticipantLeft(Participant participant);
        void OnPeersConnected(string[] participantIds);
        void OnPeersDisconnected(string[] participantIds);
        void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data);
        void OnRoomConnected(bool success);
        void OnRoomSetupProgress(float percent);
    }
}

