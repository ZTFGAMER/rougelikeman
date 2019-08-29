using System;
using UnityEngine;

public class SuperSkill1004 : SuperSkillBase
{
    private ActionBasic action = new ActionBasic();

    protected override void OnDeInit()
    {
        this.action.DeInit();
    }

    protected override void OnInit()
    {
        this.action.Init(false);
    }

    protected override void OnUseSkill()
    {
        int num = 10;
        for (int i = 0; i < num; i++)
        {
            this.action.AddActionDelegate(() => GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x22c5, base.m_Entity.position + new Vector3(0f, 1f, 0f), base.m_Entity.eulerAngles.y));
            if (i < (num - 1))
            {
                this.action.AddActionWait(0.07f);
            }
        }
    }
}

