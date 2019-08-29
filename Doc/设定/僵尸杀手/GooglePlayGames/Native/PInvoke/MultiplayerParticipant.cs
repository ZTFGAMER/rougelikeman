namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.BasicApi.Multiplayer;
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    internal class MultiplayerParticipant : BaseReferenceHolder
    {
        private static readonly Dictionary<Types.ParticipantStatus, Participant.ParticipantStatus> StatusConversion;

        static MultiplayerParticipant()
        {
            Dictionary<Types.ParticipantStatus, Participant.ParticipantStatus> dictionary = new Dictionary<Types.ParticipantStatus, Participant.ParticipantStatus> {
                { 
                    Types.ParticipantStatus.INVITED,
                    Participant.ParticipantStatus.Invited
                },
                { 
                    Types.ParticipantStatus.JOINED,
                    Participant.ParticipantStatus.Joined
                },
                { 
                    Types.ParticipantStatus.DECLINED,
                    Participant.ParticipantStatus.Declined
                },
                { 
                    Types.ParticipantStatus.LEFT,
                    Participant.ParticipantStatus.Left
                },
                { 
                    Types.ParticipantStatus.NOT_INVITED_YET,
                    Participant.ParticipantStatus.NotInvitedYet
                },
                { 
                    Types.ParticipantStatus.FINISHED,
                    Participant.ParticipantStatus.Finished
                },
                { 
                    Types.ParticipantStatus.UNRESPONSIVE,
                    Participant.ParticipantStatus.Unresponsive
                }
            };
            StatusConversion = dictionary;
        }

        internal MultiplayerParticipant(IntPtr selfPointer) : base(selfPointer)
        {
        }

        internal Participant AsParticipant()
        {
            NativePlayer player = this.Player();
            return new Participant(this.DisplayName(), this.Id(), StatusConversion[this.Status()], player?.AsPlayer(), this.IsConnectedToRoom());
        }

        internal static GooglePlayGames.Native.PInvoke.MultiplayerParticipant AutomatchingSentinel() => 
            new GooglePlayGames.Native.PInvoke.MultiplayerParticipant(Sentinels.Sentinels_AutomatchingParticipant());

        protected override void CallDispose(HandleRef selfPointer)
        {
            GooglePlayGames.Native.Cwrapper.MultiplayerParticipant.MultiplayerParticipant_Dispose(selfPointer);
        }

        internal string DisplayName() => 
            PInvokeUtilities.OutParamsToString((out_string, size) => GooglePlayGames.Native.Cwrapper.MultiplayerParticipant.MultiplayerParticipant_DisplayName(base.SelfPtr(), out_string, size));

        internal static GooglePlayGames.Native.PInvoke.MultiplayerParticipant FromPointer(IntPtr pointer)
        {
            if (PInvokeUtilities.IsNull(pointer))
            {
                return null;
            }
            return new GooglePlayGames.Native.PInvoke.MultiplayerParticipant(pointer);
        }

        internal string Id() => 
            PInvokeUtilities.OutParamsToString((out_string, size) => GooglePlayGames.Native.Cwrapper.MultiplayerParticipant.MultiplayerParticipant_Id(base.SelfPtr(), out_string, size));

        internal bool IsConnectedToRoom() => 
            (GooglePlayGames.Native.Cwrapper.MultiplayerParticipant.MultiplayerParticipant_IsConnectedToRoom(base.SelfPtr()) || (this.Status() == Types.ParticipantStatus.JOINED));

        internal NativePlayer Player()
        {
            if (!GooglePlayGames.Native.Cwrapper.MultiplayerParticipant.MultiplayerParticipant_HasPlayer(base.SelfPtr()))
            {
                return null;
            }
            return new NativePlayer(GooglePlayGames.Native.Cwrapper.MultiplayerParticipant.MultiplayerParticipant_Player(base.SelfPtr()));
        }

        internal Types.ParticipantStatus Status() => 
            GooglePlayGames.Native.Cwrapper.MultiplayerParticipant.MultiplayerParticipant_Status(base.SelfPtr());

        internal bool Valid() => 
            GooglePlayGames.Native.Cwrapper.MultiplayerParticipant.MultiplayerParticipant_Valid(base.SelfPtr());
    }
}

