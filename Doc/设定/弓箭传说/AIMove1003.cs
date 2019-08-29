using Dxx.Util;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AIMove1003 : AIMoveBase
{
    private EntityCallBase partbody;
    private EntityBase parentEntity;
    private Vector3 endpos;

    public AIMove1003(EntityCallBase entity) : base(entity)
    {
        base.ConditionUpdate = new Func<bool>(this.<AIMove1003>m__0);
        this.partbody = entity;
        this.parentEntity = this.partbody.GetParent();
    }

    [CompilerGenerated]
    private bool <AIMove1003>m__0() => 
        (base.m_Entity.m_HatredTarget != null);

    private void AIMoveEnd()
    {
        base.End();
    }

    private void AIMoveStart()
    {
        base.m_Entity.m_MoveCtrl.AIMoveStart(base.m_MoveData);
    }

    private void AIMoving()
    {
        base.m_Entity.m_MoveCtrl.AIMoving(base.m_MoveData);
    }

    private void MoveNormal()
    {
        this.UpdateEndPostion();
        this.UpdateDirection();
        this.AIMoving();
    }

    protected override void OnEnd()
    {
        base.m_Entity.m_MoveCtrl.AIMoveEnd(base.m_MoveData);
    }

    protected override void OnInitBase()
    {
        this.UpdateDirection();
        this.AIMoveStart();
    }

    protected override void OnUpdate()
    {
        this.MoveNormal();
    }

    private void UpdateDirection()
    {
        float x = this.endpos.x - base.m_Entity.position.x;
        float y = this.endpos.z - base.m_Entity.position.z;
        this.m_MoveData.angle = Utils.getAngle(x, y);
        Vector3 vector3 = new Vector3(x, 0f, y);
        this.m_MoveData.direction = vector3.normalized;
    }

    private void UpdateEndPostion()
    {
        this.endpos = this.parentEntity.GetRotateFollowPosition(base.m_Entity);
    }
}

