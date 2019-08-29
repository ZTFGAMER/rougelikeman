using Dxx.Util;
using System;

public class Weapon8001 : WeaponBase
{
    private int throughID = 0x124f81;
    private bool change1002;

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

    protected override void OnInstall()
    {
        base.OnInstall();
        object[] args = new object[] { "AttackModify%" };
        base.m_Entity.m_EntityData.ExcuteAttributes(Utils.FormatString("{0} - 25", args));
        object[] objArray2 = new object[] { 0.55f };
        base.m_Entity.AddSkillAttribute(this.throughID, objArray2);
        base.OnAttackEndStartAction = new Action(this.OnAttackEndStartActions);
        base.OnAttackEndEndAction = new Action(this.OnAttackEndEndActions);
        base.OnAttackInterruptAction = new Action(this.OnAttackInterruptActions);
    }

    protected override void OnUnInstall()
    {
        base.OnUnInstall();
        object[] args = new object[] { "AttackModify%" };
        base.m_Entity.m_EntityData.ExcuteAttributes(Utils.FormatString("{0} + 25", args));
        base.OnAttackEndStartAction = null;
        base.OnAttackEndEndAction = null;
        base.OnAttackInterruptAction = null;
        base.m_Entity.RemoveSkill(this.throughID);
    }
}

