using Dxx.Util;
using System;
using UnityEngine;

public class BulletFireLineBase : BulletBase
{
    public float maxtime = 0.3f;
    public float endtime = 0.8f;
    public float MaxLength = 7f;
    private BoxCollider mBoxCollider;
    private float time;
    private float percent;

    protected override void OnDeInit()
    {
        base.OnDeInit();
    }

    protected override void OnInit()
    {
        base.OnInit();
        this.time = 0f;
        if (base.boxList.Length > 0)
        {
            this.mBoxCollider = base.boxList[0];
            this.UpdateBoxColloder(0f);
        }
    }

    protected override void OnUpdate()
    {
        if (this.time < this.endtime)
        {
            this.time += Updater.delta;
            this.percent = this.time / this.maxtime;
            this.percent = MathDxx.Clamp01(this.percent);
            this.UpdateBoxColloder(this.percent);
        }
        else
        {
            this.UpdateBoxColloder(0f);
        }
    }

    private void UpdateBoxColloder(float percent)
    {
        if (this.mBoxCollider != null)
        {
            this.mBoxCollider.size = new Vector3(this.mBoxCollider.size.x, this.mBoxCollider.size.y, percent * this.MaxLength);
            this.mBoxCollider.center = new Vector3(this.mBoxCollider.center.x, this.mBoxCollider.center.y, (percent * this.MaxLength) / 2f);
        }
    }
}

