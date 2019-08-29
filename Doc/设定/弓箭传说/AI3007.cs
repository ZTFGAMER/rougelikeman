using System;

public class AI3007 : AIBase
{
    private bool Conditions() => 
        (base.GetIsAlive() && (base.m_Entity.m_HatredTarget != null));

    private ActionBasic.ActionBase GetActionMoveOne()
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        sequence.AddAction(new AIMove1002(base.m_Entity, 600, -1));
        sequence.AddAction(base.GetActionWait("actionwait1", 400));
        return sequence;
    }

    private ActionBasic.ActionBase GetActionMoveTwo()
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq2",
            m_Entity = base.m_Entity
        };
        sequence.AddAction(base.GetActionAttack("actionattack2", base.m_Entity.m_Data.WeaponID, true));
        sequence.AddAction(base.GetActionWaitRandom("actionwaitrandom2", 300, 500));
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
        if (base.m_Entity.IsElite)
        {
            action.AddAction(10, new AIMove1008(base.m_Entity, 1f, 400));
        }
        else
        {
            action.AddAction(10, this.GetActionMoveOne());
        }
        action.AddAction(20, this.GetActionMoveTwo());
        base.AddAction(action);
    }
}

