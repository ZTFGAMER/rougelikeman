namespace AudienceNetwork
{
    using System;

    internal interface IRewardedVideoAdBridge
    {
        int Create(string placementId, RewardData rewardData, RewardedVideoAd rewardedVideoAd);
        bool IsValid(int uniqueId);
        int Load(int uniqueId);
        void OnClick(int uniqueId, FBRewardedVideoAdBridgeCallback callback);
        void OnComplete(int uniqueId, FBRewardedVideoAdBridgeCallback callback);
        void OnDidClose(int uniqueId, FBRewardedVideoAdBridgeCallback callback);
        void OnDidFail(int uniqueId, FBRewardedVideoAdBridgeCallback callback);
        void OnDidSucceed(int uniqueId, FBRewardedVideoAdBridgeCallback callback);
        void OnError(int uniqueId, FBRewardedVideoAdBridgeErrorCallback callback);
        void OnImpression(int uniqueId, FBRewardedVideoAdBridgeCallback callback);
        void OnLoad(int uniqueId, FBRewardedVideoAdBridgeCallback callback);
        void OnWillClose(int uniqueId, FBRewardedVideoAdBridgeCallback callback);
        void Release(int uniqueId);
        bool Show(int uniqueId);
    }
}

