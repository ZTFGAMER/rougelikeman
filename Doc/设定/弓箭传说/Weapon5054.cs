using System;
using UnityEngine;

public class Weapon5054 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        int num = 0x17;
        for (int i = 0; i < num; i++)
        {
            base.action.AddActionDelegate(delegate {
                Transform transform = base.CreateBullet(Vector3.zero, 0f);
                transform.position = GameLogic.Release.MapCreatorCtrl.RandomPosition();
                BulletBase component = transform.GetComponent<BulletBase>();
                component.SetBulletAttribute(new BulletTransmit(base.m_Entity, base.BulletID, false));
                component.SetTarget(base.Target, base.ParabolaSize);
            });
            if (i < (num - 1))
            {
                base.action.AddActionWait(0.05f);
            }
        }
    }

    protected override void OnInstall()
    {
    }

    protected override void OnUnInstall()
    {
    }
}

