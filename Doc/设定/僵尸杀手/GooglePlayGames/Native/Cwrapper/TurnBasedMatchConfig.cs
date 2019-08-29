namespace GooglePlayGames.Native.Cwrapper
{
    using System;
    using System.Runtime.InteropServices;

    internal static class TurnBasedMatchConfig
    {
        [DllImport("gpg")]
        internal static extern void TurnBasedMatchConfig_Dispose(HandleRef self);
        [DllImport("gpg")]
        internal static extern long TurnBasedMatchConfig_ExclusiveBitMask(HandleRef self);
        [DllImport("gpg")]
        internal static extern uint TurnBasedMatchConfig_MaximumAutomatchingPlayers(HandleRef self);
        [DllImport("gpg")]
        internal static extern uint TurnBasedMatchConfig_MinimumAutomatchingPlayers(HandleRef self);
        [DllImport("gpg")]
        internal static extern UIntPtr TurnBasedMatchConfig_PlayerIdsToInvite_GetElement(HandleRef self, UIntPtr index, [In, Out] byte[] out_arg, UIntPtr out_size);
        [DllImport("gpg")]
        internal static extern UIntPtr TurnBasedMatchConfig_PlayerIdsToInvite_Length(HandleRef self);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool TurnBasedMatchConfig_Valid(HandleRef self);
        [DllImport("gpg")]
        internal static extern uint TurnBasedMatchConfig_Variant(HandleRef self);
    }
}

