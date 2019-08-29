using System;
using UnityEngine;

public class Weapon5009 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        int num = 7;
        float num2 = 180f / ((float) num);
        float num3 = (num / 2) * num2;
        for (int i = 0; i < num; i++)
        {
            base.CreateBulletOverride(Vector3.zero, (i * num2) - num3);
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }
}

