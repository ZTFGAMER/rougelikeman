namespace AudienceNetwork
{
    using AudienceNetwork.Utility;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public sealed class AdView : IDisposable
    {
        private int uniqueId;
        private AdSize size;
        private AdHandler handler;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <PlacementId>k__BackingField;
        public FBAdViewBridgeCallback adViewDidLoad;
        public FBAdViewBridgeCallback adViewWillLogImpression;
        public FBAdViewBridgeErrorCallback adViewDidFailWithError;
        public FBAdViewBridgeCallback adViewDidClick;
        public FBAdViewBridgeCallback adViewDidFinishClick;

        public AdView(string placementId, AdSize size)
        {
            this.PlacementId = placementId;
            this.size = size;
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                this.uniqueId = AdViewBridge.Instance.Create(placementId, this, size);
                AdViewBridge.Instance.OnLoad(this.uniqueId, this.AdViewDidLoad);
                AdViewBridge.Instance.OnImpression(this.uniqueId, this.AdViewWillLogImpression);
                AdViewBridge.Instance.OnClick(this.uniqueId, this.AdViewDidClick);
                AdViewBridge.Instance.OnError(this.uniqueId, this.AdViewDidFailWithError);
                AdViewBridge.Instance.OnFinishedClick(this.uniqueId, this.AdViewDidFinishClick);
            }
        }

        public void DisableAutoRefresh()
        {
            AdViewBridge.Instance.DisableAutoRefresh(this.uniqueId);
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
            UnityEngine.Debug.Log("Banner Ad Disposed.");
            AdViewBridge.Instance.Release(this.uniqueId);
        }

        internal void executeOnMainThread(Action action)
        {
            if (this.handler != null)
            {
                this.handler.executeOnMainThread(action);
            }
        }

        ~AdView()
        {
            this.Dispose(false);
        }

        private double heightFromType(AdSize size)
        {
            if (size != AdSize.BANNER_HEIGHT_50)
            {
                if (size == AdSize.BANNER_HEIGHT_90)
                {
                    return 90.0;
                }
                if (size == AdSize.RECTANGLE_HEIGHT_250)
                {
                    return 250.0;
                }
                return 0.0;
            }
            return 50.0;
        }

        public void LoadAd()
        {
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                AdViewBridge.Instance.Load(this.uniqueId);
            }
            else
            {
                this.AdViewDidLoad();
            }
        }

        public static implicit operator bool(AdView obj) => 
            !object.ReferenceEquals(obj, null);

        public void Register(GameObject gameObject)
        {
            this.handler = gameObject.AddComponent<AdHandler>();
        }

        public bool Show(AdPosition position)
        {
            double y = 0.0;
            if (position != AdPosition.TOP)
            {
                if (position != AdPosition.BOTTOM)
                {
                    if (position == AdPosition.CUSTOM)
                    {
                        UnityEngine.Debug.LogWarning("Use Show(double y) instead");
                    }
                }
                else
                {
                    y = AdUtility.height() - this.heightFromType(this.size);
                }
            }
            return this.Show(y);
        }

        public bool Show(double y) => 
            AdViewBridge.Instance.Show(this.uniqueId, 0.0, y, AdUtility.width(), this.heightFromType(this.size));

        public bool Show(double x, double y) => 
            AdViewBridge.Instance.Show(this.uniqueId, x, y, AdUtility.width(), this.heightFromType(this.size));

        private bool Show(double x, double y, double width, double height) => 
            AdViewBridge.Instance.Show(this.uniqueId, x, y, width, height);

        public override string ToString() => 
            $"[AdView: PlacementId={this.PlacementId}, AdViewDidLoad={this.AdViewDidLoad}, AdViewWillLogImpression={this.AdViewWillLogImpression}, AdViewDidFailWithError={this.AdViewDidFailWithError}, AdViewDidClick={this.AdViewDidClick}, adViewDidFinishClick={this.adViewDidFinishClick}]";

        public string PlacementId { get; private set; }

        public FBAdViewBridgeCallback AdViewDidLoad
        {
            internal get => 
                this.adViewDidLoad;
            set
            {
                this.adViewDidLoad = value;
                AdViewBridge.Instance.OnLoad(this.uniqueId, this.adViewDidLoad);
            }
        }

        public FBAdViewBridgeCallback AdViewWillLogImpression
        {
            internal get => 
                this.adViewWillLogImpression;
            set
            {
                this.adViewWillLogImpression = value;
                AdViewBridge.Instance.OnImpression(this.uniqueId, this.adViewWillLogImpression);
            }
        }

        public FBAdViewBridgeErrorCallback AdViewDidFailWithError
        {
            internal get => 
                this.adViewDidFailWithError;
            set
            {
                this.adViewDidFailWithError = value;
                AdViewBridge.Instance.OnError(this.uniqueId, this.adViewDidFailWithError);
            }
        }

        public FBAdViewBridgeCallback AdViewDidClick
        {
            internal get => 
                this.adViewDidClick;
            set
            {
                this.adViewDidClick = value;
                AdViewBridge.Instance.OnClick(this.uniqueId, this.adViewDidClick);
            }
        }

        public FBAdViewBridgeCallback AdViewDidFinishClick
        {
            internal get => 
                this.adViewDidFinishClick;
            set
            {
                this.adViewDidFinishClick = value;
                AdViewBridge.Instance.OnFinishedClick(this.uniqueId, this.adViewDidFinishClick);
            }
        }
    }
}

