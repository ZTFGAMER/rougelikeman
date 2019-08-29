namespace GooglePlayGames.Native.Cwrapper
{
    using System;
    using System.Runtime.InteropServices;

    internal static class Score
    {
        [DllImport("gpg")]
        internal static extern void Score_Dispose(HandleRef self);
        [DllImport("gpg")]
        internal static extern UIntPtr Score_Metadata(HandleRef self, [In, Out] byte[] out_arg, UIntPtr out_size);
        [DllImport("gpg")]
        internal static extern ulong Score_Rank(HandleRef self);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool Score_Valid(HandleRef self);
        [DllImport("gpg")]
        internal static extern ulong Score_Value(HandleRef self);
    }
}

