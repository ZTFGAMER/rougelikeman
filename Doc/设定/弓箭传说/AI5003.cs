using System;
using UnityEngine;

public class AI5003 : AIBase
{
    private int callid = 0xbcd;

    private ActionBasic.ActionBase GetAttack()
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            ConditionBase = new Func<bool>(this.GetIsAlive)
        };
        sequence.AddAction(base.GetActionAttack("actionattack", 0x1391, true));
        sequence.AddAction(base.GetActionWait("actionwait", 0x3e8));
        return sequence;
    }

    private ActionBasic.ActionBase GetCall()
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            ConditionBase1Data = 0xbcd,
            ConditionBase1 = new Func<object, bool>(this.GetCanCall)
        };
        sequence.AddAction(base.GetActionCall(0xbcd));
        sequence.AddAction(base.GetActionWait("actionwait", 0x6a4));
        return sequence;
    }

    private ActionBasic.ActionBase GetMove()
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            ConditionBase = new Func<bool>(this.GetIsAlive)
        };
        sequence.AddAction(new AIMove1008(base.m_Entity, 1f, 0x3e8));
        sequence.AddAction(base.GetActionWait("actionwait", 300));
        AIBase.ActionSequence sequence2 = new AIBase.ActionSequence {
            ConditionBase = new Func<bool>(this.GetIsAlive)
        };
        sequence2.AddAction(new AIMove1008(base.m_Entity, 1f, 0x3e8));
        sequence2.AddAction(base.GetActionWait("actionwait", 300));
        AIBase.ActionChoose action = new AIBase.ActionChoose {
            m_Entity = base.m_Entity,
            Condition = new Func<bool>(this.RandomMove3),
            ResultTrue = sequence2
        };
        sequence.AddAction(action);
        return sequence;
    }

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        AIBase.ActionChooseRandom action = new AIBase.ActionChooseRandom {
            ConditionBase = new Func<bool>(this.GetIsAlive)
        };
        action.AddAction(7, this.GetMove());
        action.AddAction(10, this.GetAttack());
        action.AddAction(10, this.GetCall());
        base.AddAction(action);
    }

    protected override void OnInitOnce()
    {
        base.InitCallData(this.callid, 2, 0x7fffffff, 1, 2, 3);
    }

    private bool RandomMove3() => 
        (Random.Range(0, 100) < 50);
}

