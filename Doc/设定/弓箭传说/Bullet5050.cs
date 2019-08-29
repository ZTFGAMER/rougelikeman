using System;
using UnityEngine;

public class Bullet5050 : BulletBase
{
    protected override void OnOverDistance()
    {
        for (int i = 0; i < 4; i++)
        {
            GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x13bb, new Vector3(base.mTransform.position.x, 1f, base.mTransform.position.z), (i * 90f) + 45f);
        }
    }
}

