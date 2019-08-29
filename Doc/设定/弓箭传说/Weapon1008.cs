using System;
using UnityEngine;

public class Weapon1008 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        for (int i = 0; i < 2; i++)
        {
            base.action.AddActionDelegate(delegate {
                if (base.m_Entity.IsElite)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        base.CreateBulletOverride(Vector3.zero, (GameLogic.Random(-base.m_Data.RandomAngle, base.m_Data.RandomAngle) + (j * 45f)) - 45f);
                    }
                }
                else
                {
                    base.CreateBulletOverride(Vector3.zero, GameLogic.Random(-base.m_Data.RandomAngle, base.m_Data.RandomAngle));
                }
            });
            base.action.AddActionWait(0.2f);
        }
    }
}

