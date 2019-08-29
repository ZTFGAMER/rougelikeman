using System;
using UnityEngine;

public class Weapon1053 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        for (int i = 0; i < 2; i++)
        {
            base.CreateBulletOverride(Vector3.zero, i * 180f);
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }
}

