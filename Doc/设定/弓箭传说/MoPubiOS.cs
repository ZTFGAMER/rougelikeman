using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MoPubiOS : MoPubBase
{
    private static readonly Dictionary<string, MoPubBinding> PluginsDict = new Dictionary<string, MoPubBinding>();

    static MoPubiOS()
    {
        MoPubBase.InitManager();
    }

    [DllImport("__Internal")]
    private static extern bool _moPubAllowLegitimateInterest();
    [DllImport("__Internal")]
    private static extern bool _moPubCanCollectPersonalInfo();
    [DllImport("__Internal")]
    private static extern string _moPubCurrentConsentIabVendorListFormat();
    [DllImport("__Internal")]
    private static extern string _moPubCurrentConsentPrivacyPolicyUrl(string isoLanguageCode = null);
    [DllImport("__Internal")]
    private static extern string _moPubCurrentConsentPrivacyPolicyVersion();
    [DllImport("__Internal")]
    private static extern int _moPubCurrentConsentStatus();
    [DllImport("__Internal")]
    private static extern string _moPubCurrentConsentVendorListUrl(string isoLanguageCode = null);
    [DllImport("__Internal")]
    private static extern string _moPubCurrentConsentVendorListVersion();
    [DllImport("__Internal")]
    private static extern void _moPubEnableLocationSupport(bool shouldUseLocation);
    [DllImport("__Internal")]
    private static extern int _moPubForceGDPRApplicable();
    [DllImport("__Internal")]
    private static extern void _moPubForceWKWebView(bool shouldForce);
    [DllImport("__Internal")]
    private static extern int _moPubGetLogLevel();
    [DllImport("__Internal")]
    private static extern string _moPubGetSDKVersion();
    [DllImport("__Internal")]
    private static extern void _moPubGrantConsent();
    [DllImport("__Internal")]
    private static extern void _moPubInitializeSdk(string adUnitId, string additionalNetworksJson, string mediationSettingsJson, bool allowLegitimateInterest, int logLevel, string adapterConfigJson, string moPubRequestOptionsJson);
    [DllImport("__Internal")]
    private static extern bool _moPubIsConsentDialogReady();
    [DllImport("__Internal")]
    private static extern int _moPubIsGDPRApplicable();
    [DllImport("__Internal")]
    private static extern bool _moPubIsSdkInitialized();
    [DllImport("__Internal")]
    private static extern void _moPubLoadConsentDialog();
    [DllImport("__Internal")]
    private static extern string _moPubPreviouslyConsentedIabVendorListFormat();
    [DllImport("__Internal")]
    private static extern string _moPubPreviouslyConsentedPrivacyPolicyVersion();
    [DllImport("__Internal")]
    private static extern string _moPubPreviouslyConsentedVendorListVersion();
    [DllImport("__Internal")]
    private static extern void _moPubReportApplicationOpen(string iTunesAppId);
    [DllImport("__Internal")]
    private static extern void _moPubRevokeConsent();
    [DllImport("__Internal")]
    private static extern void _moPubSetAllowLegitimateInterest(bool allowLegitimateInterest);
    [DllImport("__Internal")]
    private static extern void _moPubSetLogLevel(int logLevel);
    [DllImport("__Internal")]
    private static extern bool _moPubShouldShowConsentDialog();
    [DllImport("__Internal")]
    private static extern void _moPubShowConsentDialog();
    public static void CreateBanner(string adUnitId, MoPubBase.AdPosition position, MoPubBase.BannerType bannerType = 0)
    {
        MoPubLog.Log("CreateBanner", "Attempting to load ad", Array.Empty<object>());
        if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding binding))
        {
            binding.CreateBanner(bannerType, position);
        }
        else
        {
            MoPubBase.ReportAdUnitNotFound(adUnitId);
        }
    }

    public static void DestroyBanner(string adUnitId)
    {
        if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding binding))
        {
            binding.DestroyBanner();
        }
        else
        {
            MoPubBase.ReportAdUnitNotFound(adUnitId);
        }
    }

    public void DestroyInterstitialAd(string adUnitId)
    {
        if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding binding))
        {
            binding.DestroyInterstitialAd();
        }
        else
        {
            MoPubBase.ReportAdUnitNotFound(adUnitId);
        }
    }

    public static void EnableLocationSupport(bool shouldUseLocation)
    {
        _moPubEnableLocationSupport(true);
    }

    public static void ForceGdprApplicable()
    {
        _moPubForceGDPRApplicable();
    }

    public void ForceRefresh(string adUnitId)
    {
        MoPubLog.Log("ForceRefresh", "Attempting to show ad", Array.Empty<object>());
        if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding binding))
        {
            binding.ForceRefresh();
        }
        else
        {
            MoPubBase.ReportAdUnitNotFound(adUnitId);
        }
    }

    public static void ForceWKWebView(bool shouldForce)
    {
        _moPubForceWKWebView(shouldForce);
    }

    public static List<MoPubBase.Reward> GetAvailableRewards(string adUnitId)
    {
        if (!PluginsDict.TryGetValue(adUnitId, out MoPubBinding binding))
        {
            MoPubBase.ReportAdUnitNotFound(adUnitId);
            return null;
        }
        List<MoPubBase.Reward> availableRewards = binding.GetAvailableRewards();
        Debug.Log($"GetAvailableRewards found {availableRewards.Count} rewards for ad unit {adUnitId}");
        return availableRewards;
    }

    protected static string GetSdkName() => 
        ("iOS SDK v" + _moPubGetSDKVersion());

    public static bool HasRewardedVideo(string adUnitId)
    {
        if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding binding))
        {
            return binding.HasRewardedVideo();
        }
        MoPubBase.ReportAdUnitNotFound(adUnitId);
        return false;
    }

    public static void InitializeSdk(MoPubBase.SdkConfiguration sdkConfiguration)
    {
        MoPubBase.logLevel = sdkConfiguration.LogLevel;
        MoPubBase.ValidateAdUnitForSdkInit(sdkConfiguration.AdUnitId);
        _moPubInitializeSdk(sdkConfiguration.AdUnitId, sdkConfiguration.AdditionalNetworksString, sdkConfiguration.MediationSettingsJson, sdkConfiguration.AllowLegitimateInterest, (int) sdkConfiguration.LogLevel, sdkConfiguration.NetworkConfigurationsJson, sdkConfiguration.MoPubRequestOptionsJson);
    }

    public static void InitializeSdk(string anyAdUnitId)
    {
        MoPubLog.Log("InitializeSdk", "SDK initialization started", Array.Empty<object>());
        MoPubBase.ValidateAdUnitForSdkInit(anyAdUnitId);
        MoPubBase.SdkConfiguration sdkConfiguration = new MoPubBase.SdkConfiguration {
            AdUnitId = anyAdUnitId
        };
        InitializeSdk(sdkConfiguration);
    }

    public bool IsInterstialReady(string adUnitId)
    {
        if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding binding))
        {
            return binding.IsInterstitialReady;
        }
        MoPubBase.ReportAdUnitNotFound(adUnitId);
        return false;
    }

    public static void LoadBannerPluginsForAdUnits(string[] adUnitIds)
    {
        LoadPluginsForAdUnits(adUnitIds);
    }

    public static void LoadConsentDialog()
    {
        MoPubLog.Log("LoadConsentDialog", "Attempting to load consent dialog", Array.Empty<object>());
        _moPubLoadConsentDialog();
    }

    public static void LoadInterstitialPluginsForAdUnits(string[] adUnitIds)
    {
        LoadPluginsForAdUnits(adUnitIds);
    }

    private static void LoadPluginsForAdUnits(string[] adUnitIds)
    {
        foreach (string str in adUnitIds)
        {
            PluginsDict[str] = new MoPubBinding(str);
        }
        Debug.Log(adUnitIds.Length + " AdUnits loaded for plugins:\n" + string.Join(", ", adUnitIds));
    }

    public static void LoadRewardedVideoPluginsForAdUnits(string[] adUnitIds)
    {
        LoadPluginsForAdUnits(adUnitIds);
    }

    public static void RefreshBanner(string adUnitId, string keywords, string userDataKeywords = "")
    {
        MoPubLog.Log("RefreshBanner", "Attempting to show ad", Array.Empty<object>());
        if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding binding))
        {
            binding.RefreshBanner(keywords, userDataKeywords);
        }
        else
        {
            MoPubBase.ReportAdUnitNotFound(adUnitId);
        }
    }

    public static void ReportApplicationOpen(string iTunesAppId = null)
    {
        _moPubReportApplicationOpen(iTunesAppId);
    }

    public static void RequestInterstitialAd(string adUnitId, string keywords = "", string userDataKeywords = "")
    {
        MoPubLog.Log("RequestInterstitialAd", "Attempting to load ad", Array.Empty<object>());
        if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding binding))
        {
            binding.RequestInterstitialAd(keywords, userDataKeywords);
        }
        else
        {
            MoPubBase.ReportAdUnitNotFound(adUnitId);
        }
    }

    public static void RequestRewardedVideo(string adUnitId, List<MoPubBase.LocalMediationSetting> mediationSettings = null, string keywords = null, string userDataKeywords = null, double latitude = 99999.0, double longitude = 99999.0, string customerId = null)
    {
        MoPubLog.Log("RequestRewardedVideo", "Attempting to load ad", Array.Empty<object>());
        if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding binding))
        {
            binding.RequestRewardedVideo(mediationSettings, keywords, userDataKeywords, latitude, longitude, customerId);
        }
        else
        {
            MoPubBase.ReportAdUnitNotFound(adUnitId);
        }
    }

    public static void SelectReward(string adUnitId, MoPubBase.Reward selectedReward)
    {
        if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding binding))
        {
            binding.SelectedReward = selectedReward;
        }
        else
        {
            MoPubBase.ReportAdUnitNotFound(adUnitId);
        }
    }

    public void SetAutorefresh(string adUnitId, bool enabled)
    {
        if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding binding))
        {
            binding.SetAutorefresh(enabled);
        }
        else
        {
            MoPubBase.ReportAdUnitNotFound(adUnitId);
        }
    }

    public static void ShowBanner(string adUnitId, bool shouldShow)
    {
        MoPubLog.Log("ShowBanner", "Attempting to show ad", Array.Empty<object>());
        if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding binding))
        {
            binding.ShowBanner(shouldShow);
        }
        else
        {
            MoPubBase.ReportAdUnitNotFound(adUnitId);
        }
    }

    public static void ShowConsentDialog()
    {
        MoPubLog.Log("ShowConsentDialog", "Consent dialog attempting to show", Array.Empty<object>());
        _moPubShowConsentDialog();
    }

    public static void ShowInterstitialAd(string adUnitId)
    {
        MoPubLog.Log("ShowInterstitialAd", "Attempting to show ad", Array.Empty<object>());
        if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding binding))
        {
            binding.ShowInterstitialAd();
        }
        else
        {
            MoPubBase.ReportAdUnitNotFound(adUnitId);
        }
    }

    public static void ShowRewardedVideo(string adUnitId, string customData = null)
    {
        MoPubLog.Log("ShowRewardedVideo", "Attempting to show ad", Array.Empty<object>());
        if (PluginsDict.TryGetValue(adUnitId, out MoPubBinding binding))
        {
            binding.ShowRewardedVideo(customData);
        }
        else
        {
            MoPubBase.ReportAdUnitNotFound(adUnitId);
        }
    }

    public static bool IsSdkInitialized =>
        _moPubIsSdkInitialized();

    public static bool AllowLegitimateInterest
    {
        get => 
            _moPubAllowLegitimateInterest();
        set => 
            _moPubSetAllowLegitimateInterest(value);
    }

    public static MoPubBase.LogLevel SdkLogLevel
    {
        get => 
            ((MoPubBase.LogLevel) _moPubGetLogLevel());
        set
        {
            MoPubBase.logLevel = value;
            _moPubSetLogLevel((int) value);
        }
    }

    public static bool CanCollectPersonalInfo =>
        _moPubCanCollectPersonalInfo();

    public static MoPubBase.Consent.Status CurrentConsentStatus =>
        ((MoPubBase.Consent.Status) _moPubCurrentConsentStatus());

    public static bool ShouldShowConsentDialog =>
        _moPubShouldShowConsentDialog();

    public static bool IsConsentDialogReady =>
        _moPubIsConsentDialogReady();

    [Obsolete("Use the property name IsConsentDialogReady instead.")]
    public static bool IsConsentDialogLoaded =>
        IsConsentDialogReady;

    public static bool? IsGdprApplicable
    {
        get
        {
            int num = _moPubIsGDPRApplicable();
            return ((num != 0) ? ((num <= 0) ? ((bool?) false) : ((bool?) true)) : null);
        }
    }

    public static class PartnerApi
    {
        public static void GrantConsent()
        {
            MoPubiOS._moPubGrantConsent();
        }

        public static void RevokeConsent()
        {
            MoPubiOS._moPubRevokeConsent();
        }

        public static Uri CurrentConsentPrivacyPolicyUrl =>
            MoPubBase.UrlFromString(MoPubiOS._moPubCurrentConsentPrivacyPolicyUrl(MoPubBase.ConsentLanguageCode));

        public static Uri CurrentVendorListUrl =>
            MoPubBase.UrlFromString(MoPubiOS._moPubCurrentConsentVendorListUrl(MoPubBase.ConsentLanguageCode));

        public static string CurrentConsentIabVendorListFormat =>
            MoPubiOS._moPubCurrentConsentIabVendorListFormat();

        public static string CurrentConsentPrivacyPolicyVersion =>
            MoPubiOS._moPubCurrentConsentPrivacyPolicyVersion();

        public static string CurrentConsentVendorListVersion =>
            MoPubiOS._moPubCurrentConsentVendorListVersion();

        public static string PreviouslyConsentedIabVendorListFormat =>
            MoPubiOS._moPubPreviouslyConsentedIabVendorListFormat();

        public static string PreviouslyConsentedPrivacyPolicyVersion =>
            MoPubiOS._moPubPreviouslyConsentedPrivacyPolicyVersion();

        public static string PreviouslyConsentedVendorListVersion =>
            MoPubiOS._moPubPreviouslyConsentedVendorListVersion();
    }
}

