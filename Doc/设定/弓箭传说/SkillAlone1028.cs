using System;

public class SkillAlone1028 : SkillAloneBase
{
    private float percent;

    protected override void OnInstall()
    {
        this.percent = float.Parse(base.m_SkillData.Args[0]);
        base.m_Entity.m_EntityData.Modify_HitCreate2(1, this.percent);
    }

    protected override void OnUninstall()
    {
        base.m_Entity.m_EntityData.Modify_HitCreate2(-1, this.percent);
    }
}

