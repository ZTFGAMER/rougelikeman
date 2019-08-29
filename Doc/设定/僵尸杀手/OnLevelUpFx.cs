using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class OnLevelUpFx : MonoBehaviour
{
    public OverlayParticle fx;
    private Vector3 startPos;

    public void Play()
    {
        base.StartCoroutine(this.PlayCor());
    }

    [DebuggerHidden]
    private IEnumerator PlayCor() => 
        new <PlayCor>c__Iterator0 { $this = this };

    public void Start()
    {
        this.startPos = base.transform.position;
    }

    [CompilerGenerated]
    private sealed class <PlayCor>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal OnLevelUpFx $this;
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
                    if (this.$this.transform.position != this.$this.startPos)
                    {
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;
                    }
                    this.$this.fx.Play();
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

