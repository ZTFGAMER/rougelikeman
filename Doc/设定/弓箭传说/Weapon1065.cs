using Dxx.Util;
using System;
using UnityEngine;

public class Weapon1065 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        if (base.m_Entity.IsElite)
        {
            int count = 3;
            for (int i = 0; i < count; i++)
            {
                base.CreateBulletOverride(Vector3.zero, Utils.GetBulletAngle(i, count, 90f));
            }
        }
        else
        {
            base.OnAttack(args);
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }
}

