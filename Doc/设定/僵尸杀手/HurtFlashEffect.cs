using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HurtFlashEffect : MonoBehaviour
{
    private const int DefaultFlashCount = 3;
    public int flashCount = 3;
    public Color flashColor = Color.white;
    [Range(0.008333334f, 0.06666667f)]
    public float interval = 0.01666667f;
    public string fillPhaseProperty = "_FillPhase";
    public string fillColorProperty = "_FillColor";
    private MaterialPropertyBlock mpb;
    private MeshRenderer meshRenderer;

    public void Flash()
    {
        if (this.mpb == null)
        {
            this.mpb = new MaterialPropertyBlock();
        }
        if (this.meshRenderer == null)
        {
            this.meshRenderer = base.GetComponent<MeshRenderer>();
        }
        this.meshRenderer.GetPropertyBlock(this.mpb);
        base.StartCoroutine(this.FlashRoutine());
    }

    [DebuggerHidden]
    private IEnumerator FlashRoutine() => 
        new <FlashRoutine>c__Iterator0 { $this = this };

    [CompilerGenerated]
    private sealed class <FlashRoutine>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal int <fillPhase>__0;
        internal int <fillColor>__0;
        internal WaitForSeconds <wait>__0;
        internal int <i>__1;
        internal HurtFlashEffect $this;
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
                    if (this.$this.flashCount < 0)
                    {
                        this.$this.flashCount = 3;
                    }
                    this.<fillPhase>__0 = Shader.PropertyToID(this.$this.fillPhaseProperty);
                    this.<fillColor>__0 = Shader.PropertyToID(this.$this.fillColorProperty);
                    this.<wait>__0 = new WaitForSeconds(this.$this.interval);
                    this.<i>__1 = 0;
                    while (this.<i>__1 < this.$this.flashCount)
                    {
                        this.$this.mpb.SetColor(this.<fillColor>__0, this.$this.flashColor);
                        this.$this.mpb.SetFloat(this.<fillPhase>__0, 1f);
                        this.$this.meshRenderer.SetPropertyBlock(this.$this.mpb);
                        this.$current = this.<wait>__0;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        goto Label_01A9;
                    Label_0161:
                        this.<i>__1++;
                    }
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 3;
                    }
                    goto Label_01A9;

                case 1:
                    this.$this.mpb.SetFloat(this.<fillPhase>__0, 0f);
                    this.$this.meshRenderer.SetPropertyBlock(this.$this.mpb);
                    this.$current = this.<wait>__0;
                    if (!this.$disposing)
                    {
                        this.$PC = 2;
                    }
                    goto Label_01A9;

                case 2:
                    goto Label_0161;

                case 3:
                    this.$PC = -1;
                    break;
            }
            return false;
        Label_01A9:
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

