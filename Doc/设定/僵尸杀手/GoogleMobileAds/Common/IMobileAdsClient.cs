namespace GoogleMobileAds.Common
{
    using System;

    public interface IMobileAdsClient
    {
        void Initialize(string appId);
        void SetApplicationMuted(bool muted);
        void SetApplicationVolume(float volume);
        void SetiOSAppPauseOnBackground(bool pause);
    }
}

