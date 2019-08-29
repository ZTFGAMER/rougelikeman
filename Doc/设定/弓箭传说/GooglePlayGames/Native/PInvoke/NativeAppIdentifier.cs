namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class NativeAppIdentifier : BaseReferenceHolder
    {
        internal NativeAppIdentifier(IntPtr pointer) : base(pointer)
        {
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            NearbyConnectionTypes.AppIdentifier_Dispose(selfPointer);
        }

        internal static NativeAppIdentifier FromString(string appId) => 
            new NativeAppIdentifier(NearbyUtils_ConstructAppIdentifier(appId));

        internal string Id() => 
            PInvokeUtilities.OutParamsToString((out_arg, out_size) => NearbyConnectionTypes.AppIdentifier_GetIdentifier(base.SelfPtr(), out_arg, out_size));

        [DllImport("gpg")]
        internal static extern IntPtr NearbyUtils_ConstructAppIdentifier(string appId);
    }
}

