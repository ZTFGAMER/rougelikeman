using System;
using UnityEngine;

public class Weapon5055 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        GameLogic.Hold.Sound.PlayBulletCreate(0x1e9809, base.m_Entity.position);
        int num = 5;
        float num2 = 150f;
        float num3 = num2 / ((float) (num - 1));
        float num4 = (num3 * (num - 1)) / 2f;
        for (int i = 0; i < num; i++)
        {
            BulletBase base2 = base.CreateBulletOverride(Vector3.zero, (i * num3) - num4);
            base2.mBulletTransmit.attribute.ReboundWall = new EntityAttributeBase.ValueRange(1, 6, 6);
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

