using System;

public class SkillAlone1020 : SkillAloneBase
{
    protected override void OnInstall()
    {
        base.m_Entity.ChangeWeapon(0x1f44);
    }

    protected override void OnUninstall()
    {
    }
}

