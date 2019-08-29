namespace GooglePlayGames.Native.Cwrapper
{
    using System;
    using System.Runtime.InteropServices;

    internal static class PlayerStats
    {
        [DllImport("gpg")]
        internal static extern float PlayerStats_AverageSessionLength(HandleRef self);
        [DllImport("gpg")]
        internal static extern float PlayerStats_ChurnProbability(HandleRef self);
        [DllImport("gpg")]
        internal static extern int PlayerStats_DaysSinceLastPlayed(HandleRef self);
        [DllImport("gpg")]
        internal static extern void PlayerStats_Dispose(HandleRef self);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool PlayerStats_HasAverageSessionLength(HandleRef self);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool PlayerStats_HasChurnProbability(HandleRef self);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool PlayerStats_HasDaysSinceLastPlayed(HandleRef self);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool PlayerStats_HasNumberOfPurchases(HandleRef self);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool PlayerStats_HasNumberOfSessions(HandleRef self);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool PlayerStats_HasSessionPercentile(HandleRef self);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool PlayerStats_HasSpendPercentile(HandleRef self);
        [DllImport("gpg")]
        internal static extern int PlayerStats_NumberOfPurchases(HandleRef self);
        [DllImport("gpg")]
        internal static extern int PlayerStats_NumberOfSessions(HandleRef self);
        [DllImport("gpg")]
        internal static extern float PlayerStats_SessionPercentile(HandleRef self);
        [DllImport("gpg")]
        internal static extern float PlayerStats_SpendPercentile(HandleRef self);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool PlayerStats_Valid(HandleRef self);
    }
}

