using System;
using UnityEngine;

public class Weapon1033 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        for (int i = 0; i < 3; i++)
        {
            base.CreateBulletOverride(Vector3.zero, (i * 30f) - 30f);
        }
    }
}

