using Dxx.Util;
using System;
using UnityEngine;

public class Weapon1100 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        int num = 4;
        for (int i = 0; i < num; i++)
        {
            float angle = i * 90f;
            float x = MathDxx.Sin(angle);
            float z = MathDxx.Cos(angle);
            GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x44c, base.m_Entity.position + (new Vector3(x, 1f, z) * 0.5f), angle);
        }
    }
}

