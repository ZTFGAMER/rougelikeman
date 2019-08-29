using System;
using UnityEngine;

public class Weapon5047 : WeaponBase
{
    private int count = 4;

    protected override void OnAttack(object[] args)
    {
        for (int i = 0; i < this.count; i++)
        {
            base.action.AddActionDelegate(delegate {
                float[] singleArray1 = new float[] { 100011f };
                base.CreateBulletOverride(Vector3.zero, 0f).SetArgs(singleArray1);
                float[] singleArray2 = new float[] { 100012f };
                base.CreateBulletOverride(Vector3.zero, 0f).SetArgs(singleArray2);
                float[] singleArray3 = new float[] { 100011f };
                base.CreateBulletOverride(Vector3.zero, 60f).SetArgs(singleArray3);
                float[] singleArray4 = new float[] { 100012f };
                base.CreateBulletOverride(Vector3.zero, 60f).SetArgs(singleArray4);
                float[] singleArray5 = new float[] { 100011f };
                base.CreateBulletOverride(Vector3.zero, -60f).SetArgs(singleArray5);
                float[] singleArray6 = new float[] { 100012f };
                base.CreateBulletOverride(Vector3.zero, -60f).SetArgs(singleArray6);
            });
            if (i < (this.count - 1))
            {
                base.action.AddActionWait(0.15f);
            }
        }
    }

    protected override void OnInit()
    {
        base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", -0.5f);
        base.OnInit();
    }

    protected override void OnUnInstall()
    {
        base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 0.5f);
        base.OnUnInstall();
    }
}

