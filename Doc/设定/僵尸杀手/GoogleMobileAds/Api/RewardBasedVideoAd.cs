namespace GoogleMobileAds.Api
{
    using GoogleMobileAds.Common;
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class RewardBasedVideoAd
    {
        private IRewardBasedVideoAdClient client;
        private static readonly RewardBasedVideoAd instance = new RewardBasedVideoAd();

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

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event EventHandler<Reward> OnAdRewarded;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event EventHandler<EventArgs> OnAdStarted;

        private RewardBasedVideoAd()
        {
            MethodInfo method = Type.GetType("GoogleMobileAds.GoogleMobileAdsClientFactory,Assembly-CSharp").GetMethod("BuildRewardBasedVideoAdClient", BindingFlags.Public | BindingFlags.Static);
            this.client = (IRewardBasedVideoAdClient) method.Invoke(null, null);
            this.client.CreateRewardBasedVideoAd();
            this.client.OnAdLoaded += new EventHandler<EventArgs>(this.<RewardBasedVideoAd>m__0);
            this.client.OnAdFailedToLoad += new EventHandler<AdFailedToLoadEventArgs>(this.<RewardBasedVideoAd>m__1);
            this.client.OnAdOpening += new EventHandler<EventArgs>(this.<RewardBasedVideoAd>m__2);
            this.client.OnAdStarted += new EventHandler<EventArgs>(this.<RewardBasedVideoAd>m__3);
            this.client.OnAdClosed += new EventHandler<EventArgs>(this.<RewardBasedVideoAd>m__4);
            this.client.OnAdLeavingApplication += new EventHandler<EventArgs>(this.<RewardBasedVideoAd>m__5);
            this.client.OnAdRewarded += new EventHandler<Reward>(this.<RewardBasedVideoAd>m__6);
        }

        [CompilerGenerated]
        private void <RewardBasedVideoAd>m__0(object sender, EventArgs args)
        {
            if (this.OnAdLoaded != null)
            {
                this.OnAdLoaded(this, args);
            }
        }

        [CompilerGenerated]
        private void <RewardBasedVideoAd>m__1(object sender, AdFailedToLoadEventArgs args)
        {
            if (this.OnAdFailedToLoad != null)
            {
                this.OnAdFailedToLoad(this, args);
            }
        }

        [CompilerGenerated]
        private void <RewardBasedVideoAd>m__2(object sender, EventArgs args)
        {
            if (this.OnAdOpening != null)
            {
                this.OnAdOpening(this, args);
            }
        }

        [CompilerGenerated]
        private void <RewardBasedVideoAd>m__3(object sender, EventArgs args)
        {
            if (this.OnAdStarted != null)
            {
                this.OnAdStarted(this, args);
            }
        }

        [CompilerGenerated]
        private void <RewardBasedVideoAd>m__4(object sender, EventArgs args)
        {
            if (this.OnAdClosed != null)
            {
                this.OnAdClosed(this, args);
            }
        }

        [CompilerGenerated]
        private void <RewardBasedVideoAd>m__5(object sender, EventArgs args)
        {
            if (this.OnAdLeavingApplication != null)
            {
                this.OnAdLeavingApplication(this, args);
            }
        }

        [CompilerGenerated]
        private void <RewardBasedVideoAd>m__6(object sender, Reward args)
        {
            if (this.OnAdRewarded != null)
            {
                this.OnAdRewarded(this, args);
            }
        }

        public bool IsLoaded() => 
            this.client.IsLoaded();

        public void LoadAd(AdRequest request, string adUnitId)
        {
            this.client.LoadAd(request, adUnitId);
        }

        public string MediationAdapterClassName() => 
            this.client.MediationAdapterClassName();

        public void SetUserId(string userId)
        {
            this.client.SetUserId(userId);
        }

        public void Show()
        {
            this.client.ShowRewardBasedVideoAd();
        }

        public static RewardBasedVideoAd Instance =>
            instance;
    }
}

