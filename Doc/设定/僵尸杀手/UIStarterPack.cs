using IAP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class UIStarterPack : MonoBehaviour
{
    [SerializeField]
    private Text textBoosterSurvivior;
    [SerializeField]
    private Text textBoosterKillAll;
    [SerializeField]
    private Text textCoins;
    [SerializeField]
    private Text textPrice;
    [SerializeField]
    private Text timeLeft;
    [SerializeField]
    private Text packName;
    public GameObject starterMenu;
    public Button buttonBuy;
    private StarterPackPurchaseInfo sp;
    private TimeSpan timeLeftSpan;
    [HideInInspector]
    public bool autoShowCompleted;
    private string LastPackDate = "LastPackDate";
    private string LastPackIndex = "LastPackIndex";

    [DebuggerHidden]
    public IEnumerator Countdown() => 
        new <Countdown>c__Iterator1 { $this = this };

    [DebuggerHidden]
    private IEnumerator GetPrice(StarterPackPurchaseInfo _sp) => 
        new <GetPrice>c__Iterator2 { 
            _sp = _sp,
            $this = this
        };

    public StarterPackPurchaseInfo GetRandomPack(int index = -1)
    {
        if (index == -1)
        {
            return InAppManager.Instance.packs[UnityEngine.Random.Range(0, InAppManager.Instance.packs.Count)];
        }
        return InAppManager.Instance.packs[index];
    }

    public bool IsAvailable() => 
        true;

    private void OnDisable()
    {
        base.StopAllCoroutines();
    }

    private void OnEnable()
    {
        base.StartCoroutine(this.Scale());
        base.StartCoroutine(this.Countdown());
        base.StartCoroutine(this.GetPrice(this.sp));
    }

    [DebuggerHidden]
    public IEnumerator Scale() => 
        new <Scale>c__Iterator0 { $this = this };

    public void Show(bool fromMenu = false)
    {
        if (GameManager.instance.currentGameMode == GameManager.GameModes.Idle)
        {
            if (PlayerPrefs.HasKey(StaticConstants.firstEnterTime) && !PlayerPrefs.HasKey(StaticConstants.starterPackPurchased))
            {
                DateTime time = new DateTime(Convert.ToInt64(PlayerPrefs.GetString(StaticConstants.firstEnterTime)), DateTimeKind.Utc);
                this.timeLeftSpan = TimeSpan.FromHours((double) StaticConstants.StarterPackHoursDuration).Subtract(TimeManager.CurrentDateTime.Subtract(time));
                if (this.timeLeftSpan.Ticks > 0L)
                {
                    DataLoader.gui.popUpsPanel.OpenPopup();
                    base.gameObject.SetActive(true);
                    this.sp = InAppManager.Instance.starterPack;
                    StarterPackPurchaseInfo.Boosters boosters = this.sp.boosters[0];
                    this.textBoosterSurvivior.text = boosters.amount.ToString();
                    StarterPackPurchaseInfo.Boosters boosters2 = this.sp.boosters[1];
                    this.textBoosterKillAll.text = boosters2.amount.ToString();
                    this.textCoins.text = $"{this.sp.coinsReward:N0}";
                    this.packName.text = LanguageManager.instance.GetLocalizedText(this.sp.displayedPackName);
                    this.packName.set_font(LanguageManager.instance.currentLanguage.font);
                    this.buttonBuy.onClick.AddListener(() => InAppManager.Instance.BuyProductID(this.sp.index));
                    this.autoShowCompleted = true;
                }
                else
                {
                    PlayerPrefs.SetInt(StaticConstants.starterPackPurchased, 1);
                    this.Show(true);
                }
            }
            else if (GameManager.instance.IsTutorialCompleted())
            {
                this.starterMenu.SetActive(true);
                if (PlayerPrefs.HasKey(StaticConstants.starterPackPurchased))
                {
                    if (fromMenu)
                    {
                        base.gameObject.SetActive(true);
                        DataLoader.gui.popUpsPanel.OpenPopup();
                    }
                    else if (!base.gameObject.activeInHierarchy)
                    {
                        DataLoader.gui.popUpsPanel.OnDisable();
                    }
                    bool flag = false;
                    if (PlayerPrefs.HasKey(this.LastPackDate))
                    {
                        DateTime time3 = new DateTime(Convert.ToInt64(PlayerPrefs.GetString(this.LastPackDate)));
                        flag = TimeManager.CurrentDateTime.AddDays(1.0).Date.Subtract(time3.Date).Days > 1;
                    }
                    else
                    {
                        PlayerPrefs.SetString(this.LastPackDate, TimeManager.CurrentDateTime.Ticks.ToString());
                        flag = true;
                    }
                    DateTime currentDateTime = TimeManager.CurrentDateTime;
                    this.timeLeftSpan = currentDateTime.AddDays(1.0).Date.Subtract(currentDateTime);
                    if (flag)
                    {
                        PlayerPrefs.SetString(this.LastPackDate, TimeManager.CurrentDateTime.Ticks.ToString());
                        if (!PlayerPrefs.HasKey(this.LastPackIndex))
                        {
                            int index = UnityEngine.Random.Range(1, 3);
                            this.sp = this.GetRandomPack(index);
                            PlayerPrefs.SetInt(this.LastPackIndex, index);
                        }
                        else
                        {
                            int @int = PlayerPrefs.GetInt(this.LastPackIndex);
                            int index = @int;
                            do
                            {
                                index = UnityEngine.Random.Range(0, InAppManager.Instance.packs.Count);
                            }
                            while (index == @int);
                            this.sp = this.GetRandomPack(index);
                            PlayerPrefs.SetInt(this.LastPackIndex, index);
                        }
                    }
                    else
                    {
                        this.sp = this.GetRandomPack(PlayerPrefs.GetInt(this.LastPackIndex));
                    }
                    StarterPackPurchaseInfo.Boosters boosters3 = this.sp.boosters[0];
                    this.textBoosterSurvivior.text = boosters3.amount.ToString();
                    StarterPackPurchaseInfo.Boosters boosters4 = this.sp.boosters[1];
                    this.textBoosterKillAll.text = boosters4.amount.ToString();
                    this.packName.text = LanguageManager.instance.GetLocalizedText(this.sp.displayedPackName);
                    this.packName.set_font(LanguageManager.instance.currentLanguage.font);
                    this.buttonBuy.onClick.RemoveAllListeners();
                    this.buttonBuy.onClick.AddListener(() => InAppManager.Instance.BuyProductID(this.sp.index));
                    this.textCoins.text = $"{this.sp.coinsReward:N0}";
                }
                else
                {
                    PlayerPrefs.SetString(StaticConstants.firstEnterTime, TimeManager.CurrentDateTime.Ticks.ToString());
                    this.starterMenu.SetActive(true);
                }
            }
            else
            {
                this.starterMenu.SetActive(false);
            }
            base.StartCoroutine(this.GetPrice(this.sp));
            if (!base.gameObject.activeInHierarchy)
            {
                DataLoader.gui.popUpsPanel.gameObject.SetActive(false);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <Countdown>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal UIStarterPack $this;
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
                    if (this.$this.timeLeftSpan.Ticks != 0L)
                    {
                        break;
                    }
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    goto Label_014D;

                case 1:
                    break;

                case 2:
                    this.$this.timeLeftSpan = this.$this.timeLeftSpan.Add(TimeSpan.FromSeconds(-1.0));
                    break;

                default:
                    goto Label_014B;
            }
            if (this.$this.timeLeftSpan.Ticks > 0L)
            {
                this.$this.timeLeft.text = $"{this.$this.timeLeftSpan.Hours + (this.$this.timeLeftSpan.Days * 0x18):D2}:{this.$this.timeLeftSpan.Minutes:D2}:{this.$this.timeLeftSpan.Seconds:D2}";
                this.$current = new WaitForSecondsRealtime(1f);
                if (!this.$disposing)
                {
                    this.$PC = 2;
                }
                goto Label_014D;
            }
            this.$this.Show(false);
            this.$this.OnEnable();
            this.$PC = -1;
        Label_014B:
            return false;
        Label_014D:
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
    private sealed class <GetPrice>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal StarterPackPurchaseInfo _sp;
        internal UIStarterPack $this;
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
                    if (this._sp != null)
                    {
                        break;
                    }
                    goto Label_00A0;

                case 1:
                    if (this.$this.textPrice.text == string.Empty)
                    {
                        break;
                    }
                    this.$PC = -1;
                    goto Label_00A0;

                default:
                    goto Label_00A0;
            }
            this.$this.textPrice.text = InAppManager.Instance.GetPrice(this._sp.purchaseName);
            this.$current = new WaitForSeconds(0.1f);
            if (!this.$disposing)
            {
                this.$PC = 1;
            }
            return true;
        Label_00A0:
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
    private sealed class <Scale>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal Vector3 <targetScale>__0;
        internal UIStarterPack $this;
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
                    this.<targetScale>__0 = new Vector3(1.15f, 1.15f, 1.15f);
                    break;

                case 1:
                    break;

                case 2:
                    goto Label_010A;

                default:
                    goto Label_0130;
            }
            if (this.$this.transform.localScale != this.<targetScale>__0)
            {
                this.$this.transform.localScale = Vector3.MoveTowards(this.$this.transform.localScale, this.<targetScale>__0, Time.deltaTime * 5f);
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                goto Label_0132;
            }
        Label_010A:
            while (this.$this.transform.localScale != Vector3.one)
            {
                this.$this.transform.localScale = Vector3.MoveTowards(this.$this.transform.localScale, Vector3.one, Time.deltaTime * 5f);
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 2;
                }
                goto Label_0132;
            }
            this.$PC = -1;
        Label_0130:
            return false;
        Label_0132:
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

