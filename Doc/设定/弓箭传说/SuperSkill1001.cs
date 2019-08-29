using System;
using UnityEngine;

public class SuperSkill1001 : SuperSkillBase
{
    protected override void OnDeInit()
    {
    }

    protected override void OnInit()
    {
    }

    protected override void OnUseSkill()
    {
        int num = 0x10;
        float num2 = 360f / ((float) num);
        for (int i = 0; i < num; i++)
        {
            GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x22c5, base.m_Entity.position + new Vector3(0f, 1f, 0f), i * num2);
        }
    }
}

