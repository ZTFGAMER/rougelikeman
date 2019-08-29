namespace GooglePlayGames.BasicApi.Video
{
    using GooglePlayGames.BasicApi;
    using System;

    public interface IVideoClient
    {
        void GetCaptureCapabilities(Action<ResponseStatus, VideoCapabilities> callback);
        void GetCaptureState(Action<ResponseStatus, VideoCaptureState> callback);
        void IsCaptureAvailable(VideoCaptureMode captureMode, Action<ResponseStatus, bool> callback);
        bool IsCaptureSupported();
        void RegisterCaptureOverlayStateChangedListener(CaptureOverlayStateListener listener);
        void ShowCaptureOverlay();
        void UnregisterCaptureOverlayStateChangedListener();
    }
}

