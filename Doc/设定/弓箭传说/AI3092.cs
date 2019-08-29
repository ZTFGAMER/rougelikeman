using System;

public class AI3092 : AIBase
{
    private bool Conditions() => 
        (base.GetIsAlive() && (base.m_Entity.m_HatredTarget != null));

    private ActionBasic.ActionBase GetActionAttackOne()
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        if (base.m_Entity.IsElite)
        {
            sequence.AddAction(base.GetActionAttackWait(base.m_Entity.m_Data.WeaponID, 400, -1));
            sequence.AddAction(base.GetActionAttackWait(base.m_Entity.m_Data.WeaponID, 400, -1));
        }
        else
        {
            sequence.AddAction(base.GetActionAttackWait(base.m_Entity.m_Data.WeaponID, 0x3e8, -1));
        }
        sequence.AddAction(new AIMove1052(base.m_Entity, 2));
        sequence.AddAction(base.GetActionWait(string.Empty, 400));
        return sequence;
    }

    private ActionBasic.ActionBase GetActionMoveOne()
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        sequence.AddAction(new AIMove1052(base.m_Entity, 3));
        sequence.AddAction(base.GetActionWaitRandom(string.Empty, 600, 0x4b0));
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
        action.AddAction(10, this.GetActionMoveOne());
        action.AddAction(10, this.GetActionAttackOne());
        base.AddAction(action);
    }
}

