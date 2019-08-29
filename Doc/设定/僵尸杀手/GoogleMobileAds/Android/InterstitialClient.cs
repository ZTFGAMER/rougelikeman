namespace GoogleMobileAds.Android
{
    using GoogleMobileAds.Api;
    using GoogleMobileAds.Common;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using UnityEngine;

    public class InterstitialClient : AndroidJavaProxy, IInterstitialClient
    {
        private AndroidJavaObject interstitial;

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

        public InterstitialClient() : base("com.google.unity.ads.UnityAdListener")
        {
            AndroidJavaObject @static = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            object[] args = new object[] { @static, this };
            this.interstitial = new AndroidJavaObject("com.google.unity.ads.Interstitial", args);
        }

        public void CreateInterstitialAd(string adUnitId)
        {
            object[] args = new object[] { adUnitId };
            this.interstitial.Call("create", args);
        }

        public void DestroyInterstitial()
        {
            this.interstitial.Call("destroy", new object[0]);
        }

        public bool IsLoaded() => 
            this.interstitial.Call<bool>("isLoaded", new object[0]);

        public void LoadAd(AdRequest request)
        {
            object[] args = new object[] { GoogleMobileAds.Android.Utils.GetAdRequestJavaObject(request) };
            this.interstitial.Call("loadAd", args);
        }

        public string MediationAdapterClassName() => 
            this.interstitial.Call<string>("getMediationAdapterClassName", new object[0]);

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

        public void ShowInterstitial()
        {
            this.interstitial.Call("show", new object[0]);
        }
    }
}

