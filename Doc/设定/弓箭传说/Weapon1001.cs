using System;
using UnityEngine;

public class Weapon1001 : WeaponBase
{
    private Animation weaponAni;
    private const string PrevAction = "weapon1001_prev";
    private const string EndAction = "weapon1001_end";
    private const string ResetAction = "weapon1001_reset";

    public override void AttackJoyTouchDown()
    {
    }

    public override void AttackJoyTouchUp()
    {
    }

    private void OnAttackEndStartActions()
    {
        this.weaponAni.Play("weapon1001_end");
    }

    private void OnAttackInterruptActions()
    {
        this.weaponAni.Play("weapon1001_reset");
    }

    private void OnAttackStartStartActions()
    {
        this.weaponAni.Play("weapon1001_prev");
    }

    protected override void OnInstall()
    {
        base.OnInstall();
        this.weaponAni = base.m_Entity.m_WeaponHand.transform.Find("child/gong").GetComponent<Animation>();
        this.weaponAni.enabled = true;
        base.OnAttackStartStartAction = new Action(this.OnAttackStartStartActions);
        base.OnAttackEndStartAction = new Action(this.OnAttackEndStartActions);
        base.OnAttackInterruptAction = new Action(this.OnAttackInterruptActions);
    }

    protected override void OnUnInstall()
    {
        if (this.weaponAni != null)
        {
            this.weaponAni.enabled = false;
        }
        base.OnUnInstall();
        base.OnAttackStartStartAction = null;
        base.OnAttackEndStartAction = null;
        base.OnAttackInterruptAction = null;
    }
}

