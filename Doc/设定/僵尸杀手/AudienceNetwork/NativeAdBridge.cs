namespace AudienceNetwork
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    internal class NativeAdBridge : INativeAdBridge
    {
        internal static readonly string source;
        public static readonly INativeAdBridge Instance;
        private FBNativeAdBridgeCallback onImpressionCallback;
        private FBNativeAdBridgeCallback onClickCallback;
        private List<NativeAd> nativeAds = new List<NativeAd>();

        static NativeAdBridge()
        {
            string[] textArray1 = new string[] { "AudienceNetworkUnityBridge ", SdkVersion.Build, " (Unity ", Application.unityVersion, ")" };
            source = string.Concat(textArray1);
            Instance = createInstance();
        }

        internal NativeAdBridge()
        {
        }

        public virtual int Create(string placementId, NativeAd nativeAd)
        {
            this.nativeAds.Add(nativeAd);
            return (this.nativeAds.Count - 1);
        }

        private static INativeAdBridge createInstance()
        {
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                return new NativeAdBridgeAndroid();
            }
            return new NativeAdBridge();
        }

        public virtual void ExternalLogClick(int uniqueId)
        {
            FBNativeAdBridgeCallback onClickCallback = this.onClickCallback;
            if (onClickCallback != null)
            {
                onClickCallback();
            }
        }

        public virtual void ExternalLogImpression(int uniqueId)
        {
            FBNativeAdBridgeCallback onImpressionCallback = this.onImpressionCallback;
            if (onImpressionCallback != null)
            {
                onImpressionCallback();
            }
        }

        public virtual string GetAdChoicesImageURL(int uniqueId) => 
            "https://www.facebook.com/images/ad_network/ad_choices.png";

        public virtual string GetAdChoicesLinkURL(int uniqueId) => 
            "https://m.facebook.com/ads/ad_choices/";

        public virtual string GetAdChoicesText(int uniqueId) => 
            "AdChoices";

        public virtual string GetBody(int uniqueId) => 
            "Your ad integration works. Woohoo!";

        public virtual string GetCallToAction(int uniqueId) => 
            "Install Now";

        public virtual string GetCoverImageURL(int uniqueId) => 
            "https://www.facebook.com/images/ad_network/audience_network_test_cover.png";

        public virtual string GetIconImageURL(int uniqueId) => 
            "https://www.facebook.com/images/ad_network/audience_network_icon.png";

        public virtual int GetMinViewabilityPercentage(int uniqueId) => 
            1;

        public virtual string GetSocialContext(int uniqueId) => 
            "Available on the App Store";

        public virtual string GetSubtitle(int uniqueId) => 
            "An ad for Facebook";

        public virtual string GetTitle(int uniqueId) => 
            "Facebook Test Ad";

        public virtual bool IsValid(int uniqueId) => 
            true;

        public virtual int Load(int uniqueId)
        {
            this.nativeAds[uniqueId].loadAdFromData();
            return uniqueId;
        }

        public virtual void OnClick(int uniqueId, FBNativeAdBridgeCallback callback)
        {
            this.onClickCallback = callback;
        }

        public virtual void OnError(int uniqueId, FBNativeAdBridgeErrorCallback callback)
        {
        }

        public virtual void OnFinishedClick(int uniqueId, FBNativeAdBridgeCallback callback)
        {
        }

        public virtual void OnImpression(int uniqueId, FBNativeAdBridgeCallback callback)
        {
            this.onImpressionCallback = callback;
        }

        public virtual void OnLoad(int uniqueId, FBNativeAdBridgeCallback callback)
        {
        }

        public virtual void Release(int uniqueId)
        {
        }
    }
}

