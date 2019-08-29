using AudienceNetwork;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer)), RequireComponent(typeof(RectTransform))]
public class NativeAdTest : MonoBehaviour
{
    private NativeAd nativeAd;
    [Header("Text:")]
    public Text title;
    public Text socialContext;
    public Text status;
    [Header("Images:")]
    public Image coverImage;
    public Image iconImage;
    [Header("Buttons:")]
    public Text callToAction;
    public Button callToActionButton;
    [Header("Ad Choices:"), SerializeField]
    private AdChoices adChoices;

    private void Awake()
    {
        this.Log("Native ad ready to load.");
    }

    public void LoadAd()
    {
        <LoadAd>c__AnonStorey0 storey = new <LoadAd>c__AnonStorey0 {
            $this = this,
            nativeAd = new NativeAd("YOUR_PLACEMENT_ID")
        };
        this.nativeAd = storey.nativeAd;
        Button[] clickableButtons = new Button[] { this.callToActionButton };
        storey.nativeAd.RegisterGameObjectForImpression(base.gameObject, clickableButtons);
        storey.nativeAd.NativeAdDidLoad = new FBNativeAdBridgeCallback(storey.<>m__0);
        storey.nativeAd.NativeAdDidFailWithError = new FBNativeAdBridgeErrorCallback(storey.<>m__1);
        storey.nativeAd.NativeAdWillLogImpression = new FBNativeAdBridgeCallback(storey.<>m__2);
        storey.nativeAd.NativeAdDidClick = new FBNativeAdBridgeCallback(storey.<>m__3);
        storey.nativeAd.LoadAd();
        this.Log("Native ad loading...");
    }

    private void Log(string s)
    {
        this.status.text = s;
        Debug.Log(s);
    }

    public void NextScene()
    {
        SceneManager.LoadScene("RewardedVideoAdScene");
    }

    private void OnDestroy()
    {
        if (this.nativeAd != null)
        {
            this.nativeAd.Dispose();
        }
        Debug.Log("NativeAdTest was destroyed!");
    }

    private void OnGUI()
    {
        if ((this.nativeAd != null) && (this.nativeAd.CoverImage != null))
        {
            this.coverImage.set_sprite(this.nativeAd.CoverImage);
        }
        if ((this.nativeAd != null) && (this.nativeAd.IconImage != null))
        {
            this.iconImage.set_sprite(this.nativeAd.IconImage);
        }
        if ((this.nativeAd != null) && (this.nativeAd.AdChoicesImage != null))
        {
            this.adChoices.SetNativeAd(this.nativeAd);
        }
    }

    [CompilerGenerated]
    private sealed class <LoadAd>c__AnonStorey0
    {
        internal NativeAd nativeAd;
        internal NativeAdTest $this;

        internal void <>m__0()
        {
            this.$this.Log("Native ad loaded.");
            Debug.Log("Loading images...");
            this.$this.StartCoroutine(this.nativeAd.LoadIconImage(this.nativeAd.IconImageURL));
            this.$this.StartCoroutine(this.nativeAd.LoadCoverImage(this.nativeAd.CoverImageURL));
            this.$this.StartCoroutine(this.nativeAd.LoadAdChoicesImage(this.nativeAd.AdChoicesImageURL));
            Debug.Log("Images loaded.");
            this.$this.title.text = this.nativeAd.Title;
            this.$this.socialContext.text = this.nativeAd.SocialContext;
            this.$this.callToAction.text = this.nativeAd.CallToAction;
        }

        internal void <>m__1(string error)
        {
            this.$this.Log("Native ad failed to load with error: " + error);
        }

        internal void <>m__2()
        {
            this.$this.Log("Native ad logged impression.");
        }

        internal void <>m__3()
        {
            this.$this.Log("Native ad clicked.");
        }
    }
}

