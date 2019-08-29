using Dxx.Util;
using System;
using UnityEngine;

public class Weapon5085 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        if (base.m_Entity.m_HatredTarget != null)
        {
            int count = 7;
            float num2 = 0f;
            if (base.m_Entity.m_HatredTarget != null)
            {
                num2 = Utils.getAngle(base.m_Entity.m_HatredTarget.position - base.m_Entity.position);
            }
            for (int i = 0; i < count; i++)
            {
                float angle = num2 + Utils.GetBulletAngle(i, count, 180f);
                float x = MathDxx.Sin(angle);
                float z = MathDxx.Cos(angle);
                GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x13dd, base.m_Entity.position + (new Vector3(x, 0f, z) * 2f), angle);
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

