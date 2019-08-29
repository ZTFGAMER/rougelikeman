using Dxx.Util;
using System;
using UnityEngine;

public class Weapon5088 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        float num = GameLogic.Random((float) -20f, (float) 20f);
        int count = 5;
        for (int i = 0; i < count; i++)
        {
            base.CreateBulletOverride(Vector3.zero, Utils.GetBulletAngle(i, count, 120f) + num);
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }
}

