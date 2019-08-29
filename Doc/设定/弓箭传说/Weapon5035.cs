using System;
using UnityEngine;

public class Weapon5035 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        for (int i = 0; i < 8; i++)
        {
            base.CreateBulletOverride(Vector3.zero, i * 45f);
        }
    }

    protected override void OnInit()
    {
        base.OnInit();
    }
}

