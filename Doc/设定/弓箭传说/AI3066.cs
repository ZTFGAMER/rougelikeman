using System;

public class AI3066 : AIBase
{
    private bool Conditions() => 
        (base.GetIsAlive() && (base.m_Entity.m_HatredTarget != null));

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        if (!GameLogic.Hold.Guide.GetNeedGuide())
        {
            AIBase.ActionSequence action = new AIBase.ActionSequence {
                name = "actionseq",
                ConditionBase = new Func<bool>(this.Conditions)
            };
            action.AddAction(base.GetActionWaitRandom("actionwaitr1", 300, 600));
            action.AddAction(base.GetActionAttack("actionattack", 0x418, true));
            action.AddAction(base.GetActionWaitRandom("actionwaitr2", 100, 500));
            AIBase.ActionSequence sequence2 = new AIBase.ActionSequence {
                name = "actionseq1"
            };
            sequence2.AddAction(base.GetActionWaitRandom("actionwaitr3", 300, 600));
            sequence2.AddAction(action);
            base.AddAction(sequence2);
        }
    }
}

