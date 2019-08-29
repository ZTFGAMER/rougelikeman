using System;
using UnityEngine;

public class Weapon1040 : WeaponBase
{
    protected override void OnAttack(object[] args)
    {
        int num = 4;
        float num2 = 360f / ((float) num);
        for (int i = 0; i < num; i++)
        {
            base.CreateBulletOverride(Vector3.zero, ((num2 * i) - base.m_Entity.eulerAngles.y) + 45f);
        }
    }

    protected override void OnInit()
    {
        base.OnInit();
    }
}

