namespace GooglePlayGames.BasicApi.Video
{
    using GooglePlayGames.BasicApi;
    using System;

    public interface CaptureOverlayStateListener
    {
        void OnCaptureOverlayStateChanged(VideoCaptureOverlayState overlayState);
    }
}

