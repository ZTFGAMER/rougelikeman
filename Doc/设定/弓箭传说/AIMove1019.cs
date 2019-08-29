using Dxx.Util;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class AIMove1019 : AIMoveBase
{
    private EntityBase target;
    private Vector3 dir;
    public float g;
    public int attackid;
    private float endx;
    private float endz;
    private float perendx;
    private float perendz;
    private float delaytime;
    private float starttime;
    public float alltime;
    private float halftime;
    private Vector3 startpos;
    private bool bPlaySkill;
    private float speedratio;

    public AIMove1019(EntityBase entity) : base(entity)
    {
        this.g = 20f;
        this.attackid = 0x1392;
        this.alltime = 1.3f;
    }

    private void MoveNormal()
    {
        this.starttime += Updater.delta;
        if (this.starttime >= this.delaytime)
        {
            this.OnFly();
        }
    }

    protected override void OnEnd()
    {
        if (this.bPlaySkill)
        {
            base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Call", 1f - this.speedratio);
        }
        base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Skill", 1f - this.speedratio);
        base.m_Entity.ChangeWeapon(this.attackid);
        base.m_Entity.m_Weapon.Attack(Array.Empty<object>());
        GameObject obj2 = GameLogic.EffectGet("Effect/Boss/BossJumpHit5005");
        obj2.transform.position = base.m_Entity.position;
        float[] args = new float[] { 1f };
        obj2.GetComponent<BossJumpHit5005>().Init(base.m_Entity, args);
    }

    private void OnFly()
    {
        if (!this.bPlaySkill)
        {
            this.bPlaySkill = true;
            base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Call", this.speedratio - 1f);
            base.m_Entity.m_AniCtrl.SendEvent("Call", true);
        }
        float num = 1f - (((this.delaytime + this.alltime) - this.starttime) / this.alltime);
        float num2 = (((-4f * this.g) * (num - 0.5f)) * (num - 0.5f)) + this.g;
        if (this.starttime < (this.delaytime + this.halftime))
        {
            base.m_Entity.SetPosition(new Vector3(this.startpos.x + (this.perendx * num), this.startpos.y + num2, this.startpos.z + (this.perendz * num)));
        }
        else if (this.starttime < (this.delaytime + this.alltime))
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
        this.speedratio = this.alltime / 1.3f;
        this.delaytime = this.alltime * 0.23f;
        this.starttime = 0f;
        this.halftime = this.alltime / 2f;
        this.startpos = base.m_Entity.position;
        this.target = GameLogic.Self;
        this.RandomItem(out this.endx, out this.endz);
        this.perendx = this.endx - base.m_Entity.position.x;
        this.perendz = this.endz - base.m_Entity.position.z;
        base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Skill", this.speedratio - 1f);
        base.m_Entity.m_AniCtrl.SendEvent("Skill", false);
    }

    protected override void OnUpdate()
    {
        this.MoveNormal();
    }

    private void RandomItem(out float endx, out float endz)
    {
        endx = this.target.position.x;
        endz = this.target.position.z;
    }
}

