namespace GoogleMobileAds.Common
{
    using GoogleMobileAds.Api;
    using System;

    public interface IRewardBasedVideoAdClient
    {
        event EventHandler<EventArgs> OnAdClosed;

        event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

        event EventHandler<EventArgs> OnAdLeavingApplication;

        event EventHandler<EventArgs> OnAdLoaded;

        event EventHandler<EventArgs> OnAdOpening;

        event EventHandler<Reward> OnAdRewarded;

        event EventHandler<EventArgs> OnAdStarted;

        void CreateRewardBasedVideoAd();
        bool IsLoaded();
        void LoadAd(AdRequest request, string adUnitId);
        string MediationAdapterClassName();
        void SetUserId(string userId);
        void ShowRewardBasedVideoAd();
    }
}

