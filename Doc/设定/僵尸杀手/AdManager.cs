using AudienceNetwork;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class AdManager : MonoBehaviour
{
    public NativeAd nativeAd;
    public GameObject targetAdObject;
    public Button targetButton;
    private bool adLoaded;
    [CompilerGenerated]
    private static FBNativeAdBridgeErrorCallback <>f__am$cache0;
    [CompilerGenerated]
    private static FBNativeAdBridgeCallback <>f__am$cache1;
    [CompilerGenerated]
    private static FBNativeAdBridgeCallback <>f__am$cache2;

    public bool IsAdLoaded() => 
        this.adLoaded;

    public void LoadAd()
    {
        <LoadAd>c__AnonStorey0 storey = new <LoadAd>c__AnonStorey0 {
            $this = this,
            nativeAd = new NativeAd("YOUR_PLACEMENT_ID")
        };
        this.nativeAd = storey.nativeAd;
        if (this.targetAdObject != null)
        {
            if (this.targetButton != null)
            {
                Button[] clickableButtons = new Button[] { this.targetButton };
                storey.nativeAd.RegisterGameObjectForImpression(this.targetAdObject, clickableButtons);
            }
            else
            {
                storey.nativeAd.RegisterGameObjectForImpression(this.targetAdObject, new Button[0]);
            }
        }
        else
        {
            storey.nativeAd.RegisterGameObjectForImpression(base.gameObject, new Button[0]);
        }
        storey.nativeAd.NativeAdDidLoad = new FBNativeAdBridgeCallback(storey.<>m__0);
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = error => Debug.Log("Native ad failed to load with error: " + error);
        }
        storey.nativeAd.NativeAdDidFailWithError = <>f__am$cache0;
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = () => Debug.Log("Native ad logged impression.");
        }
        storey.nativeAd.NativeAdWillLogImpression = <>f__am$cache1;
        if (<>f__am$cache2 == null)
        {
            <>f__am$cache2 = () => Debug.Log("Native ad clicked.");
        }
        storey.nativeAd.NativeAdDidClick = <>f__am$cache2;
        storey.nativeAd.LoadAd();
        Debug.Log("Native ad loading...");
    }

    private void OnDestroy()
    {
        if (this.nativeAd != null)
        {
            this.nativeAd.Dispose();
        }
        Debug.Log("NativeAdTest was destroyed!");
    }

    private void Start()
    {
        this.adLoaded = false;
        this.LoadAd();
    }

    [CompilerGenerated]
    private sealed class <LoadAd>c__AnonStorey0
    {
        internal NativeAd nativeAd;
        internal AdManager $this;

        internal void <>m__0()
        {
            this.$this.adLoaded = true;
            Debug.Log("Native ad loaded.");
            Debug.Log("Loading images...");
            this.$this.StartCoroutine(this.nativeAd.LoadCoverImage(this.nativeAd.CoverImageURL));
            this.$this.StartCoroutine(this.nativeAd.LoadIconImage(this.nativeAd.IconImageURL));
            Debug.Log("Images loaded.");
        }
    }
}

