using System;
using UnityEngine;

public class Weapon1037 : WeaponBase
{
    private float range = 3f;

    protected override void OnAttack(object[] args)
    {
        int num = 3;
        for (int i = 0; i < num; i++)
        {
            base.action.AddActionDelegate(delegate {
                Transform transform = base.CreateBullet(Vector3.zero, 0f);
                transform.position = base.Target.position + new Vector3(GameLogic.Random(-this.range, this.range), 0f, GameLogic.Random(-this.range, this.range));
                BulletBase component = transform.GetComponent<BulletBase>();
                component.SetBulletAttribute(new BulletTransmit(base.m_Entity, base.BulletID, false));
                component.SetTarget(base.Target, base.ParabolaSize);
            });
            if (i < (num - 1))
            {
                base.action.AddActionWait(0.2f);
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

