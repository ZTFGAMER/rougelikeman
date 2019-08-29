using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class MoPubAndroidInterstitial
{
    private readonly AndroidJavaObject _interstitialPlugin;

    public MoPubAndroidInterstitial(string adUnitId)
    {
        object[] args = new object[] { adUnitId };
        this._interstitialPlugin = new AndroidJavaObject("com.mopub.unity.MoPubInterstitialUnityPlugin", args);
    }

    public void DestroyInterstitialAd()
    {
        this._interstitialPlugin.Call("destroy", Array.Empty<object>());
    }

    public void RequestInterstitialAd(string keywords = "", string userDataKeywords = "")
    {
        object[] args = new object[] { keywords, userDataKeywords };
        this._interstitialPlugin.Call("request", args);
    }

    public void ShowInterstitialAd()
    {
        this._interstitialPlugin.Call("show", Array.Empty<object>());
    }

    public bool IsInterstitialReady =>
        this._interstitialPlugin.Call<bool>("isReady", Array.Empty<object>());
}

