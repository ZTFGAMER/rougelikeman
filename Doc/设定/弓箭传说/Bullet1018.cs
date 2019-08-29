using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class Bullet1018 : BulletBase
{
    protected override void AwakeInit()
    {
        base.AwakeInit();
    }

    private void CreateGround()
    {
        GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x3fb, new Vector3(base.mTransform.position.x, 0f, base.mTransform.position.z), 0f);
    }

    protected override void OnDeInit()
    {
        this.CreateGround();
        base.OnDeInit();
    }

    public override void SetTarget(EntityBase entity, int size = 1)
    {
        base.SetTarget(entity, size);
        base.PosFromStart2Target += Random.Range((float) -1.5f, (float) 1.5f);
    }
}

