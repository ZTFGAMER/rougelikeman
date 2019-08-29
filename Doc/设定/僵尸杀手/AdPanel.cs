using AudienceNetwork;
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer)), RequireComponent(typeof(RectTransform))]
public class AdPanel : MonoBehaviour
{
    public AdManager adManager;
    [Header("Text:")]
    public Text title;
    public Text socialContext;
    [Header("Images:")]
    public Image coverImage;
    public Image iconImage;
    [Header("Buttons:")]
    public Text callToAction;
    public Button callToActionButton;
    private bool adIconContentFilled;
    private bool adCoverContentFilled;
    private bool adTextContentFilled;

    private void Awake()
    {
        this.adIconContentFilled = false;
        this.adCoverContentFilled = false;
        this.adTextContentFilled = false;
    }

    public void registerGameObjectForImpression()
    {
        NativeAd nativeAd = this.adManager.nativeAd;
        if ((nativeAd != null) && (base.gameObject.GetComponent<NativeAdHandler>() == null))
        {
            Button[] clickableButtons = new Button[] { this.callToActionButton };
            nativeAd.RegisterGameObjectForImpression(base.gameObject, clickableButtons);
        }
    }

    private void Update()
    {
        NativeAd nativeAd = this.adManager.nativeAd;
        if (this.adManager.IsAdLoaded() && (nativeAd != null))
        {
            if ((nativeAd.CoverImage != null) && !this.adCoverContentFilled)
            {
                this.adCoverContentFilled = true;
                this.coverImage.set_sprite(nativeAd.CoverImage);
            }
            if ((nativeAd.IconImage != null) && !this.adIconContentFilled)
            {
                this.adIconContentFilled = true;
                this.iconImage.set_sprite(nativeAd.IconImage);
            }
            if (!this.adTextContentFilled)
            {
                this.adTextContentFilled = true;
                this.title.text = nativeAd.Title;
                this.socialContext.text = nativeAd.SocialContext;
                this.callToAction.text = nativeAd.CallToAction;
            }
        }
    }
}

