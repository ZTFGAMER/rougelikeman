using System;
using UnityEngine;

public class AI5015 : AIBase
{
    private int callid = 0xbcd;

    private ActionBasic.ActionBase GetAttack(int attackid)
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            ConditionBase = new Func<bool>(this.GetIsAlive)
        };
        sequence.AddAction(base.GetActionAttack("actionattack", attackid, true));
        sequence.AddAction(base.GetActionWait("actionwait", 0x3e8));
        return sequence;
    }

    private ActionBasic.ActionBase GetCall()
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            ConditionBase1Data = this.callid,
            ConditionBase1 = new Func<object, bool>(this.GetCanCall)
        };
        sequence.AddAction(base.GetActionCall(this.callid));
        sequence.AddAction(base.GetActionWait("actionwait", 0x6a4));
        return sequence;
    }

    private ActionBasic.ActionBase GetMove()
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            ConditionBase = new Func<bool>(this.GetIsAlive)
        };
        sequence.AddAction(new AIMove1036(base.m_Entity, 1f, 0x3e8));
        sequence.AddAction(base.GetActionWait("actionwait", 300));
        AIBase.ActionSequence sequence2 = new AIBase.ActionSequence {
            ConditionBase = new Func<bool>(this.GetIsAlive)
        };
        sequence2.AddAction(new AIMove1036(base.m_Entity, 1f, 0x3e8));
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
        action.AddAction(5, this.GetAttack(0x13a4));
        action.AddAction(5, this.GetAttack(0x13a5));
        base.AddAction(action);
        base.bReRandom = true;
    }

    protected override void OnInitOnce()
    {
        base.InitCallData(this.callid, 2, 0x7fffffff, 1, 2, 3);
    }

    private bool RandomMove3() => 
        (Random.Range(0, 100) < 50);
}

