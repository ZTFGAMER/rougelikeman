using Dxx.Util;
using System;
using UnityEngine;

public class Weapon1024 : WeaponBase
{
    protected string AttackEffect = "WeaponHand1024Effect";
    protected Transform effectparent;
    private GameObject effect;

    private void AttackStartEnd()
    {
        this.RemoveEffect();
    }

    private void CreateEffect()
    {
        object[] args = new object[] { "Game/WeaponHand/", this.AttackEffect };
        this.effect = GameLogic.EffectGet(Utils.GetString(args));
        if (this.effect != null)
        {
            this.effect.transform.SetParent(this.effectparent);
            this.effect.transform.localPosition = Vector3.zero;
        }
    }

    protected override void OnInstall()
    {
        base.OnInstall();
        if (this.effectparent == null)
        {
            this.effectparent = base.m_Entity.m_Body.LeftWeapon.transform;
        }
        this.CreateEffect();
        base.OnAttackStartEndAction = (Action) Delegate.Combine(base.OnAttackStartEndAction, new Action(this.AttackStartEnd));
    }

    protected override void OnUnInstall()
    {
        base.OnUnInstall();
        this.RemoveEffect();
        base.OnAttackStartEndAction = (Action) Delegate.Remove(base.OnAttackStartEndAction, new Action(this.AttackStartEnd));
    }

    private void RemoveEffect()
    {
        if (this.effect != null)
        {
            GameLogic.EffectCache(this.effect);
            this.effect = null;
        }
    }
}

