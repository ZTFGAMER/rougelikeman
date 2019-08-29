using System;

public class SkillAlone1058 : SkillAloneBase
{
    protected override void OnInstall()
    {
        base.m_Entity.m_EntityData.Modify_BulletScale(1);
    }

    protected override void OnUninstall()
    {
        base.m_Entity.m_EntityData.Modify_BulletScale(-1);
    }
}

