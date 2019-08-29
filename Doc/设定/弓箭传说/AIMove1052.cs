using Dxx.Util;
using System;
using UnityEngine;

public class AIMove1052 : AIMoveBase
{
    private int range;
    private Vector3 endpos;

    public AIMove1052(EntityBase entity, int range) : base(entity)
    {
        this.range = range;
    }

    protected override void OnEnd()
    {
        base.m_Entity.m_MoveCtrl.AIMoveEnd(base.m_MoveData);
    }

    protected override void OnInitBase()
    {
        GameLogic.Release.MapCreatorCtrl.RandomFly(base.m_Entity, this.range, out float num, out float num2);
        this.endpos = new Vector3(num, 0f, num2);
        this.UpdateDirection();
        base.m_Entity.m_MoveCtrl.AIMoveStart(base.m_MoveData);
    }

    protected override void OnUpdate()
    {
        Vector3 vector = this.endpos - base.m_Entity.position;
        if (vector.magnitude > 0.2f)
        {
            this.UpdateDirection();
            base.m_Entity.m_MoveCtrl.AIMoving(base.m_MoveData);
        }
        else
        {
            base.End();
        }
    }

    private void UpdateDirection()
    {
        float x = this.endpos.x - base.m_Entity.position.x;
        float y = this.endpos.z - base.m_Entity.position.z;
        this.m_MoveData.angle = Utils.getAngle(x, y);
        Vector3 vector3 = new Vector3(x, 0f, y);
        this.m_MoveData.direction = vector3.normalized;
        base.m_Entity.m_AttackCtrl.RotateHero(this.m_MoveData.angle);
    }
}

