namespace GoogleMobileAds.Api
{
    using GoogleMobileAds.Common;
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class BannerView
    {
        private IBannerClient client;

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

        public BannerView(string adUnitId, AdSize adSize, AdPosition position)
        {
            MethodInfo method = Type.GetType("GoogleMobileAds.GoogleMobileAdsClientFactory,Assembly-CSharp").GetMethod("BuildBannerClient", BindingFlags.Public | BindingFlags.Static);
            this.client = (IBannerClient) method.Invoke(null, null);
            this.client.CreateBannerView(adUnitId, adSize, position);
            this.ConfigureBannerEvents();
        }

        public BannerView(string adUnitId, AdSize adSize, int x, int y)
        {
            MethodInfo method = Type.GetType("GoogleMobileAds.GoogleMobileAdsClientFactory,Assembly-CSharp").GetMethod("BuildBannerClient", BindingFlags.Public | BindingFlags.Static);
            this.client = (IBannerClient) method.Invoke(null, null);
            this.client.CreateBannerView(adUnitId, adSize, x, y);
            this.ConfigureBannerEvents();
        }

        private void ConfigureBannerEvents()
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
            this.client.DestroyBannerView();
        }

        public float GetHeightInPixels() => 
            this.client.GetHeightInPixels();

        public float GetWidthInPixels() => 
            this.client.GetWidthInPixels();

        public void Hide()
        {
            this.client.HideBannerView();
        }

        public void LoadAd(AdRequest request)
        {
            this.client.LoadAd(request);
        }

        public string MediationAdapterClassName() => 
            this.client.MediationAdapterClassName();

        public void SetPosition(AdPosition adPosition)
        {
            this.client.SetPosition(adPosition);
        }

        public void SetPosition(int x, int y)
        {
            this.client.SetPosition(x, y);
        }

        public void Show()
        {
            this.client.ShowBannerView();
        }
    }
}

