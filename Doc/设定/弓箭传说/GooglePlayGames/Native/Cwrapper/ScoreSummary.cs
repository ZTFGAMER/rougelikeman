namespace GooglePlayGames.Native.Cwrapper
{
    using System;
    using System.Runtime.InteropServices;

    internal static class ScoreSummary
    {
        [DllImport("gpg")]
        internal static extern ulong ScoreSummary_ApproximateNumberOfScores(HandleRef self);
        [DllImport("gpg")]
        internal static extern Types.LeaderboardCollection ScoreSummary_Collection(HandleRef self);
        [DllImport("gpg")]
        internal static extern IntPtr ScoreSummary_CurrentPlayerScore(HandleRef self);
        [DllImport("gpg")]
        internal static extern void ScoreSummary_Dispose(HandleRef self);
        [DllImport("gpg")]
        internal static extern UIntPtr ScoreSummary_LeaderboardId(HandleRef self, [In, Out] char[] out_arg, UIntPtr out_size);
        [DllImport("gpg")]
        internal static extern Types.LeaderboardTimeSpan ScoreSummary_TimeSpan(HandleRef self);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool ScoreSummary_Valid(HandleRef self);
    }
}

