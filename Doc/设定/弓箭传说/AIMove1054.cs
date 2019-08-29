using DG.Tweening;
using Dxx.Util;
using System;
using UnityEngine;

public class AIMove1054 : AIMoveBase
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
    private const float skillspeed = -0.5f;
    private Sequence seq_play;
    private bool bAttack;

    public AIMove1054(EntityBase entity) : base(entity)
    {
        this.flyframe = 60;
        this.g = 2f;
        this.alltime = 0.6f;
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
        base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Skill", 0.5f);
        base.m_Entity.SetObstacleCollider(true);
        if (this.seq_play != null)
        {
            TweenExtensions.Kill(this.seq_play, false);
            this.seq_play = null;
        }
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
            if (Updater.AliveTime > ((this.delaytime + this.alltime) + 1.1f))
            {
                base.End();
            }
        }
    }

    protected override void OnInitBase()
    {
        if (base.m_Entity.m_HatredTarget == null)
        {
            base.End();
        }
        else
        {
            base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Skill", -0.5f);
            this.delaytime = Updater.AliveTime + 0.4f;
            this.starttime = Updater.AliveTime;
            this.halftime = this.alltime / 2f;
            this.startpos = base.m_Entity.position;
            this.bAttack = false;
            base.m_Entity.m_AniCtrl.SendEvent("Skill", false);
            this.endx = base.m_Entity.m_HatredTarget.position.x;
            this.endz = base.m_Entity.m_HatredTarget.position.z;
            bool flag = MathDxx.RandomBool();
            this.perendx = (this.endx - base.m_Entity.position.x) / ((float) this.flyframe);
            this.perendz = (this.endz - base.m_Entity.position.z) / ((float) this.flyframe);
            base.m_Entity.SetObstacleCollider(false);
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
}

