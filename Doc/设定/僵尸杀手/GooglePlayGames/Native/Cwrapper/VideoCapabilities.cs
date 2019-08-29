namespace GooglePlayGames.Native.Cwrapper
{
    using System;
    using System.Runtime.InteropServices;

    internal static class VideoCapabilities
    {
        [DllImport("gpg")]
        internal static extern void VideoCapabilities_Dispose(HandleRef self);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool VideoCapabilities_IsCameraSupported(HandleRef self);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool VideoCapabilities_IsMicSupported(HandleRef self);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool VideoCapabilities_IsWriteStorageSupported(HandleRef self);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool VideoCapabilities_SupportsCaptureMode(HandleRef self, Types.VideoCaptureMode capture_mode);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool VideoCapabilities_SupportsQualityLevel(HandleRef self, Types.VideoQualityLevel quality_level);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool VideoCapabilities_Valid(HandleRef self);
    }
}

