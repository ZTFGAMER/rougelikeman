namespace Spine.Unity.Examples
{
    using Spine.Unity;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class FootSoldierExample : MonoBehaviour
    {
        [SpineAnimation("Idle", "", true, false)]
        public string idleAnimation;
        [SpineAnimation("", "", true, false)]
        public string attackAnimation;
        [SpineAnimation("", "", true, false)]
        public string moveAnimation;
        [SpineSlot("", "", false, true, false)]
        public string eyesSlot;
        [SpineAttachment(true, false, false, "eyesSlot", "", "", true, false)]
        public string eyesOpenAttachment;
        [SpineAttachment(true, false, false, "eyesSlot", "", "", true, false)]
        public string blinkAttachment;
        [Range(0f, 0.2f)]
        public float blinkDuration = 0.05f;
        public KeyCode attackKey = KeyCode.Mouse0;
        public KeyCode rightKey = KeyCode.D;
        public KeyCode leftKey = KeyCode.A;
        public float moveSpeed = 3f;
        private SkeletonAnimation skeletonAnimation;

        private void Apply(SkeletonRenderer skeletonRenderer)
        {
            base.StartCoroutine("Blink");
        }

        private void Awake()
        {
            this.skeletonAnimation = base.GetComponent<SkeletonAnimation>();
            this.skeletonAnimation.OnRebuild += new SkeletonRenderer.SkeletonRendererDelegate(this.Apply);
        }

        [DebuggerHidden]
        private IEnumerator Blink() => 
            new <Blink>c__Iterator0 { $this = this };

        private void Update()
        {
            if (Input.GetKey(this.attackKey))
            {
                this.skeletonAnimation.AnimationName = this.attackAnimation;
            }
            else if (Input.GetKey(this.rightKey))
            {
                this.skeletonAnimation.AnimationName = this.moveAnimation;
                this.skeletonAnimation.Skeleton.FlipX = false;
                base.transform.Translate(this.moveSpeed * Time.deltaTime, 0f, 0f);
            }
            else if (Input.GetKey(this.leftKey))
            {
                this.skeletonAnimation.AnimationName = this.moveAnimation;
                this.skeletonAnimation.Skeleton.FlipX = true;
                base.transform.Translate(-this.moveSpeed * Time.deltaTime, 0f, 0f);
            }
            else
            {
                this.skeletonAnimation.AnimationName = this.idleAnimation;
            }
        }

        [CompilerGenerated]
        private sealed class <Blink>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal FootSoldierExample $this;
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
                        break;

                    case 1:
                        this.$this.skeletonAnimation.Skeleton.SetAttachment(this.$this.eyesSlot, this.$this.blinkAttachment);
                        this.$current = new WaitForSeconds(this.$this.blinkDuration);
                        if (!this.$disposing)
                        {
                            this.$PC = 2;
                        }
                        goto Label_00E1;

                    case 2:
                        this.$this.skeletonAnimation.Skeleton.SetAttachment(this.$this.eyesSlot, this.$this.eyesOpenAttachment);
                        break;

                    default:
                        return false;
                }
                this.$current = new WaitForSeconds(UnityEngine.Random.Range((float) 0.25f, (float) 3f));
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
            Label_00E1:
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

