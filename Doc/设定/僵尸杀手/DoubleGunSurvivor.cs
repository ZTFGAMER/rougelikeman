using System;
using UnityEngine;

public class DoubleGunSurvivor : SoldierSurvivor
{
    [SerializeField]
    private ParticleSystem bulletsSpawn2;
    private bool firstGunNow = true;
    private ParticleSystem[] bulletsSpawns;

    protected override void AddBullet()
    {
        base.bulletsSpawn = this.bulletsSpawns[Convert.ToInt16(this.firstGunNow)];
        this.firstGunNow = !this.firstGunNow;
        base.AddBullet();
    }

    private void Start()
    {
        base.Start();
        this.bulletsSpawns = new ParticleSystem[] { base.bulletsSpawn, this.bulletsSpawn2 };
    }
}

