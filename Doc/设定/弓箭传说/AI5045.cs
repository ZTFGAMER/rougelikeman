using Dxx.Util;
using System;
using UnityEngine;

public class AI5045 : AIGroundBase
{
    private int ran;
    private WeightRandomCount weight = new WeightRandomCount(1);

    private bool Conditions() => 
        (base.GetIsAlive() && (base.m_Entity.m_HatredTarget != null));

    private ActionBasic.ActionBase GetActionAttacks(int attackid, int attacktime, int attackmaxtime)
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = string.Empty,
            m_Entity = base.m_Entity
        };
        sequence.AddAction(base.GetActionAttack(string.Empty, attackid, true));
        sequence.AddAction(base.GetActionWaitRandom(string.Empty, attacktime, attackmaxtime));
        return sequence;
    }

    private void onCreateBullets()
    {
        int num = 6;
        float num2 = GameLogic.Random((float) 0f, (float) 360f);
        for (int i = 0; i < num; i++)
        {
            GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x1402, base.m_Entity.position + new Vector3(0f, 1f, 0f), num2 + ((i * 360f) / ((float) num)));
        }
    }

    protected override void OnInit()
    {
        this.ran = this.weight.GetRandom();
        switch (this.ran)
        {
            case 0:
                base.AddAction(this.GetActionAttacks(0x1401, 100, 100));
                break;

            case 1:
            {
                AIMove1026 action = new AIMove1026(base.m_Entity, 4) {
                    onDown = new Action(this.onCreateBullets),
                    onUp = new Action(this.onCreateBullets)
                };
                base.AddAction(action);
                break;
            }
        }
        base.bReRandom = true;
    }

    protected override void OnInitOnce()
    {
        base.OnInitOnce();
        this.weight.Add(0, 10);
        this.weight.Add(1, 10);
    }
}

