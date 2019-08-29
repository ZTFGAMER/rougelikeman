using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DropPlace : MonoBehaviour
{
    private int countEntries;
    [SerializeField]
    private GameObject prefabBomb;
    [SerializeField]
    private int bangSize = 5;
    [SerializeField]
    private int bombStartHeight = 50;
    [SerializeField]
    private AudioClip explosionSound;
    [SerializeField]
    private GameObject spriteObject;
    private SphereCollider sphereCollider;
    private ParticleSystem bangFx;
    private float tryTime = 5f;

    [DebuggerHidden]
    private IEnumerator Bang() => 
        new <Bang>c__Iterator1 { $this = this };

    [DebuggerHidden]
    private IEnumerator CheckPlace() => 
        new <CheckPlace>c__Iterator0 { $this = this };

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.tag == "Zombie") || (collision.gameObject.tag == "ZombieBoss"))
        {
            BaseHuman component = collision.gameObject.GetComponent<BaseHuman>();
            float num = Vector3.Distance(base.transform.position, collision.transform.position);
            float num2 = (((float) component.maxCountHealth) / num) * 4f;
            if (component.TakeDamage((int) num2) < 0)
            {
                DataLoader.Instance.SaveDeadByCapsule(collision.gameObject.GetComponent<ZombieHuman>().zombieType);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            this.countEntries++;
        }
        if ((other.tag == "Cage") && this.spriteObject.activeSelf)
        {
            UnityEngine.Object.Destroy(other.gameObject);
            base.StartCoroutine(this.Bang());
            this.spriteObject.SetActive(false);
            this.bangFx.Play();
            if ((this.explosionSound != null) && !SoundManager.Instance.soundIsMuted)
            {
                AudioSource.PlayClipAtPoint(this.explosionSound, base.transform.position);
            }
            UnityEngine.Object.Destroy(base.gameObject, 1f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            this.countEntries--;
        }
    }

    private void ReadyToCheck()
    {
        base.StartCoroutine(this.CheckPlace());
    }

    private void Start()
    {
        this.sphereCollider = base.GetComponent<SphereCollider>();
        this.bangFx = base.GetComponentInChildren<ParticleSystem>();
        base.Invoke("ReadyToCheck", 0.5f);
    }

    [CompilerGenerated]
    private sealed class <Bang>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal int <i>__1;
        internal DropPlace $this;
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
                    this.$this.gameObject.layer = LayerMask.NameToLayer("DropPlace");
                    this.$this.sphereCollider.isTrigger = false;
                    this.<i>__1 = 0;
                    break;

                case 1:
                    this.<i>__1++;
                    break;

                default:
                    goto Label_00CB;
            }
            if (this.<i>__1 < this.$this.bangSize)
            {
                this.$this.sphereCollider.radius++;
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                return true;
            }
            this.$this.sphereCollider.enabled = false;
            this.$PC = -1;
        Label_00CB:
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
    private sealed class <CheckPlace>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal float <startTry>__0;
        internal DropPlace $this;
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
                    this.<startTry>__0 = Time.time;
                    break;

                case 1:
                    break;

                default:
                    goto Label_0196;
            }
            if (this.$this.countEntries > 0)
            {
                this.$this.transform.position = new Vector3(this.$this.transform.position.x + UnityEngine.Random.Range(-1, 2), this.$this.transform.position.y, this.$this.transform.position.z + UnityEngine.Random.Range(-1, 2));
                if (Time.time <= (this.<startTry>__0 + this.$this.tryTime))
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;
                }
                UnityEngine.Object.Destroy(this.$this.gameObject);
            }
            else
            {
                UnityEngine.Object.Instantiate<GameObject>(this.$this.prefabBomb, new Vector3(this.$this.transform.position.x, (float) this.$this.bombStartHeight, this.$this.transform.position.z), this.$this.prefabBomb.transform.rotation).GetComponent<Rigidbody>().AddForce(0f, -5000f, 0f);
                this.$this.spriteObject.SetActive(true);
                this.$PC = -1;
            }
        Label_0196:
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

