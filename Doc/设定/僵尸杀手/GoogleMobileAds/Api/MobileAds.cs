namespace GoogleMobileAds.Api
{
    using GoogleMobileAds.Common;
    using System;
    using System.Reflection;

    public class MobileAds
    {
        private static readonly IMobileAdsClient client = GetMobileAdsClient();

        private static IMobileAdsClient GetMobileAdsClient() => 
            ((IMobileAdsClient) Type.GetType("GoogleMobileAds.GoogleMobileAdsClientFactory,Assembly-CSharp").GetMethod("MobileAdsInstance", BindingFlags.Public | BindingFlags.Static).Invoke(null, null));

        public static void Initialize(string appId)
        {
            client.Initialize(appId);
        }

        public static void SetApplicationMuted(bool muted)
        {
            client.SetApplicationMuted(muted);
        }

        public static void SetApplicationVolume(float volume)
        {
            client.SetApplicationVolume(volume);
        }

        public static void SetiOSAppPauseOnBackground(bool pause)
        {
            client.SetiOSAppPauseOnBackground(pause);
        }
    }
}

