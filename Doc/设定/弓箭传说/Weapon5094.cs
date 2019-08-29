using Dxx.Util;
using System;
using UnityEngine;

public class Weapon5094 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        int count = 2;
        for (int i = 0; i < count; i++)
        {
            base.CreateBulletOverride(Vector3.zero, Utils.GetBulletAngle(i, count, 120f));
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }
}

