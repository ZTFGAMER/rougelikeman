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
            VideoManager.VideoManager_GetCaptureCapabilitiesResponse_Dispose(base.SelfPtr());
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
            NativeVideoCapabilities.FromPointer(VideoManager.VideoManager_GetCaptureCapabilitiesResponse_GetVideoCapabilities(base.SelfPtr()));

        internal CommonErrorStatus.ResponseStatus GetStatus() => 
            VideoManager.VideoManager_GetCaptureCapabilitiesResponse_GetStatus(base.SelfPtr());

        internal bool RequestSucceeded() => 
            (this.GetStatus() > ~CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED);
    }
}

