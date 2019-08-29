using System;

public class AI5007 : AIBase
{
    private bool Conditions() => 
        (base.GetIsAlive() && (base.m_Entity.m_HatredTarget != null));

    private ActionBasic.ActionBase GetActionAttackOne()
    {
        int waittime = 500;
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        sequence.AddAction(base.GetActionAttackWait(0x13c1, waittime, waittime));
        sequence.AddAction(base.GetActionAttackWait(0x13c1, waittime, waittime));
        sequence.AddAction(base.GetActionAttackWait(0x13c1, waittime, waittime));
        sequence.AddAction(base.GetActionAttackWait(0x13c1, waittime, waittime));
        sequence.AddAction(base.GetActionAttackWait(0x13c1, waittime, waittime));
        sequence.AddAction(base.GetActionWaitRandom(string.Empty, 800, 800));
        sequence.AddAction(this.GetActionMoveOne());
        return sequence;
    }

    private ActionBasic.ActionBase GetActionMoveOne()
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        sequence.AddAction(new AIMove1025(base.m_Entity, 3));
        sequence.AddAction(base.GetActionWait("actionwait1", 400));
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

