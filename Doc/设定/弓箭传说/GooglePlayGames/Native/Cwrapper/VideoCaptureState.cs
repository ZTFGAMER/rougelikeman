namespace GooglePlayGames.Native.Cwrapper
{
    using System;
    using System.Runtime.InteropServices;

    internal static class VideoCaptureState
    {
        [DllImport("gpg")]
        internal static extern Types.VideoCaptureMode VideoCaptureState_CaptureMode(HandleRef self);
        [DllImport("gpg")]
        internal static extern void VideoCaptureState_Dispose(HandleRef self);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool VideoCaptureState_IsCapturing(HandleRef self);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool VideoCaptureState_IsOverlayVisible(HandleRef self);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool VideoCaptureState_IsPaused(HandleRef self);
        [DllImport("gpg")]
        internal static extern Types.VideoQualityLevel VideoCaptureState_QualityLevel(HandleRef self);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool VideoCaptureState_Valid(HandleRef self);
    }
}

