using Dxx.Util;
using System;
using UnityEngine;

public class AIMove1004 : AIMoveBase
{
    private float movedis;
    private float alldis;
    private string prevRun;
    protected float randomDisMin;
    protected float randomDisMax;
    private int randomcount;

    public AIMove1004(EntityBase entity) : base(entity)
    {
        this.randomDisMin = 5f;
        this.randomDisMax = 7f;
    }

    protected override void OnEnd()
    {
    }

    protected override void OnInitBase()
    {
        this.movedis = 0f;
        this.randomcount = 5;
        base.m_Entity.m_AniCtrl.SendEvent("Skill", false);
        this.RandomNextMoveLoop();
    }

    protected override void OnUpdate()
    {
        float num = base.m_Entity.m_EntityData.GetSpeed() * Updater.delta;
        if ((this.movedis + num) > this.alldis)
        {
            num = this.alldis - this.movedis;
        }
        this.movedis += num;
        base.m_Entity.SetPositionBy(this.m_MoveData.direction * num);
        if (this.movedis >= this.alldis)
        {
            base.End();
        }
    }

    private void RandomNextMoveLoop()
    {
        float angle = GameLogic.Random((float) 0f, (float) 360f);
        float x = MathDxx.Sin(angle);
        float z = MathDxx.Cos(angle);
        Vector3 vector = new Vector3(x, 0f, z);
        this.m_MoveData.direction = vector.normalized;
        float num4 = GameLogic.Random((float) 5f, (float) 7f);
        float num5 = 0.1f;
        this.alldis = num4;
        if (Physics.SphereCast(base.m_Entity.position - (this.m_MoveData.direction * num5), base.m_Entity.GetCollidersSize(), this.m_MoveData.direction, out RaycastHit hit, this.alldis + num5, LayerManager.MapAllInt))
        {
            this.alldis = hit.distance - num5;
            if ((this.alldis < 2f) && (this.randomcount > 0))
            {
                this.randomcount--;
                this.RandomNextMoveLoop();
                return;
            }
        }
        base.m_Entity.m_AttackCtrl.RotateHero(Utils.getAngle(this.m_MoveData.direction));
    }
}

