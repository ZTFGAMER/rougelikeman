using Dxx.Util;
using System;

public class SkillAlone1042 : SkillAloneBase
{
    private float time;
    private float delaytime;
    private float hitratio;
    private float range;
    private long clockindex;

    private void CreateSkillAlone()
    {
    }

    private void OnAttack()
    {
        EntityBase target = GameLogic.Release.Entity.GetNearEntity(base.m_Entity, this.range, false);
        if (target != null)
        {
            GameLogic.Release.MapEffect.Get("Game/SkillPrefab/SkillAlone1042_One").transform.position = target.m_Body.EffectMask.transform.position;
            long beforehit = -((long) MathDxx.CeilToInt(base.m_Entity.m_EntityData.GetAttackBase() * this.hitratio));
            GameLogic.SendHit_Skill(target, beforehit);
        }
    }

    protected override void OnInstall()
    {
        this.delaytime = float.Parse(base.m_SkillData.Args[0]);
        this.hitratio = float.Parse(base.m_SkillData.Args[1]);
        this.range = float.Parse(base.m_SkillData.Args[2]);
        this.clockindex = TimeClock.Register("SkillAlone1042", this.delaytime, new Action(this.OnAttack));
    }

    protected override void OnUninstall()
    {
        TimeClock.Unregister(this.clockindex);
    }
}

