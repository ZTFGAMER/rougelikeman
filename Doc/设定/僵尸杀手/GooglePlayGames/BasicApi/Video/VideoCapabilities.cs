namespace GooglePlayGames.BasicApi.Video
{
    using GooglePlayGames.BasicApi;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class VideoCapabilities
    {
        private bool mIsCameraSupported;
        private bool mIsMicSupported;
        private bool mIsWriteStorageSupported;
        private bool[] mCaptureModesSupported;
        private bool[] mQualityLevelsSupported;
        [CompilerGenerated]
        private static Func<bool, string> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<bool, string> <>f__am$cache1;

        internal VideoCapabilities(bool isCameraSupported, bool isMicSupported, bool isWriteStorageSupported, bool[] captureModesSupported, bool[] qualityLevelsSupported)
        {
            this.mIsCameraSupported = isCameraSupported;
            this.mIsMicSupported = isMicSupported;
            this.mIsWriteStorageSupported = isWriteStorageSupported;
            this.mCaptureModesSupported = captureModesSupported;
            this.mQualityLevelsSupported = qualityLevelsSupported;
        }

        public bool SupportsCaptureMode(VideoCaptureMode captureMode)
        {
            if (captureMode != VideoCaptureMode.Unknown)
            {
                return this.mCaptureModesSupported[(int) captureMode];
            }
            Logger.w("SupportsCaptureMode called with an unknown captureMode.");
            return false;
        }

        public bool SupportsQualityLevel(VideoQualityLevel qualityLevel)
        {
            if (qualityLevel != VideoQualityLevel.Unknown)
            {
                return this.mQualityLevelsSupported[(int) qualityLevel];
            }
            Logger.w("SupportsCaptureMode called with an unknown qualityLevel.");
            return false;
        }

        public override string ToString()
        {
            object[] args = new object[5];
            args[0] = this.mIsCameraSupported;
            args[1] = this.mIsMicSupported;
            args[2] = this.mIsWriteStorageSupported;
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = p => p.ToString();
            }
            args[3] = string.Join(",", Enumerable.Select<bool, string>(this.mCaptureModesSupported, <>f__am$cache0).ToArray<string>());
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = p => p.ToString();
            }
            args[4] = string.Join(",", Enumerable.Select<bool, string>(this.mQualityLevelsSupported, <>f__am$cache1).ToArray<string>());
            return string.Format("[VideoCapabilities: mIsCameraSupported={0}, mIsMicSupported={1}, mIsWriteStorageSupported={2}, mCaptureModesSupported={3}, mQualityLevelsSupported={4}]", args);
        }

        public bool IsCameraSupported =>
            this.mIsCameraSupported;

        public bool IsMicSupported =>
            this.mIsMicSupported;

        public bool IsWriteStorageSupported =>
            this.mIsWriteStorageSupported;
    }
}

