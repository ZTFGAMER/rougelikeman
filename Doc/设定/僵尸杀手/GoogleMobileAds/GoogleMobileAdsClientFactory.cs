namespace GoogleMobileAds
{
    using GoogleMobileAds.Android;
    using GoogleMobileAds.Api;
    using GoogleMobileAds.Common;
    using System;

    public class GoogleMobileAdsClientFactory
    {
        public static IAdLoaderClient BuildAdLoaderClient(AdLoader adLoader) => 
            new AdLoaderClient(adLoader);

        public static IBannerClient BuildBannerClient() => 
            new BannerClient();

        public static IInterstitialClient BuildInterstitialClient() => 
            new InterstitialClient();

        public static INativeExpressAdClient BuildNativeExpressAdClient() => 
            new NativeExpressAdClient();

        public static IRewardBasedVideoAdClient BuildRewardBasedVideoAdClient() => 
            new RewardBasedVideoAdClient();

        public static IMobileAdsClient MobileAdsInstance() => 
            MobileAdsClient.Instance;
    }
}

