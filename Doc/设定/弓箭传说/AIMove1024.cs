using Dxx.Util;
using System;
using UnityEngine;

public class AIMove1024 : AIMoveBase
{
    private EntityBase target;
    private Vector3 dir;
    private int flyframe;
    private float g;
    private float endx;
    private float endz;
    private float perendx;
    private float perendz;
    private float delaytime;
    private float starttime;
    private float alltime;
    private float halftime;
    private Vector3 startpos;
    private int range;

    public AIMove1024(EntityBase entity, int range) : base(entity)
    {
        this.flyframe = 60;
        this.g = 0.5f;
        this.alltime = 0.35f;
        this.range = range;
    }

    private void MoveNormal()
    {
        if (Updater.AliveTime >= this.delaytime)
        {
            this.OnFly();
        }
    }

    protected override void OnEnd()
    {
        base.m_Entity.SetObstacleCollider(true);
    }

    private void OnFly()
    {
        float num = 1f - (((this.delaytime + this.alltime) - Updater.AliveTime) / this.alltime);
        float num2 = (((-4f * this.g) * (num - 0.5f)) * (num - 0.5f)) + this.g;
        if (Updater.AliveTime < (this.delaytime + this.halftime))
        {
            base.m_Entity.SetPosition(new Vector3(this.startpos.x + ((this.perendx * num) * this.flyframe), this.startpos.y + num2, this.startpos.z + ((this.perendz * num) * this.flyframe)));
        }
        else if (Updater.AliveTime < (this.delaytime + this.alltime))
        {
            base.m_Entity.SetPosition(new Vector3(this.startpos.x + ((this.perendx * num) * this.flyframe), this.startpos.y + num2, this.startpos.z + ((this.perendz * num) * this.flyframe)));
        }
        else if (Updater.AliveTime > ((this.delaytime + this.alltime) + 0.8f))
        {
            base.End();
        }
    }

    protected override void OnInitBase()
    {
        this.delaytime = Updater.AliveTime + 0.15f;
        this.starttime = Updater.AliveTime;
        this.halftime = this.alltime / 2f;
        this.startpos = base.m_Entity.position;
        this.target = GameLogic.Self;
        if (this.target == null)
        {
            base.End();
        }
        else
        {
            GameLogic.Release.MapCreatorCtrl.RandomItemSide(base.m_Entity, this.range, out this.endx, out this.endz);
            this.perendx = (this.endx - base.m_Entity.position.x) / ((float) this.flyframe);
            this.perendz = (this.endz - base.m_Entity.position.z) / ((float) this.flyframe);
            base.m_Entity.SetObstacleCollider(false);
            base.m_Entity.m_AniCtrl.SendEvent("Skill", false);
            this.dir = new Vector3(this.perendx, 0f, this.perendz);
            this.UpdateDirection();
        }
    }

    protected override void OnUpdate()
    {
        this.MoveNormal();
    }

    private void UpdateDirection()
    {
        this.m_MoveData.angle = Utils.getAngle(this.dir.x, this.dir.z);
        this.m_MoveData.direction = this.dir;
        base.m_Entity.m_AttackCtrl.RotateHero(this.m_MoveData.angle);
    }

    protected virtual int AttackID =>
        0x3ed;
}

