namespace GooglePlayGames.Native.Cwrapper
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal static class GameServices
    {
        [DllImport("gpg")]
        internal static extern void GameServices_Dispose(HandleRef self);
        [DllImport("gpg")]
        internal static extern void GameServices_Flush(HandleRef self, FlushCallback callback, IntPtr callback_arg);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool GameServices_IsAuthorized(HandleRef self);
        [DllImport("gpg")]
        internal static extern void GameServices_SignOut(HandleRef self);
        [DllImport("gpg")]
        internal static extern void GameServices_StartAuthorizationUI(HandleRef self);

        internal delegate void FlushCallback(CommonErrorStatus.FlushStatus arg0, IntPtr arg1);
    }
}

