using Dxx.Util;
using System;
using UnityEngine;

public class Bullet1010701 : BulletBase
{
    private float perangle = 2f;
    private float currentrotateangle;
    private float move1time;
    private float waittime;
    private float currenttime;
    private EntityBase mTarget;
    private bool bFind;

    protected override void OnArrowEject(EntityBase nexttarget)
    {
        this.mTarget = nexttarget;
    }

    protected override void OnInit()
    {
        base.OnInit();
        this.Refresh();
    }

    protected override void OnUpdate()
    {
        float frameDistance = 0f;
        if ((Updater.AliveTime - this.currenttime) < this.move1time)
        {
            frameDistance = base.FrameDistance;
        }
        else if ((Updater.AliveTime - this.currenttime) >= this.waittime)
        {
            frameDistance = base.FrameDistance;
            if (!this.bFind)
            {
                this.mTarget = GameLogic.Release.Entity.GetNearEntity(this, false);
                this.bFind = true;
            }
            if ((this.mTarget != null) && !this.mTarget.GetIsDead())
            {
                float x = this.mTarget.position.x - base.mTransform.position.x;
                float y = this.mTarget.position.z - base.mTransform.position.z;
                float target = Utils.getAngle(x, y);
                float current = base.mTransform.eulerAngles.y;
                float num6 = MathDxx.MoveTowardsAngle(current, target, this.perangle);
                float num7 = MathDxx.Abs((float) (current - num6));
                float num8 = MathDxx.Abs((float) ((current - num6) + 360f));
                float num9 = MathDxx.Abs((float) ((current - num6) - 360f));
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
        }
        base.mTransform.position += new Vector3(base.moveX, 0f, base.moveY * 1.23f) * frameDistance;
        base.CurrentDistance += frameDistance;
        if (base.CurrentDistance >= base.Distance)
        {
            this.overDistance();
        }
    }

    private void Refresh()
    {
        this.currentrotateangle = 0f;
        this.move1time = 0.6f;
        this.waittime = 0f;
        this.currenttime = Updater.AliveTime;
        this.bFind = false;
        this.mTarget = null;
    }
}

