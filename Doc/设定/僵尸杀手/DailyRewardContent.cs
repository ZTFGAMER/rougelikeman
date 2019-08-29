using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DailyRewardContent : MonoBehaviour
{
    [SerializeField]
    private Image dailyPresent;
    public RectTransform rect;
    public OverlayParticle claimFx;
    [Space, Header("Active"), SerializeField]
    private GameObject activeDay;
    [SerializeField]
    private Image activeMultiplier;
    [SerializeField]
    private Text activeCoinsText;
    [SerializeField]
    private Image activeBooster;
    [SerializeField]
    private Text activeBoosterText;
    [SerializeField]
    private Text activedayText;
    [SerializeField]
    private Button buttonClaim;
    [Header("Inactive"), SerializeField]
    private GameObject inactiveDay;
    [SerializeField]
    private Image inactiveMultiplier;
    [SerializeField]
    private Text inactiveCoinsText;
    [SerializeField]
    private Image inactiveBooster;
    [SerializeField]
    private Text inactiveBoosterText;
    [SerializeField]
    private Text inactivedayText;
    [Header("HalfActive"), SerializeField]
    private GameObject halfactiveDay;
    [SerializeField]
    private Image halfActiveMultiplier;
    [SerializeField]
    private Text halfactiveCoinsText;
    [SerializeField]
    private Image halfactiveBooster;
    [SerializeField]
    private Text halfactivedayText;
    [SerializeField]
    private Text halfactiveboosterCount;
    [Space]
    private DailyBonusData.RewardType rewardType;
    public Animator animActive;
    private int ID;

    public void ActivateLigth(bool onStreak)
    {
    }

    public void ActivateReward()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.claimSound, -1f);
        switch (this.rewardType)
        {
            case DailyBonusData.RewardType.Money:
                DataLoader.Instance.RefreshMoney((double) this.GetCoinsReward(), true);
                break;

            case DailyBonusData.RewardType.Multiplier:
                DataLoader.Instance.SetNewMultiplier(DataLoader.Instance.dailyBonus[this.ID].reward, StaticConstants.MultiplierDurationInSeconds * 2);
                break;

            case DailyBonusData.RewardType.Booster:
                DataLoader.Instance.BuyBoosters(DataLoader.Instance.dailyBonus[this.ID].boosterType, DataLoader.Instance.dailyBonus[this.ID].reward);
                break;
        }
        this.claimFx.Play();
        Dictionary<string, string> eventParameters = new Dictionary<string, string>();
        eventParameters.Add("TotalDays", (DataLoader.playerData.totalDaysInRow + 1).ToString());
        eventParameters.Add("Type", this.rewardType.ToString());
        AnalyticsManager.instance.LogEvent("DailyRewardClaim", eventParameters);
    }

    public float GetCoinsReward()
    {
        int currentPlayerLevel = DataLoader.Instance.GetCurrentPlayerLevel();
        return Mathf.Round((DataLoader.Instance.dailyBonus[this.ID].reward * Mathf.Pow(StaticConstants.DailyCoinMultiplier, (float) (currentPlayerLevel - 1))) + (StaticConstants.DailyCoinMultiplier2 * (currentPlayerLevel - 1)));
    }

    public void SetBooster(bool active)
    {
        this.activeBooster.gameObject.SetActive(active);
        this.inactiveBooster.gameObject.SetActive(active);
        this.halfactiveBooster.gameObject.SetActive(active);
        this.activeBoosterText.text = DataLoader.Instance.dailyBonus[this.ID].reward.ToString();
        this.halfactiveboosterCount.text = this.activeBoosterText.text;
    }

    public void SetBustersIcon(int index)
    {
        this.activeBooster.set_sprite(DataLoader.gui.multiplyImages.activeBoosters[index]);
        this.inactiveBooster.set_sprite(DataLoader.gui.multiplyImages.inactiveBoosters[index]);
        this.halfactiveBooster.set_sprite(this.activeBooster.get_sprite());
    }

    public void SetClaim(ClaimDel del)
    {
        <SetClaim>c__AnonStorey0 storey = new <SetClaim>c__AnonStorey0 {
            del = del
        };
        this.buttonClaim.onClick.AddListener(new UnityAction(storey.<>m__0));
    }

    public void SetContent(int ID)
    {
        this.ID = ID;
        this.rewardType = DataLoader.Instance.dailyBonus[ID].type;
        switch (this.rewardType)
        {
            case DailyBonusData.RewardType.Money:
                this.SetMoney(true);
                this.SetMultiplier(false);
                this.SetBooster(false);
                this.dailyPresent.set_sprite(DataLoader.gui.multiplyImages.dailyPresent[0]);
                break;

            case DailyBonusData.RewardType.Multiplier:
                this.SetMoney(false);
                this.SetMultiplier(true);
                this.SetBooster(false);
                this.dailyPresent.set_sprite(DataLoader.gui.multiplyImages.dailyPresent[1]);
                break;

            case DailyBonusData.RewardType.Booster:
                if (DataLoader.Instance.dailyBonus[ID].boosterType == SaveData.BoostersData.BoosterType.NewSurvivor)
                {
                    this.SetBustersIcon(0);
                }
                else
                {
                    this.SetBustersIcon(1);
                }
                this.SetMoney(false);
                this.SetMultiplier(false);
                this.SetBooster(true);
                break;
        }
        this.dailyPresent.gameObject.SetActive(this.rewardType == DailyBonusData.RewardType.Money);
        if (LanguageManager.instance.IsReverseLanguage(LanguageManager.instance.currentLanguage.language))
        {
            this.inactivedayText.text = (1 + (ID % 7)) + LanguageManager.instance.GetLocalizedText(LanguageKeysEnum.Day);
        }
        else
        {
            this.inactivedayText.text = LanguageManager.instance.GetLocalizedText(LanguageKeysEnum.Day) + " " + (1 + (ID % 7));
        }
        this.halfactivedayText.text = this.inactivedayText.text;
        this.inactivedayText.set_font(LanguageManager.instance.currentLanguage.font);
        this.halfactivedayText.set_font(LanguageManager.instance.currentLanguage.font);
    }

    public void SetDay(DailyContentType type)
    {
        this.activeDay.SetActive(type == DailyContentType.Active);
        this.inactiveDay.SetActive(type == DailyContentType.Inactive);
        this.halfactiveDay.SetActive(type == DailyContentType.Next);
        if (type != DailyContentType.Inactive)
        {
            if (type == DailyContentType.Next)
            {
                this.SetContent(this.ID);
            }
        }
        else
        {
            this.dailyPresent.set_sprite(DataLoader.gui.multiplyImages.dailyPresent[3]);
        }
    }

    private void SetMoney(bool money)
    {
        this.activeCoinsText.gameObject.SetActive(money);
        this.inactiveCoinsText.gameObject.SetActive(money);
        this.halfactiveCoinsText.gameObject.SetActive(money);
        float coinsReward = this.GetCoinsReward();
        this.activeCoinsText.text = coinsReward.ToString();
        this.inactiveCoinsText.text = coinsReward.ToString();
        this.halfactiveCoinsText.text = this.activeCoinsText.text;
    }

    public void SetMultiplier(bool multiplier)
    {
        this.activeMultiplier.gameObject.SetActive(multiplier);
        this.inactiveMultiplier.gameObject.SetActive(multiplier);
        this.halfActiveMultiplier.gameObject.SetActive(multiplier);
        if (multiplier)
        {
            int multiplierSpriteID = MultiplyImages.GetMultiplierSpriteID(DataLoader.Instance.dailyBonus[this.ID].reward);
            this.activeMultiplier.set_sprite(DataLoader.gui.multiplyImages.activeMultiplier[multiplierSpriteID]);
            this.inactiveMultiplier.set_sprite(DataLoader.gui.multiplyImages.inactiveMultiplier[multiplierSpriteID]);
            this.halfActiveMultiplier.set_sprite(this.activeMultiplier.get_sprite());
        }
    }

    [CompilerGenerated]
    private sealed class <SetClaim>c__AnonStorey0
    {
        internal DailyRewardContent.ClaimDel del;

        internal void <>m__0()
        {
            this.del();
        }
    }

    public delegate void ClaimDel();
}

