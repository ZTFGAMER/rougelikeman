namespace AudienceNetwork
{
    using AudienceNetwork.Utility;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    internal class InterstitialAdBridgeAndroid : InterstitialAdBridge
    {
        private static Dictionary<int, InterstitialAdContainer> interstitialAds = new Dictionary<int, InterstitialAdContainer>();
        private static int lastKey = 0;

        public override int Create(string placementId, InterstitialAd interstitialAd)
        {
            AdUtility.prepare();
            AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject obj3 = class2.GetStatic<AndroidJavaObject>("currentActivity").Call<AndroidJavaObject>("getApplicationContext", new object[0]);
            object[] args = new object[] { obj3, placementId };
            AndroidJavaObject bridgedInterstitialAd = new AndroidJavaObject("com.facebook.ads.InterstitialAd", args);
            InterstitialAdBridgeListenerProxy proxy = new InterstitialAdBridgeListenerProxy(interstitialAd, bridgedInterstitialAd);
            object[] objArray2 = new object[] { proxy };
            bridgedInterstitialAd.Call("setAdListener", objArray2);
            InterstitialAdContainer container = new InterstitialAdContainer(interstitialAd) {
                bridgedInterstitialAd = bridgedInterstitialAd,
                listenerProxy = proxy
            };
            int lastKey = InterstitialAdBridgeAndroid.lastKey;
            interstitialAds.Add(lastKey, container);
            InterstitialAdBridgeAndroid.lastKey++;
            return lastKey;
        }

        private string getImageURLForuniqueId(int uniqueId, string method)
        {
            AndroidJavaObject obj2 = this.interstitialAdForuniqueId(uniqueId);
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

        private string getStringForuniqueId(int uniqueId, string method)
        {
            AndroidJavaObject obj2 = this.interstitialAdForuniqueId(uniqueId);
            if (obj2 != null)
            {
                return obj2.Call<string>(method, new object[0]);
            }
            return null;
        }

        private AndroidJavaObject interstitialAdForuniqueId(int uniqueId)
        {
            InterstitialAdContainer container = null;
            if (interstitialAds.TryGetValue(uniqueId, out container))
            {
                return container.bridgedInterstitialAd;
            }
            return null;
        }

        public override bool IsValid(int uniqueId)
        {
            AndroidJavaObject obj2 = this.interstitialAdForuniqueId(uniqueId);
            if (obj2 != null)
            {
                return obj2.Call<bool>("isAdLoaded", new object[0]);
            }
            return false;
        }

        public override int Load(int uniqueId)
        {
            AdUtility.prepare();
            AndroidJavaObject obj2 = this.interstitialAdForuniqueId(uniqueId);
            if (obj2 != null)
            {
                obj2.Call("loadAd", new object[0]);
            }
            return uniqueId;
        }

        public override void OnClick(int uniqueId, FBInterstitialAdBridgeCallback callback)
        {
        }

        public override void OnDidClose(int uniqueId, FBInterstitialAdBridgeCallback callback)
        {
        }

        public override void OnError(int uniqueId, FBInterstitialAdBridgeErrorCallback callback)
        {
        }

        public override void OnImpression(int uniqueId, FBInterstitialAdBridgeCallback callback)
        {
        }

        public override void OnLoad(int uniqueId, FBInterstitialAdBridgeCallback callback)
        {
        }

        public override void OnWillClose(int uniqueId, FBInterstitialAdBridgeCallback callback)
        {
        }

        public override void Release(int uniqueId)
        {
            AndroidJavaObject obj2 = this.interstitialAdForuniqueId(uniqueId);
            if (obj2 != null)
            {
                obj2.Call("destroy", new object[0]);
            }
            interstitialAds.Remove(uniqueId);
        }

        public override bool Show(int uniqueId)
        {
            AndroidJavaObject obj2 = this.interstitialAdForuniqueId(uniqueId);
            if (obj2 != null)
            {
                return obj2.Call<bool>("show", new object[0]);
            }
            return false;
        }
    }
}

