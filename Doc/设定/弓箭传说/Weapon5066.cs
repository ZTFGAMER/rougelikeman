using System;
using UnityEngine;

public class Weapon5066 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        GameLogic.Hold.Sound.PlayBulletCreate(0x1e9809, base.m_Entity.position);
        int num = 4;
        float num2 = 60f;
        float num3 = num2 / ((float) (num - 1));
        float num4 = (num3 * (num - 1)) / 2f;
        for (int i = 0; i < 5; i++)
        {
            base.CreateBulletOverride(Vector3.zero, (i * num3) - num4);
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }
}

