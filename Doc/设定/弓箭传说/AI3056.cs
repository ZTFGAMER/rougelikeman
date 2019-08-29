using System;
using UnityEngine;

public class AI3056 : AIBeeBase
{
    protected override void OnDeadBefore()
    {
        base.OnDeadBefore();
        for (int i = 0; i < 4; i++)
        {
            GameLogic.Release.Bullet.CreateBullet(base.m_Entity, base.m_Entity.m_Data.WeaponID, base.m_Entity.position + new Vector3(0f, 1f, 0f), (i * 90f) + 45f);
        }
    }

    protected override void OnInit()
    {
        base.OnInit();
        base.AddAction(new AIMove1031(base.m_Entity, -1f));
    }
}

