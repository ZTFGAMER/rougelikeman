using System;
using UnityEngine;

public class Weapon1036 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        for (int i = 0; i < 3; i++)
        {
            base.CreateBulletOverride(Vector3.zero, 45f - (i * 45f));
        }
    }

    protected override void OnInit()
    {
        base.OnInit();
    }
}

