using System;

public class AI3005 : AIBase
{
    private bool Conditions() => 
        (base.GetIsAlive() && (base.m_Entity.m_HatredTarget != null));

    private ActionBasic.ActionBase GetActionAttackOne()
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        sequence.AddAction(base.GetActionAttackWait(base.m_Entity.m_Data.WeaponID, 0x3e8, -1));
        sequence.AddAction(new AIMove1052(base.m_Entity, 3));
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
        sequence.AddAction(base.GetActionWaitRandom(string.Empty, 600, 0x578));
        return sequence;
    }

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        if (base.m_Entity.IsElite)
        {
            base.AddAction(base.GetActionAttackWait(base.m_Entity.m_Data.WeaponID, 0x3e8, -1));
            base.AddAction(new AIMove1033(base.m_Entity, 0f, 0, false));
        }
        else
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
}

