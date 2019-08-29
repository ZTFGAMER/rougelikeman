namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    internal class NativeRealTimeRoom : BaseReferenceHolder
    {
        internal NativeRealTimeRoom(IntPtr selfPointer) : base(selfPointer)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            RealTimeRoom.RealTimeRoom_Dispose(selfPointer);
        }

        internal static NativeRealTimeRoom FromPointer(IntPtr selfPointer)
        {
            if (selfPointer.Equals(IntPtr.Zero))
            {
                return null;
            }
            return new NativeRealTimeRoom(selfPointer);
        }

        internal string Id() => 
            PInvokeUtilities.OutParamsToString((out_string, size) => RealTimeRoom.RealTimeRoom_Id(base.SelfPtr(), out_string, size));

        internal uint ParticipantCount() => 
            RealTimeRoom.RealTimeRoom_Participants_Length(base.SelfPtr()).ToUInt32();

        internal IEnumerable<MultiplayerParticipant> Participants() => 
            PInvokeUtilities.ToEnumerable<MultiplayerParticipant>(RealTimeRoom.RealTimeRoom_Participants_Length(base.SelfPtr()), index => new MultiplayerParticipant(RealTimeRoom.RealTimeRoom_Participants_GetElement(base.SelfPtr(), index)));

        internal Types.RealTimeRoomStatus Status() => 
            RealTimeRoom.RealTimeRoom_Status(base.SelfPtr());
    }
}

