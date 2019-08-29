using System;
using UnityEngine;

public class Bullet5082 : BulletBase
{
    protected override void OnInit()
    {
        base.OnInit();
        base.mTransform.localRotation = Quaternion.identity;
        base.bFlyRotate = false;
        base.Distance = 2.147484E+09f;
    }

    protected override void OnSetBulletAttribute()
    {
        base.mBulletTransmit.attribute.ReboundWall.UpdateCount(1);
        base.mBulletTransmit.attribute.ReboundWall.UpdateMin(0x7fffffff);
        base.mBulletTransmit.attribute.ReboundWall.UpdateMax(0x7fffffff);
        base.mReboundWallCount = base.mBulletTransmit.attribute.ReboundWall.Value;
        base.mReboundWallMaxCount = base.mReboundWallCount;
    }
}

