using Dxx.Util;
using System;
using UnityEngine;

public class Weapon1063 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        if (base.m_Entity.IsElite)
        {
            if (MathDxx.RandomBool())
            {
                int num = 4;
                for (int i = 0; i < num; i++)
                {
                    base.action.AddActionDelegate(() => base.CreateBulletOverride(Vector3.zero, 0f));
                    base.action.AddActionWait(0.18f);
                }
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    base.CreateBulletOverride(Vector3.zero, (i * 90f) + 45f);
                }
            }
        }
        else
        {
            base.OnAttack(args);
        }
    }

    protected override void OnInstall()
    {
        base.OnInstall();
    }

    protected override void OnUnInstall()
    {
        base.OnUnInstall();
    }
}

