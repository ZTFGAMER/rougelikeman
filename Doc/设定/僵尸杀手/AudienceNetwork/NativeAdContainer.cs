namespace AudienceNetwork
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal class NativeAdContainer
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private NativeAd <nativeAd>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FBNativeAdBridgeCallback <onLoad>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FBNativeAdBridgeCallback <onImpression>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FBNativeAdBridgeCallback <onClick>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FBNativeAdBridgeErrorCallback <onError>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FBNativeAdBridgeCallback <onFinishedClick>k__BackingField;
        internal AndroidJavaProxy listenerProxy;
        internal AndroidJavaObject bridgedNativeAd;

        internal NativeAdContainer(NativeAd nativeAd)
        {
            this.nativeAd = nativeAd;
        }

        public static implicit operator bool(NativeAdContainer obj) => 
            !object.ReferenceEquals(obj, null);

        internal NativeAd nativeAd { get; set; }

        internal FBNativeAdBridgeCallback onLoad { get; set; }

        internal FBNativeAdBridgeCallback onImpression { get; set; }

        internal FBNativeAdBridgeCallback onClick { get; set; }

        internal FBNativeAdBridgeErrorCallback onError { get; set; }

        internal FBNativeAdBridgeCallback onFinishedClick { get; set; }
    }
}

