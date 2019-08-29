using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField]
    private TextMesh textMesh;
    [SerializeField]
    private SpriteRenderer coinSprite;
    [SerializeField]
    private AudioClip appearanceSound;
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
        internal FloatingText $this;
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
                    this.<speed>__0 = 3f;
                    SoundManager.Instance.PlaySound(this.$this.appearanceSound, -1f);
                    this.$this.textMesh.transform.position = new Vector3(this.$this.transform.position.x, this.$this.transform.position.y + 1.5f, this.$this.transform.position.z);
                    this.$this.textMesh.transform.Rotate((float) 57f, 0f, (float) 0f);
                    break;

                case 1:
                    this.$this.transform.position = new Vector3(this.$this.transform.position.x, this.$this.transform.position.y + (this.<speed>__0 * Time.deltaTime), this.$this.transform.position.z);
                    this.$this.color = this.$this.textMesh.color;
                    this.$this.color.a -= Time.deltaTime;
                    this.$this.textMesh.color = this.$this.color;
                    this.$this.color = this.$this.coinSprite.color;
                    this.$this.color.a -= Time.deltaTime;
                    this.$this.coinSprite.color = this.$this.color;
                    break;

                default:
                    goto Label_023D;
            }
            if (this.$this.textMesh.color.a > 0f)
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
        Label_023D:
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

