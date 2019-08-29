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
        private readonly GooglePlayGames.Native.PInvoke.GameServices mServices;
        [CompilerGenerated]
        private static Func<IntPtr, GetCaptureCapabilitiesResponse> <>f__mg$cache0;
        [CompilerGenerated]
        private static GooglePlayGames.Native.Cwrapper.VideoManager.CaptureCapabilitiesCallback <>f__mg$cache1;
        [CompilerGenerated]
        private static Func<IntPtr, GetCaptureStateResponse> <>f__mg$cache2;
        [CompilerGenerated]
        private static GooglePlayGames.Native.Cwrapper.VideoManager.CaptureStateCallback <>f__mg$cache3;
        [CompilerGenerated]
        private static Func<IntPtr, IsCaptureAvailableResponse> <>f__mg$cache4;
        [CompilerGenerated]
        private static GooglePlayGames.Native.Cwrapper.VideoManager.IsCaptureAvailableCallback <>f__mg$cache5;

        internal VideoManager(GooglePlayGames.Native.PInvoke.GameServices services)
        {
            this.mServices = Misc.CheckNotNull<GooglePlayGames.Native.PInvoke.GameServices>(services);
        }

        internal void GetCaptureCapabilities(Action<GetCaptureCapabilitiesResponse> callback)
        {
            if (<>f__mg$cache1 == null)
            {
                <>f__mg$cache1 = new GooglePlayGames.Native.Cwrapper.VideoManager.CaptureCapabilitiesCallback(GooglePlayGames.Native.PInvoke.VideoManager.InternalCaptureCapabilitiesCallback);
            }
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new Func<IntPtr, GetCaptureCapabilitiesResponse>(GetCaptureCapabilitiesResponse.FromPointer);
            }
            GooglePlayGames.Native.Cwrapper.VideoManager.VideoManager_GetCaptureCapabilities(this.mServices.AsHandle(), <>f__mg$cache1, Callbacks.ToIntPtr<GetCaptureCapabilitiesResponse>(callback, <>f__mg$cache0));
        }

        internal void GetCaptureState(Action<GetCaptureStateResponse> callback)
        {
            if (<>f__mg$cache3 == null)
            {
                <>f__mg$cache3 = new GooglePlayGames.Native.Cwrapper.VideoManager.CaptureStateCallback(GooglePlayGames.Native.PInvoke.VideoManager.InternalCaptureStateCallback);
            }
            if (<>f__mg$cache2 == null)
            {
                <>f__mg$cache2 = new Func<IntPtr, GetCaptureStateResponse>(GetCaptureStateResponse.FromPointer);
            }
            GooglePlayGames.Native.Cwrapper.VideoManager.VideoManager_GetCaptureState(this.mServices.AsHandle(), <>f__mg$cache3, Callbacks.ToIntPtr<GetCaptureStateResponse>(callback, <>f__mg$cache2));
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.VideoManager.CaptureCapabilitiesCallback))]
        internal static void InternalCaptureCapabilitiesCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("VideoManager#CaptureCapabilitiesCallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.VideoManager.CaptureStateCallback))]
        internal static void InternalCaptureStateCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("VideoManager#CaptureStateCallback", Callbacks.Type.Temporary, response, data);
        }

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.VideoManager.IsCaptureAvailableCallback))]
        internal static void InternalIsCaptureAvailableCallback(IntPtr response, IntPtr data)
        {
            Callbacks.PerformInternalCallback("VideoManager#IsCaptureAvailableCallback", Callbacks.Type.Temporary, response, data);
        }

        internal void IsCaptureAvailable(Types.VideoCaptureMode captureMode, Action<IsCaptureAvailableResponse> callback)
        {
            if (<>f__mg$cache5 == null)
            {
                <>f__mg$cache5 = new GooglePlayGames.Native.Cwrapper.VideoManager.IsCaptureAvailableCallback(GooglePlayGames.Native.PInvoke.VideoManager.InternalIsCaptureAvailableCallback);
            }
            if (<>f__mg$cache4 == null)
            {
                <>f__mg$cache4 = new Func<IntPtr, IsCaptureAvailableResponse>(IsCaptureAvailableResponse.FromPointer);
            }
            GooglePlayGames.Native.Cwrapper.VideoManager.VideoManager_IsCaptureAvailable(this.mServices.AsHandle(), captureMode, <>f__mg$cache5, Callbacks.ToIntPtr<IsCaptureAvailableResponse>(callback, <>f__mg$cache4));
        }

        internal bool IsCaptureSupported() => 
            GooglePlayGames.Native.Cwrapper.VideoManager.VideoManager_IsCaptureSupported(this.mServices.AsHandle());

        internal void RegisterCaptureOverlayStateChangedListener(GooglePlayGames.Native.PInvoke.CaptureOverlayStateListenerHelper helper)
        {
            GooglePlayGames.Native.Cwrapper.VideoManager.VideoManager_RegisterCaptureOverlayStateChangedListener(this.mServices.AsHandle(), helper.AsPointer());
        }

        internal void ShowCaptureOverlay()
        {
            GooglePlayGames.Native.Cwrapper.VideoManager.VideoManager_ShowCaptureOverlay(this.mServices.AsHandle());
        }

        internal void UnregisterCaptureOverlayStateChangedListener()
        {
            GooglePlayGames.Native.Cwrapper.VideoManager.VideoManager_UnregisterCaptureOverlayStateChangedListener(this.mServices.AsHandle());
        }

        internal int NumCaptureModes =>
            2;

        internal int NumQualityLevels =>
            4;
    }
}

