namespace GooglePlayGames.BasicApi.Nearby
{
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct NearbyConnectionConfiguration
    {
        public const int MaxUnreliableMessagePayloadLength = 0x490;
        public const int MaxReliableMessagePayloadLength = 0x1000;
        private readonly Action<InitializationStatus> mInitializationCallback;
        private readonly long mLocalClientId;
        public NearbyConnectionConfiguration(Action<InitializationStatus> callback, long localClientId)
        {
            this.mInitializationCallback = Misc.CheckNotNull<Action<InitializationStatus>>(callback);
            this.mLocalClientId = localClientId;
        }

        public long LocalClientId =>
            this.mLocalClientId;
        public Action<InitializationStatus> InitializationCallback =>
            this.mInitializationCallback;
    }
}

