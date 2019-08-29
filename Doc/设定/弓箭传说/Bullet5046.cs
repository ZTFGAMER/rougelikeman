using Dxx.Util;
using System;
using TableTool;
using UnityEngine;

public class Bullet5046 : BulletBase
{
    protected float onetime = 1f;
    protected float movedis = 0.6f;
    protected int curveId = 0x186ab;
    protected AnimationCurve curve;
    private float percent;
    private float starttime;
    private float perspeed;
    private Vector3 straightpos;
    private Vector3 straightperpos;
    private Vector3 offset;

    protected override void AwakeInit()
    {
        this.curve = LocalModelManager.Instance.Curve_curve.GetCurve(this.curveId);
    }

    protected override void OnInit()
    {
        base.OnInit();
        this.starttime = Updater.AliveTime;
        this.straightpos = base.mTransform.position;
    }

    protected override void OnSetArgs()
    {
    }

    protected override void OnUpdate()
    {
        if ((Updater.AliveTime - this.starttime) > this.onetime)
        {
            this.starttime += this.onetime;
        }
        this.percent = (Updater.AliveTime - this.starttime) / this.onetime;
        this.offset = (new Vector3(-base.moveY, 0f, base.moveX) * this.movedis) * this.curve.Evaluate(this.percent);
        float frameDistance = base.FrameDistance;
        this.straightperpos = new Vector3(base.moveX, 0f, base.moveY * 1.23f) * frameDistance;
        this.straightpos += this.straightperpos;
        base.mTransform.position = this.straightpos + this.offset;
        base.CurrentDistance += frameDistance;
        if (base.CurrentDistance >= base.Distance)
        {
            this.overDistance();
        }
    }
}

