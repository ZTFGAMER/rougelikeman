namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class GetCaptureStateResponse : BaseReferenceHolder
    {
        internal GetCaptureStateResponse(IntPtr selfPointer) : base(selfPointer)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            VideoManager.VideoManager_GetCaptureStateResponse_Dispose(base.SelfPtr());
        }

        internal static GetCaptureStateResponse FromPointer(IntPtr pointer)
        {
            if (pointer.Equals(IntPtr.Zero))
            {
                return null;
            }
            return new GetCaptureStateResponse(pointer);
        }

        internal NativeVideoCaptureState GetData() => 
            NativeVideoCaptureState.FromPointer(VideoManager.VideoManager_GetCaptureStateResponse_GetVideoCaptureState(base.SelfPtr()));

        internal CommonErrorStatus.ResponseStatus GetStatus() => 
            VideoManager.VideoManager_GetCaptureStateResponse_GetStatus(base.SelfPtr());

        internal bool RequestSucceeded() => 
            (this.GetStatus() > ~CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED);
    }
}

