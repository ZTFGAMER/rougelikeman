using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ClownZombie : ZombieHuman
{
    private bool readyToThrow = true;
    [SerializeField]
    private float throwDelay = 1f;
    [SerializeField]
    private float throwDistance = 10f;
    [SerializeField]
    private LayerMask maskThrowZombie;
    private TrailRenderer zombieTrail;
    private ParticleSystem flyEndFx;

    private void DisableZombieTrail()
    {
        this.zombieTrail.enabled = false;
    }

    private void OnDestroy()
    {
        if (this.zombieTrail != null)
        {
            UnityEngine.Object.Destroy(this.zombieTrail, 1f);
        }
        if (this.flyEndFx != null)
        {
            UnityEngine.Object.Destroy(this.flyEndFx, 1f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (((other != null) && (base.targetMove != null)) && ((this.readyToThrow && (other.tag == "Zombie")) && ((Vector3.Distance(base.transform.position, base.targetMove.position) < this.throwDistance) && (Vector3.Distance(base.transform.position, base.targetMove.position) > 3f))))
        {
            Vector3 direction = new Vector3(base.targetMove.position.x - other.transform.position.x, 0f, base.targetMove.position.z - other.transform.position.z);
            if ((other.GetComponent<ZombieHuman>().size != ZombieHuman.ZombieSize.Big) && !Physics.Raycast(other.transform.position, direction, Vector3.Distance(other.transform.position, base.targetMove.position), (int) this.maskThrowZombie))
            {
                base.StartCoroutine(this.ThrowZombie(other.transform, base.targetMove.position));
                this.readyToThrow = false;
                base.Invoke("ReadyToThrow", this.throwDelay);
            }
        }
    }

    private void ReadyToThrow()
    {
        this.readyToThrow = true;
    }

    [DebuggerHidden]
    private IEnumerator ThrowZombie(Transform zombie, Vector3 targetPosition) => 
        new <ThrowZombie>c__Iterator0 { 
            zombie = zombie,
            targetPosition = targetPosition,
            $this = this
        };

    [CompilerGenerated]
    private sealed class <ThrowZombie>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal float <maxHeight>__0;
        internal Transform zombie;
        internal Vector3 targetPosition;
        internal float <timeToMove>__0;
        internal Vector3 <currentPos>__0;
        internal float <t>__0;
        internal Vector3 <vv>__1;
        internal ClownZombie $this;
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
                    if (this.$this.zombieTrail == null)
                    {
                        this.$this.zombieTrail = UnityEngine.Object.Instantiate<TrailRenderer>(GameManager.instance.prefabZombieTrail);
                    }
                    this.<maxHeight>__0 = 5f;
                    this.<timeToMove>__0 = Vector3.Distance(this.zombie.position, this.targetPosition) / 10f;
                    this.<currentPos>__0 = this.zombie.position;
                    this.<t>__0 = 0f;
                    this.$this.CancelInvoke("DisableZombieTrail");
                    break;

                case 1:
                    break;

                default:
                    goto Label_02CD;
            }
            while ((this.<t>__0 < 1.5f) && (this.zombie != null))
            {
                this.<t>__0 += Time.deltaTime / this.<timeToMove>__0;
                if (this.<t>__0 < 1f)
                {
                    this.<vv>__1 = Vector3.Lerp(this.<currentPos>__0, this.targetPosition, this.<t>__0);
                }
                else
                {
                    this.<vv>__1 = Vector3.Lerp(this.targetPosition, (this.targetPosition - this.<currentPos>__0) + this.targetPosition, this.<t>__0 - 1f);
                    if (this.zombie.position.y <= 0.1f)
                    {
                        break;
                    }
                }
                this.<vv>__1.y += Mathf.Sin(this.<t>__0 * 3.141593f) * this.<maxHeight>__0;
                this.zombie.position = this.<vv>__1;
                this.$this.zombieTrail.transform.position = this.<vv>__1;
                this.$this.zombieTrail.enabled = true;
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                return true;
            }
            if (this.zombie != null)
            {
                if (this.$this.flyEndFx == null)
                {
                    this.$this.flyEndFx = UnityEngine.Object.Instantiate<GameObject>(GameManager.instance.prefabFlyEndFx).GetComponent<ParticleSystem>();
                }
                this.$this.flyEndFx.transform.position = new Vector3(this.zombie.position.x, this.$this.flyEndFx.transform.position.y, this.zombie.position.z);
                this.$this.flyEndFx.Play();
            }
            this.$this.Invoke("DisableZombieTrail", 0.5f);
            this.$PC = -1;
        Label_02CD:
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

