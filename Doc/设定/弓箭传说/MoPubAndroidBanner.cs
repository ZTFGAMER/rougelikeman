using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class MoPubAndroidBanner
{
    private readonly AndroidJavaObject _bannerPlugin;

    public MoPubAndroidBanner(string adUnitId)
    {
        object[] args = new object[] { adUnitId };
        this._bannerPlugin = new AndroidJavaObject("com.mopub.unity.MoPubBannerUnityPlugin", args);
    }

    public void CreateBanner(MoPubBase.AdPosition position)
    {
        object[] args = new object[] { (int) position };
        this._bannerPlugin.Call("createBanner", args);
    }

    public void DestroyBanner()
    {
        this._bannerPlugin.Call("destroyBanner", Array.Empty<object>());
    }

    public void ForceRefresh()
    {
        this._bannerPlugin.Call("forceRefresh", Array.Empty<object>());
    }

    public void RefreshBanner(string keywords, string userDataKeywords = "")
    {
        object[] args = new object[] { keywords, userDataKeywords };
        this._bannerPlugin.Call("refreshBanner", args);
    }

    public void SetAutorefresh(bool enabled)
    {
        object[] args = new object[] { enabled };
        this._bannerPlugin.Call("setAutorefreshEnabled", args);
    }

    public void ShowBanner(bool shouldShow)
    {
        object[] args = new object[] { !shouldShow };
        this._bannerPlugin.Call("hideBanner", args);
    }
}

