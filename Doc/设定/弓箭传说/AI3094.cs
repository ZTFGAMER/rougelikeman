using Dxx.Util;
using System;

public class AI3094 : AIBase
{
    private WeightRandomCount weight = new WeightRandomCount(2);

    private ActionBasic.ActionBase GetActionMoveOne()
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        sequence.AddAction(new AIMove1052(base.m_Entity, 3));
        return sequence;
    }

    private ActionBasic.ActionBase GetActionMoveTwo()
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq2",
            m_Entity = base.m_Entity
        };
        sequence.ConditionBase = () => base.m_Entity.m_HatredTarget != null;
        sequence.AddAction(base.GetActionAttack("actionattack2", base.m_Entity.m_Data.WeaponID, true));
        sequence.AddAction(base.GetActionWaitRandom("actionwaitrandom2", 400, 600));
        return sequence;
    }

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        switch (this.weight.GetRandom())
        {
            case 0:
                base.AddAction(this.GetActionMoveOne());
                break;

            case 1:
                base.AddAction(this.GetActionMoveTwo());
                break;
        }
        base.bReRandom = true;
    }

    protected override void OnInitOnce()
    {
        this.weight.Add(0, 15);
        this.weight.Add(1, 10);
    }
}

