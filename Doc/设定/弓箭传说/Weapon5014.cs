using System;
using UnityEngine;

public class Weapon5014 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        for (int i = 0; i < 3; i++)
        {
            base.action.AddActionDelegate(() => base.CreateBulletOverride(Vector3.zero, 0f));
            base.action.AddActionWait(0.3f);
        }
    }

    protected override void OnInstall()
    {
        base.m_Entity.AddSkill(0x10c8e3, Array.Empty<object>());
    }

    protected override void OnUnInstall()
    {
        base.m_Entity.RemoveSkill(0x10c8e3);
    }
}

