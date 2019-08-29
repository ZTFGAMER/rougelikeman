using System;
using UnityEngine;

public class AI3068 : AIBase
{
    protected override void OnAIDeInit()
    {
    }

    protected override void OnDeadBefore()
    {
        for (int i = 0; i < 6; i++)
        {
            GameLogic.Release.Bullet.CreateBullet(base.m_Entity, base.m_Entity.m_Data.WeaponID, base.m_Entity.position + new Vector3(0f, 1f, 0f), i * 60f);
        }
    }

    protected override void OnInit()
    {
        base.AddAction(new AIMove1007(base.m_Entity));
    }
}

