using System;
using UnityEngine;

public class Weapon1021 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        Transform transform = base.CreateBullet(Vector3.zero, 0f);
        transform.position = base.Target.position;
        BulletBase component = transform.GetComponent<BulletBase>();
        component.SetBulletAttribute(new BulletTransmit(base.m_Entity, base.BulletID, false));
        component.SetTarget(base.Target, base.ParabolaSize);
    }
}

