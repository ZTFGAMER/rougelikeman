namespace GooglePlayGames.BasicApi.Nearby
{
    using GooglePlayGames.BasicApi;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct AdvertisingResult
    {
        private readonly ResponseStatus mStatus;
        private readonly string mLocalEndpointName;
        public AdvertisingResult(ResponseStatus status, string localEndpointName)
        {
            this.mStatus = status;
            this.mLocalEndpointName = Misc.CheckNotNull<string>(localEndpointName);
        }

        public bool Succeeded =>
            (this.mStatus == ResponseStatus.Success);
        public ResponseStatus Status =>
            this.mStatus;
        public string LocalEndpointName =>
            this.mLocalEndpointName;
    }
}

