using Dxx.Util;
using System;
using UnityEngine;

public class BulletFlyOffsetBase : BulletBase
{
    [Header("横向偏移距离")]
    public float offsetposx;
    [Header("横向偏移速度")]
    public float speed;
    private float symbol;
    private float offsetcurrent;

    private void init_offset()
    {
        this.symbol = (this.offsetposx < 0f) ? ((float) (-1)) : ((float) 1);
        this.offsetposx = MathDxx.Abs(this.offsetposx);
        this.speed = MathDxx.Abs(this.speed);
    }

    protected override void OnInit()
    {
        base.OnInit();
        this.offsetcurrent = 0f;
        this.init_offset();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        float num = this.speed * Updater.delta;
        if (this.offsetcurrent < this.offsetposx)
        {
            if ((this.offsetcurrent + num) > this.offsetposx)
            {
                num = this.offsetposx - this.offsetcurrent;
            }
            this.offsetcurrent += num;
        }
        else
        {
            num = 0f;
        }
        base.mTransform.position += (new Vector3(-base.moveY, 0f, base.moveX) * num) * this.symbol;
    }

    public void SetOffset(float x, float speed)
    {
        this.offsetposx = x;
        this.speed = speed;
        this.init_offset();
    }
}

