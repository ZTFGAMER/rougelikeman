using System;
using UnityEngine;

public class Weapon1088 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        float num = GameLogic.Random((float) 0f, (float) 360f);
        for (int i = 0; i < 8; i++)
        {
            base.CreateBulletOverride(Vector3.zero, (i * 0x2d) + num);
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }
}

