using Dxx.Util;
using System;
using UnityEngine;

public class SkillAlone1036 : SkillAloneBase
{
    private float delaytime;
    private float time;
    private int index;

    private void CreateBullet()
    {
        if (((base.m_Entity != null) && (base.m_Entity.m_AttackCtrl != null)) && (base.m_Entity.m_Body != null))
        {
            float currentAngle = base.m_Entity.m_AttackCtrl.GetCurrentAngle();
            BulletBase base2 = GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0xbbb, Vector3.zero, currentAngle);
            base2.transform.SetParent(base.m_Entity.m_Body.transform);
            base2.transform.localPosition = new Vector3(0f, 1f, 1f);
        }
    }

    protected override void OnInstall()
    {
        this.delaytime = float.Parse(base.m_SkillData.Args[0]);
        Updater.AddUpdate("SkillAlone1036", new Action<float>(this.OnUpdate), false);
    }

    protected override void OnUninstall()
    {
        Updater.RemoveUpdate("SkillAlone1036", new Action<float>(this.OnUpdate));
    }

    private void OnUpdate(float delta)
    {
        if (base.m_Entity.m_MoveCtrl.GetMoving())
        {
            this.time += delta;
            if (this.time >= this.delaytime)
            {
                this.time -= this.delaytime;
                this.index++;
                this.CreateBullet();
            }
        }
    }
}

