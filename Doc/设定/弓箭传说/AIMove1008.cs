using Dxx.Util;
using System;
using UnityEngine;

public class AIMove1008 : AIMoveBase
{
    private float Move_NextTime;
    protected float Move_NextX;
    protected float Move_NextY;
    private bool isStart;
    protected float time;
    private float move2playertatio;
    private GameObject effect;

    public AIMove1008(EntityBase entity, float move2playertatio, int time) : base(entity)
    {
        this.time = ((float) time) / 1000f;
        this.move2playertatio = move2playertatio;
    }

    private void AIMoveEnd()
    {
        base.End();
    }

    private void AIMoveStart()
    {
        this.m_MoveData.angle = Utils.getAngle(this.Move_NextX, this.Move_NextY);
        this.m_MoveData.direction = new Vector3(this.Move_NextX, 0f, this.Move_NextY) * 2.8f;
        base.m_Entity.m_AttackCtrl.RotateHero(this.m_MoveData.angle);
        this.m_MoveData.action = "Continuous";
        base.m_Entity.m_MoveCtrl.AIMoveStart(base.m_MoveData);
        this.CacheEffect();
        this.effect = base.m_Entity.GetEffect(this.MoveEffectID);
    }

    private void AIMoving()
    {
        base.m_Entity.m_MoveCtrl.AIMoving(base.m_MoveData);
    }

    private void CacheEffect()
    {
        if (this.effect != null)
        {
            GameLogic.EffectCache(this.effect);
            this.effect = null;
        }
    }

    private void MoveNormal()
    {
        if ((Updater.AliveTime >= this.Move_NextTime) && (Updater.AliveTime < (this.Move_NextTime + this.time)))
        {
            if (!this.isStart)
            {
                this.AIMoveStart();
                this.isStart = true;
            }
            this.AIMoving();
        }
        else if (Updater.AliveTime >= ((this.Move_NextTime + this.time) + 0.5f))
        {
            this.AIMoveEnd();
        }
    }

    protected override void OnEnd()
    {
        this.CacheEffect();
        base.m_Entity.SetSuperArmor(false);
        base.m_Entity.m_MoveCtrl.AIMoveEnd(base.m_MoveData);
        base.m_Entity.m_AniCtrl.SendEvent("Idle", true);
    }

    protected override void OnInitBase()
    {
        base.m_Entity.SetSuperArmor(true);
        this.isStart = false;
        if (base.m_Entity.m_HatredTarget == null)
        {
            base.m_Entity.m_HatredTarget = GameLogic.Self;
        }
        Vector2 vector2 = new Vector2(base.m_Entity.m_HatredTarget.position.x - base.m_Entity.position.x, base.m_Entity.m_HatredTarget.position.z - base.m_Entity.position.z);
        Vector2 normalized = vector2.normalized;
        if (GameLogic.Random((float) 0f, (float) 1f) > this.move2playertatio)
        {
            float angle = GameLogic.Random((float) 0f, (float) 360f);
            normalized.x = MathDxx.Sin(angle);
            normalized.y = MathDxx.Cos(angle);
        }
        this.Move_NextX = normalized.x;
        this.Move_NextY = normalized.y;
        if (normalized == Vector2.zero)
        {
            Vector2 vector8 = new Vector2(GameLogic.Random((float) -1f, (float) 1f), GameLogic.Random((float) -1f, (float) 1f));
            Vector2 vector7 = vector8.normalized;
            this.Move_NextX = vector7.x;
            this.Move_NextY = vector7.y;
        }
        this.Move_NextTime = Updater.AliveTime;
    }

    protected override void OnUpdate()
    {
        this.MoveNormal();
    }

    protected virtual int MoveEffectID =>
        0x2f4d6e;
}

