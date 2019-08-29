using System;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    private Rigidbody rigid;
    [SerializeField]
    private float speed = 20f;
    public int damage = 10;
    [SerializeField]
    private GameObject bulletObject;
    [SerializeField]
    private GameObject hitFx;

    private void OnCollisionEnter(Collision collision)
    {
        if (this.rigid.velocity != Vector3.zero)
        {
            this.rigid.velocity = Vector3.zero;
            base.transform.position = collision.contacts[0].point;
            if ((collision.gameObject.tag == "Zombie") || (collision.gameObject.tag == "ZombieBoss"))
            {
                collision.gameObject.GetComponent<BaseHuman>().TakeDamage(this.damage);
            }
            this.bulletObject.SetActive(false);
            this.hitFx.SetActive(true);
            UnityEngine.Object.Destroy(base.gameObject, 1f);
        }
    }

    protected virtual void Start()
    {
        this.rigid = base.GetComponent<Rigidbody>();
        this.rigid.velocity = new Vector3(base.transform.right.x, 0f, base.transform.right.z) * this.speed;
        UnityEngine.Object.Destroy(base.gameObject, 3f);
    }
}

