using System;

public class Weapon1003 : WeaponBase
{
    private void OnAttack1003()
    {
        base.m_Entity.WeaponHandShow(false);
        ActionBasic.ActionWait action = new ActionBasic.ActionWait {
            waitTime = 1f
        };
        base.action.AddAction(action);
        ActionBasic.ActionDelegate delegate2 = new ActionBasic.ActionDelegate {
            action = () => base.m_Entity.WeaponHandShow(true)
        };
        base.action.AddAction(delegate2);
    }

    protected override void OnInstall()
    {
        base.OnAttackStartEndAction = new Action(this.OnAttack1003);
        base.ParabolaSize = 2;
    }

    protected override void OnUnInstall()
    {
        base.OnAttackStartEndAction = null;
    }
}

