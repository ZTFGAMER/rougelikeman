using System;

public class SkillAlone1061 : SkillAloneBase
{
    protected override void OnInstall()
    {
        base.m_Entity.m_EntityData.Modify_OnlyDemon(1);
    }

    protected override void OnUninstall()
    {
        base.m_Entity.m_EntityData.Modify_OnlyDemon(-1);
    }
}

