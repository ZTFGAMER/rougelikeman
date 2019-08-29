namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class NativeVideoCaptureState : BaseReferenceHolder
    {
        internal NativeVideoCaptureState(IntPtr selfPtr) : base(selfPtr)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            VideoCaptureState.VideoCaptureState_Dispose(selfPointer);
        }

        internal Types.VideoCaptureMode CaptureMode() => 
            VideoCaptureState.VideoCaptureState_CaptureMode(base.SelfPtr());

        internal static NativeVideoCaptureState FromPointer(IntPtr pointer)
        {
            if (pointer.Equals(IntPtr.Zero))
            {
                return null;
            }
            return new NativeVideoCaptureState(pointer);
        }

        internal bool IsCapturing() => 
            VideoCaptureState.VideoCaptureState_IsCapturing(base.SelfPtr());

        internal bool IsOverlayVisible() => 
            VideoCaptureState.VideoCaptureState_IsOverlayVisible(base.SelfPtr());

        internal bool IsPaused() => 
            VideoCaptureState.VideoCaptureState_IsPaused(base.SelfPtr());

        internal Types.VideoQualityLevel QualityLevel() => 
            VideoCaptureState.VideoCaptureState_QualityLevel(base.SelfPtr());
    }
}

