namespace AudienceNetwork
{
    using System;
    using UnityEngine;

    internal class AdSettingsBridge : IAdSettingsBridge
    {
        public static readonly IAdSettingsBridge Instance = createInstance();

        internal AdSettingsBridge()
        {
        }

        public virtual void addTestDevice(string deviceID)
        {
        }

        private static IAdSettingsBridge createInstance()
        {
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                return new AdSettingsBridgeAndroid();
            }
            return new AdSettingsBridge();
        }

        public virtual void setUrlPrefix(string urlPrefix)
        {
        }
    }
}

