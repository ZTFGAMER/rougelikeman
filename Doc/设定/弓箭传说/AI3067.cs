using System;
using UnityEngine;

public class AI3067 : AIBase
{
    private ActionBasic action = new ActionBasic();

    private void AddAttack()
    {
        this.action.AddAction(base.GetActionWaitDelegate(0x7d0, new Action(this.Attack)));
    }

    private void Attack()
    {
        for (int i = 0; i < 4; i++)
        {
            GameLogic.Release.Bullet.CreateBullet(base.m_Entity, base.m_Entity.m_Data.WeaponID, base.m_Entity.position + new Vector3(0f, 1f, 0f), (i * 90f) + 45f);
        }
        this.AddAttack();
    }

    protected override void OnAIDeInit()
    {
        this.action.DeInit();
    }

    protected override void OnInit()
    {
        base.AddAction(new AIMove1002(base.m_Entity, GameLogic.Random(400, 800), -1));
    }

    protected override void OnInitOnce()
    {
        base.OnInitOnce();
        this.action.Init(false);
        this.AddAttack();
    }
}

