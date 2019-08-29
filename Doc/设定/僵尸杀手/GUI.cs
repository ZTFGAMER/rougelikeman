using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
    [SerializeField]
    private Text moneyText;
    [SerializeField]
    private Text textPoints;
    [SerializeField]
    private Text textBestScore;
    [SerializeField]
    private Text textNewSurvivorsLeft;
    public GameObject noInternetPanel;
    [SerializeField]
    private Animator mainMenuAnim;
    [SerializeField]
    private Image nextLocationImage;
    [SerializeField]
    private OnLevelUpFx onLevelUpFx;
    [Header("Video Reward Buster")]
    public Animator videoAnim;
    public Animator multiplierAnim;
    public Image multiplierPanelImage;
    public VideoMultiplier videoMultiplier;
    [Header("Pop-ups")]
    public PopUpsPanel popUpsPanel;
    [Header("Level"), SerializeField]
    private Text levelText;
    [SerializeField]
    private Image levelProgress;
    [Header("Survivors Upgrade Panel")]
    public UISurvivorUpgradePanel survivorUpgradePanel;
    [Header("Achievements ScrollView")]
    public UIAchievementsPanel achievementsPanel;
    [Header("Daily Reward")]
    public Image dailyPresent;
    public Animator dailyAnim;
    public GameObject newDaily;
    public Text dailyGoldMultiplier;
    public Text popupGoldMultiplier;
    [Header("Offline"), SerializeField]
    private GameObject offlinePanel;
    [SerializeField]
    private Text offlineRewardText;
    public Button offlineX2;
    public RectTransform objButtonOfflineOk;
    [Header("UIPanels"), SerializeField]
    private GameObject PlayUI;
    [SerializeField]
    private GameObject PauseUI;
    [SerializeField]
    private GameObject GameOverUI;
    [SerializeField]
    private GameObject JoystickUI;
    [SerializeField]
    private GameObject MainMenuUI;
    [SerializeField]
    private GameObject TutorialUI;
    [SerializeField]
    private GameObject AbilityTutorialUI;
    [SerializeField]
    private GameObject UpgradeTutorialUI;
    [SerializeField]
    private GameObject NextWorldReadyUI;
    [Header("Sound"), SerializeField]
    private GameObject musicON;
    [SerializeField]
    private GameObject musicOFF;
    [SerializeField]
    private GameObject soundON;
    [SerializeField]
    private GameObject soundOFF;
    [Header("MoneyBox"), SerializeField]
    private Image secretHatImage;
    public Animator secretAnimator;
    public GameObject newSecret;
    [Header("BossUI"), SerializeField]
    private Image fillProgressToBoss;
    [SerializeField]
    private Image fillBossHealth;
    [SerializeField]
    private Text bossNameText;
    [SerializeField]
    private Animator topPanelAnimator;
    [SerializeField]
    private OverlayParticle progressToBossFx;
    [SerializeField]
    private Text textButtonNewLocation;
    [SerializeField]
    private Text textCount1;
    [SerializeField]
    private Text textCount2;
    private float maxWidthProgressToBoss;
    private float maxWidthBossHealth;
    [Header("ArenaUI"), SerializeField]
    private Text textTimeRemaining;
    [SerializeField]
    private Image fillProgressMy;
    [SerializeField]
    private Image fillProgressOpponent;
    private float maxWidthArenaProgresses;
    [Header("WorldsUI"), SerializeField]
    private GameObject buttonNextWorld;
    [SerializeField]
    private GameObject buttonPrevWorld;
    [SerializeField]
    private Text textWorldName;
    [Header("LoadingUI"), SerializeField]
    private Loading loadingScreen;
    [NonSerialized]
    public bool isnewHeroOpened;
    [Space]
    public MultiplyImages multiplyImages;
    [SerializeField]
    private Animation psSurv;
    [SerializeField]
    private Animation psKillAll;
    [SerializeField]
    private Text delayResumeText;
    public GameOverContent gameOverContent;
    [HideInInspector]
    public bool pauseReady;
    private bool MoneyFxCoroutineInProgress;
    [Space]
    public RawImage rawImage;
    public GameObject devSettings;
    private int cachedPlayerLevel;
    [HideInInspector]
    public bool isNewWorldOpened;
    public UIWantedList wantedList;
    private Coroutine showWorldName;
    private List<Survivors> heroesInQueue = new List<Survivors>();
    private Coroutine showLastHeroCor;

    public void AbilityTutorialComplete()
    {
        this.AbilityTutorialUI.SetActive(false);
        PlayerPrefs.SetInt(StaticConstants.AbilityTutorialCompleted, 1);
        PlayerPrefs.Save();
        this.popUpsPanel.starterPack.Show(false);
        WavesManager.instance.StartGame();
        this.psSurv.gameObject.SetActive(false);
        this.psKillAll.gameObject.SetActive(false);
    }

    public void ChangeAnimationState(string trigger)
    {
        this.mainMenuAnim.SetTrigger(trigger);
        Dictionary<string, string> eventParameters = new Dictionary<string, string> {
            { 
                "Trigger",
                trigger
            }
        };
        AnalyticsManager.instance.LogEvent("Changed_MainMenu_Trigger", eventParameters);
    }

    private void CheckForNewWorld()
    {
        if (GameManager.instance.IsWorldOpen(GameManager.instance.currentWorldNumber + 1) && (!PlayerPrefs.HasKey(StaticConstants.GoToNextWorldPopUpShowed) || (PlayerPrefs.GetInt(StaticConstants.GoToNextWorldPopUpShowed) <= GameManager.instance.currentWorldNumber)))
        {
            this.NextWorldReadyUI.SetActive(true);
            AnalyticsManager.instance.LogEvent("NewWorldOpened", new Dictionary<string, string>());
            this.isNewWorldOpened = true;
        }
        else
        {
            this.isNewWorldOpened = false;
        }
        this.RefreshWorldsButtons();
    }

    public void DailyBossComplete()
    {
        SoundManager.Instance.PlayStepsSound(false);
        this.JoystickUI.SetActive(false);
        this.PlayUI.SetActive(false);
        this.ChangeAnimationState("GameOver");
        this.pauseReady = false;
    }

    [DebuggerHidden]
    private IEnumerator DelayedShowOpenedHero() => 
        new <DelayedShowOpenedHero>c__Iterator2 { $this = this };

    private void FillAllContent()
    {
        this.survivorUpgradePanel.CreateCells();
        this.achievementsPanel.CreateCells();
        this.wantedList.CreateCells();
    }

    public GameObject[] FillContent(RectTransform cellPrefab, ScrollRect scrollRect, int cellCount, float startBorder, float spaceBetweenCells, bool horizontal)
    {
        scrollRect.horizontal = horizontal;
        scrollRect.vertical = !horizontal;
        GameObject[] objArray = new GameObject[cellCount];
        if (horizontal)
        {
            float x = cellPrefab.rect.x + startBorder;
            for (int j = 0; j < cellCount; j++)
            {
                RectTransform transform = UnityEngine.Object.Instantiate<RectTransform>(cellPrefab, scrollRect.get_content());
                transform.anchoredPosition = new Vector2(x, transform.anchoredPosition.y);
                x += (transform.rect.width * transform.localScale.x) + spaceBetweenCells;
                objArray[j] = transform.gameObject;
            }
            scrollRect.get_content().sizeDelta = new Vector2(((cellPrefab.sizeDelta.x + spaceBetweenCells) * cellCount) - spaceBetweenCells, scrollRect.get_content().sizeDelta.y);
            return objArray;
        }
        float y = cellPrefab.rect.y - startBorder;
        for (int i = 0; i < cellCount; i++)
        {
            RectTransform transform2 = UnityEngine.Object.Instantiate<RectTransform>(cellPrefab, scrollRect.get_content());
            transform2.anchoredPosition = new Vector2(transform2.anchoredPosition.x, y);
            y -= (transform2.rect.height * transform2.localScale.y) + spaceBetweenCells;
            objArray[i] = transform2.gameObject;
        }
        scrollRect.get_content().sizeDelta = new Vector2(scrollRect.get_content().sizeDelta.x, ((cellPrefab.sizeDelta.y + spaceBetweenCells) * cellCount) - spaceBetweenCells);
        return objArray;
    }

    public void GameOver()
    {
        SoundManager.Instance.PlayStepsSound(false);
        this.JoystickUI.SetActive(false);
        this.PlayUI.SetActive(false);
        this.UpdateMoney();
        this.survivorUpgradePanel.ActivateHeroCams(true);
        if (GameManager.instance.isTutorialNow)
        {
            this.gameOverContent.SetContent(200.0, 1f, 1, 5.0, 1L, GameManager.instance.inGameTime, "Tutorial Completed!");
            this.ChangeAnimationState("TutorialCompleted");
        }
        this.wantedList.UpdateAll();
        this.pauseReady = false;
    }

    public void GetOfflineMoney()
    {
        double num = DataLoader.dataUpdateManager.LoadOfflineTime();
        float num2 = 0f;
        this.popUpsPanel.starterPack.starterMenu.SetActive(GameManager.instance.IsTutorialCompleted());
        num2 = 0f;
        for (int i = 0; i < DataLoader.playerData.heroData.Count; i++)
        {
            SaveData.HeroData data = DataLoader.playerData.heroData[i];
            num2 += DataLoader.Instance.GetHeroPower(data.heroType);
        }
        if (num > 30.0)
        {
            UnityEngine.Debug.Log(string.Concat(new object[] { "Offline gold: ", (float) ((num * num2) * StaticConstants.OfflineGoldConst), "\nSeconds: ", num }));
        }
        if (GameManager.instance.currentGameMode == GameManager.GameModes.Idle)
        {
            if ((num / 60.0) > StaticConstants.MinOfflineMinutes)
            {
                <GetOfflineMoney>c__AnonStorey8 storey = new <GetOfflineMoney>c__AnonStorey8 {
                    $this = this
                };
                if ((num / 60.0) > StaticConstants.MaxOfflineMinutes)
                {
                    UnityEngine.Debug.Log("Minutes > " + StaticConstants.MaxOfflineMinutes);
                    num = StaticConstants.MaxOfflineMinutes * 60f;
                }
                this.popUpsPanel.DisablePopupsWithoutBg();
                this.objButtonOfflineOk.anchoredPosition = new Vector2(-200f, this.objButtonOfflineOk.anchoredPosition.y);
                this.offlineX2.image.get_rectTransform().anchoredPosition = new Vector2(200f, this.offlineX2.image.get_rectTransform().anchoredPosition.y);
                this.popUpsPanel.gameObject.SetActive(true);
                this.offlineX2.gameObject.SetActive(true);
                this.offlinePanel.SetActive(true);
                storey.f = Mathf.Floor((float) ((num * num2) * StaticConstants.OfflineGoldConst));
                this.offlineRewardText.text = storey.f.ToString();
                DataLoader.Instance.RefreshMoney((double) storey.f, true);
                this.offlineX2.interactable = true;
                this.offlineX2.onClick.RemoveAllListeners();
                this.offlineX2.onClick.AddListener(new UnityAction(storey.<>m__0));
                DataLoader.dataUpdateManager.SaveOfflineTime();
            }
            else if (num > 900.0)
            {
                UnityEngine.Debug.Log("Sorry, offline time < " + StaticConstants.MinOfflineMinutes + " min, money won't be scored. You can change minimal time in StaticConstants.cs -> MinOfflineMinutes");
                if (!this.popUpsPanel.starterPack.autoShowCompleted && GameManager.instance.IsTutorialCompleted())
                {
                    this.popUpsPanel.ShowStarter();
                }
            }
        }
    }

    public void Go()
    {
        this.PlayUI.SetActive(true);
        this.JoystickUI.SetActive(true);
        GameManager.instance.Go();
        this.survivorUpgradePanel.ActivateHeroCams(false);
        if (!GameManager.instance.isTutorialNow && !PlayerPrefs.HasKey(StaticConstants.AbilityTutorialCompleted))
        {
            this.psSurv.gameObject.SetActive(true);
            this.psKillAll.gameObject.SetActive(true);
            this.AbilityTutorialUI.SetActive(true);
        }
        this.pauseReady = true;
        this.textBestScore.text = DataLoader.playerData.bestScore.ToString();
        this.RefreshBossUI();
    }

    public void GoArena()
    {
        this.PlayUI.SetActive(true);
        this.JoystickUI.SetActive(true);
        GameManager.instance.GoArena();
        this.survivorUpgradePanel.ActivateHeroCams(false);
    }

    public void GoToDailyBoss()
    {
        this.PlayUI.SetActive(true);
        this.JoystickUI.SetActive(true);
        this.ChangeAnimationState("Game");
        GameManager.instance.GoDailyBoss();
        this.pauseReady = true;
        this.textBestScore.text = DataLoader.playerData.bestScore.ToString();
        this.RefreshBossUI();
    }

    public void GoToNextWorld()
    {
        int num = GameManager.instance.ChangeWorld(1);
        if (num > 0)
        {
            this.popUpsPanel.gameObject.SetActive(true);
            this.popUpsPanel.closedWorldPanel.SetActive(true);
            foreach (Text text in this.popUpsPanel.bossKillsRemainingTexts)
            {
                text.text = num.ToString();
            }
        }
        else
        {
            this.RefreshWorldsButtons();
            if (this.NextWorldReadyUI.activeSelf)
            {
                this.NextWorldReadyUI.SetActive(false);
                PlayerPrefs.SetInt(StaticConstants.GoToNextWorldPopUpShowed, GameManager.instance.currentWorldNumber);
                PlayerPrefs.Save();
            }
        }
    }

    public void GoToPrevWorld()
    {
        GameManager.instance.ChangeWorld(-1);
        this.RefreshWorldsButtons();
    }

    public void Loading(bool state)
    {
        if (state)
        {
            this.loadingScreen.StartLoading();
        }
        else
        {
            this.loadingScreen.EndLoading();
        }
    }

    public void LogClickEvent(string eventName)
    {
        AnalyticsManager.instance.LogEvent(eventName, new Dictionary<string, string>());
    }

    public void MainMenu()
    {
        GameManager.instance.Reset();
        this.PauseUI.SetActive(false);
        DataLoader.dataUpdateManager.UpdateAfterConnect();
        DataLoader.Instance.CheckClosedWalls();
        DataLoader.gui.gameOverContent.gameOverOpened = false;
        this.pauseReady = false;
        SoundManager.Instance.soundVolume = 1f;
        this.CheckForNewWorld();
        this.UpdateMoney();
    }

    public void NoInternetPanel(bool isConnected)
    {
        this.popUpsPanel.gameObject.SetActive(!isConnected);
        this.noInternetPanel.SetActive(!isConnected);
        if (isConnected)
        {
            UnityEngine.Object.FindObjectOfType<TimeManager>().UpdateTime();
            DataLoader.dataUpdateManager.UpdateAfterConnect();
            Time.timeScale = 1f;
        }
        else
        {
            for (int i = 0; i < this.popUpsPanel.transform.childCount; i++)
            {
                if (this.popUpsPanel.transform.GetChild(i) != this.noInternetPanel.transform)
                {
                    this.popUpsPanel.transform.GetChild(i).gameObject.SetActive(false);
                }
            }
            Time.timeScale = 0f;
        }
    }

    [DebuggerHidden]
    private IEnumerator OfflineCounter(float currentMoney, float targetMoney) => 
        new <OfflineCounter>c__Iterator4 { 
            currentMoney = currentMoney,
            targetMoney = targetMoney,
            $this = this
        };

    public void OnApplicationPause(bool pause)
    {
        if ((pause && this.pauseReady) && (!GameManager.instance.isTutorialNow && PlayerPrefs.HasKey(StaticConstants.AbilityTutorialCompleted)))
        {
            this.JoystickUI.SetActive(false);
            this.PauseUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void OpenDaily(bool opened)
    {
        this.newDaily.SetActive(opened);
    }

    public void PopUpsClosed()
    {
        if (!PlayerPrefs.HasKey(StaticConstants.UpgradeTutorialCompleted))
        {
            MoneyBoxManager.instance.TrySpawnBox();
            this.UpgradeTutorialUI.SetActive(true);
            this.survivorUpgradePanel.scrollRect.enabled = false;
        }
    }

    public void RefreshArenaProgresses()
    {
        this.textTimeRemaining.text = GameManager.instance.timeLeft.ToString();
        this.fillProgressMy.get_rectTransform().sizeDelta = new Vector2((GameManager.instance.Points / ((float) GameManager.instance.currentArenaMaxPoints)) * this.maxWidthProgressToBoss, this.fillProgressMy.get_rectTransform().sizeDelta.y);
        this.fillProgressOpponent.get_rectTransform().sizeDelta = new Vector2((GameManager.instance.OpponentPoints / ((float) GameManager.instance.currentArenaMaxPoints)) * this.maxWidthProgressToBoss, this.fillProgressOpponent.get_rectTransform().sizeDelta.y);
    }

    public void RefreshBossHealth(int maxCountHealth, int countHealth, string name = "")
    {
        if (countHealth <= 0)
        {
            if (WavesManager.instance.GetNextBossTargetPoints() > 0)
            {
                this.fillProgressToBoss.get_rectTransform().sizeDelta = new Vector2(0f, this.fillProgressToBoss.get_rectTransform().sizeDelta.y);
            }
            else
            {
                this.fillProgressToBoss.transform.parent.gameObject.SetActive(false);
            }
        }
        if (WavesManager.instance.bossInDaHause)
        {
            this.fillBossHealth.get_rectTransform().sizeDelta = new Vector2((((float) countHealth) / ((float) maxCountHealth)) * this.maxWidthBossHealth, this.fillBossHealth.get_rectTransform().sizeDelta.y);
            this.bossNameText.text = LanguageManager.instance.GetLocalizedText(name);
            this.bossNameText.set_font(LanguageManager.instance.currentLanguage.font);
        }
        else
        {
            this.fillBossHealth.get_rectTransform().sizeDelta = new Vector2(0f, this.fillBossHealth.get_rectTransform().sizeDelta.y);
        }
    }

    public void RefreshBossUI()
    {
        this.fillProgressToBoss.transform.parent.gameObject.SetActive(true);
        this.fillBossHealth.transform.parent.gameObject.SetActive(false);
        this.RefreshProgressToBoss();
    }

    public void RefreshProgressToBoss()
    {
        if (!WavesManager.instance.bossInDaHause)
        {
            this.fillProgressToBoss.get_rectTransform().sizeDelta = new Vector2(((GameManager.instance.Points - WavesManager.instance.GetBossDeadAtPoints()) / ((float) WavesManager.instance.GetNextBossTargetPoints())) * this.maxWidthProgressToBoss, this.fillProgressToBoss.get_rectTransform().sizeDelta.y);
        }
        else
        {
            this.fillProgressToBoss.get_rectTransform().sizeDelta = new Vector2(this.maxWidthProgressToBoss, this.fillProgressToBoss.get_rectTransform().sizeDelta.y);
        }
    }

    public void RefreshWorldsButtons()
    {
        if (GameManager.instance.currentWorldNumber > 1)
        {
            this.buttonPrevWorld.SetActive(true);
        }
        else
        {
            this.buttonPrevWorld.SetActive(false);
        }
        if (GameManager.instance.currentWorldNumber < GameManager.instance.GetWorldsCount())
        {
            this.buttonNextWorld.SetActive(true);
        }
        else
        {
            this.buttonNextWorld.SetActive(false);
        }
        if ((GameManager.instance.bossKillsForOpenWorld[1] - DataLoader.playerData.GetZombieByType(SaveData.ZombieData.ZombieType.BOSS).totalTimesKilled) > 0)
        {
            this.textButtonNewLocation.text = (GameManager.instance.bossKillsForOpenWorld[1] - DataLoader.playerData.GetZombieByType(SaveData.ZombieData.ZombieType.BOSS).totalTimesKilled) + string.Empty;
            this.textCount1.text = this.textButtonNewLocation.text;
            this.textCount2.text = this.textButtonNewLocation.text;
            this.textButtonNewLocation.gameObject.SetActive(true);
            this.nextLocationImage.set_sprite(this.multiplyImages.nextLocationButton.inactive);
        }
        else
        {
            this.textButtonNewLocation.gameObject.SetActive(false);
            this.nextLocationImage.set_sprite(this.multiplyImages.nextLocationButton.active);
        }
        if (this.showWorldName != null)
        {
            base.StopCoroutine(this.showWorldName);
        }
        this.showWorldName = base.StartCoroutine(this.ShowWorldName());
    }

    public void Resume()
    {
        this.PauseUI.SetActive(false);
        base.StartCoroutine(this.resumeDelay());
    }

    [DebuggerHidden]
    private IEnumerator resumeDelay() => 
        new <resumeDelay>c__Iterator3 { $this = this };

    public void SetMusic(bool state)
    {
        this.musicON.SetActive(!state);
        this.musicOFF.SetActive(state);
    }

    public void SetSound(bool state)
    {
        this.soundON.SetActive(!state);
        this.soundOFF.SetActive(state);
    }

    public void SetTopPanelAnimationState(bool bossInDaHause)
    {
        if (bossInDaHause)
        {
            this.progressToBossFx.Play();
            this.fillBossHealth.transform.parent.gameObject.SetActive(true);
        }
        this.topPanelAnimator.SetBool("BossInDaHause", bossInDaHause);
    }

    public void ShowLastOpenedHero(HeroOpenType _type)
    {
        <ShowLastOpenedHero>c__AnonStorey6 storey = new <ShowLastOpenedHero>c__AnonStorey6 {
            _type = _type
        };
        this.UpdateMenuContent();
        if (DataLoader.Instance.UpdateHeroesIsOpened())
        {
            Survivors survivors;
            if (storey._type == HeroOpenType.Level)
            {
                survivors = Enumerable.LastOrDefault<Survivors>(DataLoader.Instance.survivors, new Func<Survivors, bool>(storey.<>m__0));
            }
            else
            {
                <ShowLastOpenedHero>c__AnonStorey7 storey2 = new <ShowLastOpenedHero>c__AnonStorey7 {
                    <>f__ref$6 = storey
                };
                if (DataLoader.playerData.GetTimeInGameCount(out storey2.t))
                {
                    survivors = Enumerable.Last<Survivors>(DataLoader.Instance.survivors, new Func<Survivors, bool>(storey2.<>m__0));
                }
                else
                {
                    return;
                }
            }
            this.heroesInQueue.Add(survivors);
            if (this.showLastHeroCor != null)
            {
                base.StopCoroutine(this.showLastHeroCor);
            }
            this.showLastHeroCor = base.StartCoroutine(this.DelayedShowOpenedHero());
            this.survivorUpgradePanel.scrollRect.get_content().anchoredPosition = new Vector2(0f, this.survivorUpgradePanel.scrollRect.get_content().anchoredPosition.y);
        }
    }

    [DebuggerHidden]
    private IEnumerator ShowWorldName() => 
        new <ShowWorldName>c__Iterator5 { $this = this };

    private void Start()
    {
        DataLoader.SetGui(this);
        this.FillAllContent();
        this.UpdateMenuContent();
        UnityEngine.Object.FindObjectOfType<DailyRewardManager>().ActivateDailyReward(true);
        if (GameManager.instance.isTutorialNow)
        {
            this.PlayUI.SetActive(true);
            this.JoystickUI.SetActive(true);
            this.StartTutorial();
        }
        else if (!PlayerPrefs.HasKey(StaticConstants.UpgradeTutorialCompleted))
        {
            this.UpgradeTutorialUI.SetActive(true);
            this.survivorUpgradePanel.scrollRect.enabled = false;
        }
        this.CheckForNewWorld();
        base.GetComponent<Canvas>().enabled = true;
        SoundManager.Instance.GetSavedInfo();
        DataLoader.dataUpdateManager.StartUpdate();
        DataLoader.Instance.CheckClosedWalls();
        if (this.secretAnimator.gameObject.activeInHierarchy)
        {
            this.secretAnimator.SetBool("IsOpened", true);
        }
        GameManager.instance.RefreshCurrentWorldNumber();
        this.RefreshWorldsButtons();
        this.maxWidthProgressToBoss = this.fillProgressToBoss.get_rectTransform().sizeDelta.x;
        this.maxWidthBossHealth = this.fillBossHealth.get_rectTransform().sizeDelta.x;
        this.maxWidthArenaProgresses = this.fillProgressMy.get_rectTransform().sizeDelta.x;
        this.ShowLastOpenedHero(HeroOpenType.Level);
    }

    private void StartTutorial()
    {
        this.TutorialUI.SetActive(true);
        this.ChangeAnimationState("Tutorial");
    }

    public void StartTutorialAfterReset()
    {
        this.PlayUI.SetActive(true);
        this.JoystickUI.SetActive(true);
        this.StartTutorial();
    }

    [DebuggerHidden]
    private IEnumerator StateTime(string trigger) => 
        new <StateTime>c__Iterator0 { 
            trigger = trigger,
            $this = this
        };

    public void TryToShowInterstirial()
    {
        if (((DataLoader.playerData.gamesPlayed > 3) && (this.heroesInQueue.Count == 0)) && !this.isNewWorldOpened)
        {
            this.isNewWorldOpened = false;
            AdsManager instance = AdsManager.instance;
            instance.interstitialAdsCounter++;
            if (AdsManager.instance.interstitialAdsCounter >= 1)
            {
                AdsManager.instance.ShowInterstitial();
                AdsManager.instance.interstitialAdsCounter = 0;
            }
        }
    }

    private void Update()
    {
        this.textPoints.text = Mathf.RoundToInt(GameManager.instance.Points).ToString();
        this.textNewSurvivorsLeft.text = GameManager.instance.newSurvivorsLeft.ToString();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if ((GameManager.instance.currentGameMode == GameManager.GameModes.GamePlay) || (GameManager.instance.currentGameMode == GameManager.GameModes.Arena))
            {
                DataLoader.gui.OnApplicationPause(true);
            }
            else if (!this.popUpsPanel.gameObject.activeInHierarchy && !this.gameOverContent.gameOverOpened)
            {
                this.popUpsPanel.gameObject.SetActive(true);
                this.popUpsPanel.exitPanel.SetActive(true);
            }
        }
    }

    public void UpdateExperience()
    {
        if (this.cachedPlayerLevel == 0)
        {
            this.cachedPlayerLevel = DataLoader.Instance.GetCurrentPlayerLevel();
        }
        int currentPlayerLevel = DataLoader.Instance.GetCurrentPlayerLevel();
        if (currentPlayerLevel > this.cachedPlayerLevel)
        {
            this.onLevelUpFx.Play();
            this.cachedPlayerLevel = currentPlayerLevel;
        }
        this.levelText.text = currentPlayerLevel.ToString();
        if (currentPlayerLevel == DataLoader.Instance.levelExperience.Length)
        {
            this.levelText.text = DataLoader.Instance.levelExperience.Length.ToString();
            this.levelText.resizeTextForBestFit = true;
            this.levelProgress.fillAmount = 1f;
        }
        else
        {
            this.levelProgress.fillAmount = (float) ((DataLoader.playerData.experience - DataLoader.Instance.levelExperience[currentPlayerLevel - 1]) / (DataLoader.Instance.levelExperience[currentPlayerLevel] - DataLoader.Instance.levelExperience[currentPlayerLevel - 1]));
        }
    }

    public void UpdateMenuContent()
    {
        this.survivorUpgradePanel.UpdateAllContent();
        this.achievementsPanel.UpdateAllContent();
        this.UpdateMoney();
        this.UpdateExperience();
    }

    public void UpdateMoney()
    {
        this.moneyText.text = Math.Floor(DataLoader.playerData.money).ToString();
        if (!this.MoneyFxCoroutineInProgress && (GameManager.instance.currentGameMode == GameManager.GameModes.GamePlay))
        {
            base.StartCoroutine(this.UpdateMoneyScaleFx());
        }
        this.survivorUpgradePanel.UpdateInactiveButton();
    }

    public void UpdateMoney(double money)
    {
        this.moneyText.text = Math.Round(money).ToString();
    }

    [DebuggerHidden]
    private IEnumerator UpdateMoneyScaleFx() => 
        new <UpdateMoneyScaleFx>c__Iterator1 { $this = this };

    public void UpgradeTutorialComplete()
    {
        if (!PlayerPrefs.HasKey(StaticConstants.UpgradeTutorialCompleted))
        {
            this.survivorUpgradePanel.scrollRect.enabled = true;
            this.UpgradeTutorialUI.SetActive(false);
            PlayerPrefs.SetInt(StaticConstants.UpgradeTutorialCompleted, 1);
            PlayerPrefs.Save();
        }
    }

    private void X3Off(float money)
    {
        DataLoader.Instance.RefreshMoney((double) (money * 2f), true);
        this.objButtonOfflineOk.anchoredPosition = new Vector2(0f, this.objButtonOfflineOk.anchoredPosition.y);
        this.offlineX2.gameObject.SetActive(false);
        base.StartCoroutine(this.OfflineCounter(money, money * 2f));
        Dictionary<string, string> eventParameters = new Dictionary<string, string> {
            { 
                "Money",
                money.ToString()
            }
        };
        AnalyticsManager.instance.LogEvent("OfflileX3", eventParameters);
    }

    [CompilerGenerated]
    private sealed class <DelayedShowOpenedHero>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal int <i>__1;
        internal GUI $this;
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
                    if (GameManager.instance.currentGameMode != GameManager.GameModes.Idle)
                    {
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                    }
                    else
                    {
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 2;
                        }
                    }
                    goto Label_01C9;

                case 2:
                    this.<i>__1 = 0;
                    while (this.<i>__1 < this.$this.heroesInQueue.Count)
                    {
                        this.$this.popUpsPanel.gameObject.SetActive(true);
                        this.$this.popUpsPanel.openHeroPanel.SetActive(true);
                        this.$this.survivorUpgradePanel.SetOpenedheroIcon(this.$this.heroesInQueue[this.<i>__1].heroType);
                        this.$this.popUpsPanel.heroName.text = LanguageManager.instance.GetLocalizedText(this.$this.heroesInQueue[this.<i>__1].name);
                        Dictionary<string, string> eventParameters = new Dictionary<string, string> {
                            { 
                                "Type",
                                this.$this.heroesInQueue[this.<i>__1].heroType.ToString()
                            }
                        };
                        AnalyticsManager.instance.LogEvent("NewHeroOpened", eventParameters);
                    Label_0157:
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 3;
                        }
                        goto Label_01C9;
                    Label_0172:
                        if (!this.$this.survivorUpgradePanel.animationCompleted)
                        {
                            goto Label_0157;
                        }
                        this.<i>__1++;
                    }
                    this.$this.heroesInQueue.Clear();
                    this.$PC = -1;
                    break;

                case 3:
                    goto Label_0172;
            }
            return false;
        Label_01C9:
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
    private sealed class <GetOfflineMoney>c__AnonStorey8
    {
        internal float f;
        internal GUI $this;

        internal void <>m__0()
        {
            AdsManager.instance.ShowRewarded(() => this.$this.X3Off(this.f));
        }

        internal void <>m__1()
        {
            this.$this.X3Off(this.f);
        }
    }

    [CompilerGenerated]
    private sealed class <OfflineCounter>c__Iterator4 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal float currentMoney;
        internal float <speed>__0;
        internal int <i>__1;
        internal float targetMoney;
        internal GUI $this;
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
                    this.$this.offlineRewardText.text = this.currentMoney.ToString();
                    this.<speed>__0 = 12f;
                    this.<i>__1 = 1;
                    break;

                case 1:
                    this.$this.offlineRewardText.text = (this.currentMoney + Mathf.CeilToInt((this.targetMoney / this.<speed>__0) * this.<i>__1)).ToString();
                    this.<i>__1++;
                    break;

                default:
                    goto Label_00E5;
            }
            if (this.<i>__1 <= this.<speed>__0)
            {
                this.$current = new WaitForSeconds(0.05f);
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                return true;
            }
            this.$PC = -1;
        Label_00E5:
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
    private sealed class <resumeDelay>c__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal Vector3 <defaultTextScale>__0;
        internal int <delayseconds>__0;
        internal float <i>__1;
        internal GUI $this;
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
                    this.$this.delayResumeText.transform.parent.gameObject.SetActive(true);
                    this.<defaultTextScale>__0 = this.$this.delayResumeText.transform.localScale;
                    this.<delayseconds>__0 = 3;
                    while (this.<delayseconds>__0 > 0)
                    {
                        this.$this.delayResumeText.text = this.<delayseconds>__0.ToString();
                        this.$this.delayResumeText.transform.localScale = this.<defaultTextScale>__0;
                        this.<i>__1 = this.<delayseconds>__0;
                        while (this.<i>__1 > (this.<delayseconds>__0 - 1))
                        {
                            this.$this.delayResumeText.transform.localScale = Vector3.Lerp(this.$this.delayResumeText.transform.localScale, Vector3.zero, 0.05f);
                            this.$current = new WaitForSecondsRealtime(0.02f);
                            if (!this.$disposing)
                            {
                                this.$PC = 1;
                            }
                            return true;
                        Label_0113:
                            this.<i>__1 -= 0.08f;
                        }
                        this.<delayseconds>__0--;
                    }
                    this.$this.JoystickUI.SetActive(true);
                    Time.timeScale = 1f;
                    this.$this.delayResumeText.transform.parent.gameObject.SetActive(false);
                    this.$this.delayResumeText.transform.localScale = this.<defaultTextScale>__0;
                    this.$PC = -1;
                    break;

                case 1:
                    goto Label_0113;
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
    private sealed class <ShowLastOpenedHero>c__AnonStorey6
    {
        internal HeroOpenType _type;

        internal bool <>m__0(Survivors s) => 
            ((s.requiredLevelToOpen <= DataLoader.Instance.GetCurrentPlayerLevel()) && (s.heroOpenType == this._type));
    }

    [CompilerGenerated]
    private sealed class <ShowLastOpenedHero>c__AnonStorey7
    {
        internal TimeSpan t;
        internal GUI.<ShowLastOpenedHero>c__AnonStorey6 <>f__ref$6;

        internal bool <>m__0(Survivors s) => 
            ((this.t.TotalMinutes > s.daysToOpen) && (s.heroOpenType == this.<>f__ref$6._type));
    }

    [CompilerGenerated]
    private sealed class <ShowWorldName>c__Iterator5 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal GUI $this;
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
                    this.$this.textWorldName.gameObject.SetActive(false);
                    this.$current = new WaitForSeconds(1f);
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    goto Label_024F;

                case 1:
                    this.$this.textWorldName.gameObject.SetActive(true);
                    this.$this.textWorldName.text = LanguageManager.instance.GetLocalizedText(GameManager.instance.worldNames[GameManager.instance.currentWorldNumber - 1]);
                    this.$this.textWorldName.set_font(LanguageManager.instance.currentLanguage.font);
                    this.$this.textWorldName.set_color(this.$this.textWorldName.get_color() - new Color(0f, 0f, 0f, this.$this.textWorldName.get_color().a));
                    break;

                case 2:
                    break;

                case 3:
                case 4:
                    while (this.$this.textWorldName.get_color().a > 0f)
                    {
                        this.$this.textWorldName.set_color(this.$this.textWorldName.get_color() - new Color(0f, 0f, 0f, Time.deltaTime));
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 4;
                        }
                        goto Label_024F;
                    }
                    this.$this.textWorldName.gameObject.SetActive(false);
                    this.$this.showWorldName = null;
                    this.$PC = -1;
                    goto Label_024D;

                default:
                    goto Label_024D;
            }
            if (this.$this.textWorldName.get_color().a < 1f)
            {
                this.$this.textWorldName.set_color(this.$this.textWorldName.get_color() + new Color(0f, 0f, 0f, Time.deltaTime));
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 2;
                }
            }
            else
            {
                this.$current = new WaitForSeconds(0.5f);
                if (!this.$disposing)
                {
                    this.$PC = 3;
                }
            }
            goto Label_024F;
        Label_024D:
            return false;
        Label_024F:
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
    private sealed class <StateTime>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal string trigger;
        internal GUI $this;
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
                    Time.timeScale = 1f;
                    this.$current = new WaitForSeconds(1f);
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;

                case 1:
                    this.$this.mainMenuAnim.SetTrigger(this.trigger);
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
    private sealed class <UpdateMoneyScaleFx>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal float <speed>__0;
        internal Vector3 <defaultScale>__0;
        internal float <i>__1;
        internal float <i>__2;
        internal GUI $this;
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
                    this.<speed>__0 = 8f;
                    this.$this.MoneyFxCoroutineInProgress = true;
                    this.<defaultScale>__0 = this.$this.moneyText.transform.localScale;
                    this.<i>__1 = 0f;
                    break;

                case 1:
                    this.<i>__1 += Time.deltaTime * this.<speed>__0;
                    break;

                case 2:
                    goto Label_0180;

                default:
                    goto Label_01D0;
            }
            if (this.<i>__1 < 1f)
            {
                this.$this.moneyText.transform.localScale = Vector3.Lerp(this.$this.moneyText.transform.localScale, this.$this.moneyText.transform.localScale + new Vector3(0.5f, 0.5f, 0.5f), Time.deltaTime * this.<speed>__0);
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                goto Label_01D2;
            }
            this.<i>__2 = 0f;
            while (this.<i>__2 < 1f)
            {
                this.$this.moneyText.transform.localScale = Vector3.Lerp(this.$this.moneyText.transform.localScale, this.<defaultScale>__0, Time.deltaTime * this.<speed>__0);
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 2;
                }
                goto Label_01D2;
            Label_0180:
                this.<i>__2 += Time.deltaTime;
            }
            this.$this.moneyText.transform.localScale = this.<defaultScale>__0;
            this.$this.MoneyFxCoroutineInProgress = false;
            this.$PC = -1;
        Label_01D0:
            return false;
        Label_01D2:
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
}

