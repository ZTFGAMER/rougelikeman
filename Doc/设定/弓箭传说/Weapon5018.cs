using Dxx.Util;
using System;
using UnityEngine;

public class Weapon5018 : WeaponBase
{
    private float mAngle;

    protected override void OnAttack(object[] args)
    {
        int count = 9;
        for (int i = 0; i < count; i++)
        {
            base.CreateBulletOverride(Vector3.zero, Utils.GetBulletAngle(i, count, 150f));
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

