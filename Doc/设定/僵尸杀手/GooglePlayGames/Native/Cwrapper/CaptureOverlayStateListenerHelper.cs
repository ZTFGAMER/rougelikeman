namespace GooglePlayGames.Native.Cwrapper
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal static class CaptureOverlayStateListenerHelper
    {
        [DllImport("gpg")]
        internal static extern IntPtr CaptureOverlayStateListenerHelper_Construct();
        [DllImport("gpg")]
        internal static extern void CaptureOverlayStateListenerHelper_Dispose(HandleRef self);
        [DllImport("gpg")]
        internal static extern void CaptureOverlayStateListenerHelper_SetOnCaptureOverlayStateChangedCallback(HandleRef self, OnCaptureOverlayStateChangedCallback callback, IntPtr callback_arg);

        internal delegate void OnCaptureOverlayStateChangedCallback(Types.VideoCaptureOverlayState arg0, IntPtr arg1);
    }
}

