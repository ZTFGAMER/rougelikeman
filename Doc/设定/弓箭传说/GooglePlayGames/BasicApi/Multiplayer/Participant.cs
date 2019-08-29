namespace GooglePlayGames.BasicApi.Multiplayer
{
    using System;

    public class Participant : IComparable<Participant>
    {
        private string mDisplayName = string.Empty;
        private readonly string mParticipantId = string.Empty;
        private ParticipantStatus mStatus = ParticipantStatus.Unknown;
        private GooglePlayGames.BasicApi.Multiplayer.Player mPlayer;
        private bool mIsConnectedToRoom;

        internal Participant(string displayName, string participantId, ParticipantStatus status, GooglePlayGames.BasicApi.Multiplayer.Player player, bool connectedToRoom)
        {
            this.mDisplayName = displayName;
            this.mParticipantId = participantId;
            this.mStatus = status;
            this.mPlayer = player;
            this.mIsConnectedToRoom = connectedToRoom;
        }

        public int CompareTo(Participant other) => 
            string.Compare(this.mParticipantId, other.mParticipantId, StringComparison.Ordinal);

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof(Participant))
            {
                return false;
            }
            Participant participant = (Participant) obj;
            return this.mParticipantId.Equals(participant.mParticipantId);
        }

        public override int GetHashCode() => 
            ((this.mParticipantId == null) ? 0 : this.mParticipantId.GetHashCode());

        public override string ToString() => 
            $"[Participant: '{this.mDisplayName}' (id {this.mParticipantId}), status={this.mStatus.ToString()}, player={((this.mPlayer != null) ? this.mPlayer.ToString() : "NULL")}, connected={this.mIsConnectedToRoom}]";

        public string DisplayName =>
            this.mDisplayName;

        public string ParticipantId =>
            this.mParticipantId;

        public ParticipantStatus Status =>
            this.mStatus;

        public GooglePlayGames.BasicApi.Multiplayer.Player Player =>
            this.mPlayer;

        public bool IsConnectedToRoom =>
            this.mIsConnectedToRoom;

        public bool IsAutomatch =>
            (this.mPlayer == null);

        public enum ParticipantStatus
        {
            NotInvitedYet,
            Invited,
            Joined,
            Declined,
            Left,
            Finished,
            Unresponsive,
            Unknown
        }
    }
}

