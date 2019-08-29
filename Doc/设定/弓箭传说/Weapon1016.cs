using Dxx.Util;
using System;
using UnityEngine;

public class Weapon1016 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        if (base.m_Entity.IsElite && MathDxx.RandomBool())
        {
            int count = 4;
            for (int i = 0; i < count; i++)
            {
                base.CreateBulletOverride(Vector3.zero, Utils.GetBulletAngle(i, count, 90f));
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                base.action.AddActionDelegate(() => base.CreateBulletOverride(Vector3.zero, 0f));
                if (i < 2)
                {
                    base.action.AddActionWait(0.1f);
                }
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

