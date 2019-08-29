namespace GooglePlayGames.BasicApi.Nearby
{
    using GooglePlayGames.OurUtils;
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct EndpointDetails
    {
        private readonly string mEndpointId;
        private readonly string mName;
        private readonly string mServiceId;
        public EndpointDetails(string endpointId, string name, string serviceId)
        {
            this.mEndpointId = Misc.CheckNotNull<string>(endpointId);
            this.mName = Misc.CheckNotNull<string>(name);
            this.mServiceId = Misc.CheckNotNull<string>(serviceId);
        }

        public string EndpointId =>
            this.mEndpointId;
        public string Name =>
            this.mName;
        public string ServiceId =>
            this.mServiceId;
    }
}

