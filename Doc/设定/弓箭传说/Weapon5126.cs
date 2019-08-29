using System;
using System.Collections.Generic;
using UnityEngine;

public class Weapon5126 : WeaponBase
{
    private List<Vector3> mPosList = new List<Vector3>();
    private float length = 3f;

    protected override void OnAttack(object[] args)
    {
        this.mPosList.Clear();
        this.mPosList.Add(base.Target.position);
        this.mPosList.Add(base.Target.position + new Vector3(-this.length, 0f, this.length));
        this.mPosList.Add(base.Target.position + new Vector3(-this.length, 0f, -this.length));
        this.mPosList.Add(base.Target.position + new Vector3(this.length, 0f, this.length));
        this.mPosList.Add(base.Target.position + new Vector3(this.length, 0f, -this.length));
        int num = 0;
        int count = this.mPosList.Count;
        while (num < count)
        {
            Transform transform = base.CreateBullet(Vector3.zero, 0f);
            transform.position = this.mPosList[num];
            BulletBase component = transform.GetComponent<BulletBase>();
            component.SetBulletAttribute(new BulletTransmit(base.m_Entity, base.BulletID, false));
            component.SetTarget(base.Target, base.ParabolaSize);
            num++;
        }
    }
}

