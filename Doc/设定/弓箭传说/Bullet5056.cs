using Dxx.Util;
using System;
using TableTool;
using UnityEngine;

public class Bullet5056 : BulletBase
{
    private AnimationCurve curve;
    private float mBoxTime;
    private float percent;
    private BoxCollider mBoxCollider;

    protected override void AwakeInit()
    {
        base.AwakeInit();
        this.curve = LocalModelManager.Instance.Curve_curve.GetCurve(0x186b3);
        this.mBoxCollider = base.boxList[0];
    }

    protected override void OnInit()
    {
        base.OnInit();
        this.mBoxTime = 0f;
        this.UpdateBox();
    }

    private void UpdateBox()
    {
        this.mBoxCollider.size = new Vector3(this.mBoxCollider.size.x, this.mBoxCollider.size.y, this.curve.Evaluate(this.mBoxTime) * 20f);
        this.mBoxCollider.center = new Vector3(this.mBoxCollider.center.x, this.mBoxCollider.center.y, this.mBoxCollider.size.z / 2f);
        base.mHitList.Clear();
    }

    protected override void UpdateProcess()
    {
        base.UpdateProcess();
        this.mBoxTime += Updater.delta;
        this.mBoxTime = MathDxx.Clamp01(this.mBoxTime);
        this.UpdateBox();
    }
}

