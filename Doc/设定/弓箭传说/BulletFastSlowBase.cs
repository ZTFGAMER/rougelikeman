using Dxx.Util;
using System;
using TableTool;
using UnityEngine;

public class BulletFastSlowBase : BulletBase
{
    private AnimationCurve curve;
    private float time;
    [Header("曲线变化总时间")]
    public float alltime = 0.7f;
    [Header("速度增加系数")]
    public float speedratio = 4f;

    private void InitCurve()
    {
        this.curve = LocalModelManager.Instance.Curve_curve.GetCurve(0x186a6);
    }

    protected override void OnInit()
    {
        base.OnInit();
        this.InitCurve();
        this.time = 0f;
    }

    protected override void OnUpdate()
    {
        this.time += Updater.delta;
        float num = base.FrameDistance + ((base.FrameDistance * this.speedratio) * this.curve.Evaluate(this.time / this.alltime));
        base.UpdateMoveDirection();
        base.mTransform.position += new Vector3(base.moveX, 0f, base.moveY * 1.23f) * num;
        base.CurrentDistance += num;
        if (base.CurrentDistance >= base.Distance)
        {
            this.overDistance();
        }
    }
}

