using Dxx.Util;
using System;
using UnityEngine;

public class SkillAlone1037 : SkillAloneBase
{
    private float delaytime;
    private float time;
    private int index;

    private void CreateBullet(float angle)
    {
        GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x3e9, base.m_Entity.position + new Vector3(0f, 1f, 0f), angle);
    }

    private void CreateBullets()
    {
        float currentAngle = base.m_Entity.m_AttackCtrl.GetCurrentAngle();
        this.CreateBullet(currentAngle - 45f);
        this.CreateBullet(currentAngle);
        this.CreateBullet(currentAngle + 45f);
    }

    protected override void OnInstall()
    {
        this.delaytime = float.Parse(base.m_SkillData.Args[0]);
        Updater.AddUpdate("SkillAlone1037", new Action<float>(this.OnUpdate), false);
    }

    protected override void OnUninstall()
    {
        Updater.RemoveUpdate("SkillAlone1037", new Action<float>(this.OnUpdate));
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
                this.CreateBullets();
            }
        }
    }
}

