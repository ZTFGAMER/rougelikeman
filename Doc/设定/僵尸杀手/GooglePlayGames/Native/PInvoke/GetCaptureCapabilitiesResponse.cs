namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class GetCaptureCapabilitiesResponse : BaseReferenceHolder
    {
        internal GetCaptureCapabilitiesResponse(IntPtr selfPointer) : base(selfPointer)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            GooglePlayGames.Native.Cwrapper.VideoManager.VideoManager_GetCaptureCapabilitiesResponse_Dispose(base.SelfPtr());
        }

        internal static GetCaptureCapabilitiesResponse FromPointer(IntPtr pointer)
        {
            if (pointer.Equals(IntPtr.Zero))
            {
                return null;
            }
            return new GetCaptureCapabilitiesResponse(pointer);
        }

        internal NativeVideoCapabilities GetData() => 
            NativeVideoCapabilities.FromPointer(GooglePlayGames.Native.Cwrapper.VideoManager.VideoManager_GetCaptureCapabilitiesResponse_GetVideoCapabilities(base.SelfPtr()));

        internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus GetStatus() => 
            GooglePlayGames.Native.Cwrapper.VideoManager.VideoManager_GetCaptureCapabilitiesResponse_GetStatus(base.SelfPtr());

        internal bool RequestSucceeded() => 
            (this.GetStatus() > ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED);
    }
}

