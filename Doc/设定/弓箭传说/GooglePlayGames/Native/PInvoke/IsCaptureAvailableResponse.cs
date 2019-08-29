namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class IsCaptureAvailableResponse : BaseReferenceHolder
    {
        internal IsCaptureAvailableResponse(IntPtr selfPointer) : base(selfPointer)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            VideoManager.VideoManager_IsCaptureAvailableResponse_Dispose(selfPointer);
        }

        internal static IsCaptureAvailableResponse FromPointer(IntPtr pointer)
        {
            if (pointer.Equals(IntPtr.Zero))
            {
                return null;
            }
            return new IsCaptureAvailableResponse(pointer);
        }

        internal CommonErrorStatus.ResponseStatus GetStatus() => 
            VideoManager.VideoManager_IsCaptureAvailableResponse_GetStatus(base.SelfPtr());

        internal bool IsCaptureAvailable() => 
            VideoManager.VideoManager_IsCaptureAvailableResponse_GetIsCaptureAvailable(base.SelfPtr());

        internal bool RequestSucceeded() => 
            (this.GetStatus() > ~CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED);
    }
}

