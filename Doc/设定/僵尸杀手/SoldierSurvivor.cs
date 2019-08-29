using System;
using UnityEngine;

public class SoldierSurvivor : SurvivorHuman
{
    [SerializeField]
    protected ParticleSystem bulletsSpawn;
    [SerializeField]
    protected GameObject bulletPrefab;
    [SerializeField]
    private float aimDistance = 10f;
    [SerializeField]
    private LayerMask maskAimSurvivors;
    private float reload;
    private float refindTargetTime = 1.5f;
    private float lastFindTime;
    private Transform targetZombie;

    protected virtual void AddBullet()
    {
        this.SetBulletDamage(UnityEngine.Object.Instantiate<GameObject>(this.bulletPrefab, this.bulletsSpawn.transform.position, this.bulletsSpawn.transform.rotation));
        this.bulletsSpawn.Play();
        SoundManager.Instance.PlaySound(base.shotSounds[UnityEngine.Random.Range(0, base.shotSounds.Length)], -1f);
        if ((base.animator != null) && base.shootExists)
        {
            base.animator.SetBool("Shoot", true);
        }
    }

    private void FindTargetShoot()
    {
        Vector3 vector;
        if ((this.targetZombie != null) && ((Time.time - this.lastFindTime) < this.refindTargetTime))
        {
            vector = new Vector3(this.targetZombie.position.x - base.body.position.x, 1f, this.targetZombie.position.z - base.body.position.z);
            if (Physics.Raycast(base.body.position, vector, out RaycastHit hit, this.aimDistance, (int) this.maskAimSurvivors) && ((hit.transform.tag == "Zombie") || (hit.transform.tag == "ZombieBoss")))
            {
                return;
            }
        }
        this.targetZombie = null;
        int layerMask = ((int) 1) << LayerMask.NameToLayer("Zombie");
        foreach (Collider collider in Physics.OverlapSphere(base.transform.position, this.aimDistance, layerMask))
        {
            vector = new Vector3(collider.transform.position.x - base.body.position.x, 1f, collider.transform.position.z - base.body.position.z);
            if ((Physics.Raycast(base.body.position, vector, out hit, this.aimDistance, (int) this.maskAimSurvivors) && ((hit.transform.tag == "Zombie") || (hit.transform.tag == "ZombieBoss"))) && ((this.targetZombie == null) || (Vector3.Distance(base.transform.position, hit.transform.position) < Vector3.Distance(base.transform.position, this.targetZombie.position))))
            {
                this.targetZombie = hit.transform;
            }
        }
        this.lastFindTime = Time.time;
    }

    public void SetBulletDamage(GameObject bullet)
    {
        BaseBullet component = bullet.GetComponent<BaseBullet>();
        if (component != null)
        {
            component.damage = (int) base.heroDamage;
        }
        else if (bullet.GetComponent<Mine>() != null)
        {
            bullet.GetComponent<Mine>().damage = (int) base.heroDamage;
        }
        else
        {
            bullet.GetComponent<ShotGunBullet>().damage = (int) base.heroDamage;
        }
    }

    private void Update()
    {
        if (this.reload <= 0f)
        {
            this.FindTargetShoot();
            if (this.targetZombie != null)
            {
                base.targetRotation = this.targetZombie;
            }
            base.Update();
            if (this.targetZombie != null)
            {
                base.CancelInvoke("RotateForward");
                if (base.isLookAtTarget)
                {
                    this.reload = base.shootDelay;
                    if (base.heroType == SaveData.HeroData.HeroType.MINER)
                    {
                        base.Invoke("AddBullet", 0.3f);
                        base.animator.SetTrigger("Throw");
                    }
                    else
                    {
                        this.AddBullet();
                    }
                }
            }
            base.Invoke("RotateForward", Mathf.Max((float) 1f, (float) (base.shootDelay + 0.3f)));
        }
        else
        {
            this.reload -= Time.deltaTime;
            base.Update();
        }
    }
}

