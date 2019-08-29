namespace GoogleMobileAds.Android
{
    using GoogleMobileAds.Common;
    using System;
    using UnityEngine;

    public class MobileAdsClient : IMobileAdsClient
    {
        private static MobileAdsClient instance = new MobileAdsClient();

        private MobileAdsClient()
        {
        }

        public void Initialize(string appId)
        {
            AndroidJavaObject @static = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaClass class3 = new AndroidJavaClass("com.google.android.gms.ads.MobileAds");
            object[] args = new object[] { @static, appId };
            class3.CallStatic("initialize", args);
        }

        public void SetApplicationMuted(bool muted)
        {
            AndroidJavaClass class2 = new AndroidJavaClass("com.google.android.gms.ads.MobileAds");
            object[] args = new object[] { muted };
            class2.CallStatic("setAppMuted", args);
        }

        public void SetApplicationVolume(float volume)
        {
            AndroidJavaClass class2 = new AndroidJavaClass("com.google.android.gms.ads.MobileAds");
            object[] args = new object[] { volume };
            class2.CallStatic("setAppVolume", args);
        }

        public void SetiOSAppPauseOnBackground(bool pause)
        {
        }

        public static MobileAdsClient Instance =>
            instance;
    }
}

