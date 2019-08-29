using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class UIColorAlphaChanger : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private Color targetColor;
    [SerializeField]
    private float loopDelay;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float timeBetweenColorChange;
    [SerializeField]
    private int changesCount;
    private Color currentColor;
    private Color startColor;

    [DebuggerHidden]
    private IEnumerator ChangeColor() => 
        new <ChangeColor>c__Iterator0 { $this = this };

    private void Start()
    {
        this.startColor = this.image.get_color();
        base.StartCoroutine(this.ChangeColor());
    }

    [CompilerGenerated]
    private sealed class <ChangeColor>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal int <i>__1;
        internal UIColorAlphaChanger $this;
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
                    this.$this.currentColor = this.$this.startColor;
                    break;

                case 1:
                    this.<i>__1 = 0;
                    goto Label_01AB;

                case 2:
                    goto Label_00DC;

                case 3:
                    goto Label_0159;

                case 4:
                    this.<i>__1++;
                    goto Label_01AB;

                default:
                    return false;
            }
        Label_0043:
            this.$current = new WaitForSeconds(this.$this.loopDelay);
            if (!this.$disposing)
            {
                this.$PC = 1;
            }
            goto Label_01CF;
        Label_00DC:
            if (this.$this.currentColor.a < 1f)
            {
                this.$this.currentColor.a += Time.deltaTime * this.$this.speed;
                this.$this.image.set_color(this.$this.currentColor);
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 2;
                }
                goto Label_01CF;
            }
        Label_0159:
            while (this.$this.currentColor.a > 0f)
            {
                this.$this.currentColor.a -= Time.deltaTime * this.$this.speed;
                this.$this.image.set_color(this.$this.currentColor);
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 3;
                }
                goto Label_01CF;
            }
            this.$current = new WaitForSeconds(this.$this.timeBetweenColorChange);
            if (!this.$disposing)
            {
                this.$PC = 4;
            }
            goto Label_01CF;
        Label_01AB:
            if (this.<i>__1 < this.$this.changesCount)
            {
                goto Label_00DC;
            }
            goto Label_0043;
        Label_01CF:
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

