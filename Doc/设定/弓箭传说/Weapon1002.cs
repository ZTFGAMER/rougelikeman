﻿using System;
using UnityEngine;

public class Weapon1002 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        if (base.m_Entity.IsElite)
        {
            for (int i = 0; i < 3; i++)
            {
                base.CreateBulletOverride(Vector3.zero, (i * 45f) - 45f);
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

