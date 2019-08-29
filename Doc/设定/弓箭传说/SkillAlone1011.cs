using System;

public class SkillAlone1011 : SkillAloneBase
{
    private long value;

    protected override void OnInstall()
    {
        long.TryParse(base.m_Data.Args[0], out this.value);
        base.m_Entity.m_EntityData.attribute.HitVampirePercent.UpdateValuePercent(this.value);
    }

    protected override void OnUninstall()
    {
        base.m_Entity.m_EntityData.attribute.HitVampirePercent.UpdateValuePercent(-this.value);
    }
}

