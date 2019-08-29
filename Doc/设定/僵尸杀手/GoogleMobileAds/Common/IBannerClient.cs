namespace GoogleMobileAds.Common
{
    using GoogleMobileAds.Api;
    using System;

    public interface IBannerClient
    {
        event EventHandler<EventArgs> OnAdClosed;

        event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

        event EventHandler<EventArgs> OnAdLeavingApplication;

        event EventHandler<EventArgs> OnAdLoaded;

        event EventHandler<EventArgs> OnAdOpening;

        void CreateBannerView(string adUnitId, AdSize adSize, AdPosition position);
        void CreateBannerView(string adUnitId, AdSize adSize, int x, int y);
        void DestroyBannerView();
        float GetHeightInPixels();
        float GetWidthInPixels();
        void HideBannerView();
        void LoadAd(AdRequest request);
        string MediationAdapterClassName();
        void SetPosition(AdPosition adPosition);
        void SetPosition(int x, int y);
        void ShowBannerView();
    }
}

