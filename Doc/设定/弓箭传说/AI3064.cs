using System;
using UnityEngine;

public class AI3064 : AIBase
{
    private ActionBasic action = new ActionBasic();

    private void AddAttack()
    {
        this.action.AddAction(base.GetActionWaitDelegate(0x7d0, new Action(this.Attack)));
    }

    private void Attack()
    {
        for (int i = 0; i < 15; i++)
        {
            GameLogic.Release.Bullet.CreateBullet(base.m_Entity, base.m_Entity.m_Data.WeaponID, base.m_Entity.position + new Vector3(0f, 1f, 0f), GameLogic.Random((float) 0f, (float) 360f));
        }
        this.AddAttack();
    }

    protected override void OnAIDeInit()
    {
        this.action.DeInit();
    }

    protected override void OnInit()
    {
        base.AddAction(new AIMove1002(base.m_Entity, 0x3e8, 0x3e8));
        this.AddAttack();
    }

    protected override void OnInitOnce()
    {
        base.OnInitOnce();
        this.action.Init(false);
    }
}

