using Dxx.Util;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class AIMove1002 : AIMoveBase
{
    protected float Move_NextTime;
    protected float Move_NextDurationTime;
    protected float Move_NextX;
    protected float Move_NextY;
    private float Move_NextDurationTimeMin;
    private float Move_NextDurationTimeMax;
    private int min;
    private int max;

    public AIMove1002(EntityBase entity, int min, int max = -1) : base(entity)
    {
        this.min = min;
        this.max = (max != -1) ? max : min;
    }

    private void AIMoveEnd()
    {
        base.End();
    }

    private void AIMoveStart()
    {
        this.m_MoveData.angle = Utils.getAngle(this.Move_NextX, this.Move_NextY);
        this.m_MoveData.direction = new Vector3(this.Move_NextX, 0f, this.Move_NextY);
        base.m_Entity.m_AttackCtrl.RotateHero(this.m_MoveData.angle);
        base.m_Entity.m_MoveCtrl.AIMoveStart(base.m_MoveData);
    }

    private void AIMoving()
    {
        base.m_Entity.m_MoveCtrl.AIMoving(base.m_MoveData);
    }

    protected bool IsRandomValid() => 
        true;

    private void MoveNormal()
    {
        if (Updater.AliveTime < (this.Move_NextTime + this.Move_NextDurationTime))
        {
            if (!base.m_Entity.m_MoveCtrl.GetMoving())
            {
                this.AIMoveStart();
            }
            else
            {
                this.AIMoving();
            }
        }
        else
        {
            this.AIMoveEnd();
        }
    }

    protected override void OnEnd()
    {
        base.m_Entity.m_MoveCtrl.AIMoveEnd(base.m_MoveData);
    }

    protected override void OnInitBase()
    {
        this.Move_NextDurationTimeMin = ((float) this.min) / 1000f;
        this.Move_NextDurationTimeMax = ((float) this.max) / 1000f;
        this.RandomNextMove();
    }

    protected override void OnUpdate()
    {
        if (!base.m_Entity.m_AttackCtrl.GetAttacking())
        {
            this.MoveNormal();
        }
    }

    protected virtual void RandomNextMove()
    {
        int num = 0;
        this.RandomNextMoveOnce();
        while (!this.IsRandomValid() && (num < 100))
        {
            this.RandomNextMoveOnce();
            num++;
        }
    }

    private void RandomNextMoveOnce()
    {
        this.Move_NextTime = Updater.AliveTime;
        this.Move_NextDurationTime = GameLogic.Random(this.Move_NextDurationTimeMin, this.Move_NextDurationTimeMax);
        Vector2 vector2 = new Vector2(GameLogic.Random((float) -1f, (float) 1f), GameLogic.Random((float) -1f, (float) 1f));
        Vector2 normalized = vector2.normalized;
        this.Move_NextX = normalized.x;
        this.Move_NextY = normalized.y;
    }
}

