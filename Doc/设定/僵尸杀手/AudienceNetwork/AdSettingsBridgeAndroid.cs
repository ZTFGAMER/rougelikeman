namespace AudienceNetwork
{
    using System;
    using UnityEngine;

    internal class AdSettingsBridgeAndroid : AdSettingsBridge
    {
        public override void addTestDevice(string deviceID)
        {
            object[] args = new object[] { deviceID };
            this.getAdSettingsObject().CallStatic("addTestDevice", args);
        }

        private AndroidJavaClass getAdSettingsObject() => 
            new AndroidJavaClass("com.facebook.ads.AdSettings");

        public override void setUrlPrefix(string urlPrefix)
        {
            object[] args = new object[] { urlPrefix };
            this.getAdSettingsObject().CallStatic("setUrlPrefix", args);
        }
    }
}

