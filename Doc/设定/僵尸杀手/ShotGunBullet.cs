using System;
using UnityEngine;

public class ShotGunBullet : MonoBehaviour
{
    public int damage = 0x19;
    [SerializeField]
    private float spread = 25f;
    [SerializeField]
    private float lifetime = 3f;

    private void Start()
    {
        BaseBullet[] componentsInChildren = base.GetComponentsInChildren<BaseBullet>();
        float num = (this.spread * 2f) / ((float) componentsInChildren.Length);
        float num2 = -num * (componentsInChildren.Length / 2);
        foreach (BaseBullet bullet in componentsInChildren)
        {
            bullet.transform.Rotate((float) 0f, num2 + UnityEngine.Random.Range((float) -3f, (float) 3f), (float) 0f);
            num2 += num;
            bullet.gameObject.SetActive(true);
        }
        UnityEngine.Object.Destroy(base.gameObject, this.lifetime);
    }
}

