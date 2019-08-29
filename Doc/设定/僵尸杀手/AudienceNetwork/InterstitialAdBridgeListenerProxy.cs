namespace AudienceNetwork
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal class InterstitialAdBridgeListenerProxy : AndroidJavaProxy
    {
        private InterstitialAd interstitialAd;
        private AndroidJavaObject bridgedInterstitialAd;

        public InterstitialAdBridgeListenerProxy(InterstitialAd interstitialAd, AndroidJavaObject bridgedInterstitialAd) : base("com.facebook.ads.InterstitialAdListener")
        {
            this.interstitialAd = interstitialAd;
            this.bridgedInterstitialAd = bridgedInterstitialAd;
        }

        private void onAdClicked(AndroidJavaObject ad)
        {
            this.interstitialAd.executeOnMainThread(delegate {
                if (this.interstitialAd.InterstitialAdDidClick != null)
                {
                    this.interstitialAd.InterstitialAdDidClick();
                }
            });
        }

        private void onAdLoaded(AndroidJavaObject ad)
        {
            this.interstitialAd.executeOnMainThread(delegate {
                if (this.interstitialAd.InterstitialAdDidLoad != null)
                {
                    this.interstitialAd.InterstitialAdDidLoad();
                }
            });
        }

        private void onError(AndroidJavaObject ad, AndroidJavaObject error)
        {
            <onError>c__AnonStorey0 storey = new <onError>c__AnonStorey0 {
                $this = this,
                errorMessage = error.Call<string>("getErrorMessage", new object[0])
            };
            this.interstitialAd.executeOnMainThread(new Action(storey.<>m__0));
        }

        private void onInterstitialDismissed(AndroidJavaObject ad)
        {
            this.interstitialAd.executeOnMainThread(delegate {
                if (this.interstitialAd.InterstitialAdDidClose != null)
                {
                    this.interstitialAd.InterstitialAdDidClose();
                }
            });
        }

        private void onInterstitialDisplayed(AndroidJavaObject ad)
        {
            this.interstitialAd.executeOnMainThread(delegate {
                if (this.interstitialAd.InterstitialAdWillLogImpression != null)
                {
                    this.interstitialAd.InterstitialAdWillLogImpression();
                }
            });
        }

        private void onLoggingImpression(AndroidJavaObject ad)
        {
            this.interstitialAd.executeOnMainThread(delegate {
                if (this.interstitialAd.InterstitialAdWillLogImpression != null)
                {
                    this.interstitialAd.InterstitialAdWillLogImpression();
                }
            });
        }

        [CompilerGenerated]
        private sealed class <onError>c__AnonStorey0
        {
            internal string errorMessage;
            internal InterstitialAdBridgeListenerProxy $this;

            internal void <>m__0()
            {
                if (this.$this.interstitialAd.InterstitialAdDidFailWithError != null)
                {
                    this.$this.interstitialAd.InterstitialAdDidFailWithError(this.errorMessage);
                }
            }
        }
    }
}

