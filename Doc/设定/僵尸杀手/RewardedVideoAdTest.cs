using AudienceNetwork;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RewardedVideoAdTest : MonoBehaviour
{
    private RewardedVideoAd rewardedVideoAd;
    private bool isLoaded;
    public Text statusLabel;
    [CompilerGenerated]
    private static FBRewardedVideoAdBridgeCallback <>f__am$cache0;
    [CompilerGenerated]
    private static FBRewardedVideoAdBridgeCallback <>f__am$cache1;

    public void LoadRewardedVideo()
    {
        this.statusLabel.text = "Loading rewardedVideo ad...";
        RewardedVideoAd ad = new RewardedVideoAd("YOUR_PLACEMENT_ID");
        this.rewardedVideoAd = ad;
        this.rewardedVideoAd.Register(base.gameObject);
        this.rewardedVideoAd.RewardedVideoAdDidLoad = delegate {
            Debug.Log("RewardedVideo ad loaded.");
            this.isLoaded = true;
            this.statusLabel.text = "Ad loaded. Click show to present!";
        };
        ad.RewardedVideoAdDidFailWithError = delegate (string error) {
            Debug.Log("RewardedVideo ad failed to load with error: " + error);
            this.statusLabel.text = "RewardedVideo ad failed to load. Check console for details.";
        };
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => Debug.Log("RewardedVideo ad logged impression.");
        }
        ad.RewardedVideoAdWillLogImpression = <>f__am$cache0;
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = () => Debug.Log("RewardedVideo ad clicked.");
        }
        ad.RewardedVideoAdDidClick = <>f__am$cache1;
        this.rewardedVideoAd.LoadAd();
    }

    public void NextScene()
    {
        SceneManager.LoadScene("InterstitialAdScene");
    }

    private void OnDestroy()
    {
        if (this.rewardedVideoAd != null)
        {
            this.rewardedVideoAd.Dispose();
        }
        Debug.Log("RewardedVideoAdTest was destroyed!");
    }

    public void ShowRewardedVideo()
    {
        if (this.isLoaded)
        {
            this.rewardedVideoAd.Show();
            this.isLoaded = false;
            this.statusLabel.text = string.Empty;
        }
        else
        {
            this.statusLabel.text = "Ad not loaded. Click load to request an ad.";
        }
    }
}

