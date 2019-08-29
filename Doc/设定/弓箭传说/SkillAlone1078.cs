using Dxx.Util;
using System;
using UnityEngine;

public class SkillAlone1078 : SkillAloneBase
{
    private long clockindex;
    private int bulletid;
    private int createweight;
    private float hitratio;

    private void OnAttack()
    {
        if (GameLogic.Random((float) 0f, (float) 100f) <= this.createweight)
        {
            GameLogic.Release.MapCreatorCtrl.RandomItem(base.m_Entity.position, 100, out float num, out float num2);
            Vector3 endpos = new Vector3(num, 0f, num2);
            GameLogic.Release.Bullet.CreateSlopeBullet(base.m_Entity, this.bulletid, endpos + new Vector3(0f, 21f, 0f), endpos).mBulletTransmit.SetAttack((long) MathDxx.CeilToInt(this.hitratio * base.m_Entity.m_EntityData.GetAttackBase()));
        }
    }

    protected override void OnInstall()
    {
        this.bulletid = int.Parse(base.m_SkillData.Args[0]);
        this.createweight = int.Parse(base.m_SkillData.Args[1]);
        this.hitratio = float.Parse(base.m_SkillData.Args[2]);
        base.m_Entity.Event_OnAttack = (Action) Delegate.Combine(base.m_Entity.Event_OnAttack, new Action(this.OnAttack));
    }

    protected override void OnUninstall()
    {
        base.m_Entity.Event_OnAttack = (Action) Delegate.Remove(base.m_Entity.Event_OnAttack, new Action(this.OnAttack));
    }
}

