using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class NoAdsManager : MonoBehaviour
{
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static NoAdsManager <instance>k__BackingField;
    [SerializeField]
    private Animator noAdsButtonAnim;
    [SerializeField]
    private Button shopNoAdsBuyButton;
    [SerializeField]
    private GameObject noAdsDone;
    [SerializeField]
    private GameObject textPriceObj;

    public void Awake()
    {
        instance = this;
    }

    public void CheckNoAds()
    {
        if (PlayerPrefs.HasKey(StaticConstants.interstitialAdsKey))
        {
            this.shopNoAdsBuyButton.interactable = false;
            this.noAdsDone.SetActive(true);
            this.textPriceObj.SetActive(false);
            this.noAdsButtonAnim.SetBool("IsOpened", false);
        }
        else
        {
            this.noAdsButtonAnim.SetBool("IsOpened", true);
            this.shopNoAdsBuyButton.interactable = true;
            this.noAdsDone.SetActive(false);
            this.textPriceObj.SetActive(true);
        }
    }

    public void Start()
    {
        this.CheckNoAds();
    }

    public static NoAdsManager instance
    {
        [CompilerGenerated]
        get => 
            <instance>k__BackingField;
        [CompilerGenerated]
        private set => 
            (<instance>k__BackingField = value);
    }
}

