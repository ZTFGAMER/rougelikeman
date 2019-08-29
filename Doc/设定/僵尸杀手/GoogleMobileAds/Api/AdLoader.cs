namespace GoogleMobileAds.Api
{
    using GoogleMobileAds.Common;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class AdLoader
    {
        private IAdLoaderClient adLoaderClient;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<string, Action<CustomNativeTemplateAd, string>> <CustomNativeTemplateClickHandlers>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string <AdUnitId>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private HashSet<NativeAdType> <AdTypes>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private HashSet<string> <TemplateIds>k__BackingField;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event EventHandler<CustomNativeEventArgs> OnCustomNativeTemplateAdLoaded;

        private AdLoader(Builder builder)
        {
            this.AdUnitId = string.Copy(builder.AdUnitId);
            this.CustomNativeTemplateClickHandlers = new Dictionary<string, Action<CustomNativeTemplateAd, string>>(builder.CustomNativeTemplateClickHandlers);
            this.TemplateIds = new HashSet<string>(builder.TemplateIds);
            this.AdTypes = new HashSet<NativeAdType>(builder.AdTypes);
            MethodInfo method = Type.GetType("GoogleMobileAds.GoogleMobileAdsClientFactory,Assembly-CSharp").GetMethod("BuildAdLoaderClient", BindingFlags.Public | BindingFlags.Static);
            object[] parameters = new object[] { this };
            this.adLoaderClient = (IAdLoaderClient) method.Invoke(null, parameters);
            this.adLoaderClient.OnCustomNativeTemplateAdLoaded += new EventHandler<CustomNativeEventArgs>(this.<AdLoader>m__0);
            this.adLoaderClient.OnAdFailedToLoad += new EventHandler<AdFailedToLoadEventArgs>(this.<AdLoader>m__1);
        }

        [CompilerGenerated]
        private void <AdLoader>m__0(object sender, CustomNativeEventArgs args)
        {
            this.OnCustomNativeTemplateAdLoaded(this, args);
        }

        [CompilerGenerated]
        private void <AdLoader>m__1(object sender, AdFailedToLoadEventArgs args)
        {
            if (this.OnAdFailedToLoad != null)
            {
                this.OnAdFailedToLoad(this, args);
            }
        }

        public void LoadAd(AdRequest request)
        {
            this.adLoaderClient.LoadAd(request);
        }

        public Dictionary<string, Action<CustomNativeTemplateAd, string>> CustomNativeTemplateClickHandlers { get; private set; }

        public string AdUnitId { get; private set; }

        public HashSet<NativeAdType> AdTypes { get; private set; }

        public HashSet<string> TemplateIds { get; private set; }

        public class Builder
        {
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private string <AdUnitId>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private HashSet<NativeAdType> <AdTypes>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private HashSet<string> <TemplateIds>k__BackingField;
            [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Dictionary<string, Action<CustomNativeTemplateAd, string>> <CustomNativeTemplateClickHandlers>k__BackingField;

            public Builder(string adUnitId)
            {
                this.AdUnitId = adUnitId;
                this.AdTypes = new HashSet<NativeAdType>();
                this.TemplateIds = new HashSet<string>();
                this.CustomNativeTemplateClickHandlers = new Dictionary<string, Action<CustomNativeTemplateAd, string>>();
            }

            public AdLoader Build() => 
                new AdLoader(this);

            public AdLoader.Builder ForCustomNativeAd(string templateId)
            {
                this.TemplateIds.Add(templateId);
                this.AdTypes.Add(NativeAdType.CustomTemplate);
                return this;
            }

            public AdLoader.Builder ForCustomNativeAd(string templateId, Action<CustomNativeTemplateAd, string> callback)
            {
                this.TemplateIds.Add(templateId);
                this.CustomNativeTemplateClickHandlers[templateId] = callback;
                this.AdTypes.Add(NativeAdType.CustomTemplate);
                return this;
            }

            internal string AdUnitId { get; private set; }

            internal HashSet<NativeAdType> AdTypes { get; private set; }

            internal HashSet<string> TemplateIds { get; private set; }

            internal Dictionary<string, Action<CustomNativeTemplateAd, string>> CustomNativeTemplateClickHandlers { get; private set; }
        }
    }
}

