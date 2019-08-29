namespace GoogleMobileAds.Api
{
    using GoogleMobileAds.Common;
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class NativeExpressAdView
    {
        private INativeExpressAdClient client;

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

        public NativeExpressAdView(string adUnitId, AdSize adSize, AdPosition position)
        {
            MethodInfo method = Type.GetType("GoogleMobileAds.GoogleMobileAdsClientFactory,Assembly-CSharp").GetMethod("BuildNativeExpressAdClient", BindingFlags.Public | BindingFlags.Static);
            this.client = (INativeExpressAdClient) method.Invoke(null, null);
            this.client.CreateNativeExpressAdView(adUnitId, adSize, position);
            this.ConfigureNativeExpressAdEvents();
        }

        public NativeExpressAdView(string adUnitId, AdSize adSize, int x, int y)
        {
            MethodInfo method = Type.GetType("GoogleMobileAds.GoogleMobileAdsClientFactory,Assembly-CSharp").GetMethod("BuildNativeExpressAdClient", BindingFlags.Public | BindingFlags.Static);
            this.client = (INativeExpressAdClient) method.Invoke(null, null);
            this.client.CreateNativeExpressAdView(adUnitId, adSize, x, y);
            this.ConfigureNativeExpressAdEvents();
        }

        private void ConfigureNativeExpressAdEvents()
        {
            this.client.OnAdLoaded += delegate (object sender, EventArgs args) {
                if (this.OnAdLoaded != null)
                {
                    this.OnAdLoaded(this, args);
                }
            };
            this.client.OnAdFailedToLoad += delegate (object sender, AdFailedToLoadEventArgs args) {
                if (this.OnAdFailedToLoad != null)
                {
                    this.OnAdFailedToLoad(this, args);
                }
            };
            this.client.OnAdOpening += delegate (object sender, EventArgs args) {
                if (this.OnAdOpening != null)
                {
                    this.OnAdOpening(this, args);
                }
            };
            this.client.OnAdClosed += delegate (object sender, EventArgs args) {
                if (this.OnAdClosed != null)
                {
                    this.OnAdClosed(this, args);
                }
            };
            this.client.OnAdLeavingApplication += delegate (object sender, EventArgs args) {
                if (this.OnAdLeavingApplication != null)
                {
                    this.OnAdLeavingApplication(this, args);
                }
            };
        }

        public void Destroy()
        {
            this.client.DestroyNativeExpressAdView();
        }

        public void Hide()
        {
            this.client.HideNativeExpressAdView();
        }

        public void LoadAd(AdRequest request)
        {
            this.client.LoadAd(request);
        }

        public string MediationAdapterClassName() => 
            this.client.MediationAdapterClassName();

        public void Show()
        {
            this.client.ShowNativeExpressAdView();
        }
    }
}

