using System;

public class SkillAlone1063 : SkillAloneBase
{
    protected override void OnInstall()
    {
        base.m_Entity.m_EntityData.Modify_FrontShield(1);
    }

    protected override void OnUninstall()
    {
        base.m_Entity.m_EntityData.Modify_FrontShield(-1);
    }
}

