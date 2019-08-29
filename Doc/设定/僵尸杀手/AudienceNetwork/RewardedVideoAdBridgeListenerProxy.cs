namespace AudienceNetwork
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal class RewardedVideoAdBridgeListenerProxy : AndroidJavaProxy
    {
        private RewardedVideoAd rewardedVideoAd;
        private AndroidJavaObject bridgedRewardedVideoAd;

        public RewardedVideoAdBridgeListenerProxy(RewardedVideoAd rewardedVideoAd, AndroidJavaObject bridgedRewardedVideoAd) : base("com.facebook.ads.S2SRewardedVideoAdListener")
        {
            this.rewardedVideoAd = rewardedVideoAd;
            this.bridgedRewardedVideoAd = bridgedRewardedVideoAd;
        }

        private void onAdClicked(AndroidJavaObject ad)
        {
            this.rewardedVideoAd.executeOnMainThread(delegate {
                if (this.rewardedVideoAd.RewardedVideoAdDidClick != null)
                {
                    this.rewardedVideoAd.RewardedVideoAdDidClick();
                }
            });
        }

        private void onAdLoaded(AndroidJavaObject ad)
        {
            this.rewardedVideoAd.executeOnMainThread(delegate {
                if (this.rewardedVideoAd.RewardedVideoAdDidLoad != null)
                {
                    this.rewardedVideoAd.RewardedVideoAdDidLoad();
                }
            });
        }

        private void onError(AndroidJavaObject ad, AndroidJavaObject error)
        {
            <onError>c__AnonStorey0 storey = new <onError>c__AnonStorey0 {
                $this = this,
                errorMessage = error.Call<string>("getErrorMessage", new object[0])
            };
            this.rewardedVideoAd.executeOnMainThread(new Action(storey.<>m__0));
        }

        private void onLoggingImpression(AndroidJavaObject ad)
        {
            this.rewardedVideoAd.executeOnMainThread(delegate {
                if (this.rewardedVideoAd.RewardedVideoAdWillLogImpression != null)
                {
                    this.rewardedVideoAd.RewardedVideoAdWillLogImpression();
                }
            });
        }

        private void onRewardedVideoClosed()
        {
            this.rewardedVideoAd.executeOnMainThread(delegate {
                if (this.rewardedVideoAd.RewardedVideoAdDidClose != null)
                {
                    this.rewardedVideoAd.RewardedVideoAdDidClose();
                }
            });
        }

        private void onRewardedVideoCompleted()
        {
            this.rewardedVideoAd.executeOnMainThread(delegate {
                if (this.rewardedVideoAd.RewardedVideoAdComplete != null)
                {
                    this.rewardedVideoAd.RewardedVideoAdComplete();
                }
            });
        }

        private void onRewardedVideoDisplayed(AndroidJavaObject ad)
        {
            this.rewardedVideoAd.executeOnMainThread(delegate {
                if (this.rewardedVideoAd.RewardedVideoAdWillLogImpression != null)
                {
                    this.rewardedVideoAd.RewardedVideoAdWillLogImpression();
                }
            });
        }

        private void onRewardServerFailed()
        {
            this.rewardedVideoAd.executeOnMainThread(delegate {
                if (this.rewardedVideoAd.RewardedVideoAdDidFail != null)
                {
                    this.rewardedVideoAd.RewardedVideoAdDidFail();
                }
            });
        }

        private void onRewardServerSuccess()
        {
            this.rewardedVideoAd.executeOnMainThread(delegate {
                if (this.rewardedVideoAd.RewardedVideoAdDidSucceed != null)
                {
                    this.rewardedVideoAd.RewardedVideoAdDidSucceed();
                }
            });
        }

        [CompilerGenerated]
        private sealed class <onError>c__AnonStorey0
        {
            internal string errorMessage;
            internal RewardedVideoAdBridgeListenerProxy $this;

            internal void <>m__0()
            {
                if (this.$this.rewardedVideoAd.RewardedVideoAdDidFailWithError != null)
                {
                    this.$this.rewardedVideoAd.RewardedVideoAdDidFailWithError(this.errorMessage);
                }
            }
        }
    }
}

