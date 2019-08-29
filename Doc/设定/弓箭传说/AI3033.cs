using System;
using UnityEngine;

public class AI3033 : AIBeeBase
{
    protected override void OnDeadBefore()
    {
        base.OnDeadBefore();
        if (base.m_Entity.IsElite)
        {
            int num = 8;
            for (int i = 0; i < num; i++)
            {
                GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0xbc0, base.m_Entity.position + new Vector3(0f, 1f, 0f), i * 45f);
            }
        }
    }

    protected override void OnInit()
    {
        base.OnInit();
        base.AddAction(new AIMove1031(base.m_Entity, -1f));
    }
}

