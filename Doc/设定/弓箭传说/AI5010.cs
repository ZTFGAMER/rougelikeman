using Dxx.Util;
using System;

public class AI5010 : AIGroundBase
{
    private int ran;
    private WeightRandomCount weight = new WeightRandomCount(1);
    private int callid = 0xbe4;

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

    private ActionBasic.ActionBase GetCall()
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            ConditionBase1Data = this.callid,
            ConditionBase1 = new Func<object, bool>(this.GetCanCall)
        };
        sequence.AddAction(base.GetActionCall(this.callid));
        sequence.AddAction(base.GetActionWait("actionwait", 500));
        return sequence;
    }

    protected override void OnInit()
    {
        this.ran = this.weight.GetRandom();
        if (this.ran == 0)
        {
            base.AddAction(this.GetActionAttacks(0x139a, 200, 200));
        }
        else if (this.ran == 1)
        {
            base.AddAction(this.GetCall());
        }
        else
        {
            base.AddAction(new AIMove1026(base.m_Entity, 4));
            base.AddActionWait(0.1f);
            base.AddAction(this.GetActionAttacks(0x139a, 200, 200));
        }
        base.bReRandom = true;
    }

    protected override void OnInitOnce()
    {
        base.OnInitOnce();
        this.weight.Add(0, 10);
        this.weight.Add(1, 10);
        this.weight.Add(2, 10);
        base.InitCallData(this.callid, 3, 0x7fffffff, 2, 2, 3);
    }
}

