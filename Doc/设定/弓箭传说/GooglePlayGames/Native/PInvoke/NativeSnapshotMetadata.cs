namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.BasicApi.SavedGame;
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class NativeSnapshotMetadata : BaseReferenceHolder, ISavedGameMetadata
    {
        internal NativeSnapshotMetadata(IntPtr selfPointer) : base(selfPointer)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            SnapshotMetadata.SnapshotMetadata_Dispose(base.SelfPtr());
        }

        public override string ToString()
        {
            if (base.IsDisposed())
            {
                return "[NativeSnapshotMetadata: DELETED]";
            }
            return $"[NativeSnapshotMetadata: IsOpen={this.IsOpen}, Filename={this.Filename}, Description={this.Description}, CoverImageUrl={this.CoverImageURL}, TotalTimePlayed={this.TotalTimePlayed}, LastModifiedTimestamp={this.LastModifiedTimestamp}]";
        }

        public bool IsOpen =>
            SnapshotMetadata.SnapshotMetadata_IsOpen(base.SelfPtr());

        public string Filename =>
            PInvokeUtilities.OutParamsToString((out_string, out_size) => SnapshotMetadata.SnapshotMetadata_FileName(base.SelfPtr(), out_string, out_size));

        public string Description =>
            PInvokeUtilities.OutParamsToString((out_string, out_size) => SnapshotMetadata.SnapshotMetadata_Description(base.SelfPtr(), out_string, out_size));

        public string CoverImageURL =>
            PInvokeUtilities.OutParamsToString((out_string, out_size) => SnapshotMetadata.SnapshotMetadata_CoverImageURL(base.SelfPtr(), out_string, out_size));

        public TimeSpan TotalTimePlayed
        {
            get
            {
                long num = SnapshotMetadata.SnapshotMetadata_PlayedTime(base.SelfPtr());
                if (num < 0L)
                {
                    return TimeSpan.FromMilliseconds(0.0);
                }
                return TimeSpan.FromMilliseconds((double) num);
            }
        }

        public DateTime LastModifiedTimestamp =>
            PInvokeUtilities.FromMillisSinceUnixEpoch(SnapshotMetadata.SnapshotMetadata_LastModifiedTime(base.SelfPtr()));
    }
}

