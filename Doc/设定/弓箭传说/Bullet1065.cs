using System;

public class Bullet1065 : BulletBase
{
    protected override void OnSetBulletAttribute()
    {
        base.mBulletTransmit.attribute.ReboundWall.UpdateCount(1);
        base.mBulletTransmit.attribute.ReboundWall.UpdateMin(20);
        base.mBulletTransmit.attribute.ReboundWall.UpdateMax(20);
        base.mReboundWallCount = base.mBulletTransmit.attribute.ReboundWall.Value;
        base.mReboundWallMaxCount = base.mReboundWallCount;
    }
}

