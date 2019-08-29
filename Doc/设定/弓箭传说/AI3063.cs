using Dxx.Util;
using System;
using UnityEngine;

public class AI3063 : AIBase
{
    private ActionBasic action = new ActionBasic();

    private void AddAttack()
    {
        this.action.AddAction(base.GetActionWaitDelegate(0x7d0, new Action(this.Attack)));
    }

    private void Attack()
    {
        EntityBase base2 = GameLogic.FindTarget(base.m_Entity);
        if (base2 != null)
        {
            float num = Utils.getAngle(base2.position - base.m_Entity.position);
            for (int i = 0; i < 2; i++)
            {
                GameLogic.Release.Bullet.CreateBullet(base.m_Entity, base.m_Entity.m_Data.WeaponID, base.m_Entity.position + new Vector3(0f, 1f, 0f), ((num + GameLogic.Random((float) -7f, (float) 7f)) + (i * 30f)) - 15f);
            }
        }
        this.AddAttack();
    }

    protected override void OnAIDeInit()
    {
        this.action.DeInit();
    }

    protected override void OnInit()
    {
        base.AddAction(new AIMove1052(base.m_Entity, 3));
        this.AddAttack();
    }

    protected override void OnInitOnce()
    {
        base.OnInitOnce();
        this.action.Init(false);
    }
}

