namespace Spine.Unity.Examples
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using UnityEngine;

    [SelectionBase]
    public class SpineboyBeginnerModel : MonoBehaviour
    {
        [Header("Current State")]
        public SpineBeginnerBodyState state;
        public bool facingLeft;
        [Range(-1f, 1f)]
        public float currentSpeed;
        [Header("Balance")]
        public float shootInterval = 0.12f;
        private float lastShootTime;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event Action ShootEvent;

        [DebuggerHidden]
        private IEnumerator JumpRoutine() => 
            new <JumpRoutine>c__Iterator0 { $this = this };

        public void TryJump()
        {
            base.StartCoroutine(this.JumpRoutine());
        }

        public void TryMove(float speed)
        {
            this.currentSpeed = speed;
            if (speed != 0f)
            {
                bool flag = speed < 0f;
                this.facingLeft = flag;
            }
            if (this.state != SpineBeginnerBodyState.Jumping)
            {
                this.state = (speed != 0f) ? SpineBeginnerBodyState.Running : SpineBeginnerBodyState.Idle;
            }
        }

        public void TryShoot()
        {
            float time = Time.time;
            if ((time - this.lastShootTime) > this.shootInterval)
            {
                this.lastShootTime = time;
                if (this.ShootEvent != null)
                {
                    this.ShootEvent();
                }
            }
        }

        [CompilerGenerated]
        private sealed class <JumpRoutine>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal Vector3 <pos>__1;
            internal float <t>__2;
            internal float <d>__3;
            internal float <t>__4;
            internal float <d>__5;
            internal SpineboyBeginnerModel $this;
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
                        if (this.$this.state != SpineBeginnerBodyState.Jumping)
                        {
                            this.$this.state = SpineBeginnerBodyState.Jumping;
                            this.<pos>__1 = this.$this.transform.localPosition;
                            this.<t>__2 = 0f;
                            while (this.<t>__2 < 0.6f)
                            {
                                this.<d>__3 = 20f * (0.6f - this.<t>__2);
                                this.$this.transform.Translate((Vector3) ((this.<d>__3 * Time.deltaTime) * Vector3.up));
                                this.$current = null;
                                if (!this.$disposing)
                                {
                                    this.$PC = 1;
                                }
                                goto Label_0198;
                            Label_00C6:
                                this.<t>__2 += Time.deltaTime;
                            }
                            this.<t>__4 = 0f;
                            while (this.<t>__4 < 0.6f)
                            {
                                this.<d>__5 = 20f * this.<t>__4;
                                this.$this.transform.Translate((Vector3) ((this.<d>__5 * Time.deltaTime) * Vector3.down));
                                this.$current = null;
                                if (!this.$disposing)
                                {
                                    this.$PC = 2;
                                }
                                goto Label_0198;
                            Label_014B:
                                this.<t>__4 += Time.deltaTime;
                            }
                            this.$this.transform.localPosition = this.<pos>__1;
                            this.$this.state = SpineBeginnerBodyState.Idle;
                            this.$PC = -1;
                            break;
                        }
                        break;

                    case 1:
                        goto Label_00C6;

                    case 2:
                        goto Label_014B;
                }
                return false;
            Label_0198:
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

