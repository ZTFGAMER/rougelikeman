namespace AudienceNetwork
{
    using System;

    public class AdSettings
    {
        public static void AddTestDevice(string deviceID)
        {
            AdSettingsBridge.Instance.addTestDevice(deviceID);
        }

        public static void SetUrlPrefix(string urlPrefix)
        {
            AdSettingsBridge.Instance.setUrlPrefix(urlPrefix);
        }
    }
}

