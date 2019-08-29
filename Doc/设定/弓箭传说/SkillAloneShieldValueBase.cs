using System;

public class SkillAloneShieldValueBase : SkillAloneBase
{
    private long shieldvalue;

    protected override void OnInstall()
    {
        this.shieldvalue = long.Parse(base.m_SkillData.Args[0]);
        base.m_Entity.m_EntityData.ExcuteAttributes("AddShieldValue", this.shieldvalue);
    }

    protected override void OnUninstall()
    {
        base.m_Entity.m_EntityData.ExcuteAttributes("AddShieldValue", -this.shieldvalue);
    }

    protected void ResetShieldHitValue()
    {
        base.m_Entity.m_EntityData.ResetShieldHitValue();
    }
}

