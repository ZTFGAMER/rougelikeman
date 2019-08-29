using Dxx.Util;
using System;

public class Weapon8002 : WeaponBase
{
    protected override void OnInit()
    {
        base.OnInit();
    }

    protected override void OnInstall()
    {
        base.OnInstall();
        object[] args = new object[] { "AttackModify%" };
        base.m_Entity.m_EntityData.ExcuteAttributes(Utils.FormatString("{0} - 25", args));
    }

    protected override void OnUnInstall()
    {
        base.OnUnInstall();
        object[] args = new object[] { "AttackModify%" };
        base.m_Entity.m_EntityData.ExcuteAttributes(Utils.FormatString("{0} + 25", args));
    }
}

