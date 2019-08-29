namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.BasicApi.SavedGame;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.InteropServices;

    internal class NativeSnapshotMetadataChange : BaseReferenceHolder
    {
        internal NativeSnapshotMetadataChange(IntPtr selfPointer) : base(selfPointer)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            SnapshotMetadataChange.SnapshotMetadataChange_Dispose(selfPointer);
        }

        internal static NativeSnapshotMetadataChange FromPointer(IntPtr pointer)
        {
            if (pointer.Equals(IntPtr.Zero))
            {
                return null;
            }
            return new NativeSnapshotMetadataChange(pointer);
        }

        internal class Builder : BaseReferenceHolder
        {
            internal Builder() : base(SnapshotMetadataChangeBuilder.SnapshotMetadataChange_Builder_Construct())
            {
            }

            internal NativeSnapshotMetadataChange Build() => 
                NativeSnapshotMetadataChange.FromPointer(SnapshotMetadataChangeBuilder.SnapshotMetadataChange_Builder_Create(base.SelfPtr()));

            protected override void CallDispose(HandleRef selfPointer)
            {
                SnapshotMetadataChangeBuilder.SnapshotMetadataChange_Builder_Dispose(selfPointer);
            }

            internal NativeSnapshotMetadataChange.Builder From(SavedGameMetadataUpdate update)
            {
                NativeSnapshotMetadataChange.Builder builder = this;
                if (update.IsDescriptionUpdated)
                {
                    builder = builder.SetDescription(update.UpdatedDescription);
                }
                if (update.IsCoverImageUpdated)
                {
                    builder = builder.SetCoverImageFromPngData(update.UpdatedPngCoverImage);
                }
                if (update.IsPlayedTimeUpdated)
                {
                    builder = builder.SetPlayedTime((ulong) update.UpdatedPlayedTime.Value.TotalMilliseconds);
                }
                return builder;
            }

            internal NativeSnapshotMetadataChange.Builder SetCoverImageFromPngData(byte[] pngData)
            {
                Misc.CheckNotNull<byte[]>(pngData);
                SnapshotMetadataChangeBuilder.SnapshotMetadataChange_Builder_SetCoverImageFromPngData(base.SelfPtr(), pngData, new UIntPtr((ulong) pngData.LongLength));
                return this;
            }

            internal NativeSnapshotMetadataChange.Builder SetDescription(string description)
            {
                SnapshotMetadataChangeBuilder.SnapshotMetadataChange_Builder_SetDescription(base.SelfPtr(), description);
                return this;
            }

            internal NativeSnapshotMetadataChange.Builder SetPlayedTime(ulong playedTime)
            {
                SnapshotMetadataChangeBuilder.SnapshotMetadataChange_Builder_SetPlayedTime(base.SelfPtr(), playedTime);
                return this;
            }
        }
    }
}

