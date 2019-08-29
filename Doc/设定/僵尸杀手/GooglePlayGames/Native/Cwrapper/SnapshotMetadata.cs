namespace GooglePlayGames.Native.Cwrapper
{
    using System;
    using System.Runtime.InteropServices;

    internal static class SnapshotMetadata
    {
        [DllImport("gpg")]
        internal static extern UIntPtr SnapshotMetadata_CoverImageURL(HandleRef self, [In, Out] byte[] out_arg, UIntPtr out_size);
        [DllImport("gpg")]
        internal static extern UIntPtr SnapshotMetadata_Description(HandleRef self, [In, Out] byte[] out_arg, UIntPtr out_size);
        [DllImport("gpg")]
        internal static extern void SnapshotMetadata_Dispose(HandleRef self);
        [DllImport("gpg")]
        internal static extern UIntPtr SnapshotMetadata_FileName(HandleRef self, [In, Out] byte[] out_arg, UIntPtr out_size);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool SnapshotMetadata_IsOpen(HandleRef self);
        [DllImport("gpg")]
        internal static extern long SnapshotMetadata_LastModifiedTime(HandleRef self);
        [DllImport("gpg")]
        internal static extern long SnapshotMetadata_PlayedTime(HandleRef self);
        [return: MarshalAs(UnmanagedType.I1)]
        [DllImport("gpg")]
        internal static extern bool SnapshotMetadata_Valid(HandleRef self);
    }
}

