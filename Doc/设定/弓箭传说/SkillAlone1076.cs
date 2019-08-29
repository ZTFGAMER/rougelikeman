using Dxx.Util;
using System;
using UnityEngine;

public class SkillAlone1076 : SkillAloneBase
{
    private long clockindex;
    private int bulletid;
    private float delaytime;
    private float hitratio;

    private void OnAttack()
    {
        EntityBase base2 = GameLogic.Release.Entity.FindCanAttackRandom(base.m_Entity);
        if (base2 != null)
        {
            float rota = Utils.getAngle(base2.position - base.m_Entity.position) + GameLogic.Random((float) -10f, (float) 10f);
            float num2 = Vector3.Distance(base2.position, base.m_Entity.position) - 1f;
            Vector3 pos = base2.position + (new Vector3(MathDxx.Sin(rota + 180f), 0f, MathDxx.Cos(rota + 180f)) * num2);
            pos.y = 1f;
            GameLogic.Release.Bullet.CreateBullet(base.m_Entity, this.bulletid, pos, rota).mBulletTransmit.SetAttack((long) MathDxx.CeilToInt(this.hitratio * base.m_Entity.m_EntityData.GetAttackBase()));
        }
    }

    protected override void OnInstall()
    {
        Debug.Log("1076 create");
        this.bulletid = int.Parse(base.m_SkillData.Args[0]);
        this.delaytime = float.Parse(base.m_SkillData.Args[1]);
        this.hitratio = float.Parse(base.m_SkillData.Args[2]);
        this.clockindex = TimeClock.Register("SkillAlone1076", this.delaytime, new Action(this.OnAttack));
    }

    protected override void OnUninstall()
    {
        TimeClock.Unregister(this.clockindex);
    }
}

