using Dxx.Util;
using System;
using UnityEngine;

public class Weapon5071 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        int count = 5;
        for (int i = 0; i < count; i++)
        {
            base.CreateBulletOverride(Vector3.zero, Utils.GetBulletAngle(i, count, 150f));
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }
}

