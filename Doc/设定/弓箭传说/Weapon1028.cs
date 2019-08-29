using System;
using UnityEngine;

public class Weapon1028 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        int num = 12;
        float num2 = 360f / ((float) num);
        for (int i = 0; i < num; i++)
        {
            base.CreateBulletOverride(Vector3.zero, num2 * i);
        }
    }

    protected override void OnInstall()
    {
        base.OnInstall();
    }

    protected override void OnUnInstall()
    {
        base.OnUnInstall();
    }
}

