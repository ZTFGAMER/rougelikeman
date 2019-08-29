namespace GoogleMobileAds.Common
{
    using GoogleMobileAds.Api;
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using UnityEngine;

    public class DummyClient : IBannerClient, IInterstitialClient, IRewardBasedVideoAdClient, IAdLoaderClient, INativeExpressAdClient, IMobileAdsClient
    {
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

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event EventHandler<CustomNativeEventArgs> OnCustomNativeTemplateAdLoaded;

        public DummyClient()
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void CreateAdLoader(AdLoader.Builder builder)
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void CreateBannerView(string adUnitId, AdSize adSize, AdPosition position)
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void CreateBannerView(string adUnitId, AdSize adSize, int positionX, int positionY)
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void CreateInterstitialAd(string adUnitId)
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void CreateNativeExpressAdView(string adUnitId, AdSize adSize, AdPosition position)
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void CreateNativeExpressAdView(string adUnitId, AdSize adSize, int positionX, int positionY)
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void CreateRewardBasedVideoAd()
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void DestroyBannerView()
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void DestroyInterstitial()
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void DestroyNativeExpressAdView()
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void DestroyRewardBasedVideoAd()
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public float GetHeightInPixels()
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
            return 0f;
        }

        public float GetWidthInPixels()
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
            return 0f;
        }

        public void HideBannerView()
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void HideNativeExpressAdView()
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void Initialize(string appId)
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public bool IsLoaded()
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
            return true;
        }

        public void Load(AdRequest request)
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void LoadAd(AdRequest request)
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void LoadAd(AdRequest request, string adUnitId)
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public string MediationAdapterClassName()
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
            return null;
        }

        public void SetAdSize(AdSize adSize)
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void SetApplicationMuted(bool muted)
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void SetApplicationVolume(float volume)
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void SetiOSAppPauseOnBackground(bool pause)
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void SetPosition(AdPosition adPosition)
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void SetPosition(int x, int y)
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void SetUserId(string userId)
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void ShowBannerView()
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void ShowInterstitial()
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void ShowNativeExpressAdView()
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public void ShowRewardBasedVideoAd()
        {
            UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }

        public string UserId
        {
            get
            {
                UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
                return "UserId";
            }
            set => 
                UnityEngine.Debug.Log("Dummy " + MethodBase.GetCurrentMethod().Name);
        }
    }
}

