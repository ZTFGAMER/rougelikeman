namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.BasicApi.Multiplayer;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.InteropServices;

    internal class MultiplayerInvitation : BaseReferenceHolder
    {
        internal MultiplayerInvitation(IntPtr selfPointer) : base(selfPointer)
        {
        }

        internal Invitation AsInvitation()
        {
            Participant participant;
            Invitation.InvType invType = ToInvType(this.Type());
            string invId = this.Id();
            int variant = (int) this.Variant();
            using (MultiplayerParticipant participant2 = this.Inviter())
            {
                participant = participant2?.AsParticipant();
            }
            return new Invitation(invType, invId, participant, variant);
        }

        internal uint AutomatchingSlots() => 
            MultiplayerInvitation.MultiplayerInvitation_AutomatchingSlotsAvailable(base.SelfPtr());

        protected override void CallDispose(HandleRef selfPointer)
        {
            MultiplayerInvitation.MultiplayerInvitation_Dispose(selfPointer);
        }

        internal static MultiplayerInvitation FromPointer(IntPtr selfPointer)
        {
            if (PInvokeUtilities.IsNull(selfPointer))
            {
                return null;
            }
            return new MultiplayerInvitation(selfPointer);
        }

        internal string Id() => 
            PInvokeUtilities.OutParamsToString((out_string, size) => MultiplayerInvitation.MultiplayerInvitation_Id(base.SelfPtr(), out_string, size));

        internal MultiplayerParticipant Inviter()
        {
            MultiplayerParticipant participant = new MultiplayerParticipant(MultiplayerInvitation.MultiplayerInvitation_InvitingParticipant(base.SelfPtr()));
            if (!participant.Valid())
            {
                participant.Dispose();
                return null;
            }
            return participant;
        }

        internal uint ParticipantCount() => 
            MultiplayerInvitation.MultiplayerInvitation_Participants_Length(base.SelfPtr()).ToUInt32();

        private static Invitation.InvType ToInvType(Types.MultiplayerInvitationType invitationType)
        {
            if (invitationType != Types.MultiplayerInvitationType.REAL_TIME)
            {
                if (invitationType == Types.MultiplayerInvitationType.TURN_BASED)
                {
                    return Invitation.InvType.TurnBased;
                }
            }
            else
            {
                return Invitation.InvType.RealTime;
            }
            Logger.d("Found unknown invitation type: " + invitationType);
            return Invitation.InvType.Unknown;
        }

        internal Types.MultiplayerInvitationType Type() => 
            MultiplayerInvitation.MultiplayerInvitation_Type(base.SelfPtr());

        internal uint Variant() => 
            MultiplayerInvitation.MultiplayerInvitation_Variant(base.SelfPtr());
    }
}

