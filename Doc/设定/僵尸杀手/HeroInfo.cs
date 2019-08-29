using IAP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class HeroInfo : MonoBehaviour
{
    [SerializeField]
    private Text DescriptionText;
    [SerializeField]
    private Text shortDescriptionText;
    [SerializeField]
    private RawImage rawImage;
    [SerializeField]
    private Text currentPowerText;
    [SerializeField]
    private Text currentPowerTextInactive;
    [SerializeField]
    private GameObject activePowerPlashka;
    [SerializeField]
    private GameObject inactivePowerPlashka;
    [SerializeField]
    private Text costText;
    [SerializeField]
    private Text cost2Text;
    [SerializeField]
    private Text newPower;
    [SerializeField]
    private GameObject lockedStripe;
    [SerializeField]
    private Button buttonUpgrade;
    [SerializeField]
    private Button buttonUpgrade2;
    [SerializeField]
    private Button buttonVideoUpgrade;
    [SerializeField]
    private Button buttonBuy;
    [SerializeField]
    private Text levelText;
    [SerializeField]
    private Text textPriceReal;
    [SerializeField]
    private Text heroname;
    [SerializeField]
    private Text lockedStripeText;
    [SerializeField]
    private Image popupImage;
    [SerializeField]
    private Image imageVideoUpgradeButton;
    [SerializeField]
    private Image imageVideo;
    [SerializeField]
    private OverlayParticle upgradeButtonFx;
    [SerializeField]
    private OverlayParticle videoUpgradeButtonFx;
    [SerializeField]
    private RectTransform descriptionRect;
    public Image imageUpgradeButton;
    [SerializeField, Space]
    private OverlayParticle upgradeFx;
    [SerializeField]
    private List<ScrollViewPerkContent> scrollContent;
    public Color activeTextColor;
    public Color inactiveTextColor;
    public ScrollRect scrollRect;
    private Survivors survivor;
    [NonSerialized]
    public SurviviorContent surviviorContent;
    private int productIndex;
    private int survivorIndex;

    public void Buy()
    {
        InAppManager.Instance.BuyProductID(this.productIndex);
    }

    [DebuggerHidden]
    private IEnumerator DayHeroTimer(TimeSpan timeSpan) => 
        new <DayHeroTimer>c__Iterator0 { 
            timeSpan = timeSpan,
            $this = this
        };

    [DebuggerHidden]
    public IEnumerator HideRewarded() => 
        new <HideRewarded>c__Iterator1 { $this = this };

    public void OnDisable()
    {
        base.StopAllCoroutines();
    }

    public void OnEnable()
    {
        this.UpdateInfo();
        if (this.surviviorContent.fakeVideoUpgrade.activeInHierarchy)
        {
            this.buttonUpgrade2.image.get_rectTransform().anchoredPosition = new Vector2(-200f, 0f);
            this.buttonVideoUpgrade.image.get_rectTransform().anchoredPosition = new Vector2(200f, 0f);
        }
        else
        {
            this.buttonUpgrade2.image.get_rectTransform().anchoredPosition = Vector2.zero;
            this.buttonVideoUpgrade.image.get_rectTransform().anchoredPosition = Vector2.zero;
        }
        if (DataLoader.Instance.survivors[this.survivorIndex].heroOpenType == HeroOpenType.Level)
        {
            this.lockedStripeText.text = LanguageManager.instance.GetLocalizedText(LanguageKeysEnum.Reach_x_Level_to_Unlock).Replace("x", DataLoader.Instance.survivors[this.survivorIndex].requiredLevelToOpen.ToString());
        }
        else if (DataLoader.Instance.survivors[this.survivorIndex].heroOpenType == HeroOpenType.Day)
        {
            TimeSpan timeSpan = DataLoader.playerData.firstEnterDate.Add(TimeSpan.FromDays((double) DataLoader.Instance.survivors[this.survivorIndex].daysToOpen)).Subtract(TimeManager.CurrentDateTime);
            base.StartCoroutine(this.DayHeroTimer(timeSpan));
        }
    }

    public void PlayFx(bool rewarded)
    {
        this.upgradeFx.Play();
        if (rewarded)
        {
            this.videoUpgradeButtonFx.Play();
        }
        else
        {
            this.upgradeButtonFx.Play();
        }
        this.SetVideoButton(!rewarded);
        this.UpdateInfo();
    }

    public void SetContent(int survivorIndex, Texture texture, bool isLocked, SurviviorContent content)
    {
        this.rawImage.set_texture(texture);
        this.surviviorContent = content;
        this.survivor = DataLoader.Instance.survivors[survivorIndex];
        this.DescriptionText.text = LanguageManager.instance.GetLocalizedText(this.survivor.heroStory);
        this.DescriptionText.set_font(LanguageManager.instance.currentLanguage.font);
        this.shortDescriptionText.text = LanguageManager.instance.GetLocalizedText(this.survivor.shortDescriptionKey);
        this.shortDescriptionText.set_font(LanguageManager.instance.currentLanguage.font);
        this.heroname.text = LanguageManager.instance.GetLocalizedText(this.survivor.fullname);
        this.SetIsLocked(isLocked, survivorIndex);
        this.survivorIndex = survivorIndex;
    }

    public void SetIsLocked(bool isLocked, int survivorIndex = -1)
    {
        if (isLocked)
        {
            foreach (HeroesPurchaseInfo info in InAppManager.Instance.heroesPurchases)
            {
                if (info.heroType == this.surviviorContent.heroData.heroType)
                {
                    this.productIndex = info.index;
                    this.textPriceReal.text = InAppManager.Instance.GetPriceHero(this.productIndex);
                }
            }
            this.lockedStripeText.set_font(LanguageManager.instance.currentLanguage.font);
        }
        this.descriptionRect.sizeDelta = !isLocked ? new Vector2(this.descriptionRect.sizeDelta.x, 340f) : new Vector2(this.descriptionRect.sizeDelta.x, 262f);
        this.DescriptionText.set_color(!isLocked ? Color.white : new Color(0f, 0.3215686f, 0.4745098f, 1f));
        this.buttonBuy.gameObject.SetActive(isLocked);
        this.buttonUpgrade2.gameObject.SetActive(!isLocked);
        this.buttonVideoUpgrade.gameObject.SetActive(!isLocked);
        this.buttonUpgrade.interactable = !isLocked;
        this.lockedStripe.SetActive(isLocked);
        this.activePowerPlashka.SetActive(!isLocked);
        this.inactivePowerPlashka.SetActive(isLocked);
        this.popupImage.set_sprite(!isLocked ? DataLoader.gui.multiplyImages.popup.active : DataLoader.gui.multiplyImages.popup.inactive);
    }

    public void SetUpgradeButtonInteractable(bool interactable)
    {
        this.buttonUpgrade2.interactable = interactable;
    }

    public void SetVideoButton(bool active)
    {
        this.buttonVideoUpgrade.interactable = active;
        if (active)
        {
            this.imageVideo.set_sprite(DataLoader.gui.multiplyImages.upgrageButtons[2].active);
            this.imageVideoUpgradeButton.set_sprite(DataLoader.gui.multiplyImages.upgrageButtons[1].active);
        }
        else
        {
            this.imageVideo.set_sprite(DataLoader.gui.multiplyImages.upgrageButtons[2].inactive);
            this.imageVideoUpgradeButton.set_sprite(DataLoader.gui.multiplyImages.upgrageButtons[1].inactive);
        }
    }

    public void UpdateInfo()
    {
        int currentLevel = this.surviviorContent.heroData.currentLevel;
        this.currentPowerText.text = Math.Round((double) this.surviviorContent.GetLevelPower(currentLevel)).ToString();
        this.currentPowerTextInactive.text = this.currentPowerText.text;
        if (currentLevel < DataLoader.playerData.survivorMaxLevel)
        {
            this.newPower.text = "+" + Math.Round((double) (this.surviviorContent.GetLevelPower(currentLevel + 1) - this.surviviorContent.GetLevelPower(currentLevel)), 1);
            Survivors.SurvivorLevels levels = this.survivor.levels[currentLevel];
            this.costText.text = levels.cost.ToString();
            this.cost2Text.text = this.costText.text;
            this.buttonUpgrade.interactable = true;
            this.buttonUpgrade2.interactable = true;
            this.costText.transform.parent.gameObject.SetActive(true);
            this.cost2Text.gameObject.SetActive(true);
        }
        else
        {
            this.newPower.text = LanguageManager.instance.GetLocalizedText(LanguageKeysEnum.Max_Maximum);
            this.newPower.set_font(LanguageManager.instance.currentLanguage.font);
            this.buttonUpgrade.interactable = false;
            this.buttonUpgrade2.interactable = false;
            this.costText.transform.parent.gameObject.SetActive(false);
            this.cost2Text.gameObject.SetActive(false);
        }
        this.SetVideoButton((currentLevel < DataLoader.playerData.survivorMaxLevel) && this.surviviorContent.IsVideoAvailable());
        this.levelText.text = currentLevel.ToString();
        if (this.surviviorContent.heroData.isOpened)
        {
            this.buttonUpgrade2.gameObject.SetActive(currentLevel != DataLoader.playerData.survivorMaxLevel);
            this.buttonVideoUpgrade.gameObject.SetActive(currentLevel != DataLoader.playerData.survivorMaxLevel);
        }
        else
        {
            this.buttonUpgrade2.gameObject.SetActive(false);
            this.buttonVideoUpgrade.gameObject.SetActive(false);
        }
        this.UpdatePerkContent(currentLevel);
    }

    public void UpdatePerkContent(int level)
    {
        for (int i = 0; i < this.scrollContent.Count; i++)
        {
            bool flag = level >= ((i + 1) * 0x19);
            this.scrollContent[i].inactive.SetActive(!flag);
            this.scrollContent[i].textLevel.set_color(!flag ? this.inactiveTextColor : this.activeTextColor);
            this.scrollContent[i].textPower.set_color(!flag ? this.inactiveTextColor : this.activeTextColor);
            this.scrollContent[i].imagePerk.set_sprite(!flag ? DataLoader.gui.multiplyImages.perks[i].inactive : DataLoader.gui.multiplyImages.perks[i].active);
        }
        if (level == DataLoader.playerData.survivorMaxLevel)
        {
            this.scrollRect.get_content().anchoredPosition = new Vector2(0f, 104f);
        }
    }

    public void Upgrade(bool rewarded)
    {
        string text = this.levelText.text;
        this.surviviorContent.Upgrade(rewarded);
    }

    [CompilerGenerated]
    private sealed class <DayHeroTimer>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal TimeSpan timeSpan;
        internal HeroInfo $this;
        internal object $current;
        internal bool $disposing;
        internal int $PC;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$disposing = true;
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                case 1:
                    if (this.timeSpan.TotalSeconds > 0.0)
                    {
                        this.$this.lockedStripeText.text = $"Opens in
{this.timeSpan.Days:D2}:{this.timeSpan.Hours:D2}:{this.timeSpan.Minutes:D2}:{this.timeSpan.Seconds:D2}";
                        this.timeSpan = this.timeSpan.Add(TimeSpan.FromSeconds(-1.0));
                        this.$current = new WaitForSeconds(1f);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;
                    }
                    this.$PC = -1;
                    break;
            }
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }

    [CompilerGenerated]
    private sealed class <HideRewarded>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal Vector2 <vector>__0;
        internal float <speed>__0;
        internal HeroInfo $this;
        internal object $current;
        internal bool $disposing;
        internal int $PC;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$disposing = true;
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    this.<vector>__0 = Vector2.zero;
                    this.<speed>__0 = 650f;
                    break;

                case 1:
                    break;

                default:
                    goto Label_015E;
            }
            if (this.$this.buttonVideoUpgrade.image.get_rectTransform().anchoredPosition != this.<vector>__0)
            {
                this.$this.buttonVideoUpgrade.image.get_rectTransform().anchoredPosition = Vector2.MoveTowards(this.$this.buttonVideoUpgrade.image.get_rectTransform().anchoredPosition, this.<vector>__0, Time.deltaTime * this.<speed>__0);
                this.$this.buttonUpgrade2.image.get_rectTransform().anchoredPosition = Vector2.MoveTowards(this.$this.buttonUpgrade2.image.get_rectTransform().anchoredPosition, this.<vector>__0, Time.deltaTime * this.<speed>__0);
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                return true;
            }
            this.$this.buttonUpgrade2.image.get_rectTransform().anchoredPosition = this.<vector>__0;
            this.$this.buttonVideoUpgrade.image.get_rectTransform().anchoredPosition = this.<vector>__0;
            this.$PC = -1;
        Label_015E:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }
}

