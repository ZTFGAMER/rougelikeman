using System;
using UnityEngine;

public class SkillAlone1074 : SkillAloneBase
{
    private int attack = 20;

    private void CreateBullet(float offsetangle)
    {
        if (base.m_Entity.m_HatredTarget != null)
        {
            Vector3 position = base.m_Entity.m_HatredTarget.position;
            Bullet3006 bullet = GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0xbbe) as Bullet3006;
            bullet.transform.position = base.m_Entity.m_Body.LeftBullet.transform.position;
            bullet.transform.rotation = Quaternion.Euler(0f, base.m_Entity.eulerAngles.y + offsetangle, 0f);
            BulletTransmit transmit = new BulletTransmit(base.m_Entity, 0xbbe, true);
            bullet.SetBulletAttribute(transmit);
            bullet.SetEndPos(position, offsetangle);
        }
    }

    private void OnAttack()
    {
        float offsetangle = GameLogic.Random(160, 180);
        this.CreateBullet(offsetangle);
        this.CreateBullet(-offsetangle);
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

