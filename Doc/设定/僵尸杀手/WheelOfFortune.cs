using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class WheelOfFortune : UIPresent
{
    [SerializeField]
    private RectTransform wheelCircle;
    [SerializeField]
    private Image timer;
    [SerializeField]
    private float showTime;
    [SerializeField]
    private UIWheelCell[] cells;
    [SerializeField]
    private GameObject claim;
    [SerializeField]
    private GameObject spinSkip;
    [SerializeField]
    private GameObject stopSpinObj;
    private readonly int cellsCount = 6;
    [HideInInspector]
    public bool spinning;
    private int selectedCellIndex;
    private int spinCount;
    private int targetRotation;
    private Coroutine spinCor;
    private Coroutine timerCor;

    public void ClosePopup()
    {
        if (this.spinCount > 0)
        {
            this.StopSpin();
            base.gameObject.SetActive(false);
        }
        else
        {
            base.gameObject.SetActive(false);
        }
    }

    public void GetReward()
    {
        this.cells[this.selectedCellIndex].SaveReward();
        base.GoToGameOver();
    }

    public override string GetSkipEventName() => 
        "WheelOfFortuneSkipped";

    [DebuggerHidden]
    public IEnumerator InvokeSpin() => 
        new <InvokeSpin>c__Iterator1 { $this = this };

    public void OnApplicationPause(bool pause)
    {
        if (!pause && this.spinning)
        {
            this.StopSpin();
        }
    }

    public void OnEnable()
    {
        this.timerCor = base.StartCoroutine(this.TimerFill());
    }

    public override void OnEscape()
    {
        if (this.spinCount > 0)
        {
            if (this.spinning)
            {
                this.StopSpin();
            }
            else
            {
                this.GetReward();
            }
        }
        else
        {
            base.Skip();
        }
    }

    public override void SetContent(int money)
    {
        base.gameObject.SetActive(true);
        this.spinSkip.SetActive(true);
        this.claim.SetActive(false);
        this.spinCount = 0;
        List<int> list = new List<int> { 
            0,
            1,
            2,
            3,
            4,
            5
        };
        int num = UnityEngine.Random.Range(0, list.Count);
        this.cells[list[num]].SetMoney(money, false);
        list.Remove(list[num]);
        num = UnityEngine.Random.Range(0, list.Count);
        this.cells[list[num]].SetMoney(money * 2, true);
        list.Remove(list[num]);
        num = UnityEngine.Random.Range(0, list.Count);
        this.cells[list[num]].SetBoosters(1, SaveData.BoostersData.BoosterType.NewSurvivor);
        list.Remove(list[num]);
        num = UnityEngine.Random.Range(0, list.Count);
        this.cells[list[num]].SetBoosters(1, SaveData.BoostersData.BoosterType.KillAll);
        list.Remove(list[num]);
        num = UnityEngine.Random.Range(0, list.Count);
        this.cells[list[num]].SetBoosters(2, SaveData.BoostersData.BoosterType.NewSurvivor);
        list.Remove(list[num]);
        num = UnityEngine.Random.Range(0, list.Count);
        this.cells[list[num]].SetBoosters(2, SaveData.BoostersData.BoosterType.KillAll);
        list.Remove(list[num]);
        this.stopSpinObj.SetActive(false);
    }

    public void Spin()
    {
        AdsManager.instance.ShowRewarded(() => base.StartCoroutine(this.InvokeSpin()));
    }

    [DebuggerHidden]
    private IEnumerator Spinning(int targetCell) => 
        new <Spinning>c__Iterator2 { 
            targetCell = targetCell,
            $this = this
        };

    [DebuggerHidden]
    private IEnumerator StopCellRotation() => 
        new <StopCellRotation>c__Iterator3 { $this = this };

    public void StopSpin()
    {
        this.wheelCircle.eulerAngles = new Vector3(0f, 0f, (float) this.targetRotation);
        base.StopCoroutine(this.spinCor);
        this.stopSpinObj.SetActive(false);
        this.claim.SetActive(true);
        this.spinning = false;
    }

    [DebuggerHidden]
    private IEnumerator TimerFill() => 
        new <TimerFill>c__Iterator0 { $this = this };

    [CompilerGenerated]
    private sealed class <InvokeSpin>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal WheelOfFortune $this;
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
                {
                    this.$this.StopCoroutine(this.$this.timerCor);
                    this.$this.timer.gameObject.SetActive(false);
                    this.$this.stopSpinObj.SetActive(true);
                    this.$this.selectedCellIndex = UnityEngine.Random.Range(0, this.$this.cellsCount);
                    this.$this.spinCor = this.$this.StartCoroutine(this.$this.Spinning(this.$this.selectedCellIndex));
                    this.$this.spinCount++;
                    this.$this.spinSkip.SetActive(false);
                    this.$this.claim.SetActive(false);
                    AdsManager.instance.DecreaseInterstitialCounter();
                    Dictionary<string, string> eventParameters = new Dictionary<string, string> {
                        { 
                            "SpinsInRow",
                            this.$this.spinCount.ToString()
                        }
                    };
                    AnalyticsManager.instance.LogEvent("Spin", eventParameters);
                    this.$PC = -1;
                    break;
                }
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
    private sealed class <Spinning>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal int targetCell;
        internal float <currentRotation>__0;
        internal WheelOfFortune $this;
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
                    this.$this.targetRotation = (UnityEngine.Random.Range(4, 6) * 360) + ((360 / this.$this.cellsCount) * this.targetCell);
                    this.<currentRotation>__0 = this.$this.wheelCircle.eulerAngles.z;
                    this.$this.spinning = true;
                    break;

                case 1:
                    break;

                default:
                    goto Label_0157;
            }
            if (this.<currentRotation>__0 < this.$this.targetRotation)
            {
                this.<currentRotation>__0 += (Mathf.Clamp((float) (this.$this.targetRotation - this.<currentRotation>__0), (float) 5f, (float) 360f) * Time.unscaledDeltaTime) * 2f;
                this.$this.wheelCircle.eulerAngles = new Vector3(0f, 0f, this.<currentRotation>__0);
                this.$current = new WaitForSecondsRealtime(Time.unscaledDeltaTime / 2f);
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                return true;
            }
            this.$this.spinning = false;
            this.$this.claim.SetActive(true);
            this.$this.stopSpinObj.SetActive(false);
            this.$PC = -1;
        Label_0157:
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
    private sealed class <StopCellRotation>c__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal WheelOfFortune $this;
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
                    if (this.$this.spinning)
                    {
                        for (int i = 0; i < this.$this.cells.Length; i++)
                        {
                            this.$this.cells[i].rectTransform.rotation = Quaternion.identity;
                        }
                        this.$current = new WaitForSecondsRealtime(Time.unscaledDeltaTime);
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
    private sealed class <TimerFill>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal WheelOfFortune $this;
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
                    this.$this.timer.gameObject.SetActive(true);
                    this.$this.timer.fillAmount = 1f;
                    break;

                case 1:
                    break;

                default:
                    goto Label_00CF;
            }
            if (this.$this.timer.fillAmount > 0f)
            {
                this.$this.timer.fillAmount -= Time.unscaledDeltaTime / this.$this.showTime;
                this.$current = new WaitForSecondsRealtime(Time.unscaledDeltaTime / 4f);
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                return true;
            }
            this.$this.Skip();
            this.$PC = -1;
        Label_00CF:
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

