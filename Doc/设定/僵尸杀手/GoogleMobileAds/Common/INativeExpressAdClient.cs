namespace GoogleMobileAds.Common
{
    using GoogleMobileAds.Api;
    using System;

    public interface INativeExpressAdClient
    {
        event EventHandler<EventArgs> OnAdClosed;

        event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

        event EventHandler<EventArgs> OnAdLeavingApplication;

        event EventHandler<EventArgs> OnAdLoaded;

        event EventHandler<EventArgs> OnAdOpening;

        void CreateNativeExpressAdView(string adUnitId, AdSize adSize, AdPosition position);
        void CreateNativeExpressAdView(string adUnitId, AdSize adSize, int x, int y);
        void DestroyNativeExpressAdView();
        void HideNativeExpressAdView();
        void LoadAd(AdRequest request);
        string MediationAdapterClassName();
        void ShowNativeExpressAdView();
    }
}

