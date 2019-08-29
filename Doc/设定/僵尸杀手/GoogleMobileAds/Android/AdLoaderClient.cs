namespace GoogleMobileAds.Android
{
    using GoogleMobileAds.Api;
    using GoogleMobileAds.Common;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using UnityEngine;

    public class AdLoaderClient : AndroidJavaProxy, IAdLoaderClient
    {
        private AndroidJavaObject adLoader;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<string, Action<CustomNativeTemplateAd, string>> <CustomNativeTemplateCallbacks>k__BackingField;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event EventHandler<CustomNativeEventArgs> OnCustomNativeTemplateAdLoaded;

        public AdLoaderClient(AdLoader unityAdLoader) : base("com.google.unity.ads.UnityAdLoaderListener")
        {
            AndroidJavaObject @static = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            object[] args = new object[] { @static, unityAdLoader.AdUnitId, this };
            this.adLoader = new AndroidJavaObject("com.google.unity.ads.NativeAdLoader", args);
            this.CustomNativeTemplateCallbacks = unityAdLoader.CustomNativeTemplateClickHandlers;
            if (unityAdLoader.AdTypes.Contains(NativeAdType.CustomTemplate))
            {
                foreach (string str in unityAdLoader.TemplateIds)
                {
                    object[] objArray2 = new object[] { str, this.CustomNativeTemplateCallbacks.ContainsKey(str) };
                    this.adLoader.Call("configureCustomNativeTemplateAd", objArray2);
                }
            }
            this.adLoader.Call("create", new object[0]);
        }

        public void LoadAd(AdRequest request)
        {
            object[] args = new object[] { GoogleMobileAds.Android.Utils.GetAdRequestJavaObject(request) };
            this.adLoader.Call("loadAd", args);
        }

        private void onAdFailedToLoad(string errorReason)
        {
            AdFailedToLoadEventArgs e = new AdFailedToLoadEventArgs {
                Message = errorReason
            };
            this.OnAdFailedToLoad(this, e);
        }

        public void onCustomClick(AndroidJavaObject ad, string assetName)
        {
            CustomNativeTemplateAd ad2 = new CustomNativeTemplateAd(new CustomNativeTemplateClient(ad));
            this.CustomNativeTemplateCallbacks[ad2.GetCustomTemplateId()](ad2, assetName);
        }

        public void onCustomTemplateAdLoaded(AndroidJavaObject ad)
        {
            if (this.OnCustomNativeTemplateAdLoaded != null)
            {
                CustomNativeEventArgs e = new CustomNativeEventArgs {
                    nativeAd = new CustomNativeTemplateAd(new CustomNativeTemplateClient(ad))
                };
                this.OnCustomNativeTemplateAdLoaded(this, e);
            }
        }

        private Dictionary<string, Action<CustomNativeTemplateAd, string>> CustomNativeTemplateCallbacks { get; set; }
    }
}

