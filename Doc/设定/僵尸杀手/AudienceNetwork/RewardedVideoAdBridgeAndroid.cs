namespace AudienceNetwork
{
    using AudienceNetwork.Utility;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal class RewardedVideoAdBridgeAndroid : RewardedVideoAdBridge
    {
        private static Dictionary<int, RewardedVideoAdContainer> rewardedVideoAds = new Dictionary<int, RewardedVideoAdContainer>();
        private static int lastKey = 0;

        public override int Create(string placementId, RewardData rewardData, RewardedVideoAd rewardedVideoAd)
        {
            AdUtility.prepare();
            AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject obj3 = class2.GetStatic<AndroidJavaObject>("currentActivity").Call<AndroidJavaObject>("getApplicationContext", new object[0]);
            object[] args = new object[] { obj3, placementId };
            AndroidJavaObject bridgedRewardedVideoAd = new AndroidJavaObject("com.facebook.ads.RewardedVideoAd", args);
            RewardedVideoAdBridgeListenerProxy proxy = new RewardedVideoAdBridgeListenerProxy(rewardedVideoAd, bridgedRewardedVideoAd);
            object[] objArray2 = new object[] { proxy };
            bridgedRewardedVideoAd.Call("setAdListener", objArray2);
            if (rewardData != null)
            {
                object[] objArray3 = new object[] { rewardData.UserId, rewardData.Currency };
                AndroidJavaObject obj5 = new AndroidJavaObject("com.facebook.ads.RewardData", objArray3);
                object[] objArray4 = new object[] { obj5 };
                bridgedRewardedVideoAd.Call("setRewardData", objArray4);
            }
            RewardedVideoAdContainer container = new RewardedVideoAdContainer(rewardedVideoAd) {
                bridgedRewardedVideoAd = bridgedRewardedVideoAd,
                listenerProxy = proxy
            };
            int lastKey = RewardedVideoAdBridgeAndroid.lastKey;
            rewardedVideoAds.Add(lastKey, container);
            RewardedVideoAdBridgeAndroid.lastKey++;
            return lastKey;
        }

        private string getImageURLForuniqueId(int uniqueId, string method)
        {
            AndroidJavaObject obj2 = this.rewardedVideoAdForUniqueId(uniqueId);
            if (obj2 != null)
            {
                AndroidJavaObject obj3 = obj2.Call<AndroidJavaObject>(method, new object[0]);
                if (obj3 != null)
                {
                    return obj3.Call<string>("getUrl", new object[0]);
                }
            }
            return null;
        }

        private string getStringForuniqueId(int uniqueId, string method)
        {
            AndroidJavaObject obj2 = this.rewardedVideoAdForUniqueId(uniqueId);
            if (obj2 != null)
            {
                return obj2.Call<string>(method, new object[0]);
            }
            return null;
        }

        public override bool IsValid(int uniqueId)
        {
            AndroidJavaObject obj2 = this.rewardedVideoAdForUniqueId(uniqueId);
            if (obj2 != null)
            {
                return obj2.Call<bool>("isAdLoaded", new object[0]);
            }
            return false;
        }

        public override int Load(int uniqueId)
        {
            AdUtility.prepare();
            AndroidJavaObject obj2 = this.rewardedVideoAdForUniqueId(uniqueId);
            if (obj2 != null)
            {
                obj2.Call("loadAd", new object[0]);
            }
            return uniqueId;
        }

        public override void OnClick(int uniqueId, FBRewardedVideoAdBridgeCallback callback)
        {
        }

        public override void OnDidClose(int uniqueId, FBRewardedVideoAdBridgeCallback callback)
        {
        }

        public override void OnError(int uniqueId, FBRewardedVideoAdBridgeErrorCallback callback)
        {
        }

        public override void OnImpression(int uniqueId, FBRewardedVideoAdBridgeCallback callback)
        {
        }

        public override void OnLoad(int uniqueId, FBRewardedVideoAdBridgeCallback callback)
        {
        }

        public override void OnWillClose(int uniqueId, FBRewardedVideoAdBridgeCallback callback)
        {
        }

        public override void Release(int uniqueId)
        {
            AndroidJavaObject obj2 = this.rewardedVideoAdForUniqueId(uniqueId);
            if (obj2 != null)
            {
                obj2.Call("destroy", new object[0]);
            }
            rewardedVideoAds.Remove(uniqueId);
        }

        private RewardedVideoAdContainer rewardedVideoAdContainerForUniqueId(int uniqueId)
        {
            RewardedVideoAdContainer container = null;
            if (rewardedVideoAds.TryGetValue(uniqueId, out container))
            {
                return container;
            }
            return null;
        }

        private AndroidJavaObject rewardedVideoAdForUniqueId(int uniqueId)
        {
            RewardedVideoAdContainer container = null;
            if (rewardedVideoAds.TryGetValue(uniqueId, out container))
            {
                return container.bridgedRewardedVideoAd;
            }
            return null;
        }

        public override bool Show(int uniqueId)
        {
            <Show>c__AnonStorey0 storey = new <Show>c__AnonStorey0();
            RewardedVideoAdContainer container = this.rewardedVideoAdContainerForUniqueId(uniqueId);
            storey.rewardedVideoAd = this.rewardedVideoAdForUniqueId(uniqueId);
            container.rewardedVideoAd.executeOnMainThread(new Action(storey.<>m__0));
            return true;
        }

        [CompilerGenerated]
        private sealed class <Show>c__AnonStorey0
        {
            internal AndroidJavaObject rewardedVideoAd;

            internal void <>m__0()
            {
                if (this.rewardedVideoAd != null)
                {
                    this.rewardedVideoAd.Call<bool>("show", new object[0]);
                }
            }
        }
    }
}

