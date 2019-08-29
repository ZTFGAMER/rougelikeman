namespace Spine.Unity.Examples
{
    using Spine;
    using Spine.Unity;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class SpineboyFreeze : MonoBehaviour
    {
        public SkeletonAnimation skeletonAnimation;
        public AnimationReferenceAsset freeze;
        public AnimationReferenceAsset idle;
        public Color freezeColor;
        public Color freezeBlackColor;
        public ParticleSystem particles;
        public float freezePoint = 0.5f;
        public string colorProperty = "_Color";
        public string blackTintProperty = "_Black";
        private MaterialPropertyBlock block;
        private MeshRenderer meshRenderer;

        [DebuggerHidden]
        private IEnumerator Start() => 
            new <Start>c__Iterator0 { $this = this };

        [CompilerGenerated]
        private sealed class <Start>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal ParticleSystem.MainModule <main>__0;
            internal Spine.AnimationState <state>__0;
            internal SpineboyFreeze $this;
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
                        this.$this.block = new MaterialPropertyBlock();
                        this.$this.meshRenderer = this.$this.GetComponent<MeshRenderer>();
                        this.$this.particles.Stop();
                        this.$this.particles.Clear();
                        this.<main>__0 = this.$this.particles.main;
                        this.<main>__0.loop = false;
                        this.<state>__0 = this.$this.skeletonAnimation.AnimationState;
                        break;

                    case 1:
                        this.<state>__0.SetAnimation(0, (Spine.Animation) this.$this.freeze, false);
                        this.$current = new WaitForSeconds(this.$this.freezePoint);
                        if (!this.$disposing)
                        {
                            this.$PC = 2;
                        }
                        goto Label_0254;

                    case 2:
                        this.$this.particles.Play();
                        this.$this.block.SetColor(this.$this.colorProperty, this.$this.freezeColor);
                        this.$this.block.SetColor(this.$this.blackTintProperty, this.$this.freezeBlackColor);
                        this.$this.meshRenderer.SetPropertyBlock(this.$this.block);
                        this.$current = new WaitForSeconds(2f);
                        if (!this.$disposing)
                        {
                            this.$PC = 3;
                        }
                        goto Label_0254;

                    case 3:
                        this.<state>__0.SetAnimation(0, (Spine.Animation) this.$this.idle, true);
                        this.$this.block.SetColor(this.$this.colorProperty, Color.white);
                        this.$this.block.SetColor(this.$this.blackTintProperty, Color.black);
                        this.$this.meshRenderer.SetPropertyBlock(this.$this.block);
                        this.$current = null;
                        if (!this.$disposing)
                        {
                            this.$PC = 4;
                        }
                        goto Label_0254;

                    case 4:
                        break;

                    default:
                        return false;
                }
                this.$current = new WaitForSeconds(1f);
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
            Label_0254:
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
}

