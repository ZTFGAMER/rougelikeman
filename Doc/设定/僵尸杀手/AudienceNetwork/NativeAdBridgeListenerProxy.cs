namespace AudienceNetwork
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal class NativeAdBridgeListenerProxy : AndroidJavaProxy
    {
        private NativeAd nativeAd;
        private AndroidJavaObject bridgedNativeAd;

        public NativeAdBridgeListenerProxy(NativeAd nativeAd, AndroidJavaObject bridgedNativeAd) : base("com.facebook.ads.AdListener")
        {
            this.nativeAd = nativeAd;
            this.bridgedNativeAd = bridgedNativeAd;
        }

        private void onAdClicked(AndroidJavaObject ad)
        {
            this.nativeAd.executeOnMainThread(delegate {
                if (this.nativeAd.NativeAdDidClick != null)
                {
                    this.nativeAd.NativeAdDidClick();
                }
            });
        }

        private void onAdLoaded(AndroidJavaObject ad)
        {
            this.nativeAd.executeOnMainThread(() => this.nativeAd.loadAdFromData());
        }

        private void onError(AndroidJavaObject ad, AndroidJavaObject error)
        {
            <onError>c__AnonStorey0 storey = new <onError>c__AnonStorey0 {
                $this = this,
                errorMessage = error.Call<string>("getErrorMessage", new object[0])
            };
            this.nativeAd.executeOnMainThread(new Action(storey.<>m__0));
        }

        private void onLoggingImpression(AndroidJavaObject ad)
        {
            this.nativeAd.executeOnMainThread(delegate {
                if (this.nativeAd.NativeAdWillLogImpression != null)
                {
                    this.nativeAd.NativeAdWillLogImpression();
                }
            });
        }

        [CompilerGenerated]
        private sealed class <onError>c__AnonStorey0
        {
            internal string errorMessage;
            internal NativeAdBridgeListenerProxy $this;

            internal void <>m__0()
            {
                if (this.$this.nativeAd.NativeAdDidFailWithError != null)
                {
                    this.$this.nativeAd.NativeAdDidFailWithError(this.errorMessage);
                }
            }
        }
    }
}

