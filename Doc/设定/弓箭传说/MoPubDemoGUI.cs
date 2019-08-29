using System;
using System.Collections.Generic;
using UnityEngine;

public class MoPubDemoGUI : MonoBehaviour
{
    private readonly Dictionary<string, bool> _adUnitToLoadedMapping = new Dictionary<string, bool>();
    private readonly Dictionary<string, bool> _adUnitToShownMapping = new Dictionary<string, bool>();
    private readonly Dictionary<string, List<MoPubBase.Reward>> _adUnitToRewardsMapping = new Dictionary<string, List<MoPubBase.Reward>>();
    private bool _consentDialogLoaded;
    private readonly string[] _bannerAdUnits = new string[] { "b195f8dd8ded45fe847ad89ed1d016da" };
    private readonly string[] _interstitialAdUnits = new string[] { "24534e1901884e398f1253216226017e" };
    private readonly string[] _rewardedVideoAdUnits = new string[] { "920b6145fb1546cf8b5cf2ac34638bb7" };
    private readonly string[] _rewardedRichMediaAdUnits = new string[] { "a96ae2ef41d44822af45c6328c4e1eb1" };
    [SerializeField]
    private GUISkin _skin;
    private GUIStyle _smallerFont;
    private int _sectionMarginSize;
    private GUIStyle _centeredStyle;
    private static string _customDataDefaultText = "Optional custom data";
    private string _rvCustomData = _customDataDefaultText;
    private string _rrmCustomData = _customDataDefaultText;
    private bool _canCollectPersonalInfo;
    private MoPubBase.Consent.Status _currentConsentStatus;
    private bool _shouldShowConsentDialog;
    private bool? _isGdprApplicable = false;
    private bool _isGdprForced;
    private string _status = string.Empty;

    private void AddAdUnitsToStateMaps(IEnumerable<string> adUnits)
    {
        foreach (string str in adUnits)
        {
            this._adUnitToLoadedMapping[str] = false;
            this._adUnitToShownMapping[str] = false;
        }
    }

    public void AdDismissed(string adUnit)
    {
        this._adUnitToLoadedMapping[adUnit] = false;
        this.ClearStatusLabel();
    }

    public void AdLoaded(string adUnit)
    {
        this._adUnitToLoadedMapping[adUnit] = true;
        this.UpdateStatusLabel("Loaded " + adUnit);
    }

    private void Awake()
    {
        if ((Screen.width < 960) && (Screen.height < 960))
        {
            this._skin.get_button().set_fixedHeight(50f);
        }
        GUIStyle style = new GUIStyle(this._skin.get_label());
        style.set_fontSize(this._skin.get_button().get_fontSize());
        this._smallerFont = style;
        style = new GUIStyle(this._skin.get_label());
        style.set_alignment(TextAnchor.UpperCenter);
        this._centeredStyle = style;
        this._sectionMarginSize = this._skin.get_label().get_fontSize();
        this.AddAdUnitsToStateMaps(this._bannerAdUnits);
        this.AddAdUnitsToStateMaps(this._interstitialAdUnits);
        this.AddAdUnitsToStateMaps(this._rewardedVideoAdUnits);
        this.AddAdUnitsToStateMaps(this._rewardedRichMediaAdUnits);
        this.ConsentDialogLoaded = false;
    }

    public void BannerLoaded(string adUnitId, float height)
    {
        this.AdLoaded(adUnitId);
        this._adUnitToShownMapping[adUnitId] = true;
    }

    public void ClearStatusLabel()
    {
        this.UpdateStatusLabel(string.Empty);
    }

    public void ConsentStatusChanged(MoPubBase.Consent.Status newStatus, bool canCollectPersonalInfo)
    {
        this._canCollectPersonalInfo = canCollectPersonalInfo;
        this._currentConsentStatus = newStatus;
        this._shouldShowConsentDialog = MoPubAndroid.ShouldShowConsentDialog;
        this.UpdateStatusLabel("Consent status changed");
    }

    private void CreateActionsSection()
    {
        GUILayout.Space((float) this._sectionMarginSize);
        GUILayout.Label("Actions", Array.Empty<GUILayoutOption>());
        if (GUILayout.Button("Report App Open", Array.Empty<GUILayoutOption>()))
        {
            this.ClearStatusLabel();
            MoPubAndroid.ReportApplicationOpen(null);
        }
        if (GUILayout.Button("Enable Location Support", Array.Empty<GUILayoutOption>()))
        {
            this.ClearStatusLabel();
            MoPubAndroid.EnableLocationSupport(true);
        }
    }

    private void CreateBannersSection()
    {
        GUILayout.Space(102f);
        GUILayout.Label("Banners", Array.Empty<GUILayoutOption>());
        if (!IsAdUnitArrayNullOrEmpty(this._bannerAdUnits))
        {
            foreach (string str in this._bannerAdUnits)
            {
                GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
                GUI.set_enabled(!this._adUnitToLoadedMapping[str]);
                if (GUILayout.Button(CreateRequestButtonLabel(str), Array.Empty<GUILayoutOption>()))
                {
                    Debug.Log("requesting banner with AdUnit: " + str);
                    this.UpdateStatusLabel("Requesting " + str);
                    MoPubAndroid.CreateBanner(str, MoPubBase.AdPosition.BottomCenter);
                }
                GUI.set_enabled(this._adUnitToLoadedMapping[str]);
                if (GUILayout.Button("Destroy", Array.Empty<GUILayoutOption>()))
                {
                    this.ClearStatusLabel();
                    MoPubAndroid.DestroyBanner(str);
                    this._adUnitToLoadedMapping[str] = false;
                    this._adUnitToShownMapping[str] = false;
                }
                GUI.set_enabled(!this._adUnitToLoadedMapping[str] ? false : !this._adUnitToShownMapping[str]);
                if (GUILayout.Button("Show", Array.Empty<GUILayoutOption>()))
                {
                    this.ClearStatusLabel();
                    MoPubAndroid.ShowBanner(str, true);
                    this._adUnitToShownMapping[str] = true;
                }
                GUI.set_enabled(!this._adUnitToLoadedMapping[str] ? false : this._adUnitToShownMapping[str]);
                if (GUILayout.Button("Hide", Array.Empty<GUILayoutOption>()))
                {
                    this.ClearStatusLabel();
                    MoPubAndroid.ShowBanner(str, false);
                    this._adUnitToShownMapping[str] = false;
                }
                GUI.set_enabled(true);
                GUILayout.EndHorizontal();
            }
        }
        else
        {
            GUILayout.Label("No banner AdUnits available", this._smallerFont, null);
        }
    }

    private static void CreateCustomDataField(string fieldName, ref string customDataValue)
    {
        GUI.SetNextControlName(fieldName);
        GUILayoutOption[] optionArray1 = new GUILayoutOption[] { GUILayout.MinWidth(200f) };
        customDataValue = GUILayout.TextField(customDataValue, optionArray1);
        if (Event.get_current().get_type() == 7)
        {
            if ((GUI.GetNameOfFocusedControl() == fieldName) && (customDataValue == _customDataDefaultText))
            {
                customDataValue = string.Empty;
            }
            else if ((GUI.GetNameOfFocusedControl() != fieldName) && string.IsNullOrEmpty(customDataValue))
            {
                customDataValue = _customDataDefaultText;
            }
        }
    }

    private void CreateInterstitialsSection()
    {
        GUILayout.Space((float) this._sectionMarginSize);
        GUILayout.Label("Interstitials", Array.Empty<GUILayoutOption>());
        if (!IsAdUnitArrayNullOrEmpty(this._interstitialAdUnits))
        {
            foreach (string str in this._interstitialAdUnits)
            {
                GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
                GUI.set_enabled(!this._adUnitToLoadedMapping[str]);
                if (GUILayout.Button(CreateRequestButtonLabel(str), Array.Empty<GUILayoutOption>()))
                {
                    Debug.Log("requesting interstitial with AdUnit: " + str);
                    this.UpdateStatusLabel("Requesting " + str);
                    MoPubAndroid.RequestInterstitialAd(str, string.Empty, string.Empty);
                }
                GUI.set_enabled(this._adUnitToLoadedMapping[str]);
                if (GUILayout.Button("Show", Array.Empty<GUILayoutOption>()))
                {
                    this.ClearStatusLabel();
                    MoPubAndroid.ShowInterstitialAd(str);
                }
                GUI.set_enabled(true);
                GUILayout.EndHorizontal();
            }
        }
        else
        {
            GUILayout.Label("No interstitial AdUnits available", this._smallerFont, null);
        }
    }

    private static string CreateRequestButtonLabel(string adUnit) => 
        ((adUnit.Length <= 10) ? adUnit : ("Request " + adUnit.Substring(0, 10) + "..."));

    private void CreateRewardedRichMediaSection()
    {
        GUILayout.Space((float) this._sectionMarginSize);
        GUILayout.Label("Rewarded Rich Media", Array.Empty<GUILayoutOption>());
        if (!IsAdUnitArrayNullOrEmpty(this._rewardedRichMediaAdUnits))
        {
            CreateCustomDataField("rrmCustomDataField", ref this._rrmCustomData);
            foreach (string str in this._rewardedRichMediaAdUnits)
            {
                GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
                GUI.set_enabled(!this._adUnitToLoadedMapping[str]);
                if (GUILayout.Button(CreateRequestButtonLabel(str), Array.Empty<GUILayoutOption>()))
                {
                    Debug.Log("requesting rewarded rich media with AdUnit: " + str);
                    this.UpdateStatusLabel("Requesting " + str);
                    MoPubAndroid.RequestRewardedVideo(str, null, "rewarded, video, mopub", null, 37.7833, 122.4167, "customer101");
                }
                GUI.set_enabled(this._adUnitToLoadedMapping[str]);
                if (GUILayout.Button("Show", Array.Empty<GUILayoutOption>()))
                {
                    this.ClearStatusLabel();
                    MoPubAndroid.ShowRewardedVideo(str, GetCustomData(this._rrmCustomData));
                }
                GUI.set_enabled(true);
                GUILayout.EndHorizontal();
                if ((MoPubAndroid.HasRewardedVideo(str) && this._adUnitToRewardsMapping.ContainsKey(str)) && (this._adUnitToRewardsMapping[str].Count > 1))
                {
                    GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
                    GUILayout.Space((float) this._sectionMarginSize);
                    GUILayout.Label("Select a reward:", Array.Empty<GUILayoutOption>());
                    foreach (MoPubBase.Reward reward in this._adUnitToRewardsMapping[str])
                    {
                        if (GUILayout.Button(reward.ToString(), Array.Empty<GUILayoutOption>()))
                        {
                            MoPubAndroid.SelectReward(str, reward);
                        }
                    }
                    GUILayout.Space((float) this._sectionMarginSize);
                    GUILayout.EndVertical();
                }
            }
        }
        else
        {
            GUILayout.Label("No rewarded rich media AdUnits available", this._smallerFont, null);
        }
    }

    private void CreateRewardedVideosSection()
    {
        GUILayout.Space((float) this._sectionMarginSize);
        GUILayout.Label("Rewarded Videos", Array.Empty<GUILayoutOption>());
        if (!IsAdUnitArrayNullOrEmpty(this._rewardedVideoAdUnits))
        {
            CreateCustomDataField("rvCustomDataField", ref this._rvCustomData);
            foreach (string str in this._rewardedVideoAdUnits)
            {
                GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
                GUI.set_enabled(!this._adUnitToLoadedMapping[str]);
                if (GUILayout.Button(CreateRequestButtonLabel(str), Array.Empty<GUILayoutOption>()))
                {
                    Debug.Log("requesting rewarded video with AdUnit: " + str);
                    this.UpdateStatusLabel("Requesting " + str);
                    MoPubAndroid.RequestRewardedVideo(str, null, "rewarded, video, mopub", null, 37.7833, 122.4167, "customer101");
                }
                GUI.set_enabled(this._adUnitToLoadedMapping[str]);
                if (GUILayout.Button("Show", Array.Empty<GUILayoutOption>()))
                {
                    this.ClearStatusLabel();
                    MoPubAndroid.ShowRewardedVideo(str, GetCustomData(this._rvCustomData));
                }
                GUI.set_enabled(true);
                GUILayout.EndHorizontal();
                if ((MoPubAndroid.HasRewardedVideo(str) && this._adUnitToRewardsMapping.ContainsKey(str)) && (this._adUnitToRewardsMapping[str].Count > 1))
                {
                    GUILayout.BeginVertical(Array.Empty<GUILayoutOption>());
                    GUILayout.Space((float) this._sectionMarginSize);
                    GUILayout.Label("Select a reward:", Array.Empty<GUILayoutOption>());
                    foreach (MoPubBase.Reward reward in this._adUnitToRewardsMapping[str])
                    {
                        if (GUILayout.Button(reward.ToString(), Array.Empty<GUILayoutOption>()))
                        {
                            MoPubAndroid.SelectReward(str, reward);
                        }
                    }
                    GUILayout.Space((float) this._sectionMarginSize);
                    GUILayout.EndVertical();
                }
            }
        }
        else
        {
            GUILayout.Label("No rewarded video AdUnits available", this._smallerFont, null);
        }
    }

    private void CreateStatusSection()
    {
        GUILayout.Space(40f);
        GUILayout.Label(this._status, this._smallerFont, Array.Empty<GUILayoutOption>());
    }

    private void CreateTitleSection()
    {
        int num = this._centeredStyle.get_fontSize();
        this._centeredStyle.set_fontSize(0x30);
        GUI.Label(new Rect(0f, 10f, (float) Screen.width, 60f), MoPubBase.PluginName, this._centeredStyle);
        this._centeredStyle.set_fontSize(num);
        GUI.Label(new Rect(0f, 70f, (float) Screen.width, 60f), "with " + MoPub.SdkName, this._centeredStyle);
    }

    private void CreateUserConsentSection()
    {
        GUILayout.Space((float) this._sectionMarginSize);
        GUILayout.Label("User Consent", Array.Empty<GUILayoutOption>());
        GUILayout.Label("Can collect personally identifiable information: " + this._canCollectPersonalInfo, this._smallerFont, Array.Empty<GUILayoutOption>());
        GUILayout.Label("Current consent status: " + this._currentConsentStatus, this._smallerFont, Array.Empty<GUILayoutOption>());
        GUILayout.Label("Should show consent dialog: " + this._shouldShowConsentDialog, this._smallerFont, Array.Empty<GUILayoutOption>());
        GUILayout.Label("Is GDPR applicable: " + (!this._isGdprApplicable.HasValue ? "Unknown" : this._isGdprApplicable.ToString()), this._smallerFont, Array.Empty<GUILayoutOption>());
        GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
        GUI.set_enabled(!this.ConsentDialogLoaded);
        if (GUILayout.Button("Load Consent Dialog", Array.Empty<GUILayoutOption>()))
        {
            this.UpdateStatusLabel("Loading consent dialog");
            MoPubAndroid.LoadConsentDialog();
        }
        GUI.set_enabled(this.ConsentDialogLoaded);
        if (GUILayout.Button("Show Consent Dialog", Array.Empty<GUILayoutOption>()))
        {
            this.ClearStatusLabel();
            MoPubAndroid.ShowConsentDialog();
        }
        GUI.set_enabled(!this._isGdprForced);
        if (GUILayout.Button("Force GDPR", Array.Empty<GUILayoutOption>()))
        {
            this.ClearStatusLabel();
            MoPubAndroid.ForceGdprApplicable();
            this.UpdateConsentValues();
            this._isGdprForced = true;
        }
        GUI.set_enabled(true);
        if (GUILayout.Button("Grant Consent", Array.Empty<GUILayoutOption>()))
        {
            MoPubAndroid.PartnerApi.GrantConsent();
        }
        if (GUILayout.Button("Revoke Consent", Array.Empty<GUILayoutOption>()))
        {
            MoPubAndroid.PartnerApi.RevokeConsent();
        }
        GUI.set_enabled(true);
        GUILayout.EndHorizontal();
    }

    private static string GetCustomData(string customDataFieldValue) => 
        ((customDataFieldValue == _customDataDefaultText) ? null : customDataFieldValue);

    private static bool IsAdUnitArrayNullOrEmpty(ICollection<string> adUnitArray) => 
        ((adUnitArray == null) || (adUnitArray.Count == 0));

    public void LoadAvailableRewards(string adUnitId, List<MoPubBase.Reward> availableRewards)
    {
        this._adUnitToRewardsMapping.Remove(adUnitId);
        if (availableRewards != null)
        {
            this._adUnitToRewardsMapping[adUnitId] = availableRewards;
        }
    }

    private void OnGUI()
    {
        GUI.set_skin(this._skin);
        Rect safeArea = Screen.safeArea;
        safeArea.x += 20f;
        safeArea.width -= 40f;
        GUILayout.BeginArea(safeArea);
        this.CreateTitleSection();
        this.CreateBannersSection();
        this.CreateInterstitialsSection();
        this.CreateRewardedVideosSection();
        this.CreateRewardedRichMediaSection();
        this.CreateUserConsentSection();
        this.CreateActionsSection();
        this.CreateStatusSection();
        GUILayout.EndArea();
    }

    public void SdkInitialized()
    {
        this.UpdateConsentValues();
    }

    private void Start()
    {
        string str = this._bannerAdUnits[0];
        MoPubBase.SdkConfiguration sdkConfiguration = new MoPubBase.SdkConfiguration {
            AdUnitId = str,
            LogLevel = MoPubBase.LogLevel.MPLogLevelDebug,
            MediatedNetworks = new MoPubBase.MediatedNetwork[0]
        };
        MoPubAndroid.InitializeSdk(sdkConfiguration);
        MoPubAndroid.LoadBannerPluginsForAdUnits(this._bannerAdUnits);
        MoPubAndroid.LoadInterstitialPluginsForAdUnits(this._interstitialAdUnits);
        MoPubAndroid.LoadRewardedVideoPluginsForAdUnits(this._rewardedVideoAdUnits);
        MoPubAndroid.LoadRewardedVideoPluginsForAdUnits(this._rewardedRichMediaAdUnits);
        GameObject obj2 = GameObject.Find("MoPubNativeAds");
        if (obj2 != null)
        {
            obj2.SetActive(false);
        }
    }

    private void UpdateConsentValues()
    {
        this._canCollectPersonalInfo = MoPubAndroid.CanCollectPersonalInfo;
        this._currentConsentStatus = MoPubAndroid.CurrentConsentStatus;
        this._shouldShowConsentDialog = MoPubAndroid.ShouldShowConsentDialog;
        this._isGdprApplicable = MoPubAndroid.IsGdprApplicable;
    }

    public void UpdateStatusLabel(string message)
    {
        this._status = message;
    }

    public bool ConsentDialogLoaded
    {
        private get => 
            this._consentDialogLoaded;
        set
        {
            this._consentDialogLoaded = value;
            if (this._consentDialogLoaded)
            {
                this.UpdateStatusLabel("Consent dialog loaded");
            }
        }
    }
}

