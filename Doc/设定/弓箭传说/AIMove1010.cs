using Dxx.Util;
using System;
using UnityEngine;

public class AIMove1010 : AIMoveBase
{
    private EntityBase parent;
    protected float Move_NextX;
    protected float Move_NextY;
    private bool isStart;
    private float speed;
    private int index;
    private int checkindex;
    private float fardis;

    public AIMove1010(EntityBase entity, EntityBase parent, float fardis) : base(entity)
    {
        this.parent = parent;
        this.fardis = fardis;
    }

    private void AIMoveEnd()
    {
        base.End();
    }

    private void AIMoveStart()
    {
        this.UpdateMoveDirection();
        base.m_Entity.m_MoveCtrl.AIMoveStart(base.m_MoveData);
    }

    private void AIMoving()
    {
        this.UpdateMoveDirection();
        base.m_Entity.m_MoveCtrl.AIMoving(base.m_MoveData);
    }

    private bool GetNear()
    {
        if (this.parent == null)
        {
            return false;
        }
        return (Vector3.Distance(base.m_Entity.position, this.parent.position) < (this.fardis / 2f));
    }

    private void MoveNormal()
    {
        this.AIMoving();
        if (this.GetNear())
        {
            base.End();
        }
    }

    protected override void OnEnd()
    {
        base.m_Entity.m_MoveCtrl.AIMoveEnd(base.m_MoveData);
        base.m_Entity.m_AniCtrl.SendEvent("Idle", true);
    }

    protected override void OnInitBase()
    {
        if (((this.parent != null) && !this.parent.m_MoveCtrl.GetMoving()) && (Vector3.Distance(this.parent.position, base.m_Entity.position) < (this.fardis + 2f)))
        {
            base.End();
        }
        else
        {
            float speed = base.m_Entity.m_EntityData.GetSpeed();
            float num2 = this.parent.m_EntityData.GetSpeed();
            this.speed = (num2 / speed) * 1.5f;
            this.AIMoveStart();
        }
    }

    protected override void OnUpdate()
    {
        this.MoveNormal();
    }

    private void UpdateMoveDirection()
    {
        EntityBase parent = this.parent;
        if (parent != null)
        {
            Vector2 vector2 = new Vector2(parent.position.x - base.m_Entity.position.x, parent.position.z - base.m_Entity.position.z);
            Vector2 normalized = vector2.normalized;
            this.Move_NextX = normalized.x;
            this.Move_NextY = normalized.y;
            if (normalized == Vector2.zero)
            {
                Vector2 vector8 = new Vector2(GameLogic.Random((float) -1f, (float) 1f), GameLogic.Random((float) -1f, (float) 1f));
                Vector2 vector7 = vector8.normalized;
                this.Move_NextX = vector7.x;
                this.Move_NextY = vector7.y;
            }
            this.m_MoveData.angle = Utils.getAngle(this.Move_NextX, this.Move_NextY);
            this.m_MoveData.direction = new Vector3(this.Move_NextX, 0f, this.Move_NextY) * this.speed;
            base.m_Entity.m_AttackCtrl.RotateHero(this.m_MoveData.angle);
        }
    }
}

