using System;

public class SkillAlone1008 : SkillAloneBase
{
    protected override void OnInstall()
    {
        base.m_Entity.m_EntityData.Modify_FlyWater(1);
    }

    protected override void OnUninstall()
    {
        base.m_Entity.m_EntityData.Modify_FlyWater(-1);
    }
}

