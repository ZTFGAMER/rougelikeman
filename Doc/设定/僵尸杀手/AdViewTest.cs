using AudienceNetwork;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdViewTest : MonoBehaviour
{
    private AdView adView;
    private AdPosition currentAdViewPosition;
    [CompilerGenerated]
    private static FBAdViewBridgeErrorCallback <>f__am$cache0;
    [CompilerGenerated]
    private static FBAdViewBridgeCallback <>f__am$cache1;
    [CompilerGenerated]
    private static FBAdViewBridgeCallback <>f__am$cache2;

    private void Awake()
    {
        AdView view = new AdView("YOUR_PLACEMENT_ID", AdSize.BANNER_HEIGHT_50);
        this.adView = view;
        this.adView.Register(base.gameObject);
        this.currentAdViewPosition = AdPosition.CUSTOM;
        this.adView.AdViewDidLoad = delegate {
            Debug.Log("Ad view loaded.");
            this.adView.Show((double) 100.0);
        };
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = error => Debug.Log("Ad view failed to load with error: " + error);
        }
        view.AdViewDidFailWithError = <>f__am$cache0;
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = () => Debug.Log("Ad view logged impression.");
        }
        view.AdViewWillLogImpression = <>f__am$cache1;
        if (<>f__am$cache2 == null)
        {
            <>f__am$cache2 = () => Debug.Log("Ad view clicked.");
        }
        view.AdViewDidClick = <>f__am$cache2;
        view.LoadAd();
    }

    public void ChangePosition()
    {
        switch (this.currentAdViewPosition)
        {
            case AdPosition.TOP:
                this.adView.Show(AdPosition.BOTTOM);
                this.currentAdViewPosition = AdPosition.BOTTOM;
                break;

            case AdPosition.BOTTOM:
                this.adView.Show((double) 100.0);
                this.currentAdViewPosition = AdPosition.CUSTOM;
                break;

            case AdPosition.CUSTOM:
                this.adView.Show(AdPosition.TOP);
                this.currentAdViewPosition = AdPosition.TOP;
                break;
        }
    }

    public void NextScene()
    {
        SceneManager.LoadScene("NativeAdScene");
    }

    private void OnDestroy()
    {
        if (this.adView != null)
        {
            this.adView.Dispose();
        }
        Debug.Log("AdViewTest was destroyed!");
    }
}

