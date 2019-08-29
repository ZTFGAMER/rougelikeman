namespace AudienceNetwork
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;
    using UnityEngine.UI;

    public sealed class NativeAd : IDisposable
    {
        private int uniqueId;
        private bool isLoaded;
        private int minViewabilityPercentage;
        internal const float MIN_ALPHA = 0.9f;
        internal const int MAX_ROTATION = 0x2d;
        internal const int CHECK_VIEWABILITY_INTERVAL = 1;
        private NativeAdHandler handler;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <PlacementId>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Title>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Subtitle>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <Body>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <CallToAction>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <SocialContext>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <IconImageURL>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <CoverImageURL>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <AdChoicesImageURL>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Sprite <IconImage>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Sprite <CoverImage>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Sprite <AdChoicesImage>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <AdChoicesText>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <AdChoicesLinkURL>k__BackingField;
        private FBNativeAdBridgeCallback nativeAdDidLoad;
        private FBNativeAdBridgeCallback nativeAdWillLogImpression;
        private FBNativeAdBridgeErrorCallback nativeAdDidFailWithError;
        private FBNativeAdBridgeCallback nativeAdDidClick;
        private FBNativeAdBridgeCallback nativeAdDidFinishHandlingClick;

        public NativeAd(string placementId)
        {
            this.PlacementId = placementId;
            this.uniqueId = NativeAdBridge.Instance.Create(placementId, this);
            NativeAdBridge.Instance.OnLoad(this.uniqueId, this.NativeAdDidLoad);
            NativeAdBridge.Instance.OnImpression(this.uniqueId, this.NativeAdWillLogImpression);
            NativeAdBridge.Instance.OnClick(this.uniqueId, this.NativeAdDidClick);
            NativeAdBridge.Instance.OnError(this.uniqueId, this.NativeAdDidFailWithError);
            NativeAdBridge.Instance.OnFinishedClick(this.uniqueId, this.NativeAdDidFinishHandlingClick);
        }

        private void createHandler(GameObject gameObject)
        {
            this.createHandler(null, gameObject);
        }

        private void createHandler(Camera camera, GameObject gameObject)
        {
            this.handler = gameObject.AddComponent<NativeAdHandler>();
            this.handler.camera = camera;
            this.handler.minAlpha = 0.9f;
            this.handler.maxRotation = 0x2d;
            this.handler.checkViewabilityInterval = 1;
            this.handler.validationCallback = delegate (bool success) {
                UnityEngine.Debug.Log(string.Concat(new object[] { "Native ad viewability check for unique id ", this.uniqueId, " returned success? ", success }));
                if (success)
                {
                    AdLogger.Log("Native ad with unique id " + this.uniqueId + " registered impression!");
                    this.ExternalLogImpression();
                    this.handler.stopImpressionValidation();
                }
            };
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
                this.handler.stopImpressionValidation();
                this.handler.removeFromParent();
            }
            UnityEngine.Debug.Log("Native Ad Disposed.");
            NativeAdBridge.Instance.Release(this.uniqueId);
        }

        internal void executeOnMainThread(Action action)
        {
            if (this.handler != null)
            {
                this.handler.executeOnMainThread(action);
            }
        }

        internal void ExternalClick()
        {
            NativeAdBridge.Instance.ExternalLogClick(this.uniqueId);
        }

        internal void ExternalLogImpression()
        {
            NativeAdBridge.Instance.ExternalLogImpression(this.uniqueId);
        }

        ~NativeAd()
        {
            this.Dispose(false);
        }

        private static TextureFormat imageFormat() => 
            TextureFormat.RGBA32;

        public bool IsValid() => 
            (this.isLoaded && NativeAdBridge.Instance.IsValid(this.uniqueId));

        public void LoadAd()
        {
            NativeAdBridge.Instance.Load(this.uniqueId);
        }

        [DebuggerHidden]
        public IEnumerator LoadAdChoicesImage(string url) => 
            new <LoadAdChoicesImage>c__Iterator2 { 
                url = url,
                $this = this
            };

        internal void loadAdFromData()
        {
            if (this.handler == null)
            {
                throw new InvalidOperationException("Native ad was loaded before it was registered. Ensure RegisterGameObjectForImpression () are called.");
            }
            int uniqueId = this.uniqueId;
            this.Title = NativeAdBridge.Instance.GetTitle(uniqueId);
            this.Subtitle = NativeAdBridge.Instance.GetSubtitle(uniqueId);
            this.Body = NativeAdBridge.Instance.GetBody(uniqueId);
            this.CallToAction = NativeAdBridge.Instance.GetCallToAction(uniqueId);
            this.SocialContext = NativeAdBridge.Instance.GetSocialContext(uniqueId);
            this.CoverImageURL = NativeAdBridge.Instance.GetCoverImageURL(uniqueId);
            this.IconImageURL = NativeAdBridge.Instance.GetIconImageURL(uniqueId);
            this.AdChoicesImageURL = NativeAdBridge.Instance.GetAdChoicesImageURL(uniqueId);
            this.AdChoicesText = NativeAdBridge.Instance.GetAdChoicesText(uniqueId);
            this.AdChoicesLinkURL = NativeAdBridge.Instance.GetAdChoicesLinkURL(uniqueId);
            this.isLoaded = true;
            this.minViewabilityPercentage = NativeAdBridge.Instance.GetMinViewabilityPercentage(uniqueId);
            this.handler.minViewabilityPercentage = this.minViewabilityPercentage;
            if (this.NativeAdDidLoad != null)
            {
                this.handler.executeOnMainThread(() => this.NativeAdDidLoad());
            }
            this.handler.executeOnMainThread(() => this.handler.startImpressionValidation());
        }

        [DebuggerHidden]
        public IEnumerator LoadCoverImage(string url) => 
            new <LoadCoverImage>c__Iterator1 { 
                url = url,
                $this = this
            };

        [DebuggerHidden]
        public IEnumerator LoadIconImage(string url) => 
            new <LoadIconImage>c__Iterator0 { 
                url = url,
                $this = this
            };

        public static implicit operator bool(NativeAd obj) => 
            !object.ReferenceEquals(obj, null);

        public void RegisterGameObjectForImpression(GameObject gameObject, Button[] clickableButtons)
        {
            this.RegisterGameObjectForImpression(gameObject, clickableButtons, Camera.main);
        }

        public void RegisterGameObjectForImpression(GameObject gameObject, Button[] clickableButtons, Camera camera)
        {
            foreach (Button button in clickableButtons)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(delegate {
                    AdLogger.Log("Native ad with unique id " + this.uniqueId + " clicked!");
                    this.ExternalClick();
                });
            }
            if (this.handler != null)
            {
                this.handler.stopImpressionValidation();
                this.handler.removeFromParent();
                this.createHandler(camera, gameObject);
                this.handler.startImpressionValidation();
            }
            else
            {
                this.createHandler(camera, gameObject);
            }
        }

        public override string ToString() => 
            $"[NativeAd: PlacementId={this.PlacementId}, Title={this.Title}, Subtitle={this.Subtitle}, Body={this.Body}, CallToAction={this.CallToAction}, SocialContext={this.SocialContext}, IconImageURL={this.IconImageURL}, CoverImageURL={this.CoverImageURL}, IconImage={this.IconImage}, CoverImage={this.CoverImage}, NativeAdDidLoad={this.NativeAdDidLoad}, NativeAdWillLogImpression={this.NativeAdWillLogImpression}, NativeAdDidFailWithError={this.NativeAdDidFailWithError}, NativeAdDidClick={this.NativeAdDidClick}, NativeAdDidFinishHandlingClick={this.NativeAdDidFinishHandlingClick}]";

        public string PlacementId { get; private set; }

        public string Title { get; private set; }

        public string Subtitle { get; private set; }

        public string Body { get; private set; }

        public string CallToAction { get; private set; }

        public string SocialContext { get; private set; }

        public string IconImageURL { get; private set; }

        public string CoverImageURL { get; private set; }

        public string AdChoicesImageURL { get; private set; }

        public Sprite IconImage { get; private set; }

        public Sprite CoverImage { get; private set; }

        public Sprite AdChoicesImage { get; private set; }

        public string AdChoicesText { get; private set; }

        public string AdChoicesLinkURL { get; private set; }

        public FBNativeAdBridgeCallback NativeAdDidLoad
        {
            internal get => 
                this.nativeAdDidLoad;
            set
            {
                this.nativeAdDidLoad = value;
                NativeAdBridge.Instance.OnLoad(this.uniqueId, this.nativeAdDidLoad);
            }
        }

        public FBNativeAdBridgeCallback NativeAdWillLogImpression
        {
            internal get => 
                this.nativeAdWillLogImpression;
            set
            {
                this.nativeAdWillLogImpression = value;
                NativeAdBridge.Instance.OnImpression(this.uniqueId, this.nativeAdWillLogImpression);
            }
        }

        public FBNativeAdBridgeErrorCallback NativeAdDidFailWithError
        {
            internal get => 
                this.nativeAdDidFailWithError;
            set
            {
                this.nativeAdDidFailWithError = value;
                NativeAdBridge.Instance.OnError(this.uniqueId, this.nativeAdDidFailWithError);
            }
        }

        public FBNativeAdBridgeCallback NativeAdDidClick
        {
            internal get => 
                this.nativeAdDidClick;
            set
            {
                this.nativeAdDidClick = value;
                NativeAdBridge.Instance.OnClick(this.uniqueId, this.nativeAdDidClick);
            }
        }

        public FBNativeAdBridgeCallback NativeAdDidFinishHandlingClick
        {
            internal get => 
                this.nativeAdDidFinishHandlingClick;
            set
            {
                this.nativeAdDidFinishHandlingClick = value;
                NativeAdBridge.Instance.OnFinishedClick(this.uniqueId, this.nativeAdDidFinishHandlingClick);
            }
        }

        [CompilerGenerated]
        private sealed class <LoadAdChoicesImage>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal Texture2D <texture>__0;
            internal string url;
            internal WWW <www>__0;
            internal NativeAd $this;
            internal object $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<texture>__0 = new Texture2D(4, 4, NativeAd.imageFormat(), false);
                        this.<www>__0 = new WWW(this.url);
                        this.$current = this.<www>__0;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        this.<www>__0.LoadImageIntoTexture(this.<texture>__0);
                        if (this.<texture>__0 != null)
                        {
                            this.$this.AdChoicesImage = Sprite.Create(this.<texture>__0, new Rect(0f, 0f, (float) this.<texture>__0.width, (float) this.<texture>__0.height), new Vector2(0.5f, 0.5f));
                        }
                        this.$PC = -1;
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }

        [CompilerGenerated]
        private sealed class <LoadCoverImage>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal Texture2D <texture>__0;
            internal string url;
            internal WWW <www>__0;
            internal NativeAd $this;
            internal object $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<texture>__0 = new Texture2D(4, 4, NativeAd.imageFormat(), false);
                        this.<www>__0 = new WWW(this.url);
                        this.$current = this.<www>__0;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        this.<www>__0.LoadImageIntoTexture(this.<texture>__0);
                        if (this.<texture>__0 != null)
                        {
                            this.$this.CoverImage = Sprite.Create(this.<texture>__0, new Rect(0f, 0f, (float) this.<texture>__0.width, (float) this.<texture>__0.height), new Vector2(0.5f, 0.5f));
                        }
                        this.$PC = -1;
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }

        [CompilerGenerated]
        private sealed class <LoadIconImage>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal Texture2D <texture>__0;
            internal string url;
            internal WWW <www>__0;
            internal NativeAd $this;
            internal object $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.<texture>__0 = new Texture2D(4, 4, NativeAd.imageFormat(), false);
                        this.<www>__0 = new WWW(this.url);
                        this.$current = this.<www>__0;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        this.<www>__0.LoadImageIntoTexture(this.<texture>__0);
                        if (this.<texture>__0 != null)
                        {
                            this.$this.IconImage = Sprite.Create(this.<texture>__0, new Rect(0f, 0f, (float) this.<texture>__0.width, (float) this.<texture>__0.height), new Vector2(0.5f, 0.5f));
                        }
                        this.$PC = -1;
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

