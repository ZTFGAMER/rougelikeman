namespace GooglePlayGames.Native.PInvoke
{
    using AOT;
    using GooglePlayGames.Native.Cwrapper;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    internal class CaptureOverlayStateListenerHelper : BaseReferenceHolder
    {
        [CompilerGenerated]
        private static GooglePlayGames.Native.Cwrapper.CaptureOverlayStateListenerHelper.OnCaptureOverlayStateChangedCallback <>f__mg$cache0;

        internal CaptureOverlayStateListenerHelper(IntPtr selfPointer) : base(selfPointer)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            GooglePlayGames.Native.Cwrapper.CaptureOverlayStateListenerHelper.CaptureOverlayStateListenerHelper_Dispose(selfPointer);
        }

        internal static GooglePlayGames.Native.PInvoke.CaptureOverlayStateListenerHelper Create() => 
            new GooglePlayGames.Native.PInvoke.CaptureOverlayStateListenerHelper(GooglePlayGames.Native.Cwrapper.CaptureOverlayStateListenerHelper.CaptureOverlayStateListenerHelper_Construct());

        [MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.CaptureOverlayStateListenerHelper.OnCaptureOverlayStateChangedCallback))]
        internal static void InternalOnCaptureOverlayStateChangedCallback(Types.VideoCaptureOverlayState response, IntPtr data)
        {
            Action<Types.VideoCaptureOverlayState> action = Callbacks.IntPtrToPermanentCallback<Action<Types.VideoCaptureOverlayState>>(data);
            try
            {
                action(response);
            }
            catch (Exception exception)
            {
                Logger.e("Error encountered executing InternalOnCaptureOverlayStateChangedCallback. Smothering to avoid passing exception into Native: " + exception);
            }
        }

        internal GooglePlayGames.Native.PInvoke.CaptureOverlayStateListenerHelper SetOnCaptureOverlayStateChangedCallback(Action<Types.VideoCaptureOverlayState> callback)
        {
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new GooglePlayGames.Native.Cwrapper.CaptureOverlayStateListenerHelper.OnCaptureOverlayStateChangedCallback(GooglePlayGames.Native.PInvoke.CaptureOverlayStateListenerHelper.InternalOnCaptureOverlayStateChangedCallback);
            }
            GooglePlayGames.Native.Cwrapper.CaptureOverlayStateListenerHelper.CaptureOverlayStateListenerHelper_SetOnCaptureOverlayStateChangedCallback(base.SelfPtr(), <>f__mg$cache0, Callbacks.ToIntPtr(callback));
            return this;
        }
    }
}

