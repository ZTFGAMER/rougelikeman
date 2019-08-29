using System;

public class SkillAlone1040 : SkillAloneBase
{
    protected override void OnInstall()
    {
        base.m_Entity.m_EntityData.Modify_BulletLineCount(1);
    }

    protected override void OnUninstall()
    {
        base.m_Entity.m_EntityData.Modify_BulletLineCount(-1);
    }
}

