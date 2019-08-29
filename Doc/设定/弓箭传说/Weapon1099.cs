using Dxx.Util;
using System;
using UnityEngine;

public class Weapon1099 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        int count = 4;
        for (int i = 0; i < count; i++)
        {
            float angle = Utils.GetBulletAngle(i, count, 120f);
            float x = MathDxx.Sin(angle);
            float z = MathDxx.Cos(angle);
            base.CreateBulletOverride(new Vector3(x, 0f, z) * 2f, angle);
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }
}

