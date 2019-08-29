namespace GoogleMobileAds.Common
{
    using GoogleMobileAds.Api;
    using System;

    public interface IInterstitialClient
    {
        event EventHandler<EventArgs> OnAdClosed;

        event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

        event EventHandler<EventArgs> OnAdLeavingApplication;

        event EventHandler<EventArgs> OnAdLoaded;

        event EventHandler<EventArgs> OnAdOpening;

        void CreateInterstitialAd(string adUnitId);
        void DestroyInterstitial();
        bool IsLoaded();
        void LoadAd(AdRequest request);
        string MediationAdapterClassName();
        void ShowInterstitial();
    }
}

