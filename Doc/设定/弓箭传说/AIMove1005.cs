using Dxx.Util;
using System;
using UnityEngine;

public class AIMove1005 : AIMoveBase
{
    private float starttime;
    private float Move_BackTime;
    private bool bBack;
    protected float Move_NextX;
    protected float Move_NextY;
    protected float Move_NextDurationTime;

    public AIMove1005(EntityBase entity) : base(entity)
    {
        this.Move_BackTime = 1f;
        this.Move_NextDurationTime = 2f;
    }

    private void AIMoveStart()
    {
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
        this.m_MoveData.direction = new Vector3(this.Move_NextX, 0f, this.Move_NextY) * 2f;
    }

    private void MoveNormal()
    {
        if ((Updater.AliveTime - this.starttime) < this.Move_BackTime)
        {
            if (!base.m_Entity.m_MoveCtrl.GetMoving())
            {
                this.m_MoveData.angle = Utils.getAngle(this.Move_NextX, this.Move_NextY);
                this.m_MoveData.direction = new Vector3(this.Move_NextX, 0f, this.Move_NextY);
                this.m_MoveData._moveDirection = new Vector3(-this.Move_NextX, 0f, -this.Move_NextY) * 0.3f;
                base.m_Entity.mAniCtrlBase.SetAnimationValue("Run", "Skill");
                base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Run", -0.5f);
                base.m_Entity.m_AttackCtrl.RotateHero(this.m_MoveData.angle);
                base.m_Entity.m_MoveCtrl.AIMoveStart(base.m_MoveData);
            }
            else
            {
                this.AIMoving();
            }
        }
        else if ((Updater.AliveTime - this.starttime) < this.Move_NextDurationTime)
        {
            if (!this.bBack)
            {
                this.Move2Player();
                this.m_MoveData._moveDirection = Vector3.zero;
                base.m_Entity.mAniCtrlBase.SetAnimationValue("Run", "Run");
                base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Run", 0.5f);
                base.m_Entity.m_AniCtrl.SendEvent("Run", true);
                base.m_Entity.SetCollider(true);
                this.bBack = true;
            }
            this.Move2Player();
            this.AIMoving();
        }
        else
        {
            base.End();
        }
    }

    protected override void OnEnd()
    {
        base.m_Entity.SetCollider(false);
        base.m_Entity.m_MoveCtrl.AIMoveEnd(base.m_MoveData);
    }

    protected override void OnInitBase()
    {
        this.bBack = false;
        this.starttime = Updater.AliveTime;
        if (this.CheckEnd())
        {
            base.End();
        }
        else
        {
            this.Move2Player();
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

    protected virtual float moveRatio =>
        14f;
}

