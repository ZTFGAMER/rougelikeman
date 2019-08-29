using System;

public class SkillAlone1006 : SkillAloneBase
{
    private float ratio;

    protected override void OnInstall()
    {
        float.TryParse(base.m_Data.Args[0], out this.ratio);
        base.m_Entity.m_EntityData.Modify_HP2Attack(this.ratio);
    }

    protected override void OnUninstall()
    {
        base.m_Entity.m_EntityData.Modify_HP2Attack(-this.ratio);
    }
}

