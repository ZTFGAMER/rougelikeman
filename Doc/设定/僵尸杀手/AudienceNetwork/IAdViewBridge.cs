namespace AudienceNetwork
{
    using System;

    internal interface IAdViewBridge
    {
        int Create(string placementId, AdView adView, AdSize size);
        void DisableAutoRefresh(int uniqueId);
        int Load(int uniqueId);
        void OnClick(int uniqueId, FBAdViewBridgeCallback callback);
        void OnError(int uniqueId, FBAdViewBridgeErrorCallback callback);
        void OnFinishedClick(int uniqueId, FBAdViewBridgeCallback callback);
        void OnImpression(int uniqueId, FBAdViewBridgeCallback callback);
        void OnLoad(int uniqueId, FBAdViewBridgeCallback callback);
        void Release(int uniqueId);
        bool Show(int uniqueId, double x, double y, double width, double height);
    }
}

