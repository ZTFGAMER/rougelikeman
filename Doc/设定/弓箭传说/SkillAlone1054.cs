using System;

public class SkillAlone1054 : SkillAloneBase
{
    protected override void OnInstall()
    {
        base.m_Entity.m_EntityData.Modify_TurnTableCount(1);
    }

    protected override void OnUninstall()
    {
        base.m_Entity.m_EntityData.Modify_TurnTableCount(-1);
    }
}

