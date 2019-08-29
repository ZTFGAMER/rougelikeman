namespace GoogleMobileAds.Android
{
    using GoogleMobileAds.Api;
    using GoogleMobileAds.Common;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using UnityEngine;

    public class RewardBasedVideoAdClient : AndroidJavaProxy, IRewardBasedVideoAdClient
    {
        private AndroidJavaObject androidRewardBasedVideo;
        [CompilerGenerated]
        private static EventHandler<EventArgs> <>f__am$cache0;
        [CompilerGenerated]
        private static EventHandler<AdFailedToLoadEventArgs> <>f__am$cache1;
        [CompilerGenerated]
        private static EventHandler<EventArgs> <>f__am$cache2;
        [CompilerGenerated]
        private static EventHandler<EventArgs> <>f__am$cache3;
        [CompilerGenerated]
        private static EventHandler<EventArgs> <>f__am$cache4;
        [CompilerGenerated]
        private static EventHandler<Reward> <>f__am$cache5;
        [CompilerGenerated]
        private static EventHandler<EventArgs> <>f__am$cache6;

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

        public RewardBasedVideoAdClient() : base("com.google.unity.ads.UnityRewardBasedVideoAdListener")
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = new EventHandler<EventArgs>(RewardBasedVideoAdClient.<OnAdLoaded>m__0);
            }
            this.OnAdLoaded = <>f__am$cache0;
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = new EventHandler<AdFailedToLoadEventArgs>(RewardBasedVideoAdClient.<OnAdFailedToLoad>m__1);
            }
            this.OnAdFailedToLoad = <>f__am$cache1;
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = new EventHandler<EventArgs>(RewardBasedVideoAdClient.<OnAdOpening>m__2);
            }
            this.OnAdOpening = <>f__am$cache2;
            if (<>f__am$cache3 == null)
            {
                <>f__am$cache3 = new EventHandler<EventArgs>(RewardBasedVideoAdClient.<OnAdStarted>m__3);
            }
            this.OnAdStarted = <>f__am$cache3;
            if (<>f__am$cache4 == null)
            {
                <>f__am$cache4 = new EventHandler<EventArgs>(RewardBasedVideoAdClient.<OnAdClosed>m__4);
            }
            this.OnAdClosed = <>f__am$cache4;
            if (<>f__am$cache5 == null)
            {
                <>f__am$cache5 = new EventHandler<Reward>(RewardBasedVideoAdClient.<OnAdRewarded>m__5);
            }
            this.OnAdRewarded = <>f__am$cache5;
            if (<>f__am$cache6 == null)
            {
                <>f__am$cache6 = new EventHandler<EventArgs>(RewardBasedVideoAdClient.<OnAdLeavingApplication>m__6);
            }
            this.OnAdLeavingApplication = <>f__am$cache6;
            AndroidJavaObject @static = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            object[] args = new object[] { @static, this };
            this.androidRewardBasedVideo = new AndroidJavaObject("com.google.unity.ads.RewardBasedVideo", args);
        }

        [CompilerGenerated]
        private static void <OnAdClosed>m__4(object, EventArgs)
        {
        }

        [CompilerGenerated]
        private static void <OnAdFailedToLoad>m__1(object, AdFailedToLoadEventArgs)
        {
        }

        [CompilerGenerated]
        private static void <OnAdLeavingApplication>m__6(object, EventArgs)
        {
        }

        [CompilerGenerated]
        private static void <OnAdLoaded>m__0(object, EventArgs)
        {
        }

        [CompilerGenerated]
        private static void <OnAdOpening>m__2(object, EventArgs)
        {
        }

        [CompilerGenerated]
        private static void <OnAdRewarded>m__5(object, Reward)
        {
        }

        [CompilerGenerated]
        private static void <OnAdStarted>m__3(object, EventArgs)
        {
        }

        public void CreateRewardBasedVideoAd()
        {
            this.androidRewardBasedVideo.Call("create", new object[0]);
        }

        public void DestroyRewardBasedVideoAd()
        {
            this.androidRewardBasedVideo.Call("destroy", new object[0]);
        }

        public bool IsLoaded() => 
            this.androidRewardBasedVideo.Call<bool>("isLoaded", new object[0]);

        public void LoadAd(AdRequest request, string adUnitId)
        {
            object[] args = new object[] { GoogleMobileAds.Android.Utils.GetAdRequestJavaObject(request), adUnitId };
            this.androidRewardBasedVideo.Call("loadAd", args);
        }

        public string MediationAdapterClassName() => 
            this.androidRewardBasedVideo.Call<string>("getMediationAdapterClassName", new object[0]);

        private void onAdClosed()
        {
            if (this.OnAdClosed != null)
            {
                this.OnAdClosed(this, EventArgs.Empty);
            }
        }

        private void onAdFailedToLoad(string errorReason)
        {
            if (this.OnAdFailedToLoad != null)
            {
                AdFailedToLoadEventArgs e = new AdFailedToLoadEventArgs {
                    Message = errorReason
                };
                this.OnAdFailedToLoad(this, e);
            }
        }

        private void onAdLeftApplication()
        {
            if (this.OnAdLeavingApplication != null)
            {
                this.OnAdLeavingApplication(this, EventArgs.Empty);
            }
        }

        private void onAdLoaded()
        {
            if (this.OnAdLoaded != null)
            {
                this.OnAdLoaded(this, EventArgs.Empty);
            }
        }

        private void onAdOpened()
        {
            if (this.OnAdOpening != null)
            {
                this.OnAdOpening(this, EventArgs.Empty);
            }
        }

        private void onAdRewarded(string type, float amount)
        {
            if (this.OnAdRewarded != null)
            {
                Reward e = new Reward {
                    Type = type,
                    Amount = amount
                };
                this.OnAdRewarded(this, e);
            }
        }

        private void onAdStarted()
        {
            if (this.OnAdStarted != null)
            {
                this.OnAdStarted(this, EventArgs.Empty);
            }
        }

        public void SetUserId(string userId)
        {
            object[] args = new object[] { userId };
            this.androidRewardBasedVideo.Call("setUserId", args);
        }

        public void ShowRewardBasedVideoAd()
        {
            this.androidRewardBasedVideo.Call("show", new object[0]);
        }
    }
}

