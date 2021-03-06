﻿using System;

public class AI3081 : AIBase
{
    private ActionBasic.ActionBase GetActionMoveOne(int waittime, int waitmaxtime)
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        sequence.AddAction(new AIMove1018(base.m_Entity, waittime, waitmaxtime));
        sequence.AddAction(base.GetActionWaitRandom("actionwait1", 200, 400));
        return sequence;
    }

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        AIBase.ActionSequence action = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        action.AddAction(base.GetActionAttack(string.Empty, base.m_Entity.m_Data.WeaponID, true));
        action.AddAction(base.GetActionWaitRandom("actionwait", 0x3e8, 0x5dc));
        action.AddAction(this.GetActionMoveOne(500, 0x5dc));
        AIBase.ActionChooseRandom random = new AIBase.ActionChooseRandom {
            name = "actionchooser",
            m_Entity = base.m_Entity
        };
        random.ConditionBase = new Func<bool>(this.GetIsAlive);
        random.AddAction(10, action);
        base.AddAction(random);
    }
}

