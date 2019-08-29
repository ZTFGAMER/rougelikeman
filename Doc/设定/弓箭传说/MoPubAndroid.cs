using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MoPubAndroid : MoPubBase
{
    private static readonly AndroidJavaClass PluginClass = new AndroidJavaClass("com.mopub.unity.MoPubUnityPlugin");
    private static readonly Dictionary<string, MoPubAndroidBanner> BannerPluginsDict = new Dictionary<string, MoPubAndroidBanner>();
    private static readonly Dictionary<string, MoPubAndroidInterstitial> InterstitialPluginsDict = new Dictionary<string, MoPubAndroidInterstitial>();
    private static readonly Dictionary<string, MoPubAndroidRewardedVideo> RewardedVideoPluginsDict = new Dictionary<string, MoPubAndroidRewardedVideo>();

    static MoPubAndroid()
    {
        MoPubBase.InitManager();
    }

    public static void AddFacebookTestDeviceId(string hashedDeviceId)
    {
        object[] args = new object[] { hashedDeviceId };
        PluginClass.CallStatic("addFacebookTestDeviceId", args);
    }

    public static void CreateBanner(string adUnitId, MoPubBase.AdPosition position)
    {
        MoPubLog.Log("CreateBanner", "Attempting to load ad", Array.Empty<object>());
        if (BannerPluginsDict.TryGetValue(adUnitId, out MoPubAndroidBanner banner))
        {
            banner.CreateBanner(position);
        }
        else
        {
            MoPubBase.ReportAdUnitNotFound(adUnitId);
        }
    }

    public static void DestroyBanner(string adUnitId)
    {
        if (BannerPluginsDict.TryGetValue(adUnitId, out MoPubAndroidBanner banner))
        {
            banner.DestroyBanner();
        }
        else
        {
            MoPubBase.ReportAdUnitNotFound(adUnitId);
        }
    }

    public void DestroyInterstitialAd(string adUnitId)
    {
        if (InterstitialPluginsDict.TryGetValue(adUnitId, out MoPubAndroidInterstitial interstitial))
        {
            interstitial.DestroyInterstitialAd();
        }
        else
        {
            MoPubBase.ReportAdUnitNotFound(adUnitId);
        }
    }

    public static void EnableLocationSupport(bool shouldUseLocation)
    {
        object[] args = new object[] { LocationAwareness.NORMAL.ToString() };
        PluginClass.CallStatic("setLocationAwareness", args);
    }

    public static void ForceGdprApplicable()
    {
        PluginClass.CallStatic("forceGdprApplies", Array.Empty<object>());
    }

    public void ForceRefresh(string adUnitId)
    {
        MoPubLog.Log("ForceRefresh", "Attempting to show ad", Array.Empty<object>());
        if (BannerPluginsDict.TryGetValue(adUnitId, out MoPubAndroidBanner banner))
        {
            banner.ForceRefresh();
        }
        else
        {
            MoPubBase.ReportAdUnitNotFound(adUnitId);
        }
    }

    public static List<MoPubBase.Reward> GetAvailableRewards(string adUnitId)
    {
        if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out MoPubAndroidRewardedVideo video))
        {
            return video.GetAvailableRewards();
        }
        MoPubBase.ReportAdUnitNotFound(adUnitId);
        return null;
    }

    protected static string GetSdkName() => 
        ("Android SDK v" + PluginClass.CallStatic<string>("getSDKVersion", Array.Empty<object>()));

    public static bool HasRewardedVideo(string adUnitId)
    {
        if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out MoPubAndroidRewardedVideo video))
        {
            return video.HasRewardedVideo();
        }
        MoPubBase.ReportAdUnitNotFound(adUnitId);
        return false;
    }

    public static void InitializeSdk(MoPubBase.SdkConfiguration sdkConfiguration)
    {
        MoPubBase.logLevel = sdkConfiguration.LogLevel;
        MoPubLog.Log("InitializeSdk", "SDK initialization started", Array.Empty<object>());
        MoPubBase.ValidateAdUnitForSdkInit(sdkConfiguration.AdUnitId);
        object[] args = new object[] { sdkConfiguration.AdUnitId, sdkConfiguration.AdditionalNetworksString, sdkConfiguration.MediationSettingsJson, sdkConfiguration.AllowLegitimateInterest, (int) sdkConfiguration.LogLevel, sdkConfiguration.NetworkConfigurationsJson, sdkConfiguration.MoPubRequestOptionsJson };
        PluginClass.CallStatic("initializeSdk", args);
    }

    public static void InitializeSdk(string anyAdUnitId)
    {
        MoPubBase.ValidateAdUnitForSdkInit(anyAdUnitId);
        MoPubBase.SdkConfiguration sdkConfiguration = new MoPubBase.SdkConfiguration {
            AdUnitId = anyAdUnitId
        };
        InitializeSdk(sdkConfiguration);
    }

    public bool IsInterstialReady(string adUnitId)
    {
        if (InterstitialPluginsDict.TryGetValue(adUnitId, out MoPubAndroidInterstitial interstitial))
        {
            return interstitial.IsInterstitialReady;
        }
        MoPubBase.ReportAdUnitNotFound(adUnitId);
        return false;
    }

    public static void LoadBannerPluginsForAdUnits(string[] bannerAdUnitIds)
    {
        foreach (string str in bannerAdUnitIds)
        {
            BannerPluginsDict[str] = new MoPubAndroidBanner(str);
        }
        Debug.Log(bannerAdUnitIds.Length + " banner AdUnits loaded for plugins:\n" + string.Join(", ", bannerAdUnitIds));
    }

    public static void LoadConsentDialog()
    {
        MoPubLog.Log("LoadConsentDialog", "Attempting to load consent dialog", Array.Empty<object>());
        PluginClass.CallStatic("loadConsentDialog", Array.Empty<object>());
    }

    public static void LoadInterstitialPluginsForAdUnits(string[] interstitialAdUnitIds)
    {
        foreach (string str in interstitialAdUnitIds)
        {
            InterstitialPluginsDict[str] = new MoPubAndroidInterstitial(str);
        }
        Debug.Log(interstitialAdUnitIds.Length + " interstitial AdUnits loaded for plugins:\n" + string.Join(", ", interstitialAdUnitIds));
    }

    public static void LoadRewardedVideoPluginsForAdUnits(string[] rewardedVideoAdUnitIds)
    {
        foreach (string str in rewardedVideoAdUnitIds)
        {
            RewardedVideoPluginsDict[str] = new MoPubAndroidRewardedVideo(str);
        }
        Debug.Log(rewardedVideoAdUnitIds.Length + " rewarded video AdUnits loaded for plugins:\n" + string.Join(", ", rewardedVideoAdUnitIds));
    }

    public static void RefreshBanner(string adUnitId, string keywords, string userDataKeywords = "")
    {
        MoPubLog.Log("RefreshBanner", "Attempting to show ad", Array.Empty<object>());
        if (BannerPluginsDict.TryGetValue(adUnitId, out MoPubAndroidBanner banner))
        {
            banner.RefreshBanner(keywords, userDataKeywords);
        }
        else
        {
            MoPubBase.ReportAdUnitNotFound(adUnitId);
        }
    }

    public static void ReportApplicationOpen(string iTunesAppId = null)
    {
        PluginClass.CallStatic("reportApplicationOpen", Array.Empty<object>());
    }

    public static void RequestInterstitialAd(string adUnitId, string keywords = "", string userDataKeywords = "")
    {
        MoPubLog.Log("RequestInterstitialAd", "Attempting to load ad", Array.Empty<object>());
        if (InterstitialPluginsDict.TryGetValue(adUnitId, out MoPubAndroidInterstitial interstitial))
        {
            interstitial.RequestInterstitialAd(keywords, userDataKeywords);
        }
        else
        {
            MoPubBase.ReportAdUnitNotFound(adUnitId);
        }
    }

    public static void RequestRewardedVideo(string adUnitId, List<MoPubBase.LocalMediationSetting> mediationSettings = null, string keywords = null, string userDataKeywords = null, double latitude = 99999.0, double longitude = 99999.0, string customerId = null)
    {
        MoPubLog.Log("RequestRewardedVideo", "Attempting to load ad", Array.Empty<object>());
        if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out MoPubAndroidRewardedVideo video))
        {
            video.RequestRewardedVideo(mediationSettings, keywords, userDataKeywords, latitude, longitude, customerId);
        }
        else
        {
            MoPubBase.ReportAdUnitNotFound(adUnitId);
        }
    }

    public static void SelectReward(string adUnitId, MoPubBase.Reward selectedReward)
    {
        if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out MoPubAndroidRewardedVideo video))
        {
            video.SelectReward(selectedReward);
        }
        else
        {
            MoPubBase.ReportAdUnitNotFound(adUnitId);
        }
    }

    public void SetAutorefresh(string adUnitId, bool enabled)
    {
        if (BannerPluginsDict.TryGetValue(adUnitId, out MoPubAndroidBanner banner))
        {
            banner.SetAutorefresh(enabled);
        }
        else
        {
            MoPubBase.ReportAdUnitNotFound(adUnitId);
        }
    }

    public static void ShowBanner(string adUnitId, bool shouldShow)
    {
        MoPubLog.Log("ShowBanner", "Attempting to show ad", Array.Empty<object>());
        if (BannerPluginsDict.TryGetValue(adUnitId, out MoPubAndroidBanner banner))
        {
            banner.ShowBanner(shouldShow);
        }
        else
        {
            MoPubBase.ReportAdUnitNotFound(adUnitId);
        }
    }

    public static void ShowConsentDialog()
    {
        MoPubLog.Log("ShowConsentDialog", "Consent dialog attempting to show", Array.Empty<object>());
        PluginClass.CallStatic("showConsentDialog", Array.Empty<object>());
    }

    public static void ShowInterstitialAd(string adUnitId)
    {
        MoPubLog.Log("ShowInterstitialAd", "Attempting to show ad", Array.Empty<object>());
        if (InterstitialPluginsDict.TryGetValue(adUnitId, out MoPubAndroidInterstitial interstitial))
        {
            interstitial.ShowInterstitialAd();
        }
        else
        {
            MoPubBase.ReportAdUnitNotFound(adUnitId);
        }
    }

    public static void ShowRewardedVideo(string adUnitId, string customData = null)
    {
        MoPubLog.Log("ShowRewardedVideo", "Attempting to show ad", Array.Empty<object>());
        if (RewardedVideoPluginsDict.TryGetValue(adUnitId, out MoPubAndroidRewardedVideo video))
        {
            video.ShowRewardedVideo(customData);
        }
        else
        {
            MoPubBase.ReportAdUnitNotFound(adUnitId);
        }
    }

    public static bool IsSdkInitialized =>
        PluginClass.CallStatic<bool>("isSdkInitialized", Array.Empty<object>());

    public static bool AllowLegitimateInterest
    {
        get => 
            PluginClass.CallStatic<bool>("shouldAllowLegitimateInterest", Array.Empty<object>());
        set
        {
            object[] args = new object[] { value };
            PluginClass.CallStatic("setAllowLegitimateInterest", args);
        }
    }

    public static MoPubBase.LogLevel SdkLogLevel
    {
        get => 
            PluginClass.CallStatic<int>("getLogLevel", Array.Empty<object>());
        set
        {
            object[] args = new object[] { (int) value };
            PluginClass.CallStatic<int>("setLogLevel", args);
            MoPubBase.logLevel = value;
        }
    }

    public static bool CanCollectPersonalInfo =>
        PluginClass.CallStatic<bool>("canCollectPersonalInfo", Array.Empty<object>());

    public static MoPubBase.Consent.Status CurrentConsentStatus =>
        MoPubBase.Consent.FromString(PluginClass.CallStatic<string>("getPersonalInfoConsentState", Array.Empty<object>()));

    public static bool ShouldShowConsentDialog =>
        PluginClass.CallStatic<bool>("shouldShowConsentDialog", Array.Empty<object>());

    public static bool IsConsentDialogReady =>
        PluginClass.CallStatic<bool>("isConsentDialogReady", Array.Empty<object>());

    [Obsolete("Use the property name IsConsentDialogReady instead.")]
    public static bool IsConsentDialogLoaded =>
        IsConsentDialogReady;

    public static bool? IsGdprApplicable
    {
        get
        {
            int num = PluginClass.CallStatic<int>("gdprApplies", Array.Empty<object>());
            return ((num != 0) ? ((num <= 0) ? ((bool?) false) : ((bool?) true)) : null);
        }
    }

    public enum LocationAwareness
    {
        TRUNCATED,
        DISABLED,
        NORMAL
    }

    public static class PartnerApi
    {
        public static void GrantConsent()
        {
            MoPubAndroid.PluginClass.CallStatic("grantConsent", Array.Empty<object>());
        }

        public static void RevokeConsent()
        {
            MoPubAndroid.PluginClass.CallStatic("revokeConsent", Array.Empty<object>());
        }

        public static Uri CurrentConsentPrivacyPolicyUrl
        {
            get
            {
                object[] args = new object[] { MoPubBase.ConsentLanguageCode };
                return MoPubBase.UrlFromString(MoPubAndroid.PluginClass.CallStatic<string>("getCurrentPrivacyPolicyLink", args));
            }
        }

        public static Uri CurrentVendorListUrl
        {
            get
            {
                object[] args = new object[] { MoPubBase.ConsentLanguageCode };
                return MoPubBase.UrlFromString(MoPubAndroid.PluginClass.CallStatic<string>("getCurrentVendorListLink", args));
            }
        }

        public static string CurrentConsentIabVendorListFormat =>
            MoPubAndroid.PluginClass.CallStatic<string>("getCurrentVendorListIabFormat", Array.Empty<object>());

        public static string CurrentConsentPrivacyPolicyVersion =>
            MoPubAndroid.PluginClass.CallStatic<string>("getCurrentPrivacyPolicyVersion", Array.Empty<object>());

        public static string CurrentConsentVendorListVersion =>
            MoPubAndroid.PluginClass.CallStatic<string>("getCurrentVendorListVersion", Array.Empty<object>());

        public static string PreviouslyConsentedIabVendorListFormat =>
            MoPubAndroid.PluginClass.CallStatic<string>("getConsentedVendorListIabFormat", Array.Empty<object>());

        public static string PreviouslyConsentedPrivacyPolicyVersion =>
            MoPubAndroid.PluginClass.CallStatic<string>("getConsentedPrivacyPolicyVersion", Array.Empty<object>());

        public static string PreviouslyConsentedVendorListVersion =>
            MoPubAndroid.PluginClass.CallStatic<string>("getConsentedVendorListVersion", Array.Empty<object>());
    }
}

