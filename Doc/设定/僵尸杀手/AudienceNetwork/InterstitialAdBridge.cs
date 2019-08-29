namespace AudienceNetwork
{
    using System;
    using UnityEngine;

    internal class InterstitialAdBridge : IInterstitialAdBridge
    {
        public static readonly IInterstitialAdBridge Instance = createInstance();

        internal InterstitialAdBridge()
        {
        }

        public virtual int Create(string placementId, InterstitialAd InterstitialAd) => 
            0x7b;

        private static IInterstitialAdBridge createInstance()
        {
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                return new InterstitialAdBridgeAndroid();
            }
            return new InterstitialAdBridge();
        }

        public virtual bool IsValid(int uniqueId) => 
            true;

        public virtual int Load(int uniqueId) => 
            0x7b;

        public virtual void OnClick(int uniqueId, FBInterstitialAdBridgeCallback callback)
        {
        }

        public virtual void OnDidClose(int uniqueId, FBInterstitialAdBridgeCallback callback)
        {
        }

        public virtual void OnError(int uniqueId, FBInterstitialAdBridgeErrorCallback callback)
        {
        }

        public virtual void OnImpression(int uniqueId, FBInterstitialAdBridgeCallback callback)
        {
        }

        public virtual void OnLoad(int uniqueId, FBInterstitialAdBridgeCallback callback)
        {
        }

        public virtual void OnWillClose(int uniqueId, FBInterstitialAdBridgeCallback callback)
        {
        }

        public virtual void Release(int uniqueId)
        {
        }

        public virtual bool Show(int uniqueId) => 
            true;
    }
}

