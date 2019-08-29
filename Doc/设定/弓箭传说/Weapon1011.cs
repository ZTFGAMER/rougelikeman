using System;
using UnityEngine;

public class Weapon1011 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        for (int i = 0; i < 8; i++)
        {
            base.CreateBulletOverride(Vector3.zero, 45f * i);
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }
}

