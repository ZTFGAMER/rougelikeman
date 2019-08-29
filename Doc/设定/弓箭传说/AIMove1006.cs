using Dxx.Util;
using System;
using UnityEngine;

public class AIMove1006 : AIMoveBase
{
    protected float Move_NextX;
    protected float Move_NextY;
    private int min;
    private int max;

    public AIMove1006(EntityBase entity, int min, int max) : base(entity)
    {
        this.min = min;
        this.max = max;
    }

    private void AIMoveEnd()
    {
        base.End();
    }

    private void AIMoveStart()
    {
        this.UpdateMoveData();
        base.m_Entity.m_MoveCtrl.AIMoveStart(base.m_MoveData);
    }

    private void AIMoving()
    {
        base.m_Entity.m_MoveCtrl.AIMoving(base.m_MoveData);
    }

    private bool CheckEnd() => 
        (base.m_Entity.m_HatredTarget == null);

    private void Move2Player()
    {
        Vector2 vector2 = new Vector2(base.m_Entity.m_HatredTarget.position.x - base.m_Entity.position.x, base.m_Entity.m_HatredTarget.position.z - base.m_Entity.position.z);
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
    }

    private void MoveNormal()
    {
        this.Move2Player();
        this.UpdateMoveData();
        this.AIMoving();
    }

    protected override void OnEnd()
    {
        base.m_Entity.m_MoveCtrl.AIMoveEnd(base.m_MoveData);
    }

    protected override void OnInitBase()
    {
        this.SetHatred();
        if (this.CheckEnd())
        {
            base.End();
        }
        else
        {
            this.Move2Player();
            this.AIMoveStart();
            ConditionBase conditionRandomTime = AIMoveBase.GetConditionRandomTime(this.min, this.max);
            base.ConditionUpdate = new Func<bool>(conditionRandomTime.IsEnd);
        }
    }

    protected override void OnUpdate()
    {
        if (this.CheckEnd())
        {
            base.End();
        }
        else
        {
            this.MoveNormal();
        }
    }

    protected virtual void SetHatred()
    {
    }

    private void UpdateMoveData()
    {
        this.m_MoveData.angle = Utils.getAngle(this.Move_NextX, this.Move_NextY);
        this.m_MoveData.direction = new Vector3(this.Move_NextX, 0f, this.Move_NextY);
        base.m_Entity.m_AttackCtrl.RotateHero(this.m_MoveData.angle);
    }
}

