namespace AudienceNetwork
{
    using System;

    internal interface INativeAdBridge
    {
        int Create(string placementId, NativeAd nativeAd);
        void ExternalLogClick(int uniqueId);
        void ExternalLogImpression(int uniqueId);
        string GetAdChoicesImageURL(int uniqueId);
        string GetAdChoicesLinkURL(int uniqueId);
        string GetAdChoicesText(int uniqueId);
        string GetBody(int uniqueId);
        string GetCallToAction(int uniqueId);
        string GetCoverImageURL(int uniqueId);
        string GetIconImageURL(int uniqueId);
        int GetMinViewabilityPercentage(int uniqueId);
        string GetSocialContext(int uniqueId);
        string GetSubtitle(int uniqueId);
        string GetTitle(int uniqueId);
        bool IsValid(int uniqueId);
        int Load(int uniqueId);
        void OnClick(int uniqueId, FBNativeAdBridgeCallback callback);
        void OnError(int uniqueId, FBNativeAdBridgeErrorCallback callback);
        void OnFinishedClick(int uniqueId, FBNativeAdBridgeCallback callback);
        void OnImpression(int uniqueId, FBNativeAdBridgeCallback callback);
        void OnLoad(int uniqueId, FBNativeAdBridgeCallback callback);
        void Release(int uniqueId);
    }
}

