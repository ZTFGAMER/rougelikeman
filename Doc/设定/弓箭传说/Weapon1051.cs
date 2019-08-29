using System;
using UnityEngine;

public class Weapon1051 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        int num = 6;
        for (int i = 0; i < num; i++)
        {
            base.CreateBulletOverride(Vector3.zero, i * 60f);
        }
    }
}

