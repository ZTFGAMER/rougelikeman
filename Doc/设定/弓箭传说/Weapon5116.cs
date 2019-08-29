using Dxx.Util;
using System;
using UnityEngine;

public class Weapon5116 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        int count = 7;
        for (int i = 0; i < count; i++)
        {
            base.CreateBulletOverride(Vector3.zero, Utils.GetBulletAngle(i, count, 180f));
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }
}

