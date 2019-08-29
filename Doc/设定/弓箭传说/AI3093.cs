using System;

public class AI3093 : AIBase
{
    private ActionBasic.ActionBase GetActionAttack()
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq2",
            m_Entity = base.m_Entity
        };
        sequence.ConditionBase = () => base.m_Entity.m_HatredTarget != null;
        sequence.AddAction(base.GetActionWaitRandom(string.Empty, 200, 600));
        sequence.AddAction(new AIMove1051(base.m_Entity, 1f, 0.4f));
        sequence.AddAction(base.GetActionWait(string.Empty, 100));
        sequence.AddAction(new AIMove1052(base.m_Entity, 3));
        sequence.AddAction(base.GetActionWaitRandom(string.Empty, 300, 700));
        return sequence;
    }

    protected override void OnInit()
    {
        base.AddAction(this.GetActionAttack());
    }
}

