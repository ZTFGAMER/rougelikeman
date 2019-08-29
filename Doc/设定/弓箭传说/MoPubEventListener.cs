using System;
using System.Collections.Generic;
using UnityEngine;

public class MoPubEventListener : MonoBehaviour
{
    [SerializeField]
    private MoPubDemoGUI _demoGUI;

    private void AdFailed(string adUnitId, string action, string error)
    {
        string str = "Failed to " + action + " ad unit " + adUnitId;
        if (!string.IsNullOrEmpty(error))
        {
            str = str + ": " + error;
        }
        this._demoGUI.UpdateStatusLabel("Error: " + str);
    }

    private void Awake()
    {
        if (this._demoGUI == null)
        {
            this._demoGUI = base.GetComponent<MoPubDemoGUI>();
        }
        if (this._demoGUI == null)
        {
            Debug.LogError("Missing reference to MoPubDemoGUI.  Please fix in the editor.");
            Object.Destroy(this);
        }
    }

    private void OnAdFailedEvent(string adUnitId, string error)
    {
        this.AdFailed(adUnitId, "load banner", error);
    }

    private void OnAdLoadedEvent(string adUnitId, float height)
    {
        this._demoGUI.BannerLoaded(adUnitId, height);
    }

    private void OnConsentDialogFailedEvent(string err)
    {
        this._demoGUI.UpdateStatusLabel(err);
    }

    private void OnConsentDialogLoadedEvent()
    {
        this._demoGUI.ConsentDialogLoaded = true;
    }

    private void OnConsentDialogShownEvent()
    {
        this._demoGUI.ConsentDialogLoaded = false;
    }

    private void OnConsentStatusChangedEvent(MoPubBase.Consent.Status oldStatus, MoPubBase.Consent.Status newStatus, bool canCollectPersonalInfo)
    {
        this._demoGUI.ConsentStatusChanged(newStatus, canCollectPersonalInfo);
    }

    private void OnDisable()
    {
        MoPubManager.OnSdkInitializedEvent -= new Action<string>(this.OnSdkInitializedEvent);
        MoPubManager.OnConsentStatusChangedEvent -= new Action<MoPubBase.Consent.Status, MoPubBase.Consent.Status, bool>(this.OnConsentStatusChangedEvent);
        MoPubManager.OnConsentDialogLoadedEvent -= new Action(this.OnConsentDialogLoadedEvent);
        MoPubManager.OnConsentDialogFailedEvent -= new Action<string>(this.OnConsentDialogFailedEvent);
        MoPubManager.OnConsentDialogShownEvent -= new Action(this.OnConsentDialogShownEvent);
        MoPubManager.OnAdLoadedEvent -= new Action<string, float>(this.OnAdLoadedEvent);
        MoPubManager.OnAdFailedEvent -= new Action<string, string>(this.OnAdFailedEvent);
        MoPubManager.OnInterstitialLoadedEvent -= new Action<string>(this.OnInterstitialLoadedEvent);
        MoPubManager.OnInterstitialFailedEvent -= new Action<string, string>(this.OnInterstitialFailedEvent);
        MoPubManager.OnInterstitialDismissedEvent -= new Action<string>(this.OnInterstitialDismissedEvent);
        MoPubManager.OnRewardedVideoLoadedEvent -= new Action<string>(this.OnRewardedVideoLoadedEvent);
        MoPubManager.OnRewardedVideoFailedEvent -= new Action<string, string>(this.OnRewardedVideoFailedEvent);
        MoPubManager.OnRewardedVideoFailedToPlayEvent -= new Action<string, string>(this.OnRewardedVideoFailedToPlayEvent);
        MoPubManager.OnRewardedVideoClosedEvent -= new Action<string>(this.OnRewardedVideoClosedEvent);
    }

    private void OnEnable()
    {
        MoPubManager.OnSdkInitializedEvent += new Action<string>(this.OnSdkInitializedEvent);
        MoPubManager.OnConsentStatusChangedEvent += new Action<MoPubBase.Consent.Status, MoPubBase.Consent.Status, bool>(this.OnConsentStatusChangedEvent);
        MoPubManager.OnConsentDialogLoadedEvent += new Action(this.OnConsentDialogLoadedEvent);
        MoPubManager.OnConsentDialogFailedEvent += new Action<string>(this.OnConsentDialogFailedEvent);
        MoPubManager.OnConsentDialogShownEvent += new Action(this.OnConsentDialogShownEvent);
        MoPubManager.OnAdLoadedEvent += new Action<string, float>(this.OnAdLoadedEvent);
        MoPubManager.OnAdFailedEvent += new Action<string, string>(this.OnAdFailedEvent);
        MoPubManager.OnInterstitialLoadedEvent += new Action<string>(this.OnInterstitialLoadedEvent);
        MoPubManager.OnInterstitialFailedEvent += new Action<string, string>(this.OnInterstitialFailedEvent);
        MoPubManager.OnInterstitialDismissedEvent += new Action<string>(this.OnInterstitialDismissedEvent);
        MoPubManager.OnRewardedVideoLoadedEvent += new Action<string>(this.OnRewardedVideoLoadedEvent);
        MoPubManager.OnRewardedVideoFailedEvent += new Action<string, string>(this.OnRewardedVideoFailedEvent);
        MoPubManager.OnRewardedVideoFailedToPlayEvent += new Action<string, string>(this.OnRewardedVideoFailedToPlayEvent);
        MoPubManager.OnRewardedVideoClosedEvent += new Action<string>(this.OnRewardedVideoClosedEvent);
    }

    private void OnInterstitialDismissedEvent(string adUnitId)
    {
        this._demoGUI.AdDismissed(adUnitId);
    }

    private void OnInterstitialFailedEvent(string adUnitId, string error)
    {
        this.AdFailed(adUnitId, "load interstitial", error);
    }

    private void OnInterstitialLoadedEvent(string adUnitId)
    {
        this._demoGUI.AdLoaded(adUnitId);
    }

    private void OnRewardedVideoClosedEvent(string adUnitId)
    {
        this._demoGUI.AdDismissed(adUnitId);
    }

    private void OnRewardedVideoFailedEvent(string adUnitId, string error)
    {
        this.AdFailed(adUnitId, "load rewarded video", error);
    }

    private void OnRewardedVideoFailedToPlayEvent(string adUnitId, string error)
    {
        this.AdFailed(adUnitId, "play rewarded video", error);
    }

    private void OnRewardedVideoLoadedEvent(string adUnitId)
    {
        List<MoPubBase.Reward> availableRewards = MoPubAndroid.GetAvailableRewards(adUnitId);
        this._demoGUI.AdLoaded(adUnitId);
        this._demoGUI.LoadAvailableRewards(adUnitId, availableRewards);
    }

    private void OnSdkInitializedEvent(string adUnitId)
    {
        this._demoGUI.SdkInitialized();
    }
}

