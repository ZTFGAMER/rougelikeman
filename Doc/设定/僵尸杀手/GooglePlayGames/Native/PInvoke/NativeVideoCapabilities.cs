namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class NativeVideoCapabilities : BaseReferenceHolder
    {
        internal NativeVideoCapabilities(IntPtr selfPtr) : base(selfPtr)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            VideoCapabilities.VideoCapabilities_Dispose(selfPointer);
        }

        internal static NativeVideoCapabilities FromPointer(IntPtr pointer)
        {
            if (pointer.Equals(IntPtr.Zero))
            {
                return null;
            }
            return new NativeVideoCapabilities(pointer);
        }

        internal bool IsCameraSupported() => 
            VideoCapabilities.VideoCapabilities_IsCameraSupported(base.SelfPtr());

        internal bool IsMicSupported() => 
            VideoCapabilities.VideoCapabilities_IsMicSupported(base.SelfPtr());

        internal bool IsWriteStorageSupported() => 
            VideoCapabilities.VideoCapabilities_IsWriteStorageSupported(base.SelfPtr());

        internal bool SupportsCaptureMode(Types.VideoCaptureMode captureMode) => 
            VideoCapabilities.VideoCapabilities_SupportsCaptureMode(base.SelfPtr(), captureMode);

        internal bool SupportsQualityLevel(Types.VideoQualityLevel qualityLevel) => 
            VideoCapabilities.VideoCapabilities_SupportsQualityLevel(base.SelfPtr(), qualityLevel);
    }
}

