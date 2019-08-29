using System;
using UnityEngine;

public class Weapon1020 : WeaponBase
{
    protected void CreateBullet1020(float angle)
    {
        BulletBase base2 = base.CreateBulletOverride(Vector3.zero, angle);
        base2.mBulletTransmit.mThroughEnemy = 1;
        base2.mBulletTransmit.mThroughRatio = 1f;
        base2.SetTarget(base.Target, base.ParabolaSize);
        base2.OnBulletCache = base.OnBulletCache;
    }

    protected override void OnAttack(object[] args)
    {
        this.CreateBullet1020(0f);
    }

    private void OnAttack1003()
    {
        base.m_Entity.WeaponHandShow(false);
    }

    private void OnBulletCaches()
    {
        base.m_Entity.WeaponHandShow(true);
        base.bAttackEndActionEnd = true;
    }

    protected override void OnInstall()
    {
        base.SetDizzyCantRemove();
        base.OnAttackStartEndAction = new Action(this.OnAttack1003);
        base.OnBulletCache = new Action(this.OnBulletCaches);
        base.bAttackEndActionEnd = false;
    }

    protected override void OnUnInstall()
    {
        base.OnBulletCache = null;
        base.OnAttackStartEndAction = null;
    }
}

