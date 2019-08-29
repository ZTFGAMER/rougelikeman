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

        internal TurnBasedMatch AsTurnBasedMatch(string selfPlayerId)
        {
            List<Participant> participants = new List<Participant>();
            string selfParticipantId = null;
            string pendingParticipantId = null;
            using (MultiplayerParticipant participant = this.PendingParticipant())
            {
                if (participant != null)
                {
                    pendingParticipantId = participant.Id();
                }
            }
            foreach (MultiplayerParticipant participant2 in this.Participants())
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
            return new TurnBasedMatch(this.Id(), this.Data(), (this.MatchStatus() == GooglePlayGames.Native.Cwrapper.Types.MatchStatus.COMPLETED) && !this.HasRematchId(), selfParticipantId, participants, this.AvailableAutomatchSlots(), pendingParticipantId, ToTurnStatus(this.MatchStatus()), ToMatchStatus(pendingParticipantId, this.MatchStatus()), this.Variant(), this.Version());
        }

        internal uint AvailableAutomatchSlots() => 
            TurnBasedMatch.TurnBasedMatch_AutomatchingSlotsAvailable(base.SelfPtr());

        protected override void CallDispose(HandleRef selfPointer)
        {
            TurnBasedMatch.TurnBasedMatch_Dispose(selfPointer);
        }

        internal ulong CreationTime() => 
            TurnBasedMatch.TurnBasedMatch_CreationTime(base.SelfPtr());

        internal byte[] Data()
        {
            if (!TurnBasedMatch.TurnBasedMatch_HasData(base.SelfPtr()))
            {
                Logger.d("Match has no data.");
                return null;
            }
            return PInvokeUtilities.OutParamsToArray<byte>((bytes, size) => TurnBasedMatch.TurnBasedMatch_Data(base.SelfPtr(), bytes, size));
        }

        internal string Description() => 
            PInvokeUtilities.OutParamsToString((out_string, size) => TurnBasedMatch.TurnBasedMatch_Description(base.SelfPtr(), out_string, size));

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
            PInvokeUtilities.OutParamsToString((out_string, size) => TurnBasedMatch.TurnBasedMatch_Id(base.SelfPtr(), out_string, size));

        internal GooglePlayGames.Native.Cwrapper.Types.MatchStatus MatchStatus() => 
            TurnBasedMatch.TurnBasedMatch_Status(base.SelfPtr());

        internal IEnumerable<MultiplayerParticipant> Participants() => 
            PInvokeUtilities.ToEnumerable<MultiplayerParticipant>(TurnBasedMatch.TurnBasedMatch_Participants_Length(base.SelfPtr()), index => new MultiplayerParticipant(TurnBasedMatch.TurnBasedMatch_Participants_GetElement(base.SelfPtr(), index)));

        internal MultiplayerParticipant ParticipantWithId(string participantId)
        {
            foreach (MultiplayerParticipant participant in this.Participants())
            {
                if (participant.Id().Equals(participantId))
                {
                    return participant;
                }
                participant.Dispose();
            }
            return null;
        }

        internal MultiplayerParticipant PendingParticipant()
        {
            MultiplayerParticipant participant = new MultiplayerParticipant(TurnBasedMatch.TurnBasedMatch_PendingParticipant(base.SelfPtr()));
            if (!participant.Valid())
            {
                participant.Dispose();
                return null;
            }
            return participant;
        }

        internal string RematchId() => 
            PInvokeUtilities.OutParamsToString((out_string, size) => TurnBasedMatch.TurnBasedMatch_RematchId(base.SelfPtr(), out_string, size));

        internal ParticipantResults Results() => 
            new ParticipantResults(TurnBasedMatch.TurnBasedMatch_ParticipantResults(base.SelfPtr()));

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

        private static TurnBasedMatch.MatchTurnStatus ToTurnStatus(GooglePlayGames.Native.Cwrapper.Types.MatchStatus status)
        {
            switch (status)
            {
                case GooglePlayGames.Native.Cwrapper.Types.MatchStatus.INVITED:
                    return TurnBasedMatch.MatchTurnStatus.Invited;

                case GooglePlayGames.Native.Cwrapper.Types.MatchStatus.THEIR_TURN:
                    return TurnBasedMatch.MatchTurnStatus.TheirTurn;

                case GooglePlayGames.Native.Cwrapper.Types.MatchStatus.MY_TURN:
                    return TurnBasedMatch.MatchTurnStatus.MyTurn;

                case GooglePlayGames.Native.Cwrapper.Types.MatchStatus.PENDING_COMPLETION:
                    return TurnBasedMatch.MatchTurnStatus.Complete;

                case GooglePlayGames.Native.Cwrapper.Types.MatchStatus.COMPLETED:
                    return TurnBasedMatch.MatchTurnStatus.Complete;

                case GooglePlayGames.Native.Cwrapper.Types.MatchStatus.CANCELED:
                    return TurnBasedMatch.MatchTurnStatus.Complete;

                case GooglePlayGames.Native.Cwrapper.Types.MatchStatus.EXPIRED:
                    return TurnBasedMatch.MatchTurnStatus.Complete;
            }
            return TurnBasedMatch.MatchTurnStatus.Unknown;
        }

        internal uint Variant() => 
            TurnBasedMatch.TurnBasedMatch_Variant(base.SelfPtr());

        internal uint Version() => 
            TurnBasedMatch.TurnBasedMatch_Version(base.SelfPtr());
    }
}

