using Dxx.Util;
using System;
using UnityEngine;

public class Weapon5011 : WeaponBase
{
    private float range = 45f;

    protected override void OnAttack(object[] args)
    {
        for (int i = 0; i < 30; i++)
        {
            base.action.AddActionDelegate(() => base.CreateBulletOverride(Vector3.zero, GameLogic.Random(-this.range, this.range)));
            base.action.AddActionWait(GameLogic.Random((float) 0f, (float) 0.1f));
        }
        base.action.AddActionDelegate(() => Updater.RemoveUpdate("Weapon5011", new Action<float>(this.OnUpdate)));
    }

    protected override void OnInstall()
    {
        Updater.AddUpdate("Weapon5011", new Action<float>(this.OnUpdate), false);
    }

    protected override void OnUnInstall()
    {
        Updater.RemoveUpdate("Weapon5011", new Action<float>(this.OnUpdate));
    }

    private void OnUpdate(float delta)
    {
        if (((base.m_Entity != null) && !base.m_Entity.GetIsDead()) && (base.m_Entity.m_HatredTarget != null))
        {
            float x = base.m_Entity.m_HatredTarget.position.x - base.m_Entity.position.x;
            float y = base.m_Entity.m_HatredTarget.position.z - base.m_Entity.position.z;
            float angle = Utils.getAngle(x, y);
            base.m_Entity.m_AttackCtrl.RotateHero(angle);
        }
    }
}

