using System;
using UnityEngine;

public class Weapon5105 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        GameLogic.Hold.Sound.PlayBulletCreate(0x1e9809, base.m_Entity.position);
        int num = GameLogic.Random(5, 8);
        for (int i = 0; i < num; i++)
        {
            BulletBase base2 = base.CreateBulletOverride(Vector3.zero, GameLogic.Random((float) -75f, (float) 75f));
            base2.mBulletTransmit.attribute.ReboundWall = new EntityAttributeBase.ValueRange(1, 2, 2);
            base2.UpdateBulletAttribute();
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }
}

