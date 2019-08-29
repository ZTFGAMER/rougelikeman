using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public class MoPubBinding
{
    public MoPubBase.Reward SelectedReward;
    private readonly string _adUnitId;
    [CompilerGenerated]
    private static Func<string, string[]> <>f__am$cache0;
    [CompilerGenerated]
    private static Func<string[], bool> <>f__am$cache1;

    public MoPubBinding(string adUnitId)
    {
        this._adUnitId = adUnitId;
        MoPubBase.Reward reward = new MoPubBase.Reward {
            Label = string.Empty
        };
        this.SelectedReward = reward;
    }

    [DllImport("__Internal")]
    private static extern void _moPubCreateBanner(int bannerType, int position, string adUnitId);
    [DllImport("__Internal")]
    private static extern void _moPubDestroyBanner(string adUnitId);
    [DllImport("__Internal")]
    private static extern void _moPubDestroyInterstitialAd(string adUnitId);
    [DllImport("__Internal")]
    private static extern void _moPubForceRefresh(string adUnitId);
    [DllImport("__Internal")]
    private static extern string _mopubGetAvailableRewards(string adUnitId);
    [DllImport("__Internal")]
    private static extern bool _mopubHasRewardedVideo(string adUnitId);
    [DllImport("__Internal")]
    private static extern bool _moPubIsInterstitialReady(string adUnitId);
    [DllImport("__Internal")]
    private static extern void _moPubRefreshBanner(string adUnitId, string keywords, string userDataKeywords);
    [DllImport("__Internal")]
    private static extern void _moPubRequestInterstitialAd(string adUnitId, string keywords, string userDataKeywords);
    [DllImport("__Internal")]
    private static extern void _moPubRequestRewardedVideo(string adUnitId, string json, string keywords, string userDataKeywords, double latitude, double longitude, string customerId);
    [DllImport("__Internal")]
    private static extern void _moPubSetAutorefreshEnabled(string adUnitId, bool enabled);
    [DllImport("__Internal")]
    private static extern void _moPubShowBanner(string adUnitId, bool shouldShow);
    [DllImport("__Internal")]
    private static extern void _moPubShowInterstitialAd(string adUnitId);
    [DllImport("__Internal")]
    private static extern void _moPubShowRewardedVideo(string adUnitId, string currencyName, int currencyAmount, string customData);
    public void CreateBanner(MoPubBase.BannerType bannerType, MoPubBase.AdPosition position)
    {
        _moPubCreateBanner((int) bannerType, (int) position, this._adUnitId);
    }

    public void DestroyBanner()
    {
        _moPubDestroyBanner(this._adUnitId);
    }

    public void DestroyInterstitialAd()
    {
        _moPubDestroyInterstitialAd(this._adUnitId);
    }

    public void ForceRefresh()
    {
        _moPubForceRefresh(this._adUnitId);
    }

    public List<MoPubBase.Reward> GetAvailableRewards()
    {
        string str;
        <GetAvailableRewards>c__AnonStorey0 storey = new <GetAvailableRewards>c__AnonStorey0 {
            amount = 0
        };
        string text1 = _mopubGetAvailableRewards(this._adUnitId);
        if (text1 != null)
        {
            str = text1;
        }
        else
        {
            str = string.Empty;
        }
        char[] separator = new char[] { ',' };
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = rewardString => rewardString.Split(new char[] { ':' });
        }
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = rewardComponents => rewardComponents.Length == 2;
        }
        return str.Split(separator).Select<string, string[]>(<>f__am$cache0).Where<string[]>(<>f__am$cache1).Where<string[]>(new Func<string[], bool>(storey.<>m__0)).Select<string[], MoPubBase.Reward>(new Func<string[], MoPubBase.Reward>(storey.<>m__1)).ToList<MoPubBase.Reward>();
    }

    public bool HasRewardedVideo() => 
        _mopubHasRewardedVideo(this._adUnitId);

    public void RefreshBanner(string keywords = "", string userDataKeywords = "")
    {
        _moPubRefreshBanner(this._adUnitId, keywords, userDataKeywords);
    }

    public void RequestInterstitialAd(string keywords = "", string userDataKeywords = "")
    {
        _moPubRequestInterstitialAd(this._adUnitId, keywords, userDataKeywords);
    }

    public void RequestRewardedVideo(List<MoPubBase.LocalMediationSetting> mediationSettings = null, string keywords = null, string userDataKeywords = null, double latitude = 99999.0, double longitude = 99999.0, string customerId = null)
    {
        string json = MoPubBase.LocalMediationSetting.ToJson(mediationSettings);
        _moPubRequestRewardedVideo(this._adUnitId, json, keywords, userDataKeywords, latitude, longitude, customerId);
    }

    public void SetAutorefresh(bool enabled)
    {
        _moPubSetAutorefreshEnabled(this._adUnitId, enabled);
    }

    public void ShowBanner(bool shouldShow)
    {
        _moPubShowBanner(this._adUnitId, shouldShow);
    }

    public void ShowInterstitialAd()
    {
        _moPubShowInterstitialAd(this._adUnitId);
    }

    public void ShowRewardedVideo(string customData)
    {
        _moPubShowRewardedVideo(this._adUnitId, this.SelectedReward.Label, this.SelectedReward.Amount, customData);
    }

    public bool IsInterstitialReady =>
        _moPubIsInterstitialReady(this._adUnitId);

    [CompilerGenerated]
    private sealed class <GetAvailableRewards>c__AnonStorey0
    {
        internal int amount;

        internal bool <>m__0(string[] rewardComponents) => 
            int.TryParse(rewardComponents[1], out this.amount);

        internal MoPubBase.Reward <>m__1(string[] rewardComponents) => 
            new MoPubBase.Reward { 
                Label = rewardComponents[0],
                Amount = this.amount
            };
    }
}

