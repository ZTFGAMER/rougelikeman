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
        private static CaptureOverlayStateListenerHelper.OnCaptureOverlayStateChangedCallback <>f__mg$cache0;

        internal CaptureOverlayStateListenerHelper(IntPtr selfPointer) : base(selfPointer)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            CaptureOverlayStateListenerHelper.CaptureOverlayStateListenerHelper_Dispose(selfPointer);
        }

        internal static CaptureOverlayStateListenerHelper Create() => 
            new CaptureOverlayStateListenerHelper(CaptureOverlayStateListenerHelper.CaptureOverlayStateListenerHelper_Construct());

        [MonoPInvokeCallback(typeof(CaptureOverlayStateListenerHelper.OnCaptureOverlayStateChangedCallback))]
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

        internal CaptureOverlayStateListenerHelper SetOnCaptureOverlayStateChangedCallback(Action<Types.VideoCaptureOverlayState> callback)
        {
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new CaptureOverlayStateListenerHelper.OnCaptureOverlayStateChangedCallback(CaptureOverlayStateListenerHelper.InternalOnCaptureOverlayStateChangedCallback);
            }
            CaptureOverlayStateListenerHelper.CaptureOverlayStateListenerHelper_SetOnCaptureOverlayStateChangedCallback(base.SelfPtr(), <>f__mg$cache0, Callbacks.ToIntPtr(callback));
            return this;
        }
    }
}

