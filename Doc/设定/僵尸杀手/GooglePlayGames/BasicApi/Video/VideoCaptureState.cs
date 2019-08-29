namespace GooglePlayGames.BasicApi.Video
{
    using GooglePlayGames.BasicApi;
    using System;

    public class VideoCaptureState
    {
        private bool mIsCapturing;
        private VideoCaptureMode mCaptureMode;
        private VideoQualityLevel mQualityLevel;
        private bool mIsOverlayVisible;
        private bool mIsPaused;

        internal VideoCaptureState(bool isCapturing, VideoCaptureMode captureMode, VideoQualityLevel qualityLevel, bool isOverlayVisible, bool isPaused)
        {
            this.mIsCapturing = isCapturing;
            this.mCaptureMode = captureMode;
            this.mQualityLevel = qualityLevel;
            this.mIsOverlayVisible = isOverlayVisible;
            this.mIsPaused = isPaused;
        }

        public override string ToString() => 
            $"[VideoCaptureState: mIsCapturing={this.mIsCapturing}, mCaptureMode={this.mCaptureMode.ToString()}, mQualityLevel={this.mQualityLevel.ToString()}, mIsOverlayVisible={this.mIsOverlayVisible}, mIsPaused={this.mIsPaused}]";

        public bool IsCapturing =>
            this.mIsCapturing;

        public VideoCaptureMode CaptureMode =>
            this.mCaptureMode;

        public VideoQualityLevel QualityLevel =>
            this.mQualityLevel;

        public bool IsOverlayVisible =>
            this.mIsOverlayVisible;

        public bool IsPaused =>
            this.mIsPaused;
    }
}

