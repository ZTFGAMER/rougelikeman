using System;
using UnityEngine;

public class Weapon5027 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        for (int i = 0; i < 3; i++)
        {
            base.action.AddActionDelegate(delegate {
                GameLogic.Hold.Sound.PlayBulletCreate(0x1e903c, base.m_Entity.position);
                int num = 7;
                float num2 = 180f / ((float) (num - 1));
                for (int j = 0; j < num; j++)
                {
                    base.CreateBulletOverride(Vector3.zero, (j * num2) - 90f);
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

