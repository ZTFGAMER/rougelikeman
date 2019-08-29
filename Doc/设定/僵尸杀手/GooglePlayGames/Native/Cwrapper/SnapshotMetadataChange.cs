namespace GooglePlayGames.Native.Cwrapper
{
    using System;
    using System.Runtime.InteropServices;

    internal static class SnapshotMetadataChange
    {
        [DllImport("gpg")]
        internal static extern UIntPtr SnapshotMetadataChange_Description(HandleRef self, [In, Out] char[] out_arg, UIntPtr out_size);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool SnapshotMetadataChange_DescriptionIsChanged(HandleRef self);
        [DllImport("gpg")]
        internal static extern void SnapshotMetadataChange_Dispose(HandleRef self);
        [DllImport("gpg")]
        internal static extern IntPtr SnapshotMetadataChange_Image(HandleRef self);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool SnapshotMetadataChange_ImageIsChanged(HandleRef self);
        [DllImport("gpg")]
        internal static extern ulong SnapshotMetadataChange_PlayedTime(HandleRef self);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool SnapshotMetadataChange_PlayedTimeIsChanged(HandleRef self);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool SnapshotMetadataChange_Valid(HandleRef self);
    }
}

