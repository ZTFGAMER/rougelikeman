using System;
using UnityEngine;

public class Weapon5087 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        float max = 1f;
        float num2 = 90f;
        GameLogic.Hold.Sound.PlayBulletCreate(0x1e903c, base.m_Entity.position);
        for (int i = 0; i < 20; i++)
        {
            base.CreateBulletOverride(new Vector3(GameLogic.Random(-max, max), 0f, GameLogic.Random(-max, max)), GameLogic.Random(-num2, num2));
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }
}

