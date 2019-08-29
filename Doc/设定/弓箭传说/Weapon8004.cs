using Dxx.Util;
using System;

public class Weapon8004 : Weapon1001
{
    protected override void OnInstall()
    {
        base.OnInstall();
        object[] args = new object[] { "AttackModify%" };
        base.m_Entity.m_EntityData.ExcuteAttributes(Utils.FormatString("{0} - 15", args));
    }

    protected override void OnUnInstall()
    {
        base.OnUnInstall();
        object[] args = new object[] { "AttackModify%" };
        base.m_Entity.m_EntityData.ExcuteAttributes(Utils.FormatString("{0} + 15", args));
    }
}

