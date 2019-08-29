using Dxx.Util;
using System;
using UnityEngine;

public class AIMove1009 : AIMoveBase
{
    private float starttime;
    protected float Move_NextDurationTime;
    protected float Move_BackTime;
    protected bool bBack;
    protected string runString;
    protected float runAniSpeed;
    protected float Move_NextX;
    protected float Move_NextY;

    public AIMove1009(EntityBase entity) : base(entity)
    {
        this.Move_NextDurationTime = 1.2f;
        this.Move_BackTime = 0.5f;
        this.runString = "Run";
        this.runAniSpeed = 0.5f;
    }

    private void AIMoveEnd()
    {
        base.End();
    }

    private void AIMoveStart()
    {
        this.m_MoveData.angle = Utils.getAngle(this.Move_NextX, this.Move_NextY);
        this.m_MoveData.direction = new Vector3(this.Move_NextX, 0f, this.Move_NextY) * this.moveRatio;
        base.m_Entity.m_AttackCtrl.RotateHero(this.m_MoveData.angle);
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
        Vector2 vector2 = new Vector2((base.m_Entity.m_HatredTarget.position.x - base.m_Entity.position.x) + GameLogic.Random((float) -2f, (float) 2f), (base.m_Entity.m_HatredTarget.position.z - base.m_Entity.position.z) + GameLogic.Random((float) -2f, (float) 2f));
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
        if ((Updater.AliveTime - this.starttime) < this.Move_BackTime)
        {
            if (!base.m_Entity.m_MoveCtrl.GetMoving())
            {
                this.m_MoveData.angle = Utils.getAngle(this.Move_NextX, this.Move_NextY);
                this.m_MoveData.direction = new Vector3(this.Move_NextX, 0f, this.Move_NextY) * 2f;
                this.m_MoveData._moveDirection = new Vector3(-this.Move_NextX, 0f, -this.Move_NextY);
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
                this.m_MoveData.direction = new Vector3(this.Move_NextX, 0f, this.Move_NextY) * this.moveRatio;
                this.m_MoveData._moveDirection = Vector3.zero;
                base.m_Entity.mAniCtrlBase.SetAnimationValue("Run", this.runString);
                base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Run", this.runAniSpeed);
                base.m_Entity.m_AniCtrl.SendEvent("Run", true);
                this.bBack = true;
                this.OnBackEvent();
            }
            this.AIMoving();
            this.OnSprintUpdate();
        }
        else
        {
            this.AIMoveEnd();
        }
    }

    protected virtual void OnBackEvent()
    {
    }

    protected override void OnEnd()
    {
        base.m_Entity.m_MoveCtrl.AIMoveEnd(base.m_MoveData);
        base.m_Entity.mAniCtrlBase.SetAnimationValue("Run", "Run");
    }

    protected override void OnInitBase()
    {
        this.bBack = false;
        this.starttime = Updater.AliveTime;
        this.SetHatred();
        if (this.CheckEnd())
        {
            base.End();
        }
        else
        {
            this.Move2Player();
        }
    }

    protected virtual void OnSprintUpdate()
    {
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

    protected virtual float moveRatio =>
        14f;
}

