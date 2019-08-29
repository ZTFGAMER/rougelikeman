using Dxx.Util;
using System;
using UnityEngine;

public class AIMove1001 : AIMoveBase
{
    private float startx;
    private float endx;

    public AIMove1001(EntityBase entity) : base(entity)
    {
        this.endx = -1f;
    }

    protected override void OnEnd()
    {
        base.m_Entity.m_MoveCtrl.AIMoveEnd(base.m_MoveData);
    }

    protected override void OnInitBase()
    {
        this.m_MoveData.angle = Utils.getAngle(GameLogic.Self.position - base.m_Entity.position);
        float x = MathDxx.Sin(this.m_MoveData.angle);
        float z = MathDxx.Cos(this.m_MoveData.angle);
        this.m_MoveData.direction = new Vector3(x, 0f, z);
        base.m_Entity.m_AttackCtrl.RotateHero(this.m_MoveData.angle);
        base.m_Entity.m_MoveCtrl.AIMoveStart(base.m_MoveData);
        this.startx = base.m_Entity.transform.localPosition.x;
    }

    protected override void OnUpdate()
    {
        base.m_Entity.m_MoveCtrl.AIMoving(base.m_MoveData);
        float x = base.m_Entity.transform.localPosition.x;
        if (x < this.endx)
        {
            x = this.endx;
        }
        float num2 = (x - this.endx) / (this.startx - this.endx);
        this.m_MoveData.direction = this.m_MoveData.direction.normalized * num2;
    }
}

