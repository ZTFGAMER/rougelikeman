using Dxx.Util;
using System;

public class Weapon8003 : WeaponBase
{
    private void OnAttackEndEndActions()
    {
        base.m_Entity.WeaponHandShow(true);
    }

    private void OnAttackEndStartActions()
    {
        base.m_Entity.WeaponHandShow(false);
    }

    private void OnAttackInterruptActions()
    {
        base.m_Entity.WeaponHandShow(true);
    }

    protected override void OnInit()
    {
        base.OnInit();
    }

    protected override void OnInstall()
    {
        base.OnInstall();
        object[] args = new object[] { "AttackModify%" };
        base.m_Entity.m_EntityData.ExcuteAttributes(Utils.FormatString("{0} + 45", args));
        base.OnAttackEndStartAction = new Action(this.OnAttackEndStartActions);
        base.OnAttackEndEndAction = new Action(this.OnAttackEndEndActions);
        base.OnAttackInterruptAction = new Action(this.OnAttackInterruptActions);
    }

    protected override void OnUnInstall()
    {
        base.OnUnInstall();
        object[] args = new object[] { "AttackModify%" };
        base.m_Entity.m_EntityData.ExcuteAttributes(Utils.FormatString("{0} - 45", args));
        base.OnAttackEndStartAction = null;
        base.OnAttackEndEndAction = null;
        base.OnAttackInterruptAction = null;
    }
}

