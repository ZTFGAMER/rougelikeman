using System;

public class SkillAlone2001 : SkillAloneBase
{
    protected override void OnInstall()
    {
        base.m_Entity.m_EntityData.Modify_MissHP(true);
    }

    protected override void OnUninstall()
    {
        base.m_Entity.m_EntityData.Modify_MissHP(false);
    }
}

