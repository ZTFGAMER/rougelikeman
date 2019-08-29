namespace GoogleMobileAds.Android
{
    using GoogleMobileAds.Api;
    using GoogleMobileAds.Common;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using UnityEngine;

    public class NativeExpressAdClient : AndroidJavaProxy, INativeExpressAdClient
    {
        private AndroidJavaObject nativeExpressAdView;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event EventHandler<EventArgs> OnAdClosed;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event EventHandler<EventArgs> OnAdLeavingApplication;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event EventHandler<EventArgs> OnAdLoaded;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event EventHandler<EventArgs> OnAdOpening;

        public NativeExpressAdClient() : base("com.google.unity.ads.UnityAdListener")
        {
            AndroidJavaObject @static = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            object[] args = new object[] { @static, this };
            this.nativeExpressAdView = new AndroidJavaObject("com.google.unity.ads.NativeExpressAd", args);
        }

        public void CreateNativeExpressAdView(string adUnitId, AdSize adSize, AdPosition position)
        {
            object[] args = new object[] { adUnitId, GoogleMobileAds.Android.Utils.GetAdSizeJavaObject(adSize), (int) position };
            this.nativeExpressAdView.Call("create", args);
        }

        public void CreateNativeExpressAdView(string adUnitId, AdSize adSize, int x, int y)
        {
            object[] args = new object[] { adUnitId, GoogleMobileAds.Android.Utils.GetAdSizeJavaObject(adSize), x, y };
            this.nativeExpressAdView.Call("create", args);
        }

        public void DestroyNativeExpressAdView()
        {
            this.nativeExpressAdView.Call("destroy", new object[0]);
        }

        public void HideNativeExpressAdView()
        {
            this.nativeExpressAdView.Call("hide", new object[0]);
        }

        public void LoadAd(AdRequest request)
        {
            object[] args = new object[] { GoogleMobileAds.Android.Utils.GetAdRequestJavaObject(request) };
            this.nativeExpressAdView.Call("loadAd", args);
        }

        public string MediationAdapterClassName() => 
            this.nativeExpressAdView.Call<string>("getMediationAdapterClassName", new object[0]);

        public void onAdClosed()
        {
            if (this.OnAdClosed != null)
            {
                this.OnAdClosed(this, EventArgs.Empty);
            }
        }

        public void onAdFailedToLoad(string errorReason)
        {
            if (this.OnAdFailedToLoad != null)
            {
                AdFailedToLoadEventArgs e = new AdFailedToLoadEventArgs {
                    Message = errorReason
                };
                this.OnAdFailedToLoad(this, e);
            }
        }

        public void onAdLeftApplication()
        {
            if (this.OnAdLeavingApplication != null)
            {
                this.OnAdLeavingApplication(this, EventArgs.Empty);
            }
        }

        public void onAdLoaded()
        {
            if (this.OnAdLoaded != null)
            {
                this.OnAdLoaded(this, EventArgs.Empty);
            }
        }

        public void onAdOpened()
        {
            if (this.OnAdOpening != null)
            {
                this.OnAdOpening(this, EventArgs.Empty);
            }
        }

        public void SetAdSize(AdSize adSize)
        {
            object[] args = new object[] { GoogleMobileAds.Android.Utils.GetAdSizeJavaObject(adSize) };
            this.nativeExpressAdView.Call("setAdSize", args);
        }

        public void ShowNativeExpressAdView()
        {
            this.nativeExpressAdView.Call("show", new object[0]);
        }
    }
}

