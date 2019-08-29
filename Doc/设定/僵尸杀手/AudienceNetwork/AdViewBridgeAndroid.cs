namespace AudienceNetwork
{
    using AudienceNetwork.Utility;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal class AdViewBridgeAndroid : AdViewBridge
    {
        private static Dictionary<int, AdViewContainer> adViews = new Dictionary<int, AdViewContainer>();
        private static int lastKey = 0;

        private AndroidJavaObject adViewForAdViewId(int uniqueId)
        {
            AdViewContainer container = null;
            if (adViews.TryGetValue(uniqueId, out container))
            {
                return container.bridgedAdView;
            }
            return null;
        }

        public override int Create(string placementId, AdView adView, AdSize size)
        {
            AdUtility.prepare();
            AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject obj3 = class2.GetStatic<AndroidJavaObject>("currentActivity").Call<AndroidJavaObject>("getApplicationContext", new object[0]);
            object[] args = new object[] { obj3, placementId, this.javaAdSizeFromAdSize(size) };
            AndroidJavaObject bridgedAdView = new AndroidJavaObject("com.facebook.ads.AdView", args);
            AdViewBridgeListenerProxy proxy = new AdViewBridgeListenerProxy(adView, bridgedAdView);
            object[] objArray2 = new object[] { proxy };
            bridgedAdView.Call("setAdListener", objArray2);
            AdViewContainer container = new AdViewContainer(adView) {
                bridgedAdView = bridgedAdView,
                listenerProxy = proxy
            };
            int lastKey = AdViewBridgeAndroid.lastKey;
            adViews.Add(lastKey, container);
            AdViewBridgeAndroid.lastKey++;
            return lastKey;
        }

        public override void DisableAutoRefresh(int uniqueId)
        {
            AndroidJavaObject obj2 = this.adViewForAdViewId(uniqueId);
            if (obj2 != null)
            {
                obj2.Call("disableAutoRefresh", new object[0]);
            }
        }

        private string getImageURLForAdViewId(int uniqueId, string method)
        {
            AndroidJavaObject obj2 = this.adViewForAdViewId(uniqueId);
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

        private string getStringForAdViewId(int uniqueId, string method)
        {
            AndroidJavaObject obj2 = this.adViewForAdViewId(uniqueId);
            if (obj2 != null)
            {
                return obj2.Call<string>(method, new object[0]);
            }
            return null;
        }

        private AndroidJavaObject javaAdSizeFromAdSize(AdSize size)
        {
            AndroidJavaObject obj2 = null;
            AndroidJavaClass class2 = new AndroidJavaClass("com.facebook.ads.AdSize");
            if (size != AdSize.BANNER_HEIGHT_50)
            {
                if (size != AdSize.BANNER_HEIGHT_90)
                {
                    if (size != AdSize.RECTANGLE_HEIGHT_250)
                    {
                        return obj2;
                    }
                    return class2.GetStatic<AndroidJavaObject>("RECTANGLE_HEIGHT_250");
                }
            }
            else
            {
                return class2.GetStatic<AndroidJavaObject>("BANNER_HEIGHT_50");
            }
            return class2.GetStatic<AndroidJavaObject>("BANNER_HEIGHT_90");
        }

        public override int Load(int uniqueId)
        {
            AdUtility.prepare();
            AndroidJavaObject obj2 = this.adViewForAdViewId(uniqueId);
            if (obj2 != null)
            {
                obj2.Call("loadAd", new object[0]);
            }
            return uniqueId;
        }

        public override void OnClick(int uniqueId, FBAdViewBridgeCallback callback)
        {
        }

        public override void OnError(int uniqueId, FBAdViewBridgeErrorCallback callback)
        {
        }

        public override void OnFinishedClick(int uniqueId, FBAdViewBridgeCallback callback)
        {
        }

        public override void OnImpression(int uniqueId, FBAdViewBridgeCallback callback)
        {
        }

        public override void OnLoad(int uniqueId, FBAdViewBridgeCallback callback)
        {
        }

        public override void Release(int uniqueId)
        {
            <Release>c__AnonStorey1 storey = new <Release>c__AnonStorey1();
            AndroidJavaObject @static = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            storey.adView = this.adViewForAdViewId(uniqueId);
            adViews.Remove(uniqueId);
            if (storey.adView != null)
            {
                object[] args = new object[] { new AndroidJavaRunnable(storey.<>m__0) };
                @static.Call("runOnUiThread", args);
            }
        }

        public override bool Show(int uniqueId, double x, double y, double width, double height)
        {
            <Show>c__AnonStorey0 storey = new <Show>c__AnonStorey0 {
                width = width,
                height = height,
                x = x,
                y = y,
                adView = this.adViewForAdViewId(uniqueId)
            };
            if (storey.adView == null)
            {
                return false;
            }
            storey.activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            object[] args = new object[] { new AndroidJavaRunnable(storey.<>m__0) };
            storey.activity.Call("runOnUiThread", args);
            return true;
        }

        [CompilerGenerated]
        private sealed class <Release>c__AnonStorey1
        {
            internal AndroidJavaObject adView;

            internal void <>m__0()
            {
                this.adView.Call("destroy", new object[0]);
                object[] args = new object[] { this.adView };
                this.adView.Call<AndroidJavaObject>("getParent", new object[0]).Call("removeView", args);
            }
        }

        [CompilerGenerated]
        private sealed class <Show>c__AnonStorey0
        {
            internal AndroidJavaObject activity;
            internal double width;
            internal double height;
            internal AndroidJavaObject adView;
            internal double x;
            internal double y;

            internal void <>m__0()
            {
                float num = this.activity.Call<AndroidJavaObject>("getApplicationContext", new object[0]).Call<AndroidJavaObject>("getResources", new object[0]).Call<AndroidJavaObject>("getDisplayMetrics", new object[0]).Get<float>("density");
                object[] args = new object[] { (int) (this.width * num), (int) (this.height * num) };
                AndroidJavaObject obj5 = new AndroidJavaObject("android.widget.LinearLayout$LayoutParams", args);
                object[] objArray2 = new object[] { this.activity };
                AndroidJavaObject obj6 = new AndroidJavaObject("android.widget.LinearLayout", objArray2);
                AndroidJavaClass class2 = new AndroidJavaClass("android.R$id");
                object[] objArray3 = new object[] { class2.GetStatic<int>("content") };
                AndroidJavaObject obj7 = this.activity.Call<AndroidJavaObject>("findViewById", objArray3);
                AndroidJavaObject obj8 = this.adView.Call<AndroidJavaObject>("getParent", new object[0]);
                if (obj8 != null)
                {
                    if (AndroidJNI.GetMethodID(obj8.GetRawClass(), "removeView", "(Landroid/view/View;)V") != IntPtr.Zero)
                    {
                        object[] objArray4 = new object[] { this.adView };
                        obj8.Call("removeView", objArray4);
                    }
                    else
                    {
                        AndroidJNI.ExceptionClear();
                    }
                }
                object[] objArray5 = new object[] { (int) (this.x * num), (int) (this.y * num), 0, 0 };
                obj5.Call("setMargins", objArray5);
                object[] objArray6 = new object[] { this.adView, obj5 };
                obj6.Call("addView", objArray6);
                object[] objArray7 = new object[] { obj6 };
                obj7.Call("addView", objArray7);
            }
        }
    }
}

