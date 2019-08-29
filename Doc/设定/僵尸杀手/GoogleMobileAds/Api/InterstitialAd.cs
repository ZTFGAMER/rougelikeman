namespace GoogleMobileAds.Api
{
    using GoogleMobileAds.Common;
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class InterstitialAd
    {
        private IInterstitialClient client;

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

        public InterstitialAd(string adUnitId)
        {
            MethodInfo method = Type.GetType("GoogleMobileAds.GoogleMobileAdsClientFactory,Assembly-CSharp").GetMethod("BuildInterstitialClient", BindingFlags.Public | BindingFlags.Static);
            this.client = (IInterstitialClient) method.Invoke(null, null);
            this.client.CreateInterstitialAd(adUnitId);
            this.client.OnAdLoaded += new EventHandler<EventArgs>(this.<InterstitialAd>m__0);
            this.client.OnAdFailedToLoad += new EventHandler<AdFailedToLoadEventArgs>(this.<InterstitialAd>m__1);
            this.client.OnAdOpening += new EventHandler<EventArgs>(this.<InterstitialAd>m__2);
            this.client.OnAdClosed += new EventHandler<EventArgs>(this.<InterstitialAd>m__3);
            this.client.OnAdLeavingApplication += new EventHandler<EventArgs>(this.<InterstitialAd>m__4);
        }

        [CompilerGenerated]
        private void <InterstitialAd>m__0(object sender, EventArgs args)
        {
            if (this.OnAdLoaded != null)
            {
                this.OnAdLoaded(this, args);
            }
        }

        [CompilerGenerated]
        private void <InterstitialAd>m__1(object sender, AdFailedToLoadEventArgs args)
        {
            if (this.OnAdFailedToLoad != null)
            {
                this.OnAdFailedToLoad(this, args);
            }
        }

        [CompilerGenerated]
        private void <InterstitialAd>m__2(object sender, EventArgs args)
        {
            if (this.OnAdOpening != null)
            {
                this.OnAdOpening(this, args);
            }
        }

        [CompilerGenerated]
        private void <InterstitialAd>m__3(object sender, EventArgs args)
        {
            if (this.OnAdClosed != null)
            {
                this.OnAdClosed(this, args);
            }
        }

        [CompilerGenerated]
        private void <InterstitialAd>m__4(object sender, EventArgs args)
        {
            if (this.OnAdLeavingApplication != null)
            {
                this.OnAdLeavingApplication(this, args);
            }
        }

        public void Destroy()
        {
            this.client.DestroyInterstitial();
        }

        public bool IsLoaded() => 
            this.client.IsLoaded();

        public void LoadAd(AdRequest request)
        {
            this.client.LoadAd(request);
        }

        public string MediationAdapterClassName() => 
            this.client.MediationAdapterClassName();

        public void Show()
        {
            this.client.ShowInterstitial();
        }
    }
}

