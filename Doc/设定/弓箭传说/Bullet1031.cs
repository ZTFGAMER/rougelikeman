using Dxx.Util;
using System;
using UnityEngine;

public class Bullet1031 : BulletBase
{
    private ConditionTime mCondition1;
    protected float playtime = 0.25f;
    protected float centerz = 11.8f;
    private float starttime;

    protected override void OnDeInit()
    {
        base.OnDeInit();
    }

    protected override void OnInit()
    {
        base.OnInit();
        this.starttime = 0f;
        ConditionTime time = new ConditionTime {
            time = this.playtime
        };
        this.mCondition1 = time;
        base.boxList[0].center = new Vector3(0f, 0f, 0f);
        this.BoxEnable(true);
    }

    protected override void OnUpdate()
    {
        if ((this.mCondition1 != null) && this.mCondition1.IsEnd())
        {
            this.mCondition1 = null;
            this.overDistance();
        }
        this.starttime += Updater.delta;
        if (this.starttime > this.playtime)
        {
            this.starttime = this.playtime;
        }
        base.boxList[0].center = new Vector3(0f, 0f, (this.starttime / this.playtime) * this.centerz);
        if (this.starttime >= this.playtime)
        {
            this.BoxEnable(false);
        }
    }
}

