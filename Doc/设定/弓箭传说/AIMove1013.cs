using Dxx.Util;
using System;
using UnityEngine;

public class AIMove1013 : AIMoveBase
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
    private bool bPlaySkill;

    public AIMove1013(EntityBase entity) : base(entity)
    {
        this.g = 6f;
        this.alltime = 1f;
    }

    private void MoveNormal()
    {
        this.starttime += Updater.delta;
        if (this.starttime >= this.delaytime)
        {
            this.OnFly();
        }
    }

    private void OnFly()
    {
        if (!this.bPlaySkill)
        {
            this.bPlaySkill = true;
            base.m_Entity.m_AniCtrl.SendEvent("Call", true);
            GameLogic.Hold.Sound.PlayMonsterSkill(0x4dd1e2, base.m_Entity.position);
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
            if (base.m_Entity.m_Data.WeaponID != 0)
            {
                base.m_Entity.ChangeWeapon(base.m_Entity.m_Data.WeaponID);
            }
            else
            {
                object[] args = new object[] { base.m_Entity.m_Data.WeaponID };
                SdkManager.Bugly_Report("AIMove1013.cs", Utils.FormatString("AIMove1013 {0} weapon is 0", args));
            }
            if (base.m_Entity.m_Weapon != null)
            {
                base.m_Entity.m_Weapon.Attack(Array.Empty<object>());
            }
            base.End();
        }
    }

    protected override void OnInitBase()
    {
        this.bPlaySkill = false;
        this.delaytime = 0.3f;
        this.starttime = 0f;
        this.halftime = this.alltime / 2f;
        this.startpos = base.m_Entity.position;
        this.target = GameLogic.Self;
        GameLogic.Release.MapCreatorCtrl.RandomItem(base.m_Entity, 2, out this.endx, out this.endz);
        this.perendx = this.endx - base.m_Entity.position.x;
        this.perendz = this.endz - base.m_Entity.position.z;
        base.m_Entity.m_AttackCtrl.RotateHero((float) 180f);
        base.m_Entity.m_AniCtrl.SendEvent("Skill", false);
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

