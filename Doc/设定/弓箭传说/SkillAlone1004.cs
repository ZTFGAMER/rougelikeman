using System;
using UnityEngine;

public class SkillAlone1004 : SkillAloneBase
{
    protected override void OnInstall()
    {
        base.m_Entity.OnMiss = (Action) Delegate.Combine(base.m_Entity.OnMiss, new Action(this.OnMiss));
    }

    private void OnMiss()
    {
        for (int i = 0; i < 4; i++)
        {
            GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0xbc2, base.m_Entity.position + new Vector3(0f, 1f, 0f), (float) (i * 90));
        }
    }

    protected override void OnUninstall()
    {
        base.m_Entity.OnMiss = (Action) Delegate.Remove(base.m_Entity.OnMiss, new Action(this.OnMiss));
    }
}

