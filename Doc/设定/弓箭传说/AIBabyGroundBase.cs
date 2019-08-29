using Dxx.Util;
using System;
using UnityEngine;

public class AIBabyGroundBase : AIBabyBase
{
    protected int groundIndex = -1;

    protected override ActionBasic.ActionBase GetAILogic()
    {
        if (this.groundIndex < 0)
        {
            object[] args = new object[] { base.GetType().ToString() };
            Debug.LogError(Utils.FormatString("AIBabyGroundBase[{0}] groundIndex == -1", args));
        }
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            m_Entity = base.m_Entity
        };
        sequence.AddAction(new AIMoveBabyGround(base.m_Entity, this.groundIndex, 1.1f, 2f));
        ActionBasic.ActionBase actionWait = base.GetActionWait(string.Empty, 500);
        actionWait.ConditionBase = () => !base.FindTarget();
        sequence.AddAction(actionWait);
        sequence.AddAction(base.GetActionWait(string.Empty, 100));
        if (GameLogic.Hold.BattleData.Challenge_AttackEnable())
        {
            ActionBasic.ActionBase action = base.GetActionAttack("actionattack", base.AttackID, true);
            action.ConditionBase = new Func<bool>(this.FindTarget);
            sequence.AddAction(action);
        }
        return sequence;
    }
}

