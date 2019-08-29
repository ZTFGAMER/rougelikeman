using Dxx.Util;
using System;
using UnityEngine;

public class Bullet3007 : BulletBase
{
    private ConditionTime mCondition;
    private float playtime = 0.5f;
    private float starttime;
    private float centerz = 8f;

    protected override void OnDeInit()
    {
        base.OnDeInit();
    }

    protected override void OnInit()
    {
        base.OnInit();
        this.starttime = 0f;
        ConditionTime time = new ConditionTime {
            time = 0.6f
        };
        this.mCondition = time;
    }

    protected override void OnUpdate()
    {
        if ((this.mCondition != null) && this.mCondition.IsEnd())
        {
            this.mCondition = null;
            this.overDistance();
        }
        this.starttime += Updater.delta;
        if (this.starttime > this.playtime)
        {
            this.starttime = this.playtime;
        }
        base.boxList[0].center = new Vector3(0f, 0f, (this.starttime / this.playtime) * this.centerz);
    }
}

