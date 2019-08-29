using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DataUpdateManager : MonoBehaviour
{
    private bool isPaused;
    private bool firstUpdateCompleted;
    private Coroutine dailyMultiplier;
    private bool offlineTimeLoaded;

    [DebuggerHidden]
    private IEnumerator ConnectionCheck(float everySeconds) => 
        new <ConnectionCheck>c__Iterator3 { 
            everySeconds = everySeconds,
            $this = this
        };

    [DebuggerHidden]
    private IEnumerator DailyMultiplier(TimeSpan time) => 
        new <DailyMultiplier>c__Iterator1 { 
            time = time,
            $this = this
        };

    public TimeSpan GetTimeSpan(string text)
    {
        char[] separator = new char[] { ':' };
        string[] strArray = text.Split(separator);
        return new TimeSpan(int.Parse(strArray[0]) / 0x18, int.Parse(strArray[0]) - ((int.Parse(strArray[0]) / 0x18) * 0x18), int.Parse(strArray[1]), int.Parse(strArray[2]));
    }

    public double LoadOfflineTime()
    {
        double totalSeconds = 0.0;
        if ((TimeManager.gotDateTime && PlayerPrefs.HasKey(StaticConstants.LastOnlineTime)) && PlayerPrefs.HasKey(StaticConstants.TutorialCompleted))
        {
            DateTime time = new DateTime(Convert.ToInt64(PlayerPrefs.GetString(StaticConstants.LastOnlineTime)), DateTimeKind.Utc);
            totalSeconds = TimeManager.CurrentDateTime.Subtract(time).TotalSeconds;
        }
        else
        {
            totalSeconds = 0.0;
        }
        this.offlineTimeLoaded = true;
        return totalSeconds;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            DataLoader.Instance.SaveAllData();
            this.SaveOfflineTime();
        }
        else if (this.isPaused)
        {
            if (!StaticConstants.IsConnectedToInternet())
            {
                DataLoader.gui.NoInternetPanel(false);
            }
            else
            {
                this.UpdateAfterConnect();
            }
        }
        this.isPaused = pause;
    }

    private void OnApplicationQuit()
    {
        DataLoader.Instance.SaveAllData();
        this.SaveOfflineTime();
    }

    public void SaveOfflineTime()
    {
        if (TimeManager.gotDateTime)
        {
            PlayerPrefs.SetString(StaticConstants.LastOnlineTime, TimeManager.CurrentDateTime.Ticks.ToString());
        }
    }

    public void SetInfinityMultiplierTime()
    {
        DataLoader.Instance.dailyMultiplier = 2;
        DataLoader.gui.videoMultiplier.SetInfinityPurchased();
    }

    public void StartUpdate()
    {
        this.isPaused = false;
        this.offlineTimeLoaded = false;
        base.StartCoroutine(this.ConnectionCheck(3f));
        if (StaticConstants.IsConnectedToInternet())
        {
            this.UpdateDailyMultiplier();
            this.UpdateOfflineMoney();
        }
    }

    [DebuggerHidden]
    public IEnumerator StartUpdateDailtMultiplier() => 
        new <StartUpdateDailtMultiplier>c__Iterator0 { $this = this };

    public void UpdateAfterConnect()
    {
        this.UpdateOfflineMoney();
        this.UpdateDailyMultiplier();
    }

    public void UpdateDailyMultiplier()
    {
        base.StartCoroutine(this.StartUpdateDailtMultiplier());
    }

    public void UpdateOfflineMoney()
    {
        this.offlineTimeLoaded = false;
        base.StartCoroutine(this.WaitForSquadSpawn());
    }

    [DebuggerHidden]
    private IEnumerator WaitForSquadSpawn() => 
        new <WaitForSquadSpawn>c__Iterator2();

    [CompilerGenerated]
    private sealed class <ConnectionCheck>c__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal float everySeconds;
        internal DataUpdateManager $this;
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
                    this.$current = new WaitForSeconds(6f);
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    goto Label_0121;

                case 1:
                case 3:
                    if (StaticConstants.IsConnectedToInternet())
                    {
                        if (this.$this.offlineTimeLoaded && (TimeManager.CurrentDateTime != DateTime.MinValue))
                        {
                            PlayerPrefs.SetString(StaticConstants.LastOnlineTime, TimeManager.CurrentDateTime.Ticks.ToString());
                        }
                        goto Label_00EE;
                    }
                    DataLoader.gui.NoInternetPanel(false);
                    break;

                case 2:
                    break;

                default:
                    return false;
            }
            while (!StaticConstants.IsConnectedToInternet())
            {
                this.$current = new WaitForSecondsRealtime(1f);
                if (!this.$disposing)
                {
                    this.$PC = 2;
                }
                goto Label_0121;
            }
            DataLoader.gui.NoInternetPanel(true);
        Label_00EE:
            this.$current = new WaitForSecondsRealtime(this.everySeconds);
            if (!this.$disposing)
            {
                this.$PC = 3;
            }
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
    private sealed class <DailyMultiplier>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal TimeSpan <prevSpan>__0;
        internal int <addedMinutes>__0;
        internal TimeSpan time;
        internal DataUpdateManager $this;
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
                    DataLoader.gui.videoAnim.SetBool("IsOpened", false);
                    DataLoader.gui.multiplierAnim.SetBool("IsOpened", true);
                    this.<prevSpan>__0 = this.$this.GetTimeSpan(DataLoader.gui.popupGoldMultiplier.text);
                    this.<addedMinutes>__0 = (StaticConstants.MultiplierDurationInSeconds / 60) / 12;
                    DataLoader.gui.videoMultiplier.redCircle.SetActive(false);
                    if ((this.<prevSpan>__0.TotalMinutes >= this.time.TotalMinutes) || !this.$this.firstUpdateCompleted)
                    {
                        goto Label_01F2;
                    }
                    if (DataLoader.gui.popupGoldMultiplier.get_rectTransform().localScale == Vector3.one)
                    {
                        RectTransform transform1 = DataLoader.gui.popupGoldMultiplier.get_rectTransform();
                        transform1.localScale *= 1.25f;
                    }
                    break;

                case 1:
                    break;

                case 2:
                    goto Label_02FA;

                case 3:
                    DataLoader.gui.multiplierAnim.SetBool("IsOpened", false);
                    DataLoader.gui.popupGoldMultiplier.text = "00:00:00";
                    DataLoader.Instance.dailyMultiplier = 1;
                    PlayerPrefs.SetInt(StaticConstants.MultiplierKey, 1);
                    this.$PC = -1;
                    goto Label_03B6;

                default:
                    goto Label_03B6;
            }
            if (this.<prevSpan>__0.TotalMinutes < this.time.TotalMinutes)
            {
                this.<prevSpan>__0 = this.<prevSpan>__0.Add(TimeSpan.FromMinutes((double) this.<addedMinutes>__0));
                DataLoader.gui.dailyGoldMultiplier.text = $"{this.<prevSpan>__0.Hours + (this.<prevSpan>__0.Days * 0x18):D2}:{this.<prevSpan>__0.Minutes:D2}:{this.<prevSpan>__0.Seconds:D2}";
                DataLoader.gui.popupGoldMultiplier.text = DataLoader.gui.dailyGoldMultiplier.text;
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                goto Label_03B8;
            }
            DataLoader.gui.popupGoldMultiplier.get_rectTransform().localScale = Vector3.one;
        Label_01F2:
            this.$this.firstUpdateCompleted = true;
            if (DataLoader.Instance.dailyMultiplier == 1)
            {
                UnityEngine.Debug.LogWarning("MULTIPLIER 1");
                goto Label_0322;
            }
            DataLoader.gui.multiplierPanelImage.set_sprite(DataLoader.gui.multiplyImages.activeMultiplier[MultiplyImages.GetMultiplierSpriteID(DataLoader.Instance.dailyMultiplier)]);
        Label_02FA:
            while (this.time.TotalSeconds > 0.0)
            {
                DataLoader.gui.dailyGoldMultiplier.text = $"{this.time.Hours + (this.time.Days * 0x18):D2}:{this.time.Minutes:D2}:{this.time.Seconds:D2}";
                DataLoader.gui.popupGoldMultiplier.text = DataLoader.gui.dailyGoldMultiplier.text;
                this.time = this.time.Add(TimeSpan.FromSeconds(-1.0));
                this.$current = new WaitForSeconds(1f);
                if (!this.$disposing)
                {
                    this.$PC = 2;
                }
                goto Label_03B8;
            }
        Label_0322:
            DataLoader.gui.videoMultiplier.redCircle.SetActive(true);
            DataLoader.gui.videoAnim.SetBool("IsOpened", true);
            this.$current = new WaitForSeconds(0.5f);
            if (!this.$disposing)
            {
                this.$PC = 3;
            }
            goto Label_03B8;
        Label_03B6:
            return false;
        Label_03B8:
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
    private sealed class <StartUpdateDailtMultiplier>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal DataUpdateManager $this;
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
                    if (!DataLoader.initialized)
                    {
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;
                    }
                    if (this.$this.dailyMultiplier != null)
                    {
                        this.$this.StopCoroutine(this.$this.dailyMultiplier);
                    }
                    if (!PlayerPrefs.HasKey(StaticConstants.DailyMultiplierTime))
                    {
                        this.$this.firstUpdateCompleted = true;
                        DataLoader.gui.videoAnim.SetBool("IsOpened", true);
                        DataLoader.Instance.dailyMultiplier = 1;
                        DataLoader.gui.popupGoldMultiplier.text = "00:00:00";
                        DataLoader.gui.videoMultiplier.redCircle.SetActive(true);
                        if (PlayerPrefs.HasKey(StaticConstants.infinityMultiplierPurchased))
                        {
                            this.$this.SetInfinityMultiplierTime();
                            DataLoader.gui.videoMultiplier.redCircle.SetActive(false);
                        }
                        break;
                    }
                    DataLoader.Instance.dailyMultiplier = PlayerPrefs.GetInt(StaticConstants.MultiplierKey);
                    if (PlayerPrefs.HasKey(StaticConstants.infinityMultiplierPurchased) && (DataLoader.Instance.dailyMultiplier <= 2))
                    {
                        this.$this.SetInfinityMultiplierTime();
                    }
                    else
                    {
                        DataLoader.gui.videoMultiplier.infinityTimerGreen.SetActive(false);
                        DataLoader.gui.videoMultiplier.infinityTimerImage.SetActive(false);
                        DataLoader.gui.dailyGoldMultiplier.gameObject.SetActive(true);
                        DataLoader.gui.popupGoldMultiplier.gameObject.SetActive(true);
                        if (TimeManager.gotDateTime)
                        {
                            this.$this.dailyMultiplier = this.$this.StartCoroutine(this.$this.DailyMultiplier(DataLoader.Instance.GetMultiplierTime()));
                        }
                        else
                        {
                            DataLoader.Instance.dailyMultiplier = 1;
                            DataLoader.gui.videoMultiplier.redCircle.SetActive(true);
                            DataLoader.gui.videoAnim.SetBool("IsOpened", true);
                        }
                    }
                    break;

                default:
                    goto Label_0216;
            }
            this.$PC = -1;
        Label_0216:
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
    private sealed class <WaitForSquadSpawn>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
    {
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
                    if (!DataLoader.initialized)
                    {
                        this.$current = new WaitForSeconds(1f);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;
                    }
                    if (TimeManager.CurrentDateTime != DateTime.MinValue)
                    {
                        DataLoader.gui.GetOfflineMoney();
                        PlayerPrefs.SetString(StaticConstants.LastOnlineTime, TimeManager.CurrentDateTime.Ticks.ToString());
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
}

