namespace AudienceNetwork
{
    using System;

    internal interface IInterstitialAdBridge
    {
        int Create(string placementId, InterstitialAd interstitialAd);
        bool IsValid(int uniqueId);
        int Load(int uniqueId);
        void OnClick(int uniqueId, FBInterstitialAdBridgeCallback callback);
        void OnDidClose(int uniqueId, FBInterstitialAdBridgeCallback callback);
        void OnError(int uniqueId, FBInterstitialAdBridgeErrorCallback callback);
        void OnImpression(int uniqueId, FBInterstitialAdBridgeCallback callback);
        void OnLoad(int uniqueId, FBInterstitialAdBridgeCallback callback);
        void OnWillClose(int uniqueId, FBInterstitialAdBridgeCallback callback);
        void Release(int uniqueId);
        bool Show(int uniqueId);
    }
}

