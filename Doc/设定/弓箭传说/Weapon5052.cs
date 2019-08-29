using System;
using UnityEngine;

public class Weapon5052 : WeaponBase
{
    private float angle = 15f;

    protected override void OnAttack(object[] args)
    {
        int num = 7;
        for (int i = 0; i < num; i++)
        {
            base.action.AddActionDelegate(() => base.CreateBulletOverride(Vector3.zero, 0f));
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

