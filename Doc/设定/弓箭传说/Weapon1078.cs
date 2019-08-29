using Dxx.Util;
using System;
using UnityEngine;

public class Weapon1078 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        if (base.m_Entity.IsElite)
        {
            for (int i = 0; i < 3; i++)
            {
                base.action.AddActionDelegate(delegate {
                    int count = 3;
                    for (int j = 0; j < count; j++)
                    {
                        base.CreateBulletOverride(Vector3.zero, Utils.GetBulletAngle(j, count, 120f));
                    }
                });
                base.action.AddActionWait(0.1f);
            }
        }
        else
        {
            int count = 3;
            for (int i = 0; i < count; i++)
            {
                base.CreateBulletOverride(Vector3.zero, Utils.GetBulletAngle(i, count, 120f));
            }
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }
}

