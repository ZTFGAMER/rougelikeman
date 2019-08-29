namespace GoogleMobileAds.Common
{
    using GoogleMobileAds.Api;
    using System;

    public interface IAdLoaderClient
    {
        event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

        event EventHandler<CustomNativeEventArgs> OnCustomNativeTemplateAdLoaded;

        void LoadAd(AdRequest request);
    }
}

