using System;

public class SkillAlone1041 : SkillAloneBase
{
    protected override void OnInstall()
    {
        base.m_Entity.m_EntityData.Modify_ButtetSputter(1);
    }

    protected override void OnUninstall()
    {
        base.m_Entity.m_EntityData.Modify_ButtetSputter(-1);
    }
}

