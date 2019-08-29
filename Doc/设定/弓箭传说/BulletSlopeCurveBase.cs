using System;
using TableTool;
using UnityEngine;

public class BulletSlopeCurveBase : BulletBase
{
    private Vector3 endpos;
    private Vector3 dir;
    protected float height = 2f;
    private AnimationCurve curve;
    private Vector3 temppos;
    private Vector3 curvepos = new Vector3();
    private float percent;
    private Vector3 startpos;

    protected override void AwakeInit()
    {
        base.AwakeInit();
        this.curve = LocalModelManager.Instance.Curve_curve.GetCurve(0x186a2);
    }

    protected override void OnInit()
    {
        base.OnInit();
    }

    protected override void OnUpdate()
    {
        Vector3 vector = base.mTransform.position - this.endpos;
        this.dir = vector.normalized;
        this.temppos = Vector3.MoveTowards(base.mTransform.position, this.endpos, base.FrameDistance);
        Vector3 vector2 = this.endpos - this.temppos;
        Vector3 vector3 = this.endpos - this.startpos;
        this.percent = vector2.magnitude / vector3.magnitude;
        this.curvepos.y = this.curve.Evaluate(this.percent);
        this.temppos += this.curvepos;
        Vector3 vector4 = base.mTransform.position - this.endpos;
        if (vector4.magnitude < 0.1f)
        {
            this.overDistance();
        }
    }

    public void SetEndPos(Vector3 endpos)
    {
        this.startpos = base.mTransform.position;
        this.endpos = endpos;
    }
}

