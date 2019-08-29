using System;

public class SkillAlone1024 : SkillAloneBase
{
    protected override void OnInstall()
    {
        base.m_Entity.ChangeWeapon(0x1f41);
    }

    protected override void OnUninstall()
    {
    }
}

