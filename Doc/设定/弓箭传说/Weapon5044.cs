using Dxx.Util;
using System;
using UnityEngine;

public class Weapon5044 : WeaponBase
{
    private int count = 7;
    private bool bRotate;

    protected override void OnAttack(object[] args)
    {
        for (int i = 0; i < this.count; i++)
        {
            base.action.AddActionDelegate(() => base.CreateBulletOverride(Vector3.zero, GameLogic.Random((float) -45f, (float) 45f)));
            if (i < (this.count - 1))
            {
                base.action.AddActionWait(0.2f);
            }
        }
        base.action.AddActionDelegate(() => Updater.RemoveUpdate("Weapon5044", new Action<float>(this.OnUpdate)));
    }

    protected override void OnInstall()
    {
        this.bRotate = true;
        Updater.AddUpdate("Weapon5044", new Action<float>(this.OnUpdate), false);
    }

    protected override void OnUnInstall()
    {
        Updater.RemoveUpdate("Weapon5044", new Action<float>(this.OnUpdate));
    }

    private void OnUpdate(float delta)
    {
        base.m_Entity.m_AttackCtrl.RotateHero(Utils.getAngle(base.m_Entity.m_HatredTarget.position - base.m_Entity.position));
    }
}

