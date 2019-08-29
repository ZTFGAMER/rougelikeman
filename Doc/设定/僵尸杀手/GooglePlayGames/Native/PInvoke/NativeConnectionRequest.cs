namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.BasicApi.Nearby;
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class NativeConnectionRequest : BaseReferenceHolder
    {
        internal NativeConnectionRequest(IntPtr pointer) : base(pointer)
        {
        }

        internal ConnectionRequest AsRequest() => 
            new ConnectionRequest(this.RemoteEndpointId(), this.RemoteEndpointName(), NearbyConnectionsManager.ServiceId, this.Payload());

        protected override void CallDispose(HandleRef selfPointer)
        {
            NearbyConnectionTypes.ConnectionRequest_Dispose(selfPointer);
        }

        internal static NativeConnectionRequest FromPointer(IntPtr pointer)
        {
            if (pointer == IntPtr.Zero)
            {
                return null;
            }
            return new NativeConnectionRequest(pointer);
        }

        internal byte[] Payload() => 
            PInvokeUtilities.OutParamsToArray<byte>((out_arg, out_size) => NearbyConnectionTypes.ConnectionRequest_GetPayload(base.SelfPtr(), out_arg, out_size));

        internal string RemoteEndpointId() => 
            PInvokeUtilities.OutParamsToString((out_arg, out_size) => NearbyConnectionTypes.ConnectionRequest_GetRemoteEndpointId(base.SelfPtr(), out_arg, out_size));

        internal string RemoteEndpointName() => 
            PInvokeUtilities.OutParamsToString((out_arg, out_size) => NearbyConnectionTypes.ConnectionRequest_GetRemoteEndpointName(base.SelfPtr(), out_arg, out_size));
    }
}

