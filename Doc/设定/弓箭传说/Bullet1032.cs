using Dxx.Util;
using System;
using UnityEngine;

public class Bullet1032 : BulletBase
{
    private float perangle = 2f;
    private float maxangle = 80f;
    private float currentrotateangle;
    private float move1time;
    private float waittime;
    private float currenttime;

    protected override void OnInit()
    {
        base.OnInit();
        this.currentrotateangle = 0f;
        this.move1time = 0.3f;
        this.waittime = 1.3f;
        this.currenttime = Updater.AliveTime;
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
            if ((base.m_Entity != null) && (base.m_Entity.m_HatredTarget != null))
            {
                float x = base.m_Entity.m_HatredTarget.position.x - base.mTransform.position.x;
                float y = base.m_Entity.m_HatredTarget.position.z - base.mTransform.position.z;
                float target = Utils.getAngle(x, y);
                float current = base.mTransform.eulerAngles.y;
                if (this.currentrotateangle < this.maxangle)
                {
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
        }
        base.mTransform.position += new Vector3(base.moveX, 0f, base.moveY * 1.23f) * frameDistance;
        base.CurrentDistance += frameDistance;
        if (base.CurrentDistance >= base.Distance)
        {
            this.overDistance();
        }
    }
}

