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

    public class SpineBlinkPlayer : MonoBehaviour
    {
        private const int BlinkTrack = 1;
        public AnimationReferenceAsset blinkAnimation;
        public float minimumDelay = 0.15f;
        public float maximumDelay = 3f;

        [DebuggerHidden]
        private IEnumerator Start() => 
            new <Start>c__Iterator0 { $this = this };

        [CompilerGenerated]
        private sealed class <Start>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal SkeletonAnimation <skeletonAnimation>__0;
            internal SpineBlinkPlayer $this;
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
                        this.<skeletonAnimation>__0 = this.$this.GetComponent<SkeletonAnimation>();
                        if (this.<skeletonAnimation>__0 != null)
                        {
                            break;
                        }
                        goto Label_00B1;

                    case 1:
                        break;

                    default:
                        goto Label_00B1;
                }
                this.<skeletonAnimation>__0.AnimationState.SetAnimation(1, (Spine.Animation) this.$this.blinkAnimation, false);
                this.$current = new WaitForSeconds(UnityEngine.Random.Range(this.$this.minimumDelay, this.$this.maximumDelay));
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                return true;
            Label_00B1:
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
}

