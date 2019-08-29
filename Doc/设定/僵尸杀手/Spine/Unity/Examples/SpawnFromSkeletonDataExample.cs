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

    public class SpawnFromSkeletonDataExample : MonoBehaviour
    {
        public SkeletonDataAsset skeletonDataAsset;
        [Range(0f, 100f)]
        public int count = 20;
        [SpineAnimation("", "skeletonDataAsset", true, false)]
        public string startingAnimation;

        private void DoExtraStuff(SkeletonAnimation sa, Spine.Animation spineAnimation)
        {
            sa.transform.localPosition = (Vector3) (UnityEngine.Random.insideUnitCircle * 6f);
            sa.transform.SetParent(base.transform, false);
            if (spineAnimation != null)
            {
                sa.Initialize(false);
                sa.AnimationState.SetAnimation(0, spineAnimation, true);
            }
        }

        [DebuggerHidden]
        private IEnumerator Start() => 
            new <Start>c__Iterator0 { $this = this };

        [CompilerGenerated]
        private sealed class <Start>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal Spine.Animation <spineAnimation>__0;
            internal int <i>__1;
            internal SkeletonAnimation <sa>__2;
            internal SpawnFromSkeletonDataExample $this;
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
                        if (this.$this.skeletonDataAsset != null)
                        {
                            this.$this.skeletonDataAsset.GetSkeletonData(false);
                            this.$current = new WaitForSeconds(1f);
                            if (!this.$disposing)
                            {
                                this.$PC = 1;
                            }
                            goto Label_0148;
                        }
                        goto Label_0146;

                    case 1:
                        this.<spineAnimation>__0 = this.$this.skeletonDataAsset.GetSkeletonData(false).FindAnimation(this.$this.startingAnimation);
                        this.<i>__1 = 0;
                        break;

                    case 2:
                        this.<i>__1++;
                        break;

                    default:
                        goto Label_0146;
                }
                if (this.<i>__1 < this.$this.count)
                {
                    this.<sa>__2 = SkeletonAnimation.NewSkeletonAnimationGameObject(this.$this.skeletonDataAsset);
                    this.$this.DoExtraStuff(this.<sa>__2, this.<spineAnimation>__0);
                    this.<sa>__2.gameObject.name = this.<i>__1.ToString();
                    this.$current = new WaitForSeconds(0.125f);
                    if (!this.$disposing)
                    {
                        this.$PC = 2;
                    }
                    goto Label_0148;
                }
                this.$PC = -1;
            Label_0146:
                return false;
            Label_0148:
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

