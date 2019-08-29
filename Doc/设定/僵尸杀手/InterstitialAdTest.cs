using AudienceNetwork;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InterstitialAdTest : MonoBehaviour
{
    private InterstitialAd interstitialAd;
    private bool isLoaded;
    public Text statusLabel;
    [CompilerGenerated]
    private static FBInterstitialAdBridgeCallback <>f__am$cache0;
    [CompilerGenerated]
    private static FBInterstitialAdBridgeCallback <>f__am$cache1;

    public void LoadInterstitial()
    {
        this.statusLabel.text = "Loading interstitial ad...";
        InterstitialAd ad = new InterstitialAd("YOUR_PLACEMENT_ID");
        this.interstitialAd = ad;
        this.interstitialAd.Register(base.gameObject);
        this.interstitialAd.InterstitialAdDidLoad = delegate {
            Debug.Log("Interstitial ad loaded.");
            this.isLoaded = true;
            this.statusLabel.text = "Ad loaded. Click show to present!";
        };
        ad.InterstitialAdDidFailWithError = delegate (string error) {
            Debug.Log("Interstitial ad failed to load with error: " + error);
            this.statusLabel.text = "Interstitial ad failed to load. Check console for details.";
        };
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => Debug.Log("Interstitial ad logged impression.");
        }
        ad.InterstitialAdWillLogImpression = <>f__am$cache0;
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = () => Debug.Log("Interstitial ad clicked.");
        }
        ad.InterstitialAdDidClick = <>f__am$cache1;
        this.interstitialAd.LoadAd();
    }

    public void NextScene()
    {
        SceneManager.LoadScene("AdViewScene");
    }

    private void OnDestroy()
    {
        if (this.interstitialAd != null)
        {
            this.interstitialAd.Dispose();
        }
        Debug.Log("InterstitialAdTest was destroyed!");
    }

    public void ShowInterstitial()
    {
        if (this.isLoaded)
        {
            this.interstitialAd.Show();
            this.isLoaded = false;
            this.statusLabel.text = string.Empty;
        }
        else
        {
            this.statusLabel.text = "Ad not loaded. Click load to request an ad.";
        }
    }
}

