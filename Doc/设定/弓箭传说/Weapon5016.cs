using System;
using UnityEngine;

public class Weapon5016 : Weapon1020
{
    protected override void OnAttack(object[] args)
    {
        for (int i = 0; i < 3; i++)
        {
            BulletBase base2 = base.CreateBulletOverride(Vector3.zero, 60f - (i * 60f));
            base2.SetTarget(base.Target, base.ParabolaSize);
            base2.OnBulletCache = base.OnBulletCache;
        }
    }
}

