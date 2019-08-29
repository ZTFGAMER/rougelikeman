using System;
using UnityEngine;

public class Bullet5039 : BulletBase
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

    protected override void OnUpdate()
    {
        base.mTransform.position = new Vector3(base.mTransform.position.x, base.mTransform.position.y - base.FrameDistance, base.mTransform.position.z);
        if (base.mTransform.position.y <= 0f)
        {
            this.overDistance();
        }
    }
}

