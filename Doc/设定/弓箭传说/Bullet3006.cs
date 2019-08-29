using Dxx.Util;
using System;
using UnityEngine;

public class Bullet3006 : BulletBase
{
    private float perangle;
    private float currentrotateangle;
    private float maxangle;
    private Vector3 endpos;
    private float tracktime = 1.5f;

    protected override void OnInit()
    {
        base.OnInit();
        this.currentrotateangle = 0f;
        this.perangle = -1f;
    }

    protected override void OnUpdate()
    {
        float frameDistance = base.FrameDistance;
        float x = this.endpos.x - base.mTransform.position.x;
        float y = this.endpos.z - base.mTransform.position.z;
        float target = Utils.getAngle(x, y);
        float bulletAngle = base.bulletAngle;
        this.perangle++;
        if (this.currentrotateangle < this.maxangle)
        {
            float num6 = MathDxx.MoveTowardsAngle(bulletAngle, target, this.perangle);
            if (num6 == target)
            {
                this.currentrotateangle = 2.147484E+09f;
            }
            float num7 = MathDxx.Abs((float) (bulletAngle - num6));
            float num8 = MathDxx.Abs((float) ((bulletAngle - num6) + 360f));
            float num9 = MathDxx.Abs((float) ((bulletAngle - num6) - 360f));
            if (num7 > num8)
            {
                num7 = num8;
            }
            if (num7 > num9)
            {
                num7 = num9;
            }
            this.currentrotateangle += num7;
            base.bulletAngle = num6;
            base.UpdateMoveDirection();
        }
        base.mTransform.position += new Vector3(base.moveX, 0f, base.moveY * 1.23f) * frameDistance;
        base.CurrentDistance += frameDistance;
        if (base.CurrentDistance >= base.Distance)
        {
            this.overDistance();
        }
    }

    public void SetEndPos(Vector3 endpos, float offsetangle)
    {
        this.endpos = endpos;
        this.maxangle = MathDxx.Abs(offsetangle) + 60f;
    }
}

