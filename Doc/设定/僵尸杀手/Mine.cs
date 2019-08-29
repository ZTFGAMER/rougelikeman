using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public int damage = 20;
    [SerializeField]
    private int bangSize = 4;
    [SerializeField]
    protected float bangTimeout = 2f;
    [SerializeField]
    private GameObject mineObject;
    [SerializeField]
    private GameObject bangFx;
    [SerializeField]
    private AudioClip explosionSound;
    protected Rigidbody rigid;
    private bool bangIs;

    private void Awake()
    {
        this.rigid = base.GetComponent<Rigidbody>();
        base.Invoke("BangNow", this.bangTimeout);
    }

    [DebuggerHidden]
    private IEnumerator Bang() => 
        new <Bang>c__Iterator0 { $this = this };

    private void BangNow()
    {
        base.StartCoroutine(this.Bang());
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        this.rigid.velocity = Vector3.zero;
        if ((collision.gameObject.tag == "Zombie") || (collision.gameObject.tag == "ZombieBoss"))
        {
            collision.gameObject.GetComponent<BaseHuman>().TakeDamage(this.damage);
        }
        if (!this.bangIs && (collision.gameObject.tag != "Ground"))
        {
            base.StartCoroutine(this.Bang());
        }
    }

    private void Start()
    {
        this.rigid.AddForce(new Vector3(base.transform.right.x, 0f, base.transform.right.z) * 70f);
    }

    [CompilerGenerated]
    private sealed class <Bang>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal SphereCollider <sphereCollider>__0;
        internal int <i>__1;
        internal Mine $this;
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
                    this.$this.CancelInvoke("BangNow");
                    this.$this.bangIs = true;
                    this.$this.mineObject.SetActive(false);
                    this.$this.bangFx.SetActive(true);
                    this.<sphereCollider>__0 = this.$this.GetComponent<SphereCollider>();
                    this.<i>__1 = 0;
                    break;

                case 1:
                    this.<i>__1++;
                    break;

                default:
                    goto Label_0114;
            }
            if (this.<i>__1 < this.$this.bangSize)
            {
                this.<sphereCollider>__0.radius++;
                this.$current = null;
                if (!this.$disposing)
                {
                    this.$PC = 1;
                }
                return true;
            }
            this.<sphereCollider>__0.enabled = false;
            SoundManager.Instance.PlaySound(this.$this.explosionSound, -1f);
            UnityEngine.Object.Destroy(this.$this.gameObject, 1f);
            this.$PC = -1;
        Label_0114:
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

