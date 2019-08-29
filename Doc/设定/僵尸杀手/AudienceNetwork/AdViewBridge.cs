namespace AudienceNetwork
{
    using System;
    using UnityEngine;

    internal class AdViewBridge : IAdViewBridge
    {
        public static readonly IAdViewBridge Instance = createInstance();

        internal AdViewBridge()
        {
        }

        public virtual int Create(string placementId, AdView AdView, AdSize size) => 
            0x7b;

        private static IAdViewBridge createInstance()
        {
            if (Application.platform != RuntimePlatform.OSXEditor)
            {
                return new AdViewBridgeAndroid();
            }
            return new AdViewBridge();
        }

        public virtual void DisableAutoRefresh(int uniqueId)
        {
        }

        public virtual int Load(int uniqueId) => 
            0x7b;

        public virtual void OnClick(int uniqueId, FBAdViewBridgeCallback callback)
        {
        }

        public virtual void OnError(int uniqueId, FBAdViewBridgeErrorCallback callback)
        {
        }

        public virtual void OnFinishedClick(int uniqueId, FBAdViewBridgeCallback callback)
        {
        }

        public virtual void OnImpression(int uniqueId, FBAdViewBridgeCallback callback)
        {
        }

        public virtual void OnLoad(int uniqueId, FBAdViewBridgeCallback callback)
        {
        }

        public virtual void Release(int uniqueId)
        {
        }

        public virtual bool Show(int uniqueId, double x, double y, double width, double height) => 
            true;
    }
}

