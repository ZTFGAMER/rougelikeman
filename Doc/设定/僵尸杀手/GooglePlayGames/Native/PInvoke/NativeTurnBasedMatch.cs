namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.BasicApi.Multiplayer;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    internal class NativeTurnBasedMatch : BaseReferenceHolder
    {
        internal NativeTurnBasedMatch(IntPtr selfPointer) : base(selfPointer)
        {
        }

        internal GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch AsTurnBasedMatch(string selfPlayerId)
        {
            List<Participant> participants = new List<Participant>();
            string selfParticipantId = null;
            string pendingParticipantId = null;
            using (GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant = this.PendingParticipant())
            {
                if (participant != null)
                {
                    pendingParticipantId = participant.Id();
                }
            }
            foreach (GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant2 in this.Participants())
            {
                using (participant2)
                {
                    using (NativePlayer player = participant2.Player())
                    {
                        if ((player != null) && player.Id().Equals(selfPlayerId))
                        {
                            selfParticipantId = participant2.Id();
                        }
                    }
                    participants.Add(participant2.AsParticipant());
                }
            }
            return new GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch(this.Id(), this.Data(), (this.MatchStatus() == GooglePlayGames.Native.Cwrapper.Types.MatchStatus.COMPLETED) && !this.HasRematchId(), selfParticipantId, participants, this.AvailableAutomatchSlots(), pendingParticipantId, ToTurnStatus(this.MatchStatus()), ToMatchStatus(pendingParticipantId, this.MatchStatus()), this.Variant(), this.Version());
        }

        internal uint AvailableAutomatchSlots() => 
            GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_AutomatchingSlotsAvailable(base.SelfPtr());

        protected override void CallDispose(HandleRef selfPointer)
        {
            GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Dispose(selfPointer);
        }

        internal ulong CreationTime() => 
            GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_CreationTime(base.SelfPtr());

        internal byte[] Data()
        {
            if (!GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_HasData(base.SelfPtr()))
            {
                Logger.d("Match has no data.");
                return null;
            }
            return PInvokeUtilities.OutParamsToArray<byte>((bytes, size) => GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Data(base.SelfPtr(), bytes, size));
        }

        internal string Description() => 
            PInvokeUtilities.OutParamsToString((out_string, size) => GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Description(base.SelfPtr(), out_string, size));

        internal static NativeTurnBasedMatch FromPointer(IntPtr selfPointer)
        {
            if (PInvokeUtilities.IsNull(selfPointer))
            {
                return null;
            }
            return new NativeTurnBasedMatch(selfPointer);
        }

        internal bool HasRematchId()
        {
            string str = this.RematchId();
            return (string.IsNullOrEmpty(str) || !str.Equals("(null)"));
        }

        internal string Id() => 
            PInvokeUtilities.OutParamsToString((out_string, size) => GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Id(base.SelfPtr(), out_string, size));

        internal GooglePlayGames.Native.Cwrapper.Types.MatchStatus MatchStatus() => 
            GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Status(base.SelfPtr());

        internal IEnumerable<GooglePlayGames.Native.PInvoke.MultiplayerParticipant> Participants() => 
            PInvokeUtilities.ToEnumerable<GooglePlayGames.Native.PInvoke.MultiplayerParticipant>(GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Participants_Length(base.SelfPtr()), index => new GooglePlayGames.Native.PInvoke.MultiplayerParticipant(GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Participants_GetElement(base.SelfPtr(), index)));

        internal GooglePlayGames.Native.PInvoke.MultiplayerParticipant ParticipantWithId(string participantId)
        {
            foreach (GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant in this.Participants())
            {
                if (participant.Id().Equals(participantId))
                {
                    return participant;
                }
                participant.Dispose();
            }
            return null;
        }

        internal GooglePlayGames.Native.PInvoke.MultiplayerParticipant PendingParticipant()
        {
            GooglePlayGames.Native.PInvoke.MultiplayerParticipant participant = new GooglePlayGames.Native.PInvoke.MultiplayerParticipant(GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_PendingParticipant(base.SelfPtr()));
            if (!participant.Valid())
            {
                participant.Dispose();
                return null;
            }
            return participant;
        }

        internal string RematchId() => 
            PInvokeUtilities.OutParamsToString((out_string, size) => GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_RematchId(base.SelfPtr(), out_string, size));

        internal GooglePlayGames.Native.PInvoke.ParticipantResults Results() => 
            new GooglePlayGames.Native.PInvoke.ParticipantResults(GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_ParticipantResults(base.SelfPtr()));

        private static GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus ToMatchStatus(string pendingParticipantId, GooglePlayGames.Native.Cwrapper.Types.MatchStatus status)
        {
            switch (status)
            {
                case GooglePlayGames.Native.Cwrapper.Types.MatchStatus.INVITED:
                    return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Active;

                case GooglePlayGames.Native.Cwrapper.Types.MatchStatus.THEIR_TURN:
                    return ((pendingParticipantId != null) ? GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Active : GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.AutoMatching);

                case GooglePlayGames.Native.Cwrapper.Types.MatchStatus.MY_TURN:
                    return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Active;

                case GooglePlayGames.Native.Cwrapper.Types.MatchStatus.PENDING_COMPLETION:
                    return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Complete;

                case GooglePlayGames.Native.Cwrapper.Types.MatchStatus.COMPLETED:
                    return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Complete;

                case GooglePlayGames.Native.Cwrapper.Types.MatchStatus.CANCELED:
                    return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Cancelled;

                case GooglePlayGames.Native.Cwrapper.Types.MatchStatus.EXPIRED:
                    return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Expired;
            }
            return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchStatus.Unknown;
        }

        private static GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus ToTurnStatus(GooglePlayGames.Native.Cwrapper.Types.MatchStatus status)
        {
            switch (status)
            {
                case GooglePlayGames.Native.Cwrapper.Types.MatchStatus.INVITED:
                    return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.Invited;

                case GooglePlayGames.Native.Cwrapper.Types.MatchStatus.THEIR_TURN:
                    return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.TheirTurn;

                case GooglePlayGames.Native.Cwrapper.Types.MatchStatus.MY_TURN:
                    return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.MyTurn;

                case GooglePlayGames.Native.Cwrapper.Types.MatchStatus.PENDING_COMPLETION:
                    return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.Complete;

                case GooglePlayGames.Native.Cwrapper.Types.MatchStatus.COMPLETED:
                    return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.Complete;

                case GooglePlayGames.Native.Cwrapper.Types.MatchStatus.CANCELED:
                    return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.Complete;

                case GooglePlayGames.Native.Cwrapper.Types.MatchStatus.EXPIRED:
                    return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.Complete;
            }
            return GooglePlayGames.BasicApi.Multiplayer.TurnBasedMatch.MatchTurnStatus.Unknown;
        }

        internal uint Variant() => 
            GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Variant(base.SelfPtr());

        internal uint Version() => 
            GooglePlayGames.Native.Cwrapper.TurnBasedMatch.TurnBasedMatch_Version(base.SelfPtr());
    }
}

