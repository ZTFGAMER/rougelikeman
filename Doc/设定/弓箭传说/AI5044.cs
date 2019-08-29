using Dxx.Util;
using System;
using System.Collections.Generic;

public class AI5044 : AIGroundBase
{
    private int ran;
    private WeightRandomCount weight = new WeightRandomCount(1);
    private List<int> bulletids;

    public AI5044()
    {
        List<int> list = new List<int> { 
            0x13fd,
            0x13fe
        };
        this.bulletids = list;
    }

    private bool Conditions() => 
        (base.GetIsAlive() && (base.m_Entity.m_HatredTarget != null));

    private int get_random_bulletid()
    {
        int num = GameLogic.Random(0, this.bulletids.Count);
        return this.bulletids[num];
    }

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

    protected override void OnInit()
    {
        this.ran = this.weight.GetRandom();
        switch (this.ran)
        {
            case 0:
            {
                int num2 = GameLogic.Random(2, 4);
                for (int i = 0; i < num2; i++)
                {
                    base.AddAction(this.GetActionAttacks(this.get_random_bulletid(), 100, 100));
                }
                break;
            }
            case 1:
                base.AddAction(new AIMove1026(base.m_Entity, 4));
                break;

            case 2:
            {
                int num4 = 2;
                for (int i = 0; i < num4; i++)
                {
                    base.AddAction(this.GetActionAttacks(0x13ff, 100, 100));
                }
                break;
            }
            case 3:
                base.AddAction(this.GetActionAttacks(0x1400, 100, 100));
                break;
        }
        base.bReRandom = true;
    }

    protected override void OnInitOnce()
    {
        base.OnInitOnce();
        this.weight.Add(0, 10);
        this.weight.Add(1, 10);
        this.weight.Add(2, 10);
        this.weight.Add(3, 10);
    }
}

