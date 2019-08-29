using System;

public class SkillAlone1026 : SkillAloneBase
{
    protected override void OnInstall()
    {
        base.m_Entity.ChangeWeapon(0x1f43);
    }

    protected override void OnUninstall()
    {
    }
}

