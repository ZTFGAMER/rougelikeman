namespace GooglePlayGames.Native.Cwrapper
{
    using System;
    using System.Runtime.InteropServices;

    internal static class RealTimeRoom
    {
        [DllImport("gpg")]
        internal static extern ulong RealTimeRoom_AutomatchWaitEstimate(HandleRef self);
        [DllImport("gpg")]
        internal static extern IntPtr RealTimeRoom_CreatingParticipant(HandleRef self);
        [DllImport("gpg")]
        internal static extern ulong RealTimeRoom_CreationTime(HandleRef self);
        [DllImport("gpg")]
        internal static extern UIntPtr RealTimeRoom_Description(HandleRef self, [In, Out] char[] out_arg, UIntPtr out_size);
        [DllImport("gpg")]
        internal static extern void RealTimeRoom_Dispose(HandleRef self);
        [DllImport("gpg")]
        internal static extern UIntPtr RealTimeRoom_Id(HandleRef self, [In, Out] byte[] out_arg, UIntPtr out_size);
        [DllImport("gpg")]
        internal static extern IntPtr RealTimeRoom_Participants_GetElement(HandleRef self, UIntPtr index);
        [DllImport("gpg")]
        internal static extern UIntPtr RealTimeRoom_Participants_Length(HandleRef self);
        [DllImport("gpg")]
        internal static extern uint RealTimeRoom_RemainingAutomatchingSlots(HandleRef self);
        [DllImport("gpg")]
        internal static extern Types.RealTimeRoomStatus RealTimeRoom_Status(HandleRef self);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool RealTimeRoom_Valid(HandleRef self);
        [DllImport("gpg")]
        internal static extern uint RealTimeRoom_Variant(HandleRef self);
    }
}

