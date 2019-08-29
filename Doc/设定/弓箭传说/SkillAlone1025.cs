using System;

public class SkillAlone1025 : SkillAloneBase
{
    protected override void OnInstall()
    {
        base.m_Entity.ChangeWeapon(0x1f42);
    }

    protected override void OnUninstall()
    {
    }
}

