using Heyzap;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static AdsManager <instance>k__BackingField;
    [HideInInspector]
    public int interstitialAdsCounter;
    private HZInterstitialAd.AdDisplayListener listener;
    [CompilerGenerated]
    private static HZInterstitialAd.AdDisplayListener <>f__am$cache0;

    public AdsManager()
    {
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = new HZInterstitialAd.AdDisplayListener(AdsManager.<listener>m__0);
        }
        this.listener = <>f__am$cache0;
    }

    [CompilerGenerated]
    private static void <listener>m__0(string adState, string adTag)
    {
        if (adState.Equals("audio_starting"))
        {
            SoundManager.Instance.MuteAll();
        }
        if (adState.Equals("audio_finished"))
        {
            SoundManager.Instance.UnMuteAll();
        }
    }

    private void Awake()
    {
        if ((instance != null) && (instance != this))
        {
            UnityEngine.Object.Destroy(base.gameObject);
        }
        else
        {
            instance = this;
            UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
        }
    }

    public void DecreaseInterstitialCounter()
    {
        if (this.interstitialAdsCounter == 0)
        {
            this.interstitialAdsCounter--;
        }
    }

    public void ShowBanner()
    {
        HZBannerShowOptions showOptions = new HZBannerShowOptions {
            Position = "bottom"
        };
        HZBannerAd.ShowWithOptions(showOptions);
    }

    public void ShowInterstitial()
    {
        if (!PlayerPrefs.HasKey(StaticConstants.interstitialAdsKey) && HZInterstitialAd.IsAvailable())
        {
            HZInterstitialAd.Show();
            HZInterstitialAd.SetDisplayListener(this.listener);
        }
    }

    public void ShowRewarded(RewardedDel del)
    {
        <ShowRewarded>c__AnonStorey0 storey = new <ShowRewarded>c__AnonStorey0 {
            del = del
        };
        if (HZIncentivizedAd.IsAvailable())
        {
            HZIncentivizedAd.Show();
            SoundManager.Instance.MuteAll();
        }
        HZIncentivizedAd.AdDisplayListener listener = new HZIncentivizedAd.AdDisplayListener(storey.<>m__0);
        HZIncentivizedAd.SetDisplayListener(listener);
    }

    private void Start()
    {
        HeyzapAds.Start("66d46934a47c787e8e84005aeeb0f0df", 0);
        HZIncentivizedAd.Fetch();
    }

    public void TestSuite()
    {
        HeyzapAds.ShowMediationTestSuite();
    }

    public static AdsManager instance
    {
        [CompilerGenerated]
        get => 
            <instance>k__BackingField;
        [CompilerGenerated]
        private set => 
            (<instance>k__BackingField = value);
    }

    [CompilerGenerated]
    private sealed class <ShowRewarded>c__AnonStorey0
    {
        internal AdsManager.RewardedDel del;

        internal void <>m__0(string adState, string adTag)
        {
            if (adState.Equals("incentivized_result_complete"))
            {
                this.del();
                SoundManager.Instance.UnMuteAll();
            }
            if (adState.Equals("incentivized_result_incomplete"))
            {
                SoundManager.Instance.UnMuteAll();
            }
        }
    }

    public delegate void RewardedDel();
}

