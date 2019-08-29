namespace GooglePlayGames.Native.PInvoke
{
    using GooglePlayGames.BasicApi.Nearby;
    using GooglePlayGames.Native.Cwrapper;
    using System;
    using System.Runtime.InteropServices;

    internal class NativeConnectionResponse : BaseReferenceHolder
    {
        internal NativeConnectionResponse(IntPtr pointer) : base(pointer)
        {
        }

        internal ConnectionResponse AsResponse(long localClientId)
        {
            switch ((this.ResponseCode() + 4))
            {
                case ~NearbyConnectionTypes.ConnectionResponse_ResponseCode.ERROR_INTERNAL:
                    return ConnectionResponse.EndpointNotConnected(localClientId, this.RemoteEndpointId());

                case NearbyConnectionTypes.ConnectionResponse_ResponseCode.ACCEPTED:
                    return ConnectionResponse.AlreadyConnected(localClientId, this.RemoteEndpointId());

                case NearbyConnectionTypes.ConnectionResponse_ResponseCode.REJECTED:
                    return ConnectionResponse.NetworkNotConnected(localClientId, this.RemoteEndpointId());

                case ~NearbyConnectionTypes.ConnectionResponse_ResponseCode.ERROR_ENDPOINT_NOT_CONNECTED:
                    return ConnectionResponse.InternalError(localClientId, this.RemoteEndpointId());

                case ((NearbyConnectionTypes.ConnectionResponse_ResponseCode) 5):
                    return ConnectionResponse.Accepted(localClientId, this.RemoteEndpointId(), this.Payload());

                case ((NearbyConnectionTypes.ConnectionResponse_ResponseCode) 6):
                    return ConnectionResponse.Rejected(localClientId, this.RemoteEndpointId());
            }
            throw new InvalidOperationException("Found connection response of unknown type: " + this.ResponseCode());
        }

        protected override void CallDispose(HandleRef selfPointer)
        {
            NearbyConnectionTypes.ConnectionResponse_Dispose(selfPointer);
        }

        internal static NativeConnectionResponse FromPointer(IntPtr pointer)
        {
            if (pointer == IntPtr.Zero)
            {
                return null;
            }
            return new NativeConnectionResponse(pointer);
        }

        internal byte[] Payload() => 
            PInvokeUtilities.OutParamsToArray<byte>((out_arg, out_size) => NearbyConnectionTypes.ConnectionResponse_GetPayload(base.SelfPtr(), out_arg, out_size));

        internal string RemoteEndpointId() => 
            PInvokeUtilities.OutParamsToString((out_arg, out_size) => NearbyConnectionTypes.ConnectionResponse_GetRemoteEndpointId(base.SelfPtr(), out_arg, out_size));

        internal NearbyConnectionTypes.ConnectionResponse_ResponseCode ResponseCode() => 
            NearbyConnectionTypes.ConnectionResponse_GetStatus(base.SelfPtr());
    }
}

