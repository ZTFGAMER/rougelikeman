using IAP;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoMultiplier : MonoBehaviour
{
    public Image multiplierImage;
    public GameObject infinityTimerImage;
    public GameObject infinityTimerGreen;
    public GameObject imageDone;
    public Text timeText;
    public Text textInfinityPrice;
    public Button buyButton;
    public Button rewardButton;
    public Image imageRewardButton;
    public Image imageRewardVideo;
    [SerializeField]
    private OverlayParticle prefabFX;
    public GameObject redCircle;
    [SerializeField]
    private Text staticTimeText;

    public void ButtonColor(bool inactive)
    {
        if (inactive)
        {
            this.imageRewardButton.set_sprite(DataLoader.gui.multiplyImages.rewardedButtons[1].rewardedButton);
            this.imageRewardVideo.set_sprite(DataLoader.gui.multiplyImages.rewardedButtons[1].videoImage);
        }
        else
        {
            this.imageRewardButton.set_sprite(DataLoader.gui.multiplyImages.rewardedButtons[0].rewardedButton);
            this.imageRewardVideo.set_sprite(DataLoader.gui.multiplyImages.rewardedButtons[0].videoImage);
        }
    }

    public void BuyInfinityMultiplier()
    {
        InAppManager.Instance.BuyProductID(InAppManager.Instance.infinityMultiplier.index);
    }

    private void GetReward()
    {
        Dictionary<string, string> eventParameters = new Dictionary<string, string> {
            { 
                "CurrentMultiplier",
                DataLoader.Instance.dailyMultiplier.ToString()
            }
        };
        AnalyticsManager.instance.LogEvent("IncreaseMultiplierTimeVideo", eventParameters);
        DataLoader.Instance.SetNewMultiplier((DataLoader.Instance.dailyMultiplier != 1) ? DataLoader.Instance.dailyMultiplier : 2, StaticConstants.MultiplierDurationInSeconds);
        this.prefabFX.Play();
    }

    public void OnEnable()
    {
        bool flag = PlayerPrefs.HasKey(StaticConstants.infinityMultiplierPurchased);
        this.textInfinityPrice.gameObject.SetActive(!flag);
        this.buyButton.interactable = !flag;
        this.ButtonColor((DataLoader.Instance.dailyMultiplier <= 2) && flag);
        this.textInfinityPrice.text = InAppManager.Instance.GetInfinityMultiplierPrice();
        this.imageDone.SetActive(flag);
        this.multiplierImage.set_sprite(DataLoader.gui.multiplyImages.activeMultiplier[MultiplyImages.GetMultiplierSpriteID(DataLoader.Instance.dailyMultiplier)]);
    }

    public void Reward()
    {
        AdsManager.instance.ShowRewarded(() => this.GetReward());
    }

    public void SetInfinityPurchased()
    {
        this.imageDone.SetActive(true);
        this.textInfinityPrice.gameObject.SetActive(false);
        bool flag = DataLoader.Instance.dailyMultiplier <= 2;
        this.rewardButton.interactable = !flag;
        this.infinityTimerGreen.SetActive(flag);
        this.infinityTimerImage.SetActive(flag);
        this.infinityTimerImage.SetActive(flag);
        this.timeText.gameObject.SetActive(!flag);
        this.rewardButton.interactable = !flag;
        this.ButtonColor(flag);
        DataLoader.gui.dailyGoldMultiplier.gameObject.SetActive(!flag);
        DataLoader.gui.multiplierAnim.SetBool("IsOpened", true);
        DataLoader.gui.videoAnim.SetBool("IsOpened", false);
    }

    public void Start()
    {
        this.prefabFX = UnityEngine.Object.Instantiate<OverlayParticle>(this.prefabFX);
        this.prefabFX.transform.position = base.transform.position;
        TimeSpan span = TimeSpan.FromSeconds((double) StaticConstants.MultiplierDurationInSeconds);
        this.staticTimeText.text = $"+{span.Hours:D2}:{span.Minutes:D2}:{span.Seconds:D2}";
    }
}

