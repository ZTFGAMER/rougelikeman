using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextUI : MonoBehaviour
{
    public Text text;
    [SerializeField]
    private Image coinImage;
    private Color color;

    private void Start()
    {
        base.StartCoroutine(this.Up());
    }

    [DebuggerHidden]
    private IEnumerator Up() => 
        new <Up>c__Iterator0 { $this = this };

    [CompilerGenerated]
    private sealed class <Up>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal float <speed>__0;
        internal FloatingTextUI $this;
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
                    this.<speed>__0 = 160f;
                    break;

                case 1:
                    this.$this.text.get_rectTransform().anchoredPosition = new Vector2(this.$this.text.get_rectTransform().anchoredPosition.x, this.$this.text.get_rectTransform().anchoredPosition.y + (this.<speed>__0 * Time.deltaTime));
                    this.$this.color = this.$this.text.get_color();
                    this.$this.color.a -= Time.deltaTime;
                    this.$this.text.set_color(this.$this.color);
                    this.$this.color = this.$this.coinImage.get_color();
                    this.$this.color.a -= Time.deltaTime;
                    this.$this.coinImage.set_color(this.$this.color);
                    break;

                default:
                    goto Label_018A;
            }
            if (this.$this.text.get_color().a > 0f)
            {
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                return true;
            }
            UnityEngine.Object.Destroy(this.$this.gameObject);
            this.$PC = -1;
        Label_018A:
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

