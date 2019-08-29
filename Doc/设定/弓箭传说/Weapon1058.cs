using System;
using UnityEngine;

public class Weapon1058 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        for (int i = 0; i < 3; i++)
        {
            base.CreateBulletOverride(Vector3.zero, (i * 45f) - 45f);
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }
}

