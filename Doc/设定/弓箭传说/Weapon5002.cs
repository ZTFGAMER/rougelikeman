using System;
using UnityEngine;

public class Weapon5002 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        GameLogic.Hold.Sound.PlayBulletCreate(0x1e9809, base.m_Entity.position);
        for (int i = 0; i < 5; i++)
        {
            base.CreateBulletOverride(Vector3.zero, (float) ((i * 30) - 60));
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }
}

