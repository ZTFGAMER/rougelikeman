using System;
using UnityEngine;

public class Weapon5003 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        for (int i = 0; i < 2; i++)
        {
            base.action.AddActionDelegate(delegate {
                GameLogic.Hold.Sound.PlayBulletCreate(0x1e9809, base.m_Entity.position);
                for (int j = 0; j < 3; j++)
                {
                    base.CreateBulletOverride(Vector3.zero, (float) ((j * 0x2d) - 0x2d));
                }
            });
            base.action.AddActionWait(0.3f);
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }
}

