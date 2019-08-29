namespace AudienceNetwork
{
    using System;

    internal interface IAdSettingsBridge
    {
        void addTestDevice(string deviceID);
        void setUrlPrefix(string urlPrefix);
    }
}

