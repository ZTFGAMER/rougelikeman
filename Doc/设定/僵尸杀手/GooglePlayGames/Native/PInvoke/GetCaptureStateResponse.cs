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
            GooglePlayGames.Native.Cwrapper.VideoManager.VideoManager_GetCaptureStateResponse_Dispose(base.SelfPtr());
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
            NativeVideoCaptureState.FromPointer(GooglePlayGames.Native.Cwrapper.VideoManager.VideoManager_GetCaptureStateResponse_GetVideoCaptureState(base.SelfPtr()));

        internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus GetStatus() => 
            GooglePlayGames.Native.Cwrapper.VideoManager.VideoManager_GetCaptureStateResponse_GetStatus(base.SelfPtr());

        internal bool RequestSucceeded() => 
            (this.GetStatus() > ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED);
    }
}

