namespace AudienceNetwork
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal class RewardedVideoAdContainer
    {
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private RewardedVideoAd <rewardedVideoAd>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FBRewardedVideoAdBridgeCallback <onLoad>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FBRewardedVideoAdBridgeCallback <onImpression>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FBRewardedVideoAdBridgeCallback <onClick>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FBRewardedVideoAdBridgeErrorCallback <onError>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FBRewardedVideoAdBridgeCallback <onDidClose>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FBRewardedVideoAdBridgeCallback <onWillClose>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FBRewardedVideoAdBridgeCallback <onComplete>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FBRewardedVideoAdBridgeCallback <onDidSucceed>k__BackingField;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private FBRewardedVideoAdBridgeCallback <onDidFail>k__BackingField;
        internal AndroidJavaProxy listenerProxy;
        internal AndroidJavaObject bridgedRewardedVideoAd;

        internal RewardedVideoAdContainer(RewardedVideoAd rewardedVideoAd)
        {
            this.rewardedVideoAd = rewardedVideoAd;
        }

        public static implicit operator bool(RewardedVideoAdContainer obj) => 
            !object.ReferenceEquals(obj, null);

        public override string ToString() => 
            $"[RewardedVideoAdContainer: rewardedVideoAd={this.rewardedVideoAd}, onLoad={this.onLoad}]";

        internal RewardedVideoAd rewardedVideoAd { get; set; }

        internal FBRewardedVideoAdBridgeCallback onLoad { get; set; }

        internal FBRewardedVideoAdBridgeCallback onImpression { get; set; }

        internal FBRewardedVideoAdBridgeCallback onClick { get; set; }

        internal FBRewardedVideoAdBridgeErrorCallback onError { get; set; }

        internal FBRewardedVideoAdBridgeCallback onDidClose { get; set; }

        internal FBRewardedVideoAdBridgeCallback onWillClose { get; set; }

        internal FBRewardedVideoAdBridgeCallback onComplete { get; set; }

        internal FBRewardedVideoAdBridgeCallback onDidSucceed { get; set; }

        internal FBRewardedVideoAdBridgeCallback onDidFail { get; set; }
    }
}

