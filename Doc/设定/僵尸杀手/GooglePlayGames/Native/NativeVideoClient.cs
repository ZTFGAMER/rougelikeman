namespace GooglePlayGames.Native
{
    using GooglePlayGames.BasicApi;
    using GooglePlayGames.BasicApi.Video;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.Native.PInvoke;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.CompilerServices;

    internal class NativeVideoClient : IVideoClient
    {
        private readonly GooglePlayGames.Native.PInvoke.VideoManager mManager;

        internal NativeVideoClient(GooglePlayGames.Native.PInvoke.VideoManager manager)
        {
            this.mManager = Misc.CheckNotNull<GooglePlayGames.Native.PInvoke.VideoManager>(manager);
        }

        private GooglePlayGames.BasicApi.Video.VideoCapabilities FromNativeVideoCapabilities(NativeVideoCapabilities capabilities)
        {
            bool[] captureModesSupported = new bool[this.mManager.NumCaptureModes];
            captureModesSupported[0] = capabilities.SupportsCaptureMode(Types.VideoCaptureMode.FILE);
            captureModesSupported[1] = capabilities.SupportsCaptureMode(Types.VideoCaptureMode.STREAM);
            bool[] qualityLevelsSupported = new bool[this.mManager.NumQualityLevels];
            qualityLevelsSupported[0] = capabilities.SupportsQualityLevel(Types.VideoQualityLevel.SD);
            qualityLevelsSupported[1] = capabilities.SupportsQualityLevel(Types.VideoQualityLevel.HD);
            qualityLevelsSupported[2] = capabilities.SupportsQualityLevel(Types.VideoQualityLevel.XHD);
            qualityLevelsSupported[3] = capabilities.SupportsQualityLevel(Types.VideoQualityLevel.FULLHD);
            return new GooglePlayGames.BasicApi.Video.VideoCapabilities(capabilities.IsCameraSupported(), capabilities.IsMicSupported(), capabilities.IsWriteStorageSupported(), captureModesSupported, qualityLevelsSupported);
        }

        private GooglePlayGames.BasicApi.Video.VideoCaptureState FromNativeVideoCaptureState(NativeVideoCaptureState captureState) => 
            new GooglePlayGames.BasicApi.Video.VideoCaptureState(captureState.IsCapturing(), ConversionUtils.ConvertNativeVideoCaptureMode(captureState.CaptureMode()), ConversionUtils.ConvertNativeVideoQualityLevel(captureState.QualityLevel()), captureState.IsOverlayVisible(), captureState.IsPaused());

        public void GetCaptureCapabilities(Action<GooglePlayGames.BasicApi.ResponseStatus, GooglePlayGames.BasicApi.Video.VideoCapabilities> callback)
        {
            <GetCaptureCapabilities>c__AnonStorey0 storey = new <GetCaptureCapabilities>c__AnonStorey0 {
                callback = callback,
                $this = this
            };
            Misc.CheckNotNull<Action<GooglePlayGames.BasicApi.ResponseStatus, GooglePlayGames.BasicApi.Video.VideoCapabilities>>(storey.callback);
            storey.callback = CallbackUtils.ToOnGameThread<GooglePlayGames.BasicApi.ResponseStatus, GooglePlayGames.BasicApi.Video.VideoCapabilities>(storey.callback);
            this.mManager.GetCaptureCapabilities(new Action<GetCaptureCapabilitiesResponse>(storey.<>m__0));
        }

        public void GetCaptureState(Action<GooglePlayGames.BasicApi.ResponseStatus, GooglePlayGames.BasicApi.Video.VideoCaptureState> callback)
        {
            <GetCaptureState>c__AnonStorey1 storey = new <GetCaptureState>c__AnonStorey1 {
                callback = callback,
                $this = this
            };
            Misc.CheckNotNull<Action<GooglePlayGames.BasicApi.ResponseStatus, GooglePlayGames.BasicApi.Video.VideoCaptureState>>(storey.callback);
            storey.callback = CallbackUtils.ToOnGameThread<GooglePlayGames.BasicApi.ResponseStatus, GooglePlayGames.BasicApi.Video.VideoCaptureState>(storey.callback);
            this.mManager.GetCaptureState(new Action<GetCaptureStateResponse>(storey.<>m__0));
        }

        public void IsCaptureAvailable(GooglePlayGames.BasicApi.VideoCaptureMode captureMode, Action<GooglePlayGames.BasicApi.ResponseStatus, bool> callback)
        {
            <IsCaptureAvailable>c__AnonStorey2 storey = new <IsCaptureAvailable>c__AnonStorey2 {
                callback = callback
            };
            Misc.CheckNotNull<Action<GooglePlayGames.BasicApi.ResponseStatus, bool>>(storey.callback);
            storey.callback = CallbackUtils.ToOnGameThread<GooglePlayGames.BasicApi.ResponseStatus, bool>(storey.callback);
            this.mManager.IsCaptureAvailable(ConversionUtils.ConvertVideoCaptureMode(captureMode), new Action<IsCaptureAvailableResponse>(storey.<>m__0));
        }

        public bool IsCaptureSupported() => 
            this.mManager.IsCaptureSupported();

        public void RegisterCaptureOverlayStateChangedListener(CaptureOverlayStateListener listener)
        {
            <RegisterCaptureOverlayStateChangedListener>c__AnonStorey3 storey = new <RegisterCaptureOverlayStateChangedListener>c__AnonStorey3 {
                listener = listener
            };
            Misc.CheckNotNull<CaptureOverlayStateListener>(storey.listener);
            GooglePlayGames.Native.PInvoke.CaptureOverlayStateListenerHelper helper = GooglePlayGames.Native.PInvoke.CaptureOverlayStateListenerHelper.Create().SetOnCaptureOverlayStateChangedCallback(new Action<Types.VideoCaptureOverlayState>(storey.<>m__0));
            this.mManager.RegisterCaptureOverlayStateChangedListener(helper);
        }

        public void ShowCaptureOverlay()
        {
            this.mManager.ShowCaptureOverlay();
        }

        public void UnregisterCaptureOverlayStateChangedListener()
        {
            this.mManager.UnregisterCaptureOverlayStateChangedListener();
        }

        [CompilerGenerated]
        private sealed class <GetCaptureCapabilities>c__AnonStorey0
        {
            internal Action<GooglePlayGames.BasicApi.ResponseStatus, GooglePlayGames.BasicApi.Video.VideoCapabilities> callback;
            internal NativeVideoClient $this;

            internal void <>m__0(GetCaptureCapabilitiesResponse response)
            {
                GooglePlayGames.BasicApi.ResponseStatus status = ConversionUtils.ConvertResponseStatus(response.GetStatus());
                if (!response.RequestSucceeded())
                {
                    this.callback(status, null);
                }
                else
                {
                    this.callback(status, this.$this.FromNativeVideoCapabilities(response.GetData()));
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetCaptureState>c__AnonStorey1
        {
            internal Action<GooglePlayGames.BasicApi.ResponseStatus, GooglePlayGames.BasicApi.Video.VideoCaptureState> callback;
            internal NativeVideoClient $this;

            internal void <>m__0(GetCaptureStateResponse response)
            {
                GooglePlayGames.BasicApi.ResponseStatus status = ConversionUtils.ConvertResponseStatus(response.GetStatus());
                if (!response.RequestSucceeded())
                {
                    this.callback(status, null);
                }
                else
                {
                    this.callback(status, this.$this.FromNativeVideoCaptureState(response.GetData()));
                }
            }
        }

        [CompilerGenerated]
        private sealed class <IsCaptureAvailable>c__AnonStorey2
        {
            internal Action<GooglePlayGames.BasicApi.ResponseStatus, bool> callback;

            internal void <>m__0(IsCaptureAvailableResponse response)
            {
                GooglePlayGames.BasicApi.ResponseStatus status = ConversionUtils.ConvertResponseStatus(response.GetStatus());
                if (!response.RequestSucceeded())
                {
                    this.callback(status, false);
                }
                else
                {
                    this.callback(status, response.IsCaptureAvailable());
                }
            }
        }

        [CompilerGenerated]
        private sealed class <RegisterCaptureOverlayStateChangedListener>c__AnonStorey3
        {
            internal CaptureOverlayStateListener listener;

            internal void <>m__0(Types.VideoCaptureOverlayState response)
            {
                this.listener.OnCaptureOverlayStateChanged(ConversionUtils.ConvertNativeVideoCaptureOverlayState(response));
            }
        }
    }
}

