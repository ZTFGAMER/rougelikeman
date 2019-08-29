namespace GooglePlayGames.Native.PInvoke
{
    using AOT;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal class VideoManager
    {
        private readonly GameServices mServices;
        [CompilerGenerated]
        private static Func<IntPtr, GetCaptureCapabilitiesResponse> <>f__mg$cache0;
        [CompilerGenerated]
        private static VideoManager.CaptureCapabilitiesCallback <>f__mg$cache1;
        [CompilerGenerated]
        private static Func<IntPtr, GetCaptureStateResponse> <>f__mg$cache2;
        [CompilerGenerated]
        private static VideoManager.CaptureStateCallback <>f__mg$cache3;
        [CompilerGenerated]
        private static Func<IntPtr, IsCaptureAvailableResponse> <>f__mg$cache4;
        [CompilerGenerated]
        private static VideoManager.IsCaptureAvailableCallback <>f__mg$cache5;

        internal VideoManager(GameServices services)
        {
            this.mServices = Misc.CheckNotNull<GameServices>(services);
        }

        internal void GetCaptureCapabilities(Action<GetCaptureCapabilitiesResponse> callback)
        {
            if (<>f__mg$cache1 == null)
            {
                <>f__mg$cache1 = new VideoManager.CaptureCapabilitiesCallback(VideoManager.InternalCaptureCapabilitiesCallback);
            }
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new Func<IntPtr, GetCaptureCapabilitiesResponse>(GetCaptureCapabilitiesResponse.FromPointer);
            }
            VideoManager.VideoManager_GetCaptureCapabilities(this.mServices.AsHandle(), <>f__mg$cache1, Callbacks.ToIntPtr<GetCaptureCapabilitiesResponse>(callback, <>f__mg$cache0));
        }

        internal void GetCaptureState(Action<GetCaptureStateResponse> callback)
        {
            if (<>f__mg$cache3 == null)
            {
                <>f__mg$cache3 = new VideoManager.CaptureStateCallback(VideoManager.InternalCaptureStateCallback);
            }
            if (<>f__mg$cache2 == null)
            {
                <>f__mg$cache2 = new Func<IntPtr, GetCaptureStateResponse>(GetCaptureStateResponse.FromPointer);
            }
            VideoManager.VideoManager_GetCaptureState(this.mServices.AsHandle(), <>f__mg$cache3, Callbacks.ToIntPtr<GetCaptureStateResponse>(callback, <>f__mg$cache2));
        }

        [MonoPInvokeCallback(typeof(VideoManager.CaptureCapabilitiesCallback))]
        internal static void InternalCaptureCapabilitiesCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("VideoManager#CaptureCapabilitiesCallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(VideoManager.CaptureStateCallback))]
        internal static void InternalCaptureStateCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("VideoManager#CaptureStateCallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(VideoManager.IsCaptureAvailableCallback))]
        internal static void InternalIsCaptureAvailableCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("VideoManager#IsCaptureAvailableCallback", Callbacks.Type.Temporary, response, data);
        }

        internal void IsCaptureAvailable(Types.VideoCaptureMode captureMode, Action<IsCaptureAvailableResponse> callback)
        {
            if (<>f__mg$cache5 == null)
            {
                <>f__mg$cache5 = new VideoManager.IsCaptureAvailableCallback(VideoManager.InternalIsCaptureAvailableCallback);
            }
            if (<>f__mg$cache4 == null)
            {
                <>f__mg$cache4 = new Func<IntPtr, IsCaptureAvailableResponse>(IsCaptureAvailableResponse.FromPointer);
            }
            VideoManager.VideoManager_IsCaptureAvailable(this.mServices.AsHandle(), captureMode, <>f__mg$cache5, Callbacks.ToIntPtr<IsCaptureAvailableResponse>(callback, <>f__mg$cache4));
        }

        internal bool IsCaptureSupported() => 
            VideoManager.VideoManager_IsCaptureSupported(this.mServices.AsHandle());

        internal void RegisterCaptureOverlayStateChangedListener(CaptureOverlayStateListenerHelper helper)
        {
            VideoManager.VideoManager_RegisterCaptureOverlayStateChangedListener(this.mServices.AsHandle(), helper.AsPointer());
        }

        internal void ShowCaptureOverlay()
        {
            VideoManager.VideoManager_ShowCaptureOverlay(this.mServices.AsHandle());
        }

        internal void UnregisterCaptureOverlayStateChangedListener()
        {
            VideoManager.VideoManager_UnregisterCaptureOverlayStateChangedListener(this.mServices.AsHandle());
        }

        internal int NumCaptureModes =>
            2;

        internal int NumQualityLevels =>
            4;
    }
}

