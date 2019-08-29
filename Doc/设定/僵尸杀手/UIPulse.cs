using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UIPulse : MonoBehaviour
{
    [SerializeField]
    private float loopDelay;
    [SerializeField]
    private int pulseCount;
    [SerializeField]
    private float maxScale;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float timeBetweenPulse;

    private void OnDisable()
    {
        base.StopAllCoroutines();
    }

    private void OnEnable()
    {
        base.StartCoroutine(this.Pulse());
    }

    [DebuggerHidden]
    private IEnumerator Pulse() => 
        new <Pulse>c__Iterator0 { $this = this };

    [CompilerGenerated]
    private sealed class <Pulse>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal int <i>__1;
        internal UIPulse $this;
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
                    break;

                case 1:
                    this.<i>__1 = 0;
                    goto Label_01AF;

                case 2:
                    goto Label_00CE;

                case 3:
                    goto Label_0158;

                case 4:
                    this.<i>__1++;
                    goto Label_01AF;

                default:
                    return false;
            }
        Label_002D:
            this.$current = new WaitForSeconds(this.$this.loopDelay);
            if (!this.$disposing)
            {
                this.$PC = 1;
            }
            goto Label_01D3;
        Label_00CE:
            if (this.$this.transform.localScale != (Vector3.one * this.$this.maxScale))
            {
                this.$this.transform.localScale = Vector3.MoveTowards(this.$this.transform.localScale, Vector3.one * this.$this.maxScale, Time.deltaTime * this.$this.speed);
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 2;
                }
                goto Label_01D3;
            }
        Label_0158:
            while (this.$this.transform.localScale != Vector3.one)
            {
                this.$this.transform.localScale = Vector3.MoveTowards(this.$this.transform.localScale, Vector3.one, Time.deltaTime * this.$this.speed);
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 3;
                }
                goto Label_01D3;
            }
            this.$current = new WaitForSeconds(this.$this.timeBetweenPulse);
            if (!this.$disposing)
            {
                this.$PC = 4;
            }
            goto Label_01D3;
        Label_01AF:
            if (this.<i>__1 < this.$this.pulseCount)
            {
                goto Label_00CE;
            }
            goto Label_002D;
        Label_01D3:
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

