using System;
using UnityEngine;

public class Weapon5101 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        for (int i = 0; i < 2; i++)
        {
            base.CreateBulletOverride(Vector3.zero, (i * 60f) - 30f);
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }
}

