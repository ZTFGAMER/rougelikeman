using System;

public class Weapon5017 : Weapon1020
{
    protected override void OnInstall()
    {
        base.OnInstall();
        base.m_Entity.AddSkill(0x10c8e3, Array.Empty<object>());
    }

    protected override void OnUnInstall()
    {
        base.OnUnInstall();
        base.m_Entity.RemoveSkill(0x10c8e3);
    }
}

