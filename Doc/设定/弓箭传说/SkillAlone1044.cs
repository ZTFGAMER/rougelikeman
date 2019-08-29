using Dxx.Util;
using System;
using UnityEngine;

public class SkillAlone1044 : SkillAloneBase
{
    private GameObject good;
    private Transform child;
    private float time;
    private float delaytime;
    private float hitratio;
    private float range;
    private long clockindex;

    private void CreateBullet(Vector3 startpos, Vector3 endpos)
    {
        BulletBase base2 = GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0xbbc);
        base2.transform.position = startpos;
        base2.transform.LookAt(endpos);
        BulletTransmit bullet = new BulletTransmit(base.m_Entity, 0xbbc, true);
        base2.SetBulletAttribute(bullet);
    }

    private void CreateSkillAlone()
    {
        object[] args = new object[] { base.ClassID };
        this.good = GameLogic.EffectGet(Utils.FormatString("Game/SkillPrefab/SkillAlone{0}", args));
        this.good.transform.SetParent(base.m_Entity.m_Body.EffectMask.transform);
        this.good.transform.localPosition = Vector3.zero;
        this.good.transform.localRotation = Quaternion.identity;
        this.good.transform.localScale = Vector3.one;
        this.child = this.good.transform.Find("child");
    }

    private void OnAttack()
    {
        EntityBase base2 = GameLogic.Release.Entity.GetNearEntity(base.m_Entity, 100f, false);
        if (base2 != null)
        {
            this.CreateBullet(this.child.position, base2.position);
        }
    }

    protected override void OnInstall()
    {
        this.delaytime = float.Parse(base.m_SkillData.Args[0]);
        this.hitratio = float.Parse(base.m_SkillData.Args[1]);
        this.range = float.Parse(base.m_SkillData.Args[2]);
        this.CreateSkillAlone();
        this.clockindex = TimeClock.Register("SkillAlone1044", this.delaytime, new Action(this.OnAttack));
    }

    protected override void OnUninstall()
    {
        Object.Destroy(this.good);
        TimeClock.Unregister(this.clockindex);
    }
}

