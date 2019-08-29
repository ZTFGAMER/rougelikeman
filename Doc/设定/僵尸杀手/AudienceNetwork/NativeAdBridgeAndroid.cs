namespace AudienceNetwork
{
    using AudienceNetwork.Utility;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    internal class NativeAdBridgeAndroid : NativeAdBridge
    {
        private static Dictionary<int, NativeAdContainer> nativeAds = new Dictionary<int, NativeAdContainer>();
        private static int lastKey = 0;

        public override int Create(string placementId, NativeAd nativeAd)
        {
            AdUtility.prepare();
            AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject obj3 = class2.GetStatic<AndroidJavaObject>("currentActivity").Call<AndroidJavaObject>("getApplicationContext", new object[0]);
            object[] args = new object[] { obj3, placementId };
            AndroidJavaObject bridgedNativeAd = new AndroidJavaObject("com.facebook.ads.NativeAd", args);
            NativeAdBridgeListenerProxy proxy = new NativeAdBridgeListenerProxy(nativeAd, bridgedNativeAd);
            object[] objArray2 = new object[] { proxy };
            bridgedNativeAd.Call("setAdListener", objArray2);
            NativeAdContainer container = new NativeAdContainer(nativeAd) {
                bridgedNativeAd = bridgedNativeAd,
                listenerProxy = proxy
            };
            int lastKey = NativeAdBridgeAndroid.lastKey;
            nativeAds.Add(lastKey, container);
            NativeAdBridgeAndroid.lastKey++;
            return lastKey;
        }

        public override void ExternalLogClick(int uniqueId)
        {
            AndroidJavaObject obj2 = this.nativeAdForNativeAdId(uniqueId);
            if (obj2 != null)
            {
                object[] args = new object[] { NativeAdBridge.source };
                obj2.Call("logExternalClick", args);
            }
        }

        public override void ExternalLogImpression(int uniqueId)
        {
            AndroidJavaObject obj2 = this.nativeAdForNativeAdId(uniqueId);
            if (obj2 != null)
            {
                obj2.Call("logExternalImpression", new object[0]);
            }
        }

        public override string GetAdChoicesImageURL(int uniqueId) => 
            this.getImageURLForNativeAdId(uniqueId, "getAdChoicesIcon");

        public override string GetAdChoicesLinkURL(int uniqueId) => 
            this.getStringForNativeAdId(uniqueId, "getAdChoicesLinkUrl");

        public override string GetAdChoicesText(int uniqueId) => 
            this.getStringForNativeAdId(uniqueId, "getAdChoicesText");

        public override string GetBody(int uniqueId) => 
            this.getStringForNativeAdId(uniqueId, "getAdBody");

        public override string GetCallToAction(int uniqueId) => 
            this.getStringForNativeAdId(uniqueId, "getAdCallToAction");

        public override string GetCoverImageURL(int uniqueId) => 
            this.getImageURLForNativeAdId(uniqueId, "getAdCoverImage");

        public override string GetIconImageURL(int uniqueId) => 
            this.getImageURLForNativeAdId(uniqueId, "getAdIcon");

        private string getId(int uniqueId)
        {
            AndroidJavaObject obj2 = this.nativeAdForNativeAdId(uniqueId);
            if (obj2 != null)
            {
                return obj2.Call<string>("getId", new object[0]);
            }
            return null;
        }

        private string getImageURLForNativeAdId(int uniqueId, string method)
        {
            AndroidJavaObject obj2 = this.nativeAdForNativeAdId(uniqueId);
            if (obj2 != null)
            {
                AndroidJavaObject obj3 = obj2.Call<AndroidJavaObject>(method, new object[0]);
                if (obj3 != null)
                {
                    return obj3.Call<string>("getUrl", new object[0]);
                }
            }
            return null;
        }

        public override int GetMinViewabilityPercentage(int uniqueId)
        {
            AndroidJavaObject obj2 = this.nativeAdForNativeAdId(uniqueId);
            if (obj2 != null)
            {
                return obj2.Call<int>("getMinViewabilityPercentage", new object[0]);
            }
            return 1;
        }

        public override string GetSocialContext(int uniqueId) => 
            this.getStringForNativeAdId(uniqueId, "getAdSocialContext");

        private string getStringForNativeAdId(int uniqueId, string method)
        {
            AndroidJavaObject obj2 = this.nativeAdForNativeAdId(uniqueId);
            if (obj2 != null)
            {
                return obj2.Call<string>(method, new object[0]);
            }
            return null;
        }

        public override string GetSubtitle(int uniqueId) => 
            this.getStringForNativeAdId(uniqueId, "getAdSubtitle");

        public override string GetTitle(int uniqueId) => 
            this.getStringForNativeAdId(uniqueId, "getAdTitle");

        public override bool IsValid(int uniqueId)
        {
            AndroidJavaObject obj2 = this.nativeAdForNativeAdId(uniqueId);
            if (obj2 != null)
            {
                return obj2.Call<bool>("isAdLoaded", new object[0]);
            }
            return false;
        }

        public override int Load(int uniqueId)
        {
            AdUtility.prepare();
            AndroidJavaObject obj2 = this.nativeAdForNativeAdId(uniqueId);
            if (obj2 != null)
            {
                object[] args = new object[] { NativeAdBridge.source };
                obj2.Call("registerExternalLogReceiver", args);
                obj2.Call("loadAd", new object[0]);
            }
            return uniqueId;
        }

        private AndroidJavaObject nativeAdForNativeAdId(int uniqueId)
        {
            NativeAdContainer container = null;
            if (nativeAds.TryGetValue(uniqueId, out container))
            {
                return container.bridgedNativeAd;
            }
            return null;
        }

        public override void OnClick(int uniqueId, FBNativeAdBridgeCallback callback)
        {
        }

        public override void OnError(int uniqueId, FBNativeAdBridgeErrorCallback callback)
        {
        }

        public override void OnFinishedClick(int uniqueId, FBNativeAdBridgeCallback callback)
        {
        }

        public override void OnImpression(int uniqueId, FBNativeAdBridgeCallback callback)
        {
        }

        public override void OnLoad(int uniqueId, FBNativeAdBridgeCallback callback)
        {
        }

        public override void Release(int uniqueId)
        {
            nativeAds.Remove(uniqueId);
        }

        private bool sendIntentToBroadcastManager(int uniqueId, string intent)
        {
            if (intent != null)
            {
                AndroidJavaObject @static = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                object[] args = new object[] { intent + ":" + this.getId(uniqueId) };
                AndroidJavaObject obj3 = new AndroidJavaObject("android.content.Intent", args);
                AndroidJavaClass class3 = new AndroidJavaClass("android.support.v4.content.LocalBroadcastManager");
                object[] objArray2 = new object[] { @static };
                object[] objArray3 = new object[] { obj3 };
                return class3.CallStatic<AndroidJavaObject>("getInstance", objArray2).Call<bool>("sendBroadcast", objArray3);
            }
            return false;
        }
    }
}

