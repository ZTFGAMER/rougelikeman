using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class SurviviorContent : MonoBehaviour
{
    [SerializeField]
    private HeroInfo heroInfo;
    [SerializeField]
    private Image heroIcon;
    [SerializeField]
    private Text heroNameText;
    [SerializeField]
    private Text costText;
    [SerializeField]
    private Text levelText;
    [SerializeField]
    private Button upgradeButton;
    public RawImage rawImage;
    [SerializeField]
    private OverlayParticle upgradeFX;
    [SerializeField]
    private Animator upgradeButtonAnimator;
    [SerializeField]
    private RectTransform imageLevel;
    [SerializeField]
    private Image imagePerkFillBar;
    [SerializeField]
    private Image imagePerk;
    [SerializeField]
    private Image imageUpgradeButton;
    [SerializeField]
    private Text newLevelPowerText;
    [SerializeField]
    private GameObject newLevelPower;
    public GameObject fakeVideoUpgrade;
    [SerializeField]
    private Image imageInfoShining;
    [SerializeField]
    private RectTransform powerPlashKa;
    [SerializeField]
    private RectTransform buttonInfoRect;
    [Header("Locked"), SerializeField]
    private Text levelRequiredText;
    [SerializeField]
    private GameObject levelRequired;
    [SerializeField]
    private GameObject lockObj;
    [SerializeField]
    private GameObject lockedName;
    [SerializeField]
    private Text lockedNameText;
    [SerializeField]
    private GameObject currentlevel;
    [SerializeField]
    private GameObject infoButton;
    [SerializeField]
    private GameObject objPurchaseHero;
    private List<Survivors.SurvivorLevels> levelsInfo;
    [NonSerialized]
    public SaveData.HeroData heroData;
    [NonSerialized]
    public int heroDataIndex;
    private SaveData.HeroData.HeroType type;
    [Space]
    private Vector2 startSize;
    public float speed = 200f;
    public float scale = 1.2f;
    public OverlayParticle unlockFX;
    private Camera heroCamera;
    private Animator anim;
    [HideInInspector]
    public bool canMakeVideoUpgrade;
    private Coroutine dayHeroCor;
    private int heroIndex;

    public void ActivateCamera(bool active)
    {
        active = false;
        this.heroCamera.enabled = active;
    }

    [DebuggerHidden]
    private IEnumerator DayHeroTimer(TimeSpan timeSpan) => 
        new <DayHeroTimer>c__Iterator1 { 
            timeSpan = timeSpan,
            $this = this
        };

    public void DelayedVideoUpgrade()
    {
        this.SetVideoButton(false);
        this.IncreaseLevel(true);
        if (this.heroInfo.gameObject.activeInHierarchy)
        {
            this.heroInfo.UpdateInfo();
        }
    }

    public void DisableCharacter(bool isOpened)
    {
        this.lockObj.SetActive(!isOpened);
        this.upgradeButton.gameObject.SetActive(isOpened);
        this.currentlevel.SetActive(isOpened);
        this.levelRequired.SetActive(!isOpened);
        this.lockedName.SetActive(!isOpened);
        this.objPurchaseHero.SetActive(!isOpened);
        this.newLevelPower.gameObject.SetActive(isOpened);
    }

    public float GetLevelPower(int level)
    {
        if (level < 1)
        {
            level = 1;
        }
        Survivors.SurvivorLevels levels = DataLoader.Instance.survivors[this.heroDataIndex].levels[level - 1];
        return (levels.power * 10f);
    }

    public void IncreaseLevel(bool rewarded)
    {
        if (!this.heroInfo.gameObject.activeInHierarchy)
        {
            this.upgradeFX.Play();
            base.StartCoroutine(this.LevelScaler());
        }
        SoundManager.Instance.PlaySound(SoundManager.Instance.upgradeSound, -1f);
        this.heroData.currentLevel++;
        for (int i = 0; i < DataLoader.playerData.heroData.Count; i++)
        {
            SaveData.HeroData data = DataLoader.playerData.heroData[i];
            if (data.heroType == DataLoader.Instance.survivors[this.heroDataIndex].heroType)
            {
                DataLoader.playerData.heroData[i] = this.heroData;
                break;
            }
        }
        this.UpdateContent();
        DataLoader.Instance.UpdateIdleHero(this.heroData.heroType);
        DataLoader.gui.UpdateMenuContent();
        DataLoader.Instance.SaveAllData();
        DataLoader.gui.UpgradeTutorialComplete();
        if ((this.heroInfo.surviviorContent == this) && this.heroInfo.gameObject.activeInHierarchy)
        {
            this.heroInfo.PlayFx(rewarded);
            this.heroInfo.UpdateInfo();
        }
    }

    [DebuggerHidden]
    public IEnumerator InfoShining() => 
        new <InfoShining>c__Iterator4 { $this = this };

    public bool IsEnoughMoney()
    {
        Survivors.SurvivorLevels levels = this.levelsInfo[this.heroData.currentLevel];
        return (levels.cost < DataLoader.playerData.money);
    }

    public bool IsVideoAvailable() => 
        this.canMakeVideoUpgrade;

    [DebuggerHidden]
    private IEnumerator LevelScaler() => 
        new <LevelScaler>c__Iterator2 { $this = this };

    public void OpenHeroInfo(bool isLocked)
    {
        if (PlayerPrefs.HasKey(StaticConstants.UpgradeTutorialCompleted))
        {
            this.heroInfo.SetContent(this.heroDataIndex, this.rawImage.get_texture(), isLocked, this);
            DataLoader.gui.popUpsPanel.gameObject.SetActive(true);
            this.heroInfo.gameObject.SetActive(true);
            this.UpdateInactiveButton();
        }
    }

    public void SetContent(int panelIndex)
    {
        this.heroIcon.set_sprite(DataLoader.Instance.survivors[panelIndex].icon);
        this.levelsInfo = DataLoader.Instance.survivors[panelIndex].levels;
        this.type = DataLoader.Instance.survivors[panelIndex].heroType;
        this.heroDataIndex = panelIndex;
        RenderTexture texture = new RenderTexture(DataLoader.gui.survivorUpgradePanel.renderTextureSurvivorPrefab);
        this.rawImage.set_texture(texture);
        float num = ((2f * DataLoader.gui.survivorUpgradePanel.heroCamPrefab.orthographicSize) * DataLoader.gui.survivorUpgradePanel.heroCamPrefab.aspect) + 1f;
        this.heroCamera = UnityEngine.Object.Instantiate<Camera>(DataLoader.gui.survivorUpgradePanel.heroCamPrefab, new Vector3(10000f + (panelIndex * num), 0f, 0f), Quaternion.identity, TransformParentManager.Instance.upgradePanelCams);
        SurvivorHuman human = UnityEngine.Object.Instantiate<SurvivorHuman>(DataLoader.Instance.survivors[panelIndex].survivorPrefab, this.heroCamera.transform.GetChild(0));
        SkinnedMeshRenderer componentInChildren = human.GetComponentInChildren<SkinnedMeshRenderer>();
        Renderer renderer2 = human.GetComponentInChildren<MeshRenderer>();
        componentInChildren.materials[0] = new Material(componentInChildren.materials[0]);
        renderer2.materials[0] = new Material(renderer2.materials[0]);
        componentInChildren.materials[0].SetFloat("_Outline", 0.04f);
        renderer2.materials[0].SetFloat("_Outline", 0.04f);
        foreach (Component component in human.gameObject.GetComponents<Component>())
        {
            if (!(component is Transform))
            {
                UnityEngine.Object.Destroy(component);
            }
        }
        this.heroData = Enumerable.FirstOrDefault<SaveData.HeroData>(DataLoader.playerData.heroData, hd => hd.heroType == DataLoader.Instance.survivors[this.heroDataIndex].heroType);
        human.transform.Rotate(new Vector3(0f, 180f, 0f));
        this.anim = human.GetComponentInChildren<Animator>();
        this.anim.SetBool("Rest", false);
        this.anim.SetBool("Run", false);
        this.heroCamera.targetTexture = texture;
        this.UpdateContent();
        this.startSize = this.imageLevel.sizeDelta;
        this.SetVideoButton(false);
        base.StartCoroutine(this.WaitForSurvivorStendUp());
    }

    public void SetLocalizedText()
    {
        this.heroNameText.text = LanguageManager.instance.GetLocalizedText(DataLoader.Instance.survivors[this.heroDataIndex].name);
        this.lockedNameText.text = this.heroNameText.text;
        if (DataLoader.Instance.survivors[this.heroDataIndex].heroOpenType == HeroOpenType.Level)
        {
            this.levelRequiredText.text = LanguageManager.instance.GetLocalizedText(LanguageKeysEnum.Level) + " " + DataLoader.Instance.survivors[this.heroDataIndex].requiredLevelToOpen;
        }
        this.levelRequiredText.set_font(LanguageManager.instance.currentLanguage.font);
    }

    public void SetPerk(int currentLevel)
    {
        this.imagePerk.gameObject.SetActive(currentLevel > 0x18);
        this.imagePerkFillBar.fillAmount = ((float) (currentLevel % 0x19)) / 25f;
        if (currentLevel >= 0x19)
        {
            this.imagePerk.set_sprite(DataLoader.gui.multiplyImages.perks[0].active);
        }
        if (currentLevel >= 50)
        {
            this.imagePerk.set_sprite(DataLoader.gui.multiplyImages.perks[1].active);
        }
        if (currentLevel >= 0x4b)
        {
            this.imagePerk.set_sprite(DataLoader.gui.multiplyImages.perks[2].active);
        }
        if (currentLevel >= 100)
        {
            this.imagePerk.set_sprite(DataLoader.gui.multiplyImages.perks[3].active);
        }
    }

    [DebuggerHidden]
    public IEnumerator SetPowerPlashkaSize(bool open) => 
        new <SetPowerPlashkaSize>c__Iterator3 { 
            open = open,
            $this = this
        };

    public bool SetVideoButton(bool active)
    {
        if ((this.heroData.isOpened && (this.heroData.currentLevel < DataLoader.playerData.survivorMaxLevel)) && active)
        {
            this.fakeVideoUpgrade.SetActive(true);
            this.upgradeButton.interactable = false;
            if (!this.canMakeVideoUpgrade)
            {
                base.StartCoroutine(this.SetPowerPlashkaSize(true));
                this.canMakeVideoUpgrade = true;
            }
            return true;
        }
        this.fakeVideoUpgrade.SetActive(false);
        this.canMakeVideoUpgrade = false;
        this.upgradeButton.interactable = true;
        base.StartCoroutine(this.SetPowerPlashkaSize(false));
        return false;
    }

    public void UpdateContent()
    {
        this.heroData = Enumerable.FirstOrDefault<SaveData.HeroData>(DataLoader.playerData.heroData, hd => hd.heroType == DataLoader.Instance.survivors[this.heroDataIndex].heroType);
        this.SetLocalizedText();
        this.upgradeButton.interactable = true;
        this.DisableCharacter(this.heroData.isOpened);
        if (this.heroData.currentLevel >= DataLoader.playerData.survivorMaxLevel)
        {
            this.SetVideoButton(false);
        }
        if (this.upgradeButtonAnimator.gameObject.activeInHierarchy)
        {
            this.upgradeButtonAnimator.SetBool("Active", false);
        }
        if (this.heroData.currentLevel == DataLoader.playerData.survivorMaxLevel)
        {
            this.upgradeButton.interactable = false;
            this.costText.text = LanguageManager.instance.GetLocalizedText(LanguageKeysEnum.Max_Maximum);
            this.costText.set_font(LanguageManager.instance.currentLanguage.font);
        }
        else
        {
            this.UpdateInactiveButton();
            Survivors.SurvivorLevels levels = this.levelsInfo[this.heroData.currentLevel];
            this.costText.text = levels.cost.ToString();
        }
        int currentLevel = this.heroData.currentLevel;
        this.SetPerk(currentLevel);
        if ((currentLevel < DataLoader.playerData.survivorMaxLevel) && this.heroData.isOpened)
        {
            object[] objArray1 = new object[] { "<color=#ffffff>", Math.Round((double) this.GetLevelPower(this.heroData.currentLevel)), "</color>+", Math.Round((double) (this.GetLevelPower(this.heroData.currentLevel + 1) - this.GetLevelPower(this.heroData.currentLevel)), 1) };
            this.newLevelPowerText.text = string.Concat(objArray1);
        }
        else
        {
            this.newLevelPowerText.text = "<color=#ffffff>" + Math.Round((double) this.GetLevelPower(this.heroData.currentLevel)) + "</color>";
        }
        this.levelText.text = currentLevel.ToString();
        if ((DataLoader.Instance.survivors[this.heroDataIndex].heroOpenType == HeroOpenType.Day) && !this.heroData.isOpened)
        {
            TimeSpan timeSpan = DataLoader.playerData.firstEnterDate.Add(TimeSpan.FromDays((double) DataLoader.Instance.survivors[this.heroDataIndex].daysToOpen)).Subtract(TimeManager.CurrentDateTime);
            if (this.dayHeroCor != null)
            {
                base.StopCoroutine(this.dayHeroCor);
            }
            this.dayHeroCor = base.StartCoroutine(this.DayHeroTimer(timeSpan));
        }
    }

    public void UpdateInactiveButton()
    {
        if (((this.heroData.currentLevel < DataLoader.playerData.survivorMaxLevel) && this.IsEnoughMoney()) && this.heroData.isOpened)
        {
            if (this.upgradeButtonAnimator.gameObject.activeInHierarchy)
            {
                this.upgradeButtonAnimator.SetBool("Active", true);
            }
            this.imageUpgradeButton.set_sprite(DataLoader.gui.multiplyImages.upgrageButtons[0].active);
            this.upgradeButton.interactable = true;
            if (this.heroInfo.surviviorContent == this)
            {
                this.heroInfo.imageUpgradeButton.set_sprite(DataLoader.gui.multiplyImages.upgrageButtons[3].active);
                this.heroInfo.SetUpgradeButtonInteractable(true);
            }
        }
        else
        {
            if (this.upgradeButtonAnimator.gameObject.activeInHierarchy)
            {
                this.upgradeButtonAnimator.SetBool("Active", false);
            }
            if (this.heroInfo.surviviorContent == this)
            {
                this.heroInfo.imageUpgradeButton.set_sprite(DataLoader.gui.multiplyImages.upgrageButtons[3].inactive);
                this.heroInfo.SetUpgradeButtonInteractable(false);
            }
            this.upgradeButton.interactable = false;
            this.imageUpgradeButton.set_sprite(DataLoader.gui.multiplyImages.upgrageButtons[0].inactive);
        }
    }

    public void Upgrade(bool rewarded)
    {
        if (this.heroData.isOpened || (this.heroData.currentLevel >= DataLoader.playerData.survivorMaxLevel))
        {
            if (rewarded)
            {
                AdsManager.instance.ShowRewarded(delegate {
                    base.Invoke("DelayedVideoUpgrade", 0.5f);
                    Dictionary<string, string> eventParameters = new Dictionary<string, string>();
                    SaveData.HeroData data = DataLoader.playerData.heroData[this.heroDataIndex];
                    eventParameters.Add("HeroType", data.heroType.ToString());
                    SaveData.HeroData data2 = DataLoader.playerData.heroData[this.heroDataIndex];
                    eventParameters.Add("Level", data2.currentLevel.ToString());
                    AnalyticsManager.instance.LogEvent("RewardedHeroUpgrade", eventParameters);
                });
            }
            else
            {
                Survivors.SurvivorLevels levels = this.levelsInfo[this.heroData.currentLevel];
                if (DataLoader.Instance.RefreshMoney((double) -levels.cost, false))
                {
                    this.IncreaseLevel(false);
                    Dictionary<string, string> eventParameters = new Dictionary<string, string>();
                    SaveData.HeroData data = DataLoader.playerData.heroData[this.heroDataIndex];
                    eventParameters.Add("HeroType", data.heroType.ToString());
                    SaveData.HeroData data2 = DataLoader.playerData.heroData[this.heroDataIndex];
                    eventParameters.Add("Level", data2.currentLevel.ToString());
                    AnalyticsManager.instance.LogEvent("HeroUpgrade", eventParameters);
                }
            }
        }
    }

    [DebuggerHidden]
    private IEnumerator WaitForSurvivorStendUp() => 
        new <WaitForSurvivorStendUp>c__Iterator0 { $this = this };

    [CompilerGenerated]
    private sealed class <DayHeroTimer>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal TimeSpan timeSpan;
        internal SurviviorContent $this;
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
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    goto Label_0121;

                case 1:
                case 2:
                    if (this.timeSpan.TotalSeconds > 0.0)
                    {
                        this.$this.levelRequiredText.text = $"{this.timeSpan.Days:D2}:{this.timeSpan.Hours:D2}:{this.timeSpan.Minutes:D2}:{this.timeSpan.Seconds:D2}";
                        this.timeSpan = this.timeSpan.Add(TimeSpan.FromSeconds(-1.0));
                        this.$current = new WaitForSecondsRealtime(1f);
                        if (!this.$disposing)
                        {
                            this.$PC = 2;
                        }
                        goto Label_0121;
                    }
                    DataLoader.gui.ShowLastOpenedHero(HeroOpenType.Day);
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_0121:
            return true;
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
    private sealed class <InfoShining>c__Iterator4 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal Color <color>__0;
        internal float <shiningspeed>__0;
        internal SurviviorContent $this;
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
                    this.<color>__0 = this.$this.imageInfoShining.get_color();
                    this.<shiningspeed>__0 = 2f;
                    goto Label_011D;

                case 1:
                    break;

                case 2:
                    goto Label_0108;

                default:
                    goto Label_015F;
            }
        Label_009F:
            if (this.<color>__0.a > 0f)
            {
                this.<color>__0.a -= Time.deltaTime * this.<shiningspeed>__0;
                this.$this.imageInfoShining.set_color(this.<color>__0);
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                goto Label_0161;
            }
        Label_0108:
            while (this.<color>__0.a < 1f)
            {
                this.<color>__0.a += Time.deltaTime * this.<shiningspeed>__0;
                this.$this.imageInfoShining.set_color(this.<color>__0);
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 2;
                }
                goto Label_0161;
            }
        Label_011D:
            if (this.$this.fakeVideoUpgrade.activeInHierarchy)
            {
                goto Label_009F;
            }
            this.<color>__0.a = 0f;
            this.$this.imageInfoShining.set_color(this.<color>__0);
            this.$PC = -1;
        Label_015F:
            return false;
        Label_0161:
            return true;
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
    private sealed class <LevelScaler>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal SurviviorContent $this;
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
                    if (this.$this.imageLevel.sizeDelta.x < (this.$this.startSize.x * this.$this.scale))
                    {
                        this.$this.imageLevel.sizeDelta += new Vector2(Time.deltaTime * this.$this.speed, Time.deltaTime * this.$this.speed);
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        goto Label_0173;
                    }
                    break;

                case 2:
                    break;

                default:
                    goto Label_0171;
            }
            while (this.$this.imageLevel.sizeDelta.x > this.$this.startSize.x)
            {
                this.$this.imageLevel.sizeDelta -= new Vector2(Time.deltaTime * this.$this.speed, Time.deltaTime * this.$this.speed);
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 2;
                }
                goto Label_0173;
            }
            this.$this.imageLevel.sizeDelta = this.$this.startSize;
            this.$PC = -1;
        Label_0171:
            return false;
        Label_0173:
            return true;
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
    private sealed class <SetPowerPlashkaSize>c__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal float <powerplashkaspeed>__0;
        internal bool open;
        internal SurviviorContent $this;
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
                    this.<powerplashkaspeed>__0 = 550f;
                    if (!this.open)
                    {
                        break;
                    }
                    this.$this.powerPlashKa.sizeDelta = new Vector2(this.$this.powerPlashKa.sizeDelta.x, 250f);
                    this.$this.buttonInfoRect.sizeDelta = new Vector2(this.$this.buttonInfoRect.sizeDelta.x, 267f);
                    goto Label_01A0;

                case 1:
                    break;

                case 2:
                    this.$PC = -1;
                    goto Label_01C2;

                default:
                    goto Label_01C2;
            }
            while (this.$this.powerPlashKa.sizeDelta.y > 165f)
            {
                this.$this.powerPlashKa.sizeDelta = Vector2.MoveTowards(this.$this.powerPlashKa.sizeDelta, new Vector2(this.$this.powerPlashKa.sizeDelta.x, 165f), this.<powerplashkaspeed>__0 * Time.deltaTime);
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                goto Label_01C4;
            }
            this.$this.powerPlashKa.sizeDelta = new Vector2(this.$this.powerPlashKa.sizeDelta.x, 165f);
            this.$this.buttonInfoRect.sizeDelta = new Vector2(this.$this.buttonInfoRect.sizeDelta.x, 338f);
        Label_01A0:
            this.$current = null;
            if (!this.$disposing)
            {
                this.$PC = 2;
            }
            goto Label_01C4;
        Label_01C2:
            return false;
        Label_01C4:
            return true;
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
    private sealed class <WaitForSurvivorStendUp>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal SurviviorContent $this;
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
                    if (this.$this.anim.GetCurrentAnimatorStateInfo(0).IsName("Rest"))
                    {
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;
                    }
                    this.$this.heroCamera.Render();
                    this.$this.ActivateCamera(false);
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
}

