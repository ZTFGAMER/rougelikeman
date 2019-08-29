using System;
using UnityEngine;

public class Weapon5022 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        for (int i = 0; i < 5; i++)
        {
            base.action.AddActionDelegate(delegate {
                GameLogic.Hold.Sound.PlayBulletCreate(0x1e9809, base.m_Entity.position);
                for (int j = 0; j < 2; j++)
                {
                    base.CreateBulletOverride(new Vector3(1f, 0f, 0f) * (1f - (j * 2f)), 0f);
                }
            });
            base.action.AddActionWait(0.12f);
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }
}

