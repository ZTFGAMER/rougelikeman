using System;
using TableTool;
using UnityEngine;

public class BulletCallBase : BulletBase
{
    private Vector3 endpos;
    protected float height = 2f;
    private AnimationCurve curve;
    private Vector3 temppos;
    private Vector3 curvepos = new Vector3();
    private float percent;
    private Vector3 startpos;
    private Vector3 straightpos;
    public bool bShowCallEffect = true;

    protected override void AwakeInit()
    {
        base.AwakeInit();
        this.curve = LocalModelManager.Instance.Curve_curve.GetCurve(0x186a2);
    }

    protected override void OnOverDistance()
    {
        if (base.m_Entity != null)
        {
            EntityMonsterBase entity = base.m_Entity as EntityMonsterBase;
            if ((entity != null) && (entity.GetAI() != null))
            {
                entity.GetAI().CallOne(this.endpos, this.bShowCallEffect);
            }
        }
    }

    protected override void OnUpdate()
    {
        this.straightpos = Vector3.MoveTowards(this.straightpos, this.endpos, base.FrameDistance);
        Vector3 vector = this.straightpos - this.startpos;
        Vector3 vector2 = this.endpos - this.startpos;
        this.percent = vector.magnitude / vector2.magnitude;
        this.curvepos.y = this.curve.Evaluate(this.percent) * this.height;
        this.temppos = this.straightpos + this.curvepos;
        base.mTransform.position = this.temppos;
        Vector3 vector3 = base.mTransform.position - this.endpos;
        if (vector3.magnitude < 0.1f)
        {
            this.overDistance();
        }
    }

    public void SetEndPos(Vector3 endpos)
    {
        this.startpos = base.mTransform.position;
        this.straightpos = this.startpos;
        this.endpos = endpos;
    }
}

