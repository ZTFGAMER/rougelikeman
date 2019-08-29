using System;
using UnityEngine;

public class Bullet5040 : BulletBase
{
    protected override void AwakeInit()
    {
        base.AwakeInit();
    }

    private void CreateGround()
    {
        GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x13c9, new Vector3(base.mTransform.position.x, 0f, base.mTransform.position.z), 0f);
    }

    protected override void OnDeInit()
    {
        this.CreateGround();
        base.OnDeInit();
    }
}

