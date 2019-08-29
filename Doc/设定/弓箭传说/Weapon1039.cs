using Dxx.Util;
using System;
using UnityEngine;

public class Weapon1039 : WeaponBase
{
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
            this.effect.transform.SetParent(base.m_Entity.m_Body.LeftWeapon.transform);
            this.effect.transform.localPosition = Vector3.zero;
        }
    }

    protected override void OnAttack(object[] args)
    {
        int num = 5;
        for (int i = 0; i < num; i++)
        {
            base.action.AddActionDelegate(() => base.CreateBulletOverride(Vector3.zero, 0f));
            base.action.AddActionWait(0.3f);
        }
    }

    protected override void OnInstall()
    {
        this.CreateEffect();
        base.OnAttackStartEndAction = (Action) Delegate.Combine(base.OnAttackStartEndAction, new Action(this.AttackStartEnd));
    }

    protected override void OnUnInstall()
    {
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

    protected virtual string AttackEffect =>
        "WeaponHand1024Effect";
}

