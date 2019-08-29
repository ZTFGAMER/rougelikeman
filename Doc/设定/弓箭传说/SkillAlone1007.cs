using System;

public class SkillAlone1007 : SkillAloneBase
{
    protected override void OnInstall()
    {
        base.m_Entity.m_EntityData.Modify_FlyStone(1);
    }

    protected override void OnUninstall()
    {
        base.m_Entity.m_EntityData.Modify_FlyStone(-1);
    }
}

