using Dxx.Util;
using System;
using UnityEngine;

public class AIMove1055 : AIMoveBase
{
    private EntityBase target;
    private Vector3 dir;
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

    public AIMove1055(EntityBase entity) : base(entity)
    {
        this.g = 5f;
        this.alltime = 0.4f;
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
        GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x13e7, base.m_Entity.m_Body.FootMask.transform.position, 0f);
    }

    private void OnFly()
    {
        float num = 1f - (((this.delaytime + this.alltime) - Updater.AliveTime) / this.alltime);
        float num2 = (((-4f * this.g) * (num - 0.5f)) * (num - 0.5f)) + this.g;
        if (Updater.AliveTime < (this.delaytime + this.halftime))
        {
            base.m_Entity.SetPosition(new Vector3(this.startpos.x + (this.perendx * num), this.startpos.y + num2, this.startpos.z + (this.perendz * num)));
        }
        else if (Updater.AliveTime < (this.delaytime + this.alltime))
        {
            base.m_Entity.SetPosition(new Vector3(this.startpos.x + (this.perendx * num), this.startpos.y + num2, this.startpos.z + (this.perendz * num)));
        }
        else
        {
            base.m_Entity.SetPosition(new Vector3(base.m_Entity.position.x, 0f, base.m_Entity.position.z));
            base.End();
        }
    }

    protected override void OnInitBase()
    {
        this.delaytime = Updater.AliveTime + 0.2f;
        this.starttime = Updater.AliveTime;
        this.halftime = this.alltime / 2f;
        this.startpos = base.m_Entity.position;
        this.target = GameLogic.Self;
        this.endx = this.target.position.x;
        this.endz = this.target.position.z;
        this.perendx = this.endx - base.m_Entity.position.x;
        this.perendz = this.endz - base.m_Entity.position.z;
        base.m_Entity.m_AniCtrl.SendEvent("Skill", false);
    }

    protected override void OnUpdate()
    {
        this.MoveNormal();
    }
}

