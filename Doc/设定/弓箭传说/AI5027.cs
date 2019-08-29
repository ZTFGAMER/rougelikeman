using System;

public class AI5027 : AIBase
{
    public const int DivideCount = 6;

    protected override void OnAIDeInit()
    {
    }

    protected override void OnDeadBefore()
    {
        base.Divide(0xc01, 6);
    }

    protected override void OnInit()
    {
        AIBase.ActionSequence action = new AIBase.ActionSequence {
            name = "actionseq",
            ConditionBase = new Func<bool>(this.GetIsAlive)
        };
        action.AddAction(new AIMove1004(base.m_Entity));
        AIBase.ActionSequence sequence2 = new AIBase.ActionSequence {
            name = "actionseq1"
        };
        sequence2.AddAction(base.GetActionWaitRandom("actionwaitr3", 200, 400));
        sequence2.AddAction(action);
        base.AddAction(sequence2);
    }
}

