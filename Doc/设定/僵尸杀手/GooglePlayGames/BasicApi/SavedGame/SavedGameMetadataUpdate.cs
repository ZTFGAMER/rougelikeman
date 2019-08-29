namespace GooglePlayGames.BasicApi.SavedGame
{
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct SavedGameMetadataUpdate
    {
        private readonly bool mDescriptionUpdated;
        private readonly string mNewDescription;
        private readonly bool mCoverImageUpdated;
        private readonly byte[] mNewPngCoverImage;
        private readonly TimeSpan? mNewPlayedTime;
        private SavedGameMetadataUpdate(Builder builder)
        {
            this.mDescriptionUpdated = builder.mDescriptionUpdated;
            this.mNewDescription = builder.mNewDescription;
            this.mCoverImageUpdated = builder.mCoverImageUpdated;
            this.mNewPngCoverImage = builder.mNewPngCoverImage;
            this.mNewPlayedTime = builder.mNewPlayedTime;
        }

        public bool IsDescriptionUpdated =>
            this.mDescriptionUpdated;
        public string UpdatedDescription =>
            this.mNewDescription;
        public bool IsCoverImageUpdated =>
            this.mCoverImageUpdated;
        public byte[] UpdatedPngCoverImage =>
            this.mNewPngCoverImage;
        public bool IsPlayedTimeUpdated =>
            this.mNewPlayedTime.HasValue;
        public TimeSpan? UpdatedPlayedTime =>
            this.mNewPlayedTime;
        [StructLayout(LayoutKind.Sequential)]
        public struct Builder
        {
            internal bool mDescriptionUpdated;
            internal string mNewDescription;
            internal bool mCoverImageUpdated;
            internal byte[] mNewPngCoverImage;
            internal TimeSpan? mNewPlayedTime;
            public SavedGameMetadataUpdate.Builder WithUpdatedDescription(string description)
            {
                this.mNewDescription = Misc.CheckNotNull<string>(description);
                this.mDescriptionUpdated = true;
                return this;
            }

            public SavedGameMetadataUpdate.Builder WithUpdatedPngCoverImage(byte[] newPngCoverImage)
            {
                this.mCoverImageUpdated = true;
                this.mNewPngCoverImage = newPngCoverImage;
                return this;
            }

            public SavedGameMetadataUpdate.Builder WithUpdatedPlayedTime(TimeSpan newPlayedTime)
            {
                if (newPlayedTime.TotalMilliseconds > 1.8446744073709552E+19)
                {
                    throw new InvalidOperationException("Timespans longer than ulong.MaxValue milliseconds are not allowed");
                }
                this.mNewPlayedTime = new TimeSpan?(newPlayedTime);
                return this;
            }

            public SavedGameMetadataUpdate Build() => 
                new SavedGameMetadataUpdate(this);
        }
    }
}

