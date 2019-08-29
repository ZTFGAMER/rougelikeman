using System;

public class Weapon1013 : WeaponBase
{
    protected override void OnInstall()
    {
        base.m_Entity.AddSkill(0x10c8e3, Array.Empty<object>());
    }

    protected override void OnUnInstall()
    {
        base.m_Entity.RemoveSkill(0x10c8e3);
    }
}

