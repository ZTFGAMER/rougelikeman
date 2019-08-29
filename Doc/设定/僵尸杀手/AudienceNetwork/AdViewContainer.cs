namespace AudienceNetwork
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal class AdViewContainer
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private AdView <adView>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FBAdViewBridgeCallback <onLoad>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FBAdViewBridgeCallback <onImpression>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FBAdViewBridgeCallback <onClick>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FBAdViewBridgeErrorCallback <onError>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FBAdViewBridgeCallback <onFinishedClick>k__BackingField;
        internal AndroidJavaProxy listenerProxy;
        internal AndroidJavaObject bridgedAdView;

        internal AdViewContainer(AdView adView)
        {
            this.adView = adView;
        }

        public static implicit operator bool(AdViewContainer obj) => 
            !object.ReferenceEquals(obj, null);

        public override string ToString() => 
            $"[AdViewContainer: adView={this.adView}, onLoad={this.onLoad}]";

        internal AdView adView { get; set; }

        internal FBAdViewBridgeCallback onLoad { get; set; }

        internal FBAdViewBridgeCallback onImpression { get; set; }

        internal FBAdViewBridgeCallback onClick { get; set; }

        internal FBAdViewBridgeErrorCallback onError { get; set; }

        internal FBAdViewBridgeCallback onFinishedClick { get; set; }
    }
}

