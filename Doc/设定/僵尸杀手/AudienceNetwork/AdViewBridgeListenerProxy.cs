namespace AudienceNetwork
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal class AdViewBridgeListenerProxy : AndroidJavaProxy
    {
        private AdView adView;
        private AndroidJavaObject bridgedAdView;

        public AdViewBridgeListenerProxy(AdView adView, AndroidJavaObject bridgedAdView) : base("com.facebook.ads.AdListener")
        {
            this.adView = adView;
            this.bridgedAdView = bridgedAdView;
        }

        private void onAdClicked(AndroidJavaObject ad)
        {
            this.adView.executeOnMainThread(delegate {
                if (this.adView.AdViewDidClick != null)
                {
                    this.adView.AdViewDidClick();
                }
            });
        }

        private void onAdLoaded(AndroidJavaObject ad)
        {
            this.adView.executeOnMainThread(delegate {
                if (this.adView.AdViewDidLoad != null)
                {
                    this.adView.AdViewDidLoad();
                }
            });
        }

        private void onError(AndroidJavaObject ad, AndroidJavaObject error)
        {
            <onError>c__AnonStorey0 storey = new <onError>c__AnonStorey0 {
                $this = this,
                errorMessage = error.Call<string>("getErrorMessage", new object[0])
            };
            this.adView.executeOnMainThread(new Action(storey.<>m__0));
        }

        private void onLoggingImpression(AndroidJavaObject ad)
        {
            this.adView.executeOnMainThread(delegate {
                if (this.adView.AdViewWillLogImpression != null)
                {
                    this.adView.AdViewWillLogImpression();
                }
            });
        }

        [CompilerGenerated]
        private sealed class <onError>c__AnonStorey0
        {
            internal string errorMessage;
            internal AdViewBridgeListenerProxy $this;

            internal void <>m__0()
            {
                if (this.$this.adView.AdViewDidFailWithError != null)
                {
                    this.$this.adView.AdViewDidFailWithError(this.errorMessage);
                }
            }
        }
    }
}

