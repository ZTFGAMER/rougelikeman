using Dxx.Util;
using System;
using UnityEngine;

public class Weapon1101 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        float num = GameLogic.Random((float) -20f, (float) 20f);
        int count = 3;
        for (int i = 0; i < count; i++)
        {
            base.CreateBulletOverride(Vector3.zero, Utils.GetBulletAngle(i, count, 90f) + num);
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }
}

