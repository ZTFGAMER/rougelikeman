using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class UIStreak : MonoBehaviour
{
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static UIStreak <instance>k__BackingField;
    [SerializeField]
    private Text text;
    [SerializeField]
    private Text textFront;
    [SerializeField]
    private Text textBack;
    [SerializeField]
    private int[] streakCount;
    [SerializeField]
    private float streakDelay;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private OverlayParticle fx;
    private int currentStreak;
    private int lastStreakIndex;
    private float speed = 5f;
    private Coroutine textCoroutine;
    private Coroutine fxPlay;

    private void Awake()
    {
        instance = this;
    }

    public int GetStreakIndex(int count)
    {
        int index = 0;
        while (index < this.streakCount.Length)
        {
            if (count < this.streakCount[index])
            {
                return index;
            }
            index++;
        }
        return index;
    }

    public void IncreaseStreak()
    {
        this.currentStreak++;
        this.SetStreak();
        base.CancelInvoke("ResetStreak");
        base.Invoke("ResetStreak", this.streakDelay);
    }

    public void IsStreakKill()
    {
        int streakIndex = this.GetStreakIndex(this.currentStreak);
        if (this.lastStreakIndex < streakIndex)
        {
            this.lastStreakIndex = streakIndex;
            this.SwitchIndex(this.lastStreakIndex);
        }
    }

    [DebuggerHidden]
    private IEnumerator OnEnablePlay() => 
        new <OnEnablePlay>c__Iterator1 { $this = this };

    public void ResetStreak()
    {
        this.currentStreak = 0;
        this.lastStreakIndex = 0;
    }

    private void SetStreak()
    {
        if (this.currentStreak >= this.streakCount[0])
        {
            this.SwitchIndex(UnityEngine.Random.Range(0, this.streakCount.Length));
            this.ResetStreak();
        }
    }

    private void Start()
    {
        this.text.gameObject.SetActive(false);
    }

    private void StartTextCoroutine(string _text)
    {
        this.text.text = _text;
        this.textFront.text = this.text.text;
        this.textBack.text = this.text.text;
        this.anim.Play("Two");
        if (this.fxPlay != null)
        {
            base.StopCoroutine(this.fxPlay);
        }
        this.fxPlay = base.StartCoroutine(this.OnEnablePlay());
    }

    private void SwitchIndex(int index)
    {
        switch (index)
        {
            case 0:
                this.text.gameObject.SetActive(false);
                break;

            case 1:
                this.StartTextCoroutine("Killing Spree");
                break;

            case 2:
                this.StartTextCoroutine("Brutal");
                break;

            case 3:
                this.StartTextCoroutine("Rampage");
                break;

            case 4:
                this.StartTextCoroutine("Killing Frenzy");
                break;

            case 5:
                this.StartTextCoroutine("Overkill");
                break;

            case 6:
                this.StartTextCoroutine("Unstoppable");
                break;

            case 7:
                this.StartTextCoroutine("Killpocalypse");
                break;

            case 8:
                this.StartTextCoroutine("Killionaire");
                break;
        }
    }

    [DebuggerHidden]
    public IEnumerator TextCor() => 
        new <TextCor>c__Iterator0 { $this = this };

    public static UIStreak instance
    {
        [CompilerGenerated]
        get => 
            <instance>k__BackingField;
        [CompilerGenerated]
        private set => 
            (<instance>k__BackingField = value);
    }

    [CompilerGenerated]
    private sealed class <OnEnablePlay>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal UIStreak $this;
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
                    if (!this.$this.fx.gameObject.activeInHierarchy)
                    {
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        goto Label_00D1;
                    }
                    this.$this.fx.Play();
                    break;

                case 2:
                    break;

                case 3:
                    this.$PC = -1;
                    goto Label_00CF;

                default:
                    goto Label_00CF;
            }
            while (this.$this.fx.gameObject.activeInHierarchy)
            {
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 2;
                }
                goto Label_00D1;
            }
            this.$current = null;
            if (!this.$disposing)
            {
                this.$PC = 3;
            }
            goto Label_00D1;
        Label_00CF:
            return false;
        Label_00D1:
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
    private sealed class <TextCor>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal UIStreak $this;
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
                    this.$this.text.gameObject.SetActive(true);
                    this.$this.text.transform.localScale = Vector3.zero;
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    goto Label_0258;

                case 1:
                case 2:
                    if (this.$this.text.transform.localScale != (Vector3.one * 1.5f))
                    {
                        this.$this.text.transform.localScale = Vector3.MoveTowards(this.$this.text.transform.localScale, Vector3.one * 1.5f, Time.deltaTime * this.$this.speed);
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 2;
                        }
                        goto Label_0258;
                    }
                    break;

                case 3:
                    break;

                case 4:
                case 5:
                    while (this.$this.text.transform.localScale != Vector3.zero)
                    {
                        this.$this.text.transform.localScale = Vector3.MoveTowards(this.$this.text.transform.localScale, Vector3.zero, Time.deltaTime * this.$this.speed);
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 5;
                        }
                        goto Label_0258;
                    }
                    this.$PC = -1;
                    goto Label_0256;

                default:
                    goto Label_0256;
            }
            while (this.$this.text.transform.localScale != Vector3.one)
            {
                this.$this.text.transform.localScale = Vector3.MoveTowards(this.$this.text.transform.localScale, Vector3.one, Time.deltaTime * this.$this.speed);
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 3;
                }
                goto Label_0258;
            }
            this.$current = new WaitForSeconds(1.8f);
            if (!this.$disposing)
            {
                this.$PC = 4;
            }
            goto Label_0258;
        Label_0256:
            return false;
        Label_0258:
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

