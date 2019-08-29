using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class GameOverContent : MonoBehaviour
{
    [SerializeField]
    private Text coinsCountText;
    [SerializeField]
    private Text expCountText;
    [SerializeField]
    private Text hatersCountText;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text timeText;
    [SerializeField]
    private Text popUpName;
    [SerializeField]
    private Button buttonX2;
    [SerializeField]
    private RectTransform buttonOk;
    [SerializeField]
    private RectTransform popUpRect;
    [SerializeField]
    private GameObject sectetPlashka;
    [SerializeField]
    private Text secretLevel;
    [SerializeField]
    private Image multiplierImage;
    public GameObject buttonX2obj;
    public GameObject multiplierImageObj;
    public UIPresentController presentController;
    private double completedCoins;
    private int currentMultipler;
    private int currentTime;
    [HideInInspector]
    public bool gameOverOpened;

    public void AdsReward()
    {
        base.StopAllCoroutines();
        this.buttonX2.interactable = false;
        this.coinsCountText.text = Math.Round(this.completedCoins).ToString();
        AdsManager.instance.ShowRewarded(() => this.GetX2());
    }

    public void AnimateCoins(double newCoins)
    {
        if (this.currentMultipler > 1)
        {
            this.multiplierImageObj.SetActive(true);
            base.StartCoroutine(this.AnimateScore(this.completedCoins, (newCoins * this.currentMultipler) - newCoins, 0.0, 0, 0f, true));
            this.currentMultipler = 1;
        }
    }

    [DebuggerHidden]
    private IEnumerator AnimateScore(double coins, double newCoins, double exp, int hatersCount, float startDelay, bool onlyCoins = false) => 
        new <AnimateScore>c__Iterator0 { 
            newCoins = newCoins,
            onlyCoins = onlyCoins,
            coins = coins,
            startDelay = startDelay,
            exp = exp,
            hatersCount = hatersCount,
            $this = this
        };

    public void ExitToMenu()
    {
        DataLoader.gui.MainMenu();
        DataLoader.gui.ChangeAnimationState("MainMenu");
        DataLoader.gui.TryToShowInterstirial();
    }

    [DebuggerHidden]
    public IEnumerator GetDelayedX2() => 
        new <GetDelayedX2>c__Iterator1 { $this = this };

    private void GetX2()
    {
        base.StartCoroutine(this.GetDelayedX2());
    }

    public void SetContent(double newcoins, float multiplier, int newHaters, double exp, long score, int time, string popupname = "Game Over")
    {
        Dictionary<string, string> dictionary;
        base.StopAllCoroutines();
        this.completedCoins = 0.0;
        this.currentMultipler = (int) multiplier;
        this.buttonX2obj.SetActive(true);
        this.currentTime = time;
        if ((this.currentTime < 10) && (AdsManager.instance.interstitialAdsCounter == 0))
        {
            AdsManager instance = AdsManager.instance;
            instance.interstitialAdsCounter--;
        }
        if (multiplier > 1f)
        {
            this.multiplierImage.set_sprite(DataLoader.gui.multiplyImages.activeMultiplier[MultiplyImages.GetMultiplierSpriteID((int) multiplier)]);
            this.multiplierImage.gameObject.SetActive(true);
        }
        else
        {
            this.multiplierImage.gameObject.SetActive(false);
        }
        this.multiplierImageObj.SetActive(false);
        base.StartCoroutine(this.AnimateScore(0.0, newcoins, exp, newHaters, 1.5f, false));
        if (popupname == "Tutorial Completed!")
        {
            this.popUpName.text = LanguageManager.instance.GetLocalizedText(LanguageManager.instance.GetLocalizedText(LanguageKeysEnum.Tutorial_Completed));
            this.popUpName.set_font(LanguageManager.instance.currentLanguage.font);
            this.popUpName.fontSize = 0x4b;
            this.buttonX2.gameObject.SetActive(false);
            this.buttonOk.anchoredPosition = Vector2.zero;
            dictionary = new Dictionary<string, string> {
                { 
                    "TimeInSeconds",
                    this.TimeInSeconds(time)
                }
            };
            AnalyticsManager.instance.LogEvent("TutorialComleted", dictionary);
        }
        else
        {
            this.popUpName.text = LanguageManager.instance.GetLocalizedText(LanguageManager.instance.GetLocalizedText(LanguageKeysEnum.Game_over));
            this.popUpName.set_font(LanguageManager.instance.currentLanguage.font);
            this.popUpName.fontSize = 100;
            this.buttonX2.gameObject.SetActive(true);
            this.buttonOk.anchoredPosition = new Vector2(-200f, 0f);
            dictionary = new Dictionary<string, string> {
                { 
                    "TimeInSeconds",
                    this.TimeInSeconds(time)
                },
                { 
                    "Coins",
                    newcoins.ToString()
                },
                { 
                    "Multiplier",
                    multiplier.ToString()
                },
                { 
                    "Experience",
                    exp.ToString()
                },
                { 
                    "NewHaters",
                    newHaters.ToString()
                },
                { 
                    "Score",
                    score.ToString()
                }
            };
            AnalyticsManager.instance.LogEvent("GameOver", dictionary);
        }
        if (PlayerPrefs.HasKey(StaticConstants.AbilityTutorialCompleted))
        {
            SoundManager.Instance.soundVolume = 0.15f;
            SoundManager.Instance.PlaySound(SoundManager.Instance.gameOverSound, 1f);
        }
        this.buttonX2.interactable = true;
        this.scoreText.text = LanguageManager.instance.GetLocalizedText(LanguageKeysEnum.Score) + ": " + score;
        this.scoreText.set_font(LanguageManager.instance.currentLanguage.font);
        this.timeText.text = $"{time / 60:D2}:{time - ((time / 60) * 60):D2}";
        this.popUpRect.sizeDelta = new Vector2(this.popUpRect.sizeDelta.x, 1100f);
        this.gameOverOpened = true;
    }

    public string TimeInSeconds(int timeInSeconds)
    {
        string str = (timeInSeconds - (timeInSeconds % 10)) + "-" + ((timeInSeconds - (timeInSeconds % 10)) + 10);
        UnityEngine.Debug.Log("PlayTime: " + str);
        return str;
    }

    private void Update()
    {
        if ((!Input.GetKeyDown(KeyCode.Escape) || !this.gameOverOpened) || (this.popUpRect.anchoredPosition == Vector2.zero))
        {
        }
    }

    [CompilerGenerated]
    private sealed class <AnimateScore>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal double newCoins;
        internal bool onlyCoins;
        internal double coins;
        internal float startDelay;
        internal float <speed>__0;
        internal int <haters>__0;
        internal double <xp>__0;
        internal int <i>__1;
        internal double exp;
        internal int hatersCount;
        internal GameOverContent $this;
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
                    this.$this.completedCoins += this.newCoins;
                    if (!this.onlyCoins)
                    {
                        this.$this.expCountText.text = "0";
                        this.$this.hatersCountText.text = "0";
                    }
                    this.coins = Math.Round(this.coins);
                    this.$this.coinsCountText.text = this.coins.ToString();
                    this.$current = new WaitForSecondsRealtime(this.startDelay);
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    goto Label_0263;

                case 1:
                    this.<speed>__0 = 15f;
                    this.<haters>__0 = 0;
                    this.<xp>__0 = 0.0;
                    this.<i>__1 = 1;
                    break;

                case 2:
                    this.$this.coinsCountText.text = Math.Floor((double) (this.coins + ((this.newCoins / ((double) this.<speed>__0)) * this.<i>__1))).ToString();
                    if (!this.onlyCoins)
                    {
                        this.<xp>__0 = (this.exp / ((double) this.<speed>__0)) * this.<i>__1;
                        this.$this.expCountText.text = Math.Round(this.<xp>__0).ToString();
                        if (this.<haters>__0 <= this.hatersCount)
                        {
                            this.$this.hatersCountText.text = this.<haters>__0.ToString();
                            this.<haters>__0++;
                        }
                    }
                    this.<i>__1++;
                    break;

                default:
                    goto Label_0261;
            }
            if (this.<i>__1 <= this.<speed>__0)
            {
                this.$current = new WaitForSecondsRealtime(0.05f);
                if (!this.$disposing)
                {
                    this.$PC = 2;
                }
                goto Label_0263;
            }
            this.$this.coinsCountText.text = Math.Round(this.$this.completedCoins).ToString();
            this.$this.multiplierImageObj.SetActive(false);
            this.$this.AnimateCoins(this.newCoins);
            this.$PC = -1;
        Label_0261:
            return false;
        Label_0263:
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
    private sealed class <GetDelayedX2>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal GameOverContent $this;
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
                    this.$current = new WaitForSecondsRealtime(0.5f);
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;

                case 1:
                    AnalyticsManager.instance.LogEvent("GameOverReward", new Dictionary<string, string>());
                    this.$this.buttonX2.gameObject.SetActive(false);
                    this.$this.buttonOk.anchoredPosition = Vector2.zero;
                    DataLoader.Instance.RefreshMoney(this.$this.completedCoins, true);
                    this.$this.multiplierImage.gameObject.SetActive(true);
                    this.$this.buttonX2obj.SetActive(false);
                    switch (this.$this.currentMultipler)
                    {
                        case 1:
                            this.$this.multiplierImage.set_sprite(DataLoader.gui.multiplyImages.activeMultiplier[0]);
                            goto Label_01AD;

                        case 2:
                            this.$this.multiplierImage.set_sprite(DataLoader.gui.multiplyImages.activeMultiplier[2]);
                            goto Label_01AD;

                        case 3:
                            this.$this.multiplierImage.set_sprite(DataLoader.gui.multiplyImages.activeMultiplier[4]);
                            goto Label_01AD;

                        case 4:
                            this.$this.multiplierImage.set_sprite(DataLoader.gui.multiplyImages.activeMultiplier[5]);
                            goto Label_01AD;

                        case 5:
                            this.$this.multiplierImage.set_sprite(DataLoader.gui.multiplyImages.activeMultiplier[6]);
                            goto Label_01AD;
                    }
                    break;

                default:
                    goto Label_0206;
            }
        Label_01AD:
            DataLoader.gui.survivorUpgradePanel.SetRandomVideo();
            if (AdsManager.instance.interstitialAdsCounter == 0)
            {
                AdsManager instance = AdsManager.instance;
                instance.interstitialAdsCounter--;
            }
            this.$this.currentMultipler = 2;
            this.$this.AnimateCoins(this.$this.completedCoins);
            this.$PC = -1;
        Label_0206:
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

