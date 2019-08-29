using System;
using UnityEngine;

public class WeaponCallBase : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        BulletCallBase base2 = base.CreateBulletOverride(Vector3.zero, 0f) as BulletCallBase;
        base2.SetTarget(null, 1);
        base2.SetEndPos(base.m_Entity.CallEndPos);
    }
}

