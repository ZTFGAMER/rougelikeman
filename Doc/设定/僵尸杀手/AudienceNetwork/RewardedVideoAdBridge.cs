namespace AudienceNetwork
{
    using System;
    using UnityEngine;

    internal class RewardedVideoAdBridge : IRewardedVideoAdBridge
    {
        public static readonly IRewardedVideoAdBridge Instance = createInstance();

        internal RewardedVideoAdBridge()
        {
        }

        public virtual int Create(string placementId, RewardData rewardData, RewardedVideoAd RewardedVideoAd) => 
            0x7b;

        private static IRewardedVideoAdBridge createInstance()
        {
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                return new RewardedVideoAdBridgeAndroid();
            }
            return new RewardedVideoAdBridge();
        }

        public virtual bool IsValid(int uniqueId) => 
            true;

        public virtual int Load(int uniqueId) => 
            0x7b;

        public virtual void OnClick(int uniqueId, FBRewardedVideoAdBridgeCallback callback)
        {
        }

        public virtual void OnComplete(int uniqueId, FBRewardedVideoAdBridgeCallback callback)
        {
        }

        public virtual void OnDidClose(int uniqueId, FBRewardedVideoAdBridgeCallback callback)
        {
        }

        public virtual void OnDidFail(int uniqueId, FBRewardedVideoAdBridgeCallback callback)
        {
        }

        public virtual void OnDidSucceed(int uniqueId, FBRewardedVideoAdBridgeCallback callback)
        {
        }

        public virtual void OnError(int uniqueId, FBRewardedVideoAdBridgeErrorCallback callback)
        {
        }

        public virtual void OnImpression(int uniqueId, FBRewardedVideoAdBridgeCallback callback)
        {
        }

        public virtual void OnLoad(int uniqueId, FBRewardedVideoAdBridgeCallback callback)
        {
        }

        public virtual void OnWillClose(int uniqueId, FBRewardedVideoAdBridgeCallback callback)
        {
        }

        public virtual void Release(int uniqueId)
        {
        }

        public virtual bool Show(int uniqueId) => 
            true;
    }
}

