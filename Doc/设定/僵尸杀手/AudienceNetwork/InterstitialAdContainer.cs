namespace AudienceNetwork
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal class InterstitialAdContainer
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private InterstitialAd <interstitialAd>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FBInterstitialAdBridgeCallback <onLoad>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FBInterstitialAdBridgeCallback <onImpression>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FBInterstitialAdBridgeCallback <onClick>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FBInterstitialAdBridgeErrorCallback <onError>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FBInterstitialAdBridgeCallback <onDidClose>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FBInterstitialAdBridgeCallback <onWillClose>k__BackingField;
        internal AndroidJavaProxy listenerProxy;
        internal AndroidJavaObject bridgedInterstitialAd;

        internal InterstitialAdContainer(InterstitialAd interstitialAd)
        {
            this.interstitialAd = interstitialAd;
        }

        public static implicit operator bool(InterstitialAdContainer obj) => 
            !object.ReferenceEquals(obj, null);

        public override string ToString() => 
            $"[InterstitialAdContainer: interstitialAd={this.interstitialAd}, onLoad={this.onLoad}]";

        internal InterstitialAd interstitialAd { get; set; }

        internal FBInterstitialAdBridgeCallback onLoad { get; set; }

        internal FBInterstitialAdBridgeCallback onImpression { get; set; }

        internal FBInterstitialAdBridgeCallback onClick { get; set; }

        internal FBInterstitialAdBridgeErrorCallback onError { get; set; }

        internal FBInterstitialAdBridgeCallback onDidClose { get; set; }

        internal FBInterstitialAdBridgeCallback onWillClose { get; set; }
    }
}

