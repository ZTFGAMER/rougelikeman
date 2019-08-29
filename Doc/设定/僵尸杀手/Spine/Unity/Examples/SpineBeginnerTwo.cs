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

    public class SpineBeginnerTwo : MonoBehaviour
    {
        [SpineAnimation("", "", true, false)]
        public string runAnimationName;
        [SpineAnimation("", "", true, false)]
        public string idleAnimationName;
        [SpineAnimation("", "", true, false)]
        public string walkAnimationName;
        [SpineAnimation("", "", true, false)]
        public string shootAnimationName;
        [Header("Transitions"), SpineAnimation("", "", true, false)]
        public string idleTurnAnimationName;
        [SpineAnimation("", "", true, false)]
        public string runToIdleAnimationName;
        public float runWalkDuration = 1.5f;
        private SkeletonAnimation skeletonAnimation;
        public Spine.AnimationState spineAnimationState;
        public Skeleton skeleton;

        [DebuggerHidden]
        private IEnumerator DoDemoRoutine() => 
            new <DoDemoRoutine>c__Iterator0 { $this = this };

        private void Start()
        {
            this.skeletonAnimation = base.GetComponent<SkeletonAnimation>();
            this.spineAnimationState = this.skeletonAnimation.AnimationState;
            this.skeleton = this.skeletonAnimation.Skeleton;
            base.StartCoroutine(this.DoDemoRoutine());
        }

        [CompilerGenerated]
        private sealed class <DoDemoRoutine>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal SpineBeginnerTwo $this;
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
                    case 5:
                        this.$this.spineAnimationState.SetAnimation(0, this.$this.walkAnimationName, true);
                        this.$current = new WaitForSeconds(this.$this.runWalkDuration);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        break;

                    case 1:
                        this.$this.spineAnimationState.SetAnimation(0, this.$this.runAnimationName, true);
                        this.$current = new WaitForSeconds(this.$this.runWalkDuration);
                        if (!this.$disposing)
                        {
                            this.$PC = 2;
                        }
                        break;

                    case 2:
                        this.$this.spineAnimationState.SetAnimation(0, this.$this.runToIdleAnimationName, false);
                        this.$this.spineAnimationState.AddAnimation(0, this.$this.idleAnimationName, true, 0f);
                        this.$current = new WaitForSeconds(1f);
                        if (!this.$disposing)
                        {
                            this.$PC = 3;
                        }
                        break;

                    case 3:
                        this.$this.skeleton.FlipX = true;
                        this.$this.spineAnimationState.SetAnimation(0, this.$this.idleTurnAnimationName, false);
                        this.$this.spineAnimationState.AddAnimation(0, this.$this.idleAnimationName, true, 0f);
                        this.$current = new WaitForSeconds(0.5f);
                        if (!this.$disposing)
                        {
                            this.$PC = 4;
                        }
                        break;

                    case 4:
                        this.$this.skeleton.FlipX = false;
                        this.$this.spineAnimationState.SetAnimation(0, this.$this.idleTurnAnimationName, false);
                        this.$this.spineAnimationState.AddAnimation(0, this.$this.idleAnimationName, true, 0f);
                        this.$current = new WaitForSeconds(0.5f);
                        if (!this.$disposing)
                        {
                            this.$PC = 5;
                        }
                        break;

                    default:
                        return false;
                }
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

