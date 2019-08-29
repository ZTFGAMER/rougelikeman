using System;

public class SkillAlone1062 : SkillAloneBase
{
    protected override void OnInstall()
    {
        base.m_Entity.m_EntityData.Modify_BabyResistBullet(1);
    }

    protected override void OnUninstall()
    {
        base.m_Entity.m_EntityData.Modify_BabyResistBullet(-1);
    }
}

