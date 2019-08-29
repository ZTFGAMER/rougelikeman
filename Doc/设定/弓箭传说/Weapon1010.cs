using System;
using UnityEngine;

public class Weapon1010 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        for (int i = 0; i < 5; i++)
        {
            base.CreateBulletOverride(Vector3.zero, (i * 30f) - 60f);
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }
}

