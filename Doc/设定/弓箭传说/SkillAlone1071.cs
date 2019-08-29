using Dxx.Util;
using System;
using UnityEngine;

public class SkillAlone1071 : SkillAloneBase
{
    private long clockindex;
    private float delaytime;
    private float hitratio;

    private void OnAttack()
    {
        Vector3 position = base.m_Entity.position;
        float angle = GameLogic.Random((float) 0f, (float) 360f);
        Vector3 vector2 = new Vector3(MathDxx.Sin(angle), 1f, MathDxx.Cos(angle)) * 2f;
        float rota = GameLogic.Random((float) 0f, (float) 360f);
        GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0xbc4, position + vector2, rota).mBulletTransmit.SetAttack((long) MathDxx.CeilToInt(this.hitratio * base.m_Entity.m_EntityData.GetAttackBase()));
    }

    protected override void OnInstall()
    {
        this.delaytime = float.Parse(base.m_SkillData.Args[0]);
        this.hitratio = float.Parse(base.m_SkillData.Args[1]);
        this.clockindex = TimeClock.Register("SkillAlone1071", this.delaytime, new Action(this.OnAttack));
    }

    protected override void OnUninstall()
    {
        TimeClock.Unregister(this.clockindex);
    }
}

