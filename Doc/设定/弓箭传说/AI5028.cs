using Dxx.Util;
using System;

public class AI5028 : AIBase
{
    private WeightRandomCount mWeightRandom = new WeightRandomCount(2, 2);

    private ActionBasic.ActionBase GetJump()
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            m_Entity = base.m_Entity
        };
        sequence.AddAction(new AIMove1049(base.m_Entity));
        sequence.AddAction(base.GetActionWait(string.Empty, 0x3e8));
        return sequence;
    }

    private ActionBasic.ActionBase GetMove()
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        sequence.AddAction(base.GetActionRemoveMove());
        sequence.AddAction(new AIMove1011(base.m_Entity, 600, 0x3e8));
        EntityBase target = GameLogic.FindTarget(base.m_Entity);
        if (target != null)
        {
            sequence.AddAction(base.GetActionRotateToEntity(target));
        }
        sequence.AddAction(new AIMove1048(base.m_Entity, 600));
        sequence.AddAction(base.GetActionWait(string.Empty, 500));
        if (target != null)
        {
            sequence.AddAction(base.GetActionRotateToEntity(target));
        }
        sequence.AddAction(new AIMove1048(base.m_Entity, 600));
        sequence.AddAction(base.GetActionWait(string.Empty, 500));
        if (target != null)
        {
            sequence.AddAction(base.GetActionRotateToEntity(target));
        }
        sequence.AddAction(new AIMove1048(base.m_Entity, 600));
        sequence.AddAction(base.GetActionWait(string.Empty, 0x3e8));
        return sequence;
    }

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        switch (this.mWeightRandom.GetRandom())
        {
            case 0:
                base.AddAction(this.GetMove());
                break;

            case 1:
                base.AddAction(this.GetJump());
                break;
        }
        base.bReRandom = true;
    }
}

