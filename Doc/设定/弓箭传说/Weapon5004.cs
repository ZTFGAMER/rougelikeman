using System;
using UnityEngine;

public class Weapon5004 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        for (int i = 0; i < 5; i++)
        {
            base.action.AddActionDelegate(delegate {
                GameLogic.Hold.Sound.PlayBulletCreate(0x1e9809, base.m_Entity.position);
                for (int j = 0; j < 1; j++)
                {
                    base.CreateBulletOverride(Vector3.zero, 0f);
                }
            });
            base.action.AddActionWait(0.2f);
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }
}

