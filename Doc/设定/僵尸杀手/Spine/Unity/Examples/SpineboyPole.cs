namespace Spine.Unity.Examples
{
    using Spine;
    using Spine.Unity;
    using Spine.Unity.Modules;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class SpineboyPole : MonoBehaviour
    {
        public SkeletonAnimation skeletonAnimation;
        public SkeletonRenderSeparator separator;
        [Space(18f)]
        public AnimationReferenceAsset run;
        public AnimationReferenceAsset pole;
        public float startX;
        public float endX;
        private const float Speed = 18f;
        private const float RunTimeScale = 1.5f;

        private void SetXPosition(float x)
        {
            Vector3 localPosition = base.transform.localPosition;
            localPosition.x = x;
            base.transform.localPosition = localPosition;
        }

        [DebuggerHidden]
        private IEnumerator Start() => 
            new <Start>c__Iterator0 { $this = this };

        [CompilerGenerated]
        private sealed class <Start>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal Spine.AnimationState <state>__0;
            internal TrackEntry <poleTrack>__1;
            internal SpineboyPole $this;
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
                        this.<state>__0 = this.$this.skeletonAnimation.state;
                        break;

                    case 1:
                        goto Label_00DD;

                    case 2:
                        this.$current = new WaitForSeconds(1f);
                        if (!this.$disposing)
                        {
                            this.$PC = 3;
                        }
                        goto Label_01A6;

                    case 3:
                        break;

                    default:
                        return false;
                }
                this.$this.SetXPosition(this.$this.startX);
                this.$this.separator.enabled = false;
                this.<state>__0.SetAnimation(0, (Spine.Animation) this.$this.run, true);
                this.<state>__0.TimeScale = 1.5f;
            Label_00DD:
                while (this.$this.transform.localPosition.x < this.$this.endX)
                {
                    this.$this.transform.Translate((Vector3.right * 18f) * Time.deltaTime);
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    goto Label_01A6;
                }
                this.$this.SetXPosition(this.$this.endX);
                this.$this.separator.enabled = true;
                this.<poleTrack>__1 = this.<state>__0.SetAnimation(0, (Spine.Animation) this.$this.pole, false);
                this.$current = new WaitForSpineAnimationComplete(this.<poleTrack>__1);
                if (!this.$disposing)
                {
                    this.$PC = 2;
                }
            Label_01A6:
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

