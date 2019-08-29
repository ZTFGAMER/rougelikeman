using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class JumpingZombie : ZombieHuman
{
    private bool readyToThrow = true;
    [SerializeField]
    private float jumpDelay = 1f;
    [SerializeField]
    private float jumpDistance = 10f;
    [SerializeField]
    private LayerMask maskJumpZombie;

    private void FixedUpdate()
    {
        base.FixedUpdate();
        if ((base.targetMove != null) && ((this.readyToThrow && (Vector3.Distance(base.transform.position, base.targetMove.position) < this.jumpDistance)) && (Vector3.Distance(base.transform.position, base.targetMove.position) > 3f)))
        {
            Vector3 direction = new Vector3(base.targetMove.position.x - base.transform.position.x, 0f, base.targetMove.position.z - base.transform.position.z);
            if (!Physics.Raycast(base.transform.position, direction, Vector3.Distance(base.transform.position, base.targetMove.position), (int) this.maskJumpZombie))
            {
                base.StartCoroutine(this.Jump());
                this.readyToThrow = false;
                base.Invoke("ReadyToThrow", this.jumpDelay);
            }
        }
    }

    [DebuggerHidden]
    private IEnumerator Jump() => 
        new <Jump>c__Iterator0 { $this = this };

    private void ReadyToThrow()
    {
        this.readyToThrow = true;
    }

    [CompilerGenerated]
    private sealed class <Jump>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal Vector3 <targetPosition>__0;
        internal float <maxHeight>__0;
        internal float <timeToMove>__0;
        internal Vector3 <currentPos>__0;
        internal float <t>__0;
        internal Vector3 <vv>__1;
        internal JumpingZombie $this;
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
                    this.<targetPosition>__0 = this.$this.targetMove.position;
                    this.<maxHeight>__0 = 2f;
                    this.<timeToMove>__0 = Vector3.Distance(this.$this.transform.position, this.<targetPosition>__0) / 15f;
                    this.<currentPos>__0 = this.$this.transform.position;
                    this.<t>__0 = 0f;
                    break;

                case 1:
                    break;

                default:
                    goto Label_01B1;
            }
            if (this.<t>__0 < 1.5f)
            {
                this.<t>__0 += Time.deltaTime / this.<timeToMove>__0;
                if (this.<t>__0 < 1f)
                {
                    this.<vv>__1 = Vector3.Lerp(this.<currentPos>__0, this.<targetPosition>__0, this.<t>__0);
                }
                else
                {
                    this.<vv>__1 = Vector3.Lerp(this.<targetPosition>__0, (this.<targetPosition>__0 - this.<currentPos>__0) + this.<targetPosition>__0, this.<t>__0 - 1f);
                    if (this.$this.transform.position.y <= 0f)
                    {
                        goto Label_01AA;
                    }
                }
                this.<vv>__1.y += Mathf.Sin(this.<t>__0 * 3.141593f) * this.<maxHeight>__0;
                this.$this.transform.position = this.<vv>__1;
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                return true;
            }
        Label_01AA:
            this.$PC = -1;
        Label_01B1:
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

