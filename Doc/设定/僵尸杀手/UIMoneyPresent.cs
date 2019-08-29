using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class UIMoneyPresent : UIPresent
{
    [SerializeField]
    private Text moneyText;
    [SerializeField]
    private Button buttonX2;
    [SerializeField]
    private GameObject x2Claim;
    [SerializeField]
    private GameObject claim;
    private int money;

    public void ClaimMoney()
    {
        DataLoader.Instance.RefreshMoney((double) this.money, true);
        base.GoToGameOver();
    }

    public void DoubleMoney()
    {
        AdsManager.instance.ShowRewarded(() => base.StartCoroutine(this.InvokeX2()));
    }

    public override string GetSkipEventName() => 
        "MoneyPresentSkipped";

    [DebuggerHidden]
    public IEnumerator InvokeX2() => 
        new <InvokeX2>c__Iterator0 { $this = this };

    public override void SetContent(int money)
    {
        base.gameObject.SetActive(true);
        this.x2Claim.SetActive(true);
        this.claim.SetActive(false);
        float num = 0f;
        for (int i = 0; i < DataLoader.playerData.heroData.Count; i++)
        {
            SaveData.HeroData data = DataLoader.playerData.heroData[i];
            num += DataLoader.Instance.GetHeroPower(data.heroType);
        }
        this.money = ((int) num) * 2;
        this.moneyText.text = this.money.ToString();
    }

    [CompilerGenerated]
    private sealed class <InvokeX2>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal UIMoneyPresent $this;
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
                    this.$this.buttonX2.interactable = false;
                    this.$current = new WaitForSecondsRealtime(0.3f);
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;

                case 1:
                    this.$this.x2Claim.SetActive(false);
                    this.$this.claim.SetActive(true);
                    this.$this.money *= 2;
                    this.$this.moneyText.text = this.$this.money.ToString();
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

