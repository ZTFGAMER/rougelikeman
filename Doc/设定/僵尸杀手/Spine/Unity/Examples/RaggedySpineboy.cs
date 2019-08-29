namespace Spine.Unity.Examples
{
    using Spine.Unity.Modules;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class RaggedySpineboy : MonoBehaviour
    {
        public LayerMask groundMask;
        public float restoreDuration = 0.5f;
        public Vector2 launchVelocity = new Vector2(50f, 100f);
        private SkeletonRagdoll2D ragdoll;
        private Collider2D naturalCollider;

        private void AddRigidbody()
        {
            base.gameObject.AddComponent<Rigidbody2D>().freezeRotation = true;
            this.naturalCollider.enabled = true;
        }

        private void Launch()
        {
            this.RemoveRigidbody();
            this.ragdoll.Apply();
            float x = UnityEngine.Random.Range(-this.launchVelocity.x, this.launchVelocity.x);
            this.ragdoll.RootRigidbody.velocity = new Vector2(x, this.launchVelocity.y);
            base.StartCoroutine(this.WaitUntilStopped());
        }

        private void OnMouseUp()
        {
            if (this.naturalCollider.enabled)
            {
                this.Launch();
            }
        }

        private void RemoveRigidbody()
        {
            UnityEngine.Object.Destroy(base.GetComponent<Rigidbody2D>());
            this.naturalCollider.enabled = false;
        }

        [DebuggerHidden]
        private IEnumerator Restore() => 
            new <Restore>c__Iterator0 { $this = this };

        private void Start()
        {
            this.ragdoll = base.GetComponent<SkeletonRagdoll2D>();
            this.naturalCollider = base.GetComponent<Collider2D>();
        }

        [DebuggerHidden]
        private IEnumerator WaitUntilStopped() => 
            new <WaitUntilStopped>c__Iterator1 { $this = this };

        [CompilerGenerated]
        private sealed class <Restore>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal Vector3 <estimatedPos>__0;
            internal Vector3 <rbPosition>__0;
            internal Vector3 <skeletonPoint>__0;
            internal RaycastHit2D <hit>__0;
            internal RaggedySpineboy $this;
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
                        this.<estimatedPos>__0 = this.$this.ragdoll.EstimatedSkeletonPosition;
                        this.<rbPosition>__0 = (Vector3) this.$this.ragdoll.RootRigidbody.position;
                        this.<skeletonPoint>__0 = this.<estimatedPos>__0;
                        this.<hit>__0 = Physics2D.Raycast(this.<rbPosition>__0, this.<estimatedPos>__0 - this.<rbPosition>__0, Vector3.Distance(this.<estimatedPos>__0, this.<rbPosition>__0), (int) this.$this.groundMask);
                        if (this.<hit>__0.collider != null)
                        {
                            this.<skeletonPoint>__0 = (Vector3) this.<hit>__0.point;
                        }
                        this.$this.ragdoll.RootRigidbody.isKinematic = true;
                        this.$this.ragdoll.SetSkeletonPosition(this.<skeletonPoint>__0);
                        this.$current = this.$this.ragdoll.SmoothMix(0f, this.$this.restoreDuration);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        this.$this.ragdoll.Remove();
                        this.$this.AddRigidbody();
                        this.$PC = -1;
                        break;
                }
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

        [CompilerGenerated]
        private sealed class <WaitUntilStopped>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal float <t>__0;
            internal RaggedySpineboy $this;
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
                        this.$current = new WaitForSeconds(0.5f);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        goto Label_00E7;

                    case 1:
                        this.<t>__0 = 0f;
                        break;

                    case 2:
                        break;

                    default:
                        goto Label_00E5;
                }
                if (this.<t>__0 < 0.5f)
                {
                    this.<t>__0 = (this.$this.ragdoll.RootRigidbody.velocity.magnitude <= 0.09f) ? (this.<t>__0 + Time.deltaTime) : 0f;
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 2;
                    }
                    goto Label_00E7;
                }
                this.$this.StartCoroutine(this.$this.Restore());
                this.$PC = -1;
            Label_00E5:
                return false;
            Label_00E7:
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

