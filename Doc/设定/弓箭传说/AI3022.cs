using System;

public class AI3022 : AIBase
{
    private ActionBasic.ActionBase GetActionMoveOne(int waittime, int waitmaxtime)
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        sequence.AddAction(base.GetActionWaitRandom("actionwait1", 500, 0x3e8));
        sequence.AddAction(new AIMove1013(base.m_Entity));
        sequence.AddAction(base.GetActionWaitRandom("actionwait1", waittime, waitmaxtime));
        return sequence;
    }

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        AIBase.ActionChooseRandom action = new AIBase.ActionChooseRandom {
            name = "actionchooser",
            m_Entity = base.m_Entity
        };
        action.ConditionBase = new Func<bool>(this.GetIsAlive);
        action.AddAction(5, this.GetActionMoveOne(0x5dc, 0x7d0));
        if (base.m_Entity.IsElite)
        {
            action.AddAction(5, base.GetActionAttackWait(0x441, 0x3e8, 0x5dc));
        }
        base.AddAction(action);
    }
}

