namespace GooglePlayGames.BasicApi.Nearby
{
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct ConnectionResponse
    {
        private static readonly byte[] EmptyPayload;
        private readonly long mLocalClientId;
        private readonly string mRemoteEndpointId;
        private readonly Status mResponseStatus;
        private readonly byte[] mPayload;
        private ConnectionResponse(long localClientId, string remoteEndpointId, Status code, byte[] payload)
        {
            this.mLocalClientId = localClientId;
            this.mRemoteEndpointId = Misc.CheckNotNull<string>(remoteEndpointId);
            this.mResponseStatus = code;
            this.mPayload = Misc.CheckNotNull<byte[]>(payload);
        }

        public long LocalClientId =>
            this.mLocalClientId;
        public string RemoteEndpointId =>
            this.mRemoteEndpointId;
        public Status ResponseStatus =>
            this.mResponseStatus;
        public byte[] Payload =>
            this.mPayload;
        public static ConnectionResponse Rejected(long localClientId, string remoteEndpointId) => 
            new ConnectionResponse(localClientId, remoteEndpointId, Status.Rejected, EmptyPayload);

        public static ConnectionResponse NetworkNotConnected(long localClientId, string remoteEndpointId) => 
            new ConnectionResponse(localClientId, remoteEndpointId, Status.ErrorNetworkNotConnected, EmptyPayload);

        public static ConnectionResponse InternalError(long localClientId, string remoteEndpointId) => 
            new ConnectionResponse(localClientId, remoteEndpointId, Status.ErrorInternal, EmptyPayload);

        public static ConnectionResponse EndpointNotConnected(long localClientId, string remoteEndpointId) => 
            new ConnectionResponse(localClientId, remoteEndpointId, Status.ErrorEndpointNotConnected, EmptyPayload);

        public static ConnectionResponse Accepted(long localClientId, string remoteEndpointId, byte[] payload) => 
            new ConnectionResponse(localClientId, remoteEndpointId, Status.Accepted, payload);

        public static ConnectionResponse AlreadyConnected(long localClientId, string remoteEndpointId) => 
            new ConnectionResponse(localClientId, remoteEndpointId, Status.ErrorAlreadyConnected, EmptyPayload);

        static ConnectionResponse()
        {
            EmptyPayload = new byte[0];
        }
        public enum Status
        {
            Accepted,
            Rejected,
            ErrorInternal,
            ErrorNetworkNotConnected,
            ErrorEndpointNotConnected,
            ErrorAlreadyConnected
        }
    }
}

