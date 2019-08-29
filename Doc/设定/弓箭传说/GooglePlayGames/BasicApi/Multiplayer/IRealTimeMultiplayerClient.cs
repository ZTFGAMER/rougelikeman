namespace GooglePlayGames.BasicApi.Multiplayer
{
    using System;
    using System.Collections.Generic;

    public interface IRealTimeMultiplayerClient
    {
        void AcceptFromInbox(RealTimeMultiplayerListener listener);
        void AcceptInvitation(string invitationId, RealTimeMultiplayerListener listener);
        void CreateQuickGame(uint minOpponents, uint maxOpponents, uint variant, RealTimeMultiplayerListener listener);
        void CreateQuickGame(uint minOpponents, uint maxOpponents, uint variant, ulong exclusiveBitMask, RealTimeMultiplayerListener listener);
        void CreateWithInvitationScreen(uint minOpponents, uint maxOppponents, uint variant, RealTimeMultiplayerListener listener);
        void DeclineInvitation(string invitationId);
        void GetAllInvitations(Action<Invitation[]> callback);
        List<Participant> GetConnectedParticipants();
        Invitation GetInvitation();
        Participant GetParticipant(string participantId);
        Participant GetSelf();
        bool IsRoomConnected();
        void LeaveRoom();
        void SendMessage(bool reliable, string participantId, byte[] data);
        void SendMessage(bool reliable, string participantId, byte[] data, int offset, int length);
        void SendMessageToAll(bool reliable, byte[] data);
        void SendMessageToAll(bool reliable, byte[] data, int offset, int length);
        void ShowWaitingRoomUI();
    }
}

