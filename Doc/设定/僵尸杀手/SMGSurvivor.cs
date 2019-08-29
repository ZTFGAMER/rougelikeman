using System;
using UnityEngine;

public class SMGSurvivor : SoldierSurvivor
{
    [SerializeField]
    private float spread = 7f;
    [SerializeField]
    private float spreadSpeed = 2f;
    private float currentSpreadDirection;

    protected override void AddBullet()
    {
        GameObject bullet = UnityEngine.Object.Instantiate<GameObject>(base.bulletPrefab, base.bulletsSpawn.transform.position, base.bulletsSpawn.transform.rotation);
        base.SetBulletDamage(bullet);
        if ((this.currentSpreadDirection >= this.spread) || (this.currentSpreadDirection <= -this.spread))
        {
            this.spreadSpeed *= -1f;
        }
        this.currentSpreadDirection += this.spreadSpeed;
        bullet.transform.Rotate((float) 0f, this.currentSpreadDirection, (float) 0f);
        base.bulletsSpawn.Play();
        SoundManager.Instance.PlaySound(base.shotSounds[UnityEngine.Random.Range(0, base.shotSounds.Length)], -1f);
        if ((base.animator != null) && base.shootExists)
        {
            base.animator.SetBool("Shoot", true);
        }
    }
}

