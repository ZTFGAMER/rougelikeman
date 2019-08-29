using Dxx.Util;
using System;
using UnityEngine;

public class Weapon5073 : Weapon1024
{
    private float angle = 45f;
    private int percount = 3;
    private BulletRedLineCtrls ctrl = new BulletRedLineCtrls();

    protected override void OnAttack(object[] args)
    {
        this.ctrl.Deinit();
        for (int i = 0; i < this.percount; i++)
        {
            base.CreateBulletOverride(Vector3.zero, Utils.GetBulletAngle(i, this.percount, 90f));
        }
    }

    protected override void OnInstall()
    {
        base.AttackEffect = "WeaponHand1066Effect";
        float[] angles = new float[3];
        angles[0] = -45f;
        angles[2] = 45f;
        this.ctrl.Init(base.m_Entity, this.percount, angles);
        base.OnInstall();
    }

    protected override void OnUnInstall()
    {
        this.ctrl.Deinit();
        base.OnUnInstall();
    }
}

