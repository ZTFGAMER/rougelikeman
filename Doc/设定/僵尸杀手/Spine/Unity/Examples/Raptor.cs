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

    public class Raptor : MonoBehaviour
    {
        public AnimationReferenceAsset walk;
        public AnimationReferenceAsset gungrab;
        public AnimationReferenceAsset gunkeep;
        private SkeletonAnimation skeletonAnimation;

        [DebuggerHidden]
        private IEnumerator GunGrabRoutine() => 
            new <GunGrabRoutine>c__Iterator0 { $this = this };

        private void Start()
        {
            this.skeletonAnimation = base.GetComponent<SkeletonAnimation>();
            base.StartCoroutine(this.GunGrabRoutine());
        }

        [CompilerGenerated]
        private sealed class <GunGrabRoutine>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal Raptor $this;
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
                        this.$this.skeletonAnimation.AnimationState.SetAnimation(0, (Spine.Animation) this.$this.walk, true);
                        break;

                    case 1:
                        this.$this.skeletonAnimation.AnimationState.SetAnimation(1, (Spine.Animation) this.$this.gungrab, false);
                        this.$current = new WaitForSeconds(UnityEngine.Random.Range((float) 0.5f, (float) 3f));
                        if (!this.$disposing)
                        {
                            this.$PC = 2;
                        }
                        goto Label_0107;

                    case 2:
                        this.$this.skeletonAnimation.AnimationState.SetAnimation(1, (Spine.Animation) this.$this.gunkeep, false);
                        break;

                    default:
                        return false;
                }
                this.$current = new WaitForSeconds(UnityEngine.Random.Range((float) 0.5f, (float) 3f));
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
            Label_0107:
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

