using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class UIBoosterPresent : UIPresent
{
    [SerializeField]
    private GameObject openSkip;
    [SerializeField]
    private GameObject claim;
    [SerializeField]
    private GameObject boosterPlashka;
    [SerializeField]
    private RectTransform boxCap;
    [SerializeField]
    private Image newBoosterImage;
    private SaveData.BoostersData.BoosterType currentBooster;

    public void Claim()
    {
        base.GoToGameOver();
    }

    public SaveData.BoostersData.BoosterType GetRandomBooster()
    {
        switch (UnityEngine.Random.Range(0, 2))
        {
            case 0:
                this.newBoosterImage.set_sprite(DataLoader.gui.multiplyImages.activeBoosters[0]);
                return SaveData.BoostersData.BoosterType.NewSurvivor;

            case 1:
                this.newBoosterImage.set_sprite(DataLoader.gui.multiplyImages.activeBoosters[1]);
                return SaveData.BoostersData.BoosterType.KillAll;
        }
        return SaveData.BoostersData.BoosterType.NewSurvivor;
    }

    public override string GetSkipEventName() => 
        "BoosterPresentSkipped";

    [DebuggerHidden]
    public IEnumerator InvokeOpen() => 
        new <InvokeOpen>c__Iterator0 { $this = this };

    public void Open()
    {
        AdsManager.instance.ShowRewarded(() => base.StartCoroutine(this.InvokeOpen()));
    }

    public override void SetContent(int money)
    {
        base.gameObject.SetActive(true);
        this.openSkip.SetActive(true);
        this.claim.SetActive(false);
        this.newBoosterImage.gameObject.SetActive(false);
        this.boosterPlashka.SetActive(true);
        this.boxCap.anchoredPosition = new Vector2(0f, 200f);
        this.currentBooster = this.GetRandomBooster();
    }

    [CompilerGenerated]
    private sealed class <InvokeOpen>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal UIBoosterPresent $this;
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
                    this.$current = new WaitForSecondsRealtime(0.3f);
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;

                case 1:
                    this.$this.openSkip.SetActive(false);
                    this.$this.claim.SetActive(true);
                    this.$this.boosterPlashka.SetActive(false);
                    this.$this.newBoosterImage.gameObject.SetActive(true);
                    DataLoader.Instance.BuyBoosters(this.$this.currentBooster, 1);
                    this.$this.boxCap.anchoredPosition = new Vector2(100f, -224f);
                    AdsManager.instance.DecreaseInterstitialCounter();
                    AnalyticsManager.instance.LogEvent("MoneyPresentX2", new Dictionary<string, string>());
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

