using System;

public class AI3106 : AIBase
{
    private ActionBasic.ActionBase GetActionMoveOne()
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        sequence.AddAction(new AIMoveBomberman(base.m_Entity, 3));
        sequence.AddAction(base.GetActionWait("actionwait1", 600));
        return sequence;
    }

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        base.AddAction(this.GetActionMoveOne());
    }
}

