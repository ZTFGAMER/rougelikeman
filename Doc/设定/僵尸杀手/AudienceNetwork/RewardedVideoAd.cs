namespace AudienceNetwork
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public sealed class RewardedVideoAd : IDisposable
    {
        private int uniqueId;
        private bool isLoaded;
        private AdHandler handler;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <PlacementId>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AudienceNetwork.RewardData <RewardData>k__BackingField;
        public FBRewardedVideoAdBridgeCallback rewardedVideoAdDidLoad;
        public FBRewardedVideoAdBridgeCallback rewardedVideoAdWillLogImpression;
        public FBRewardedVideoAdBridgeErrorCallback rewardedVideoAdDidFailWithError;
        public FBRewardedVideoAdBridgeCallback rewardedVideoAdDidClick;
        public FBRewardedVideoAdBridgeCallback rewardedVideoAdWillClose;
        public FBRewardedVideoAdBridgeCallback rewardedVideoAdDidClose;
        public FBRewardedVideoAdBridgeCallback rewardedVideoAdComplete;
        public FBRewardedVideoAdBridgeCallback rewardedVideoAdDidSucceed;
        public FBRewardedVideoAdBridgeCallback rewardedVideoAdDidFail;

        public RewardedVideoAd(string placementId) : this(placementId, null)
        {
        }

        public RewardedVideoAd(string placementId, AudienceNetwork.RewardData rewardData)
        {
            this.PlacementId = placementId;
            this.RewardData = rewardData;
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                this.uniqueId = RewardedVideoAdBridge.Instance.Create(placementId, this.RewardData, this);
                RewardedVideoAdBridge.Instance.OnLoad(this.uniqueId, this.RewardedVideoAdDidLoad);
                RewardedVideoAdBridge.Instance.OnImpression(this.uniqueId, this.RewardedVideoAdWillLogImpression);
                RewardedVideoAdBridge.Instance.OnClick(this.uniqueId, this.RewardedVideoAdDidClick);
                RewardedVideoAdBridge.Instance.OnError(this.uniqueId, this.RewardedVideoAdDidFailWithError);
                RewardedVideoAdBridge.Instance.OnWillClose(this.uniqueId, this.RewardedVideoAdWillClose);
                RewardedVideoAdBridge.Instance.OnDidClose(this.uniqueId, this.RewardedVideoAdDidClose);
                RewardedVideoAdBridge.Instance.OnComplete(this.uniqueId, this.RewardedVideoAdComplete);
                RewardedVideoAdBridge.Instance.OnDidSucceed(this.uniqueId, this.RewardedVideoAdDidSucceed);
                RewardedVideoAdBridge.Instance.OnDidFail(this.uniqueId, this.RewardedVideoAdDidFail);
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
            UnityEngine.Debug.Log("RewardedVideo Ad Disposed.");
            RewardedVideoAdBridge.Instance.Release(this.uniqueId);
        }

        internal void executeOnMainThread(Action action)
        {
            if (this.handler != null)
            {
                this.handler.executeOnMainThread(action);
            }
        }

        ~RewardedVideoAd()
        {
            this.Dispose(false);
        }

        public bool IsValid()
        {
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                return (this.isLoaded && RewardedVideoAdBridge.Instance.IsValid(this.uniqueId));
            }
            return true;
        }

        public void LoadAd()
        {
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                RewardedVideoAdBridge.Instance.Load(this.uniqueId);
            }
            else
            {
                this.RewardedVideoAdDidLoad();
            }
        }

        internal void loadAdFromData()
        {
            this.isLoaded = true;
        }

        public static implicit operator bool(RewardedVideoAd obj) => 
            !object.ReferenceEquals(obj, null);

        public void Register(GameObject gameObject)
        {
            this.handler = gameObject.AddComponent<AdHandler>();
        }

        public bool Show() => 
            RewardedVideoAdBridge.Instance.Show(this.uniqueId);

        public override string ToString() => 
            $"[RewardedVideoAd: PlacementId={this.PlacementId}, RewardedVideoAdDidLoad={this.RewardedVideoAdDidLoad}, RewardedVideoAdWillLogImpression={this.RewardedVideoAdWillLogImpression}, RewardedVideoAdDidFailWithError={this.RewardedVideoAdDidFailWithError}, RewardedVideoAdDidClick={this.RewardedVideoAdDidClick}, RewardedVideoAdWillClose={this.RewardedVideoAdWillClose}, RewardedVideoAdDidClose={this.RewardedVideoAdDidClose}, RewardedVideoAdComplete={this.RewardedVideoAdComplete}, RewardedVideoAdDidSucceed={this.RewardedVideoAdDidSucceed}, RewardedVideoAdDidFail={this.RewardedVideoAdDidFail}]";

        public string PlacementId { get; private set; }

        public AudienceNetwork.RewardData RewardData { get; private set; }

        public FBRewardedVideoAdBridgeCallback RewardedVideoAdDidLoad
        {
            internal get => 
                this.rewardedVideoAdDidLoad;
            set
            {
                this.rewardedVideoAdDidLoad = value;
                RewardedVideoAdBridge.Instance.OnLoad(this.uniqueId, this.rewardedVideoAdDidLoad);
            }
        }

        public FBRewardedVideoAdBridgeCallback RewardedVideoAdWillLogImpression
        {
            internal get => 
                this.rewardedVideoAdWillLogImpression;
            set
            {
                this.rewardedVideoAdWillLogImpression = value;
                RewardedVideoAdBridge.Instance.OnImpression(this.uniqueId, this.rewardedVideoAdWillLogImpression);
            }
        }

        public FBRewardedVideoAdBridgeErrorCallback RewardedVideoAdDidFailWithError
        {
            internal get => 
                this.rewardedVideoAdDidFailWithError;
            set
            {
                this.rewardedVideoAdDidFailWithError = value;
                RewardedVideoAdBridge.Instance.OnError(this.uniqueId, this.rewardedVideoAdDidFailWithError);
            }
        }

        public FBRewardedVideoAdBridgeCallback RewardedVideoAdDidClick
        {
            internal get => 
                this.rewardedVideoAdDidClick;
            set
            {
                this.rewardedVideoAdDidClick = value;
                RewardedVideoAdBridge.Instance.OnClick(this.uniqueId, this.rewardedVideoAdDidClick);
            }
        }

        public FBRewardedVideoAdBridgeCallback RewardedVideoAdWillClose
        {
            internal get => 
                this.rewardedVideoAdWillClose;
            set
            {
                this.rewardedVideoAdWillClose = value;
                RewardedVideoAdBridge.Instance.OnWillClose(this.uniqueId, this.rewardedVideoAdWillClose);
            }
        }

        public FBRewardedVideoAdBridgeCallback RewardedVideoAdDidClose
        {
            internal get => 
                this.rewardedVideoAdDidClose;
            set
            {
                this.rewardedVideoAdDidClose = value;
                RewardedVideoAdBridge.Instance.OnDidClose(this.uniqueId, this.rewardedVideoAdDidClose);
            }
        }

        public FBRewardedVideoAdBridgeCallback RewardedVideoAdComplete
        {
            internal get => 
                this.rewardedVideoAdComplete;
            set
            {
                this.rewardedVideoAdComplete = value;
                RewardedVideoAdBridge.Instance.OnComplete(this.uniqueId, this.rewardedVideoAdComplete);
            }
        }

        public FBRewardedVideoAdBridgeCallback RewardedVideoAdDidSucceed
        {
            internal get => 
                this.rewardedVideoAdDidSucceed;
            set
            {
                this.rewardedVideoAdDidSucceed = value;
                RewardedVideoAdBridge.Instance.OnDidSucceed(this.uniqueId, this.rewardedVideoAdDidSucceed);
            }
        }

        public FBRewardedVideoAdBridgeCallback RewardedVideoAdDidFail
        {
            internal get => 
                this.rewardedVideoAdDidFail;
            set
            {
                this.rewardedVideoAdDidFail = value;
                RewardedVideoAdBridge.Instance.OnDidFail(this.uniqueId, this.rewardedVideoAdDidFail);
            }
        }
    }
}

