using Dxx.Util;
using System;
using UnityEngine;

public class AIMove1053 : AIMoveBase
{
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
    private bool bAttack;
    private int rangemin;
    private int rangemax;

    public AIMove1053(EntityBase entity, int rangemin, int rangemax) : base(entity)
    {
        this.flyframe = 60;
        this.g = 1f;
        this.alltime = 0.3f;
        this.rangemin = rangemin;
        this.rangemax = rangemax;
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
        float num2 = 0f;
        if (Updater.AliveTime < (this.delaytime + this.halftime))
        {
            base.m_Entity.SetPosition(new Vector3(this.startpos.x + ((this.perendx * num) * this.flyframe), this.startpos.y + num2, this.startpos.z + ((this.perendz * num) * this.flyframe)));
        }
        else if (Updater.AliveTime < (this.delaytime + this.alltime))
        {
            base.m_Entity.SetPosition(new Vector3(this.startpos.x + ((this.perendx * num) * this.flyframe), this.startpos.y + num2, this.startpos.z + ((this.perendz * num) * this.flyframe)));
        }
        else if (Updater.AliveTime > (this.delaytime + this.alltime))
        {
            if (!this.bAttack)
            {
                for (int i = 0; i < 4; i++)
                {
                    GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x13d5, base.m_Entity.position + new Vector3(0f, 1f, 0f), (i * 90) + 45f);
                }
                this.bAttack = true;
            }
            if (Updater.AliveTime > ((this.delaytime + this.alltime) + 0.7f))
            {
                base.End();
            }
        }
    }

    protected override void OnInitBase()
    {
        this.delaytime = Updater.AliveTime + 0.15f;
        this.starttime = Updater.AliveTime;
        this.halftime = this.alltime / 2f;
        this.startpos = base.m_Entity.position;
        this.bAttack = false;
        bool dir = MathDxx.RandomBool();
        if (!GameLogic.Release.MapCreatorCtrl.RandomItemLine(base.m_Entity, dir, this.rangemin, this.rangemax, out this.endx, out this.endz))
        {
            dir = !dir;
            if (!GameLogic.Release.MapCreatorCtrl.RandomItemLine(base.m_Entity, dir, this.rangemin, this.rangemax, out this.endx, out this.endz))
            {
                base.End();
                return;
            }
        }
        this.perendx = (this.endx - base.m_Entity.position.x) / ((float) this.flyframe);
        this.perendz = (this.endz - base.m_Entity.position.z) / ((float) this.flyframe);
        base.m_Entity.SetObstacleCollider(false);
        base.m_Entity.m_AniCtrl.SendEvent("Skill", false);
        this.dir = new Vector3(this.perendx, 0f, this.perendz);
        this.UpdateDirection();
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
}

