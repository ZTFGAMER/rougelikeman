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
            GooglePlayGames.Native.Cwrapper.VideoManager.VideoManager_IsCaptureAvailableResponse_Dispose(selfPointer);
        }

        internal static IsCaptureAvailableResponse FromPointer(IntPtr pointer)
        {
            if (pointer.Equals(IntPtr.Zero))
            {
                return null;
            }
            return new IsCaptureAvailableResponse(pointer);
        }

        internal GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus GetStatus() => 
            GooglePlayGames.Native.Cwrapper.VideoManager.VideoManager_IsCaptureAvailableResponse_GetStatus(base.SelfPtr());

        internal bool IsCaptureAvailable() => 
            GooglePlayGames.Native.Cwrapper.VideoManager.VideoManager_IsCaptureAvailableResponse_GetIsCaptureAvailable(base.SelfPtr());

        internal bool RequestSucceeded() => 
            (this.GetStatus() > ~GooglePlayGames.Native.Cwrapper.CommonErrorStatus.ResponseStatus.ERROR_LICENSE_CHECK_FAILED);
    }
}

