using System;

public class SkillAlone1009 : SkillAloneBase
{
    protected override void OnInstall()
    {
        base.m_Entity.m_EntityData.Modify_BulletThroughCount(1);
    }

    protected override void OnUninstall()
    {
        base.m_Entity.m_EntityData.Modify_BulletThroughCount(-1);
    }
}

