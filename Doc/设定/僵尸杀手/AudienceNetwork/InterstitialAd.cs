namespace AudienceNetwork
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public sealed class InterstitialAd : IDisposable
    {
        private int uniqueId;
        private bool isLoaded;
        private AdHandler handler;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <PlacementId>k__BackingField;
        public FBInterstitialAdBridgeCallback interstitialAdDidLoad;
        public FBInterstitialAdBridgeCallback interstitialAdWillLogImpression;
        public FBInterstitialAdBridgeErrorCallback interstitialAdDidFailWithError;
        public FBInterstitialAdBridgeCallback interstitialAdDidClick;
        public FBInterstitialAdBridgeCallback interstitialAdWillClose;
        public FBInterstitialAdBridgeCallback interstitialAdDidClose;

        public InterstitialAd(string placementId)
        {
            this.PlacementId = placementId;
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                this.uniqueId = InterstitialAdBridge.Instance.Create(placementId, this);
                InterstitialAdBridge.Instance.OnLoad(this.uniqueId, this.InterstitialAdDidLoad);
                InterstitialAdBridge.Instance.OnImpression(this.uniqueId, this.InterstitialAdWillLogImpression);
                InterstitialAdBridge.Instance.OnClick(this.uniqueId, this.InterstitialAdDidClick);
                InterstitialAdBridge.Instance.OnError(this.uniqueId, this.InterstitialAdDidFailWithError);
                InterstitialAdBridge.Instance.OnWillClose(this.uniqueId, this.InterstitialAdWillClose);
                InterstitialAdBridge.Instance.OnDidClose(this.uniqueId, this.InterstitialAdDidClose);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool iAmBeingCalledFromDisposeAndNotFinalize)
        {
            if (this.handler != null)
            {
                this.handler.removeFromParent();
            }
            UnityEngine.Debug.Log("Interstitial Ad Disposed.");
            InterstitialAdBridge.Instance.Release(this.uniqueId);
        }

        internal void executeOnMainThread(Action action)
        {
            if (this.handler != null)
            {
                this.handler.executeOnMainThread(action);
            }
        }

        ~InterstitialAd()
        {
            this.Dispose(false);
        }

        public bool IsValid()
        {
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                return (this.isLoaded && InterstitialAdBridge.Instance.IsValid(this.uniqueId));
            }
            return true;
        }

        public void LoadAd()
        {
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                InterstitialAdBridge.Instance.Load(this.uniqueId);
            }
            else
            {
                this.InterstitialAdDidLoad();
            }
        }

        internal void loadAdFromData()
        {
            this.isLoaded = true;
        }

        public static implicit operator bool(InterstitialAd obj) => 
            !object.ReferenceEquals(obj, null);

        public void Register(GameObject gameObject)
        {
            this.handler = gameObject.AddComponent<AdHandler>();
        }

        public bool Show() => 
            InterstitialAdBridge.Instance.Show(this.uniqueId);

        public override string ToString() => 
            $"[InterstitialAd: PlacementId={this.PlacementId}, InterstitialAdDidLoad={this.InterstitialAdDidLoad}, InterstitialAdWillLogImpression={this.InterstitialAdWillLogImpression}, InterstitialAdDidFailWithError={this.InterstitialAdDidFailWithError}, InterstitialAdDidClick={this.InterstitialAdDidClick}, InterstitialAdWillClose={this.InterstitialAdWillClose}, InterstitialAdDidClose={this.InterstitialAdDidClose}]";

        public string PlacementId { get; private set; }

        public FBInterstitialAdBridgeCallback InterstitialAdDidLoad
        {
            internal get => 
                this.interstitialAdDidLoad;
            set
            {
                this.interstitialAdDidLoad = value;
                InterstitialAdBridge.Instance.OnLoad(this.uniqueId, this.interstitialAdDidLoad);
            }
        }

        public FBInterstitialAdBridgeCallback InterstitialAdWillLogImpression
        {
            internal get => 
                this.interstitialAdWillLogImpression;
            set
            {
                this.interstitialAdWillLogImpression = value;
                InterstitialAdBridge.Instance.OnImpression(this.uniqueId, this.interstitialAdWillLogImpression);
            }
        }

        public FBInterstitialAdBridgeErrorCallback InterstitialAdDidFailWithError
        {
            internal get => 
                this.interstitialAdDidFailWithError;
            set
            {
                this.interstitialAdDidFailWithError = value;
                InterstitialAdBridge.Instance.OnError(this.uniqueId, this.interstitialAdDidFailWithError);
            }
        }

        public FBInterstitialAdBridgeCallback InterstitialAdDidClick
        {
            internal get => 
                this.interstitialAdDidClick;
            set
            {
                this.interstitialAdDidClick = value;
                InterstitialAdBridge.Instance.OnClick(this.uniqueId, this.interstitialAdDidClick);
            }
        }

        public FBInterstitialAdBridgeCallback InterstitialAdWillClose
        {
            internal get => 
                this.interstitialAdWillClose;
            set
            {
                this.interstitialAdWillClose = value;
                InterstitialAdBridge.Instance.OnWillClose(this.uniqueId, this.interstitialAdWillClose);
            }
        }

        public FBInterstitialAdBridgeCallback InterstitialAdDidClose
        {
            internal get => 
                this.interstitialAdDidClose;
            set
            {
                this.interstitialAdDidClose = value;
                InterstitialAdBridge.Instance.OnDidClose(this.uniqueId, this.interstitialAdDidClose);
            }
        }
    }
}

