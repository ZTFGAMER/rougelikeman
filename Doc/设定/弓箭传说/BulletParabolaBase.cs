using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class BulletParabolaBase : BulletBase
{
    [Header("水平飞行子弹目的地偏移随机 最小值")]
    public float PosOffsetMin;
    [Header("水平飞行子弹目的地偏移随机 最大值")]
    public float PosOffsetMax;

    public override void SetTarget(EntityBase entity, int size = 1)
    {
        base.SetTarget(entity, size);
        if (this.PosOffsetMax < this.PosOffsetMin)
        {
            this.PosOffsetMax = this.PosOffsetMin;
        }
        base.PosFromStart2Target += GameLogic.Random(this.PosOffsetMin, this.PosOffsetMax);
    }
}

