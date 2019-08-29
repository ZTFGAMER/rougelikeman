using Dxx.Util;
using System;
using UnityEngine;

public class AIMove1043 : AIMoveBase
{
    private float Move_NextTime;
    protected float Move_NextX;
    protected float Move_NextY;
    private bool isStart;
    protected float time;
    private float movetime;

    public AIMove1043(EntityBase entity, int time) : base(entity)
    {
        this.time = ((float) time) / 1000f;
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

    private void MoveNormal()
    {
        if ((Updater.AliveTime >= this.Move_NextTime) && (Updater.AliveTime < (this.Move_NextTime + this.time)))
        {
            if (!this.isStart)
            {
                this.AIMoveStart();
                this.isStart = true;
            }
            this.movetime += Updater.delta;
            if (this.movetime >= 0.3f)
            {
                this.movetime -= 0.3f;
                GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x13b6, base.m_Entity.position + new Vector3(0f, 2f, 0f), GameLogic.Random((float) 0f, (float) 360f));
            }
            this.AIMoving();
        }
        else if (Updater.AliveTime >= (this.Move_NextTime + this.time))
        {
            this.AIMoveEnd();
        }
    }

    protected override void OnEnd()
    {
        base.m_Entity.m_MoveCtrl.AIMoveEnd(base.m_MoveData);
        base.m_Entity.m_AniCtrl.SendEvent("Idle", true);
    }

    protected override void OnInitBase()
    {
        this.movetime = 0f;
        this.isStart = false;
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
        this.Move_NextTime = Updater.AliveTime;
    }

    protected override void OnUpdate()
    {
        this.MoveNormal();
    }
}

