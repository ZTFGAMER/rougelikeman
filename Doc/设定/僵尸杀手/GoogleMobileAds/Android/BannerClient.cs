namespace GoogleMobileAds.Android
{
    using GoogleMobileAds.Api;
    using GoogleMobileAds.Common;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using UnityEngine;

    public class BannerClient : AndroidJavaProxy, IBannerClient
    {
        private AndroidJavaObject bannerView;

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

        public BannerClient() : base("com.google.unity.ads.UnityAdListener")
        {
            AndroidJavaObject @static = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            object[] args = new object[] { @static, this };
            this.bannerView = new AndroidJavaObject("com.google.unity.ads.Banner", args);
        }

        public void CreateBannerView(string adUnitId, AdSize adSize, AdPosition position)
        {
            object[] args = new object[] { adUnitId, GoogleMobileAds.Android.Utils.GetAdSizeJavaObject(adSize), (int) position };
            this.bannerView.Call("create", args);
        }

        public void CreateBannerView(string adUnitId, AdSize adSize, int x, int y)
        {
            object[] args = new object[] { adUnitId, GoogleMobileAds.Android.Utils.GetAdSizeJavaObject(adSize), x, y };
            this.bannerView.Call("create", args);
        }

        public void DestroyBannerView()
        {
            this.bannerView.Call("destroy", new object[0]);
        }

        public float GetHeightInPixels() => 
            this.bannerView.Call<float>("getHeightInPixels", new object[0]);

        public float GetWidthInPixels() => 
            this.bannerView.Call<float>("getWidthInPixels", new object[0]);

        public void HideBannerView()
        {
            this.bannerView.Call("hide", new object[0]);
        }

        public void LoadAd(AdRequest request)
        {
            object[] args = new object[] { GoogleMobileAds.Android.Utils.GetAdRequestJavaObject(request) };
            this.bannerView.Call("loadAd", args);
        }

        public string MediationAdapterClassName() => 
            this.bannerView.Call<string>("getMediationAdapterClassName", new object[0]);

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

        public void SetPosition(AdPosition adPosition)
        {
            object[] args = new object[] { (int) adPosition };
            this.bannerView.Call("setPosition", args);
        }

        public void SetPosition(int x, int y)
        {
            object[] args = new object[] { x, y };
            this.bannerView.Call("setPosition", args);
        }

        public void ShowBannerView()
        {
            this.bannerView.Call("show", new object[0]);
        }
    }
}

