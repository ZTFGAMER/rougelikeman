using Dxx.Util;
using System;
using UnityEngine;

public class AIMoveBabyNormal : AIMoveBase
{
    private EntityBase mParent;
    protected float Move_NextTime;
    protected float Move_NextDurationTime;
    protected float Move_NextX;
    protected float Move_NextY;
    private float Move_NextDurationTimeMin;
    private float Move_NextDurationTimeMax;
    private int min;
    private int max;
    private float fardis;

    public AIMoveBabyNormal(EntityBase entity, int min, int max, float fardis) : base(entity)
    {
        if (entity is EntityCallBase)
        {
            EntityCallBase base2 = entity as EntityCallBase;
            if (base2 != null)
            {
                this.mParent = base2.GetParent();
            }
        }
        this.min = min;
        if (max < min)
        {
            max = min;
        }
        this.max = max;
        this.fardis = fardis;
    }

    private void AIMoveEnd()
    {
        base.End();
    }

    private void AIMoveStart()
    {
        this.m_MoveData.angle = Utils.getAngle(this.Move_NextX, this.Move_NextY);
        this.m_MoveData.direction = new Vector3(this.Move_NextX, 0f, this.Move_NextY) * 0.1f;
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
        if ((this.mParent != null) && (Vector3.Distance(base.m_Entity.position, this.mParent.position) > this.fardis))
        {
            this.Move_NextTime = Updater.AliveTime;
            this.Move_NextDurationTime = this.Move_NextDurationTimeMax;
            float angle = Utils.getAngle(this.mParent.position - base.m_Entity.position) + GameLogic.Random((float) -15f, (float) 15f);
            this.Move_NextX = MathDxx.Sin(angle);
            this.Move_NextY = MathDxx.Cos(angle);
        }
        else
        {
            int num2 = 0;
            this.RandomNextMoveOnce();
            while (!this.IsRandomValid() && (num2 < 100))
            {
                this.RandomNextMoveOnce();
                num2++;
            }
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

