using Dxx.Util;
using System;
using UnityEngine;

public class SkillAlone1046 : SkillAloneBase
{
    private float angle;
    private Vector3 offsetpos;

    private void CreateBullet(float dis)
    {
        Bullet3014 bullet = GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0xbc6) as Bullet3014;
        bullet.transform.position = base.m_Entity.position + (this.offsetpos * dis);
        bullet.transform.rotation = Quaternion.Euler(0f, base.m_Entity.eulerAngles.y, 0f);
        BulletTransmit transmit = new BulletTransmit(base.m_Entity, 0xbc6, true);
        bullet.SetBulletAttribute(transmit);
    }

    private void OnAttack()
    {
        this.angle = base.m_Entity.eulerAngles.y + 90f;
        this.offsetpos = new Vector3(MathDxx.Sin(this.angle), 0f, MathDxx.Cos(this.angle));
        this.CreateBullet(-1f);
        this.CreateBullet(1f);
    }

    protected override void OnInstall()
    {
        base.m_Entity.Event_OnAttack = (Action) Delegate.Combine(base.m_Entity.Event_OnAttack, new Action(this.OnAttack));
    }

    protected override void OnUninstall()
    {
        base.m_Entity.Event_OnAttack = (Action) Delegate.Remove(base.m_Entity.Event_OnAttack, new Action(this.OnAttack));
    }
}

