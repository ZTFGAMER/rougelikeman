using System;

public class AI1806 : AIBase
{
    private ActionBasic.ActionBase GetActionMoveTwo()
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq2",
            m_Entity = base.m_Entity
        };
        sequence.ConditionBase = () => base.m_Entity.m_HatredTarget != null;
        sequence.AddAction(base.GetActionAttack("actionattack2", base.m_Entity.m_Data.WeaponID, true));
        return sequence;
    }

    protected override void OnInit()
    {
        base.AddAction(this.GetActionMoveTwo());
        base.AddAction(base.GetActionWaitRandom(string.Empty, 400, 600));
    }
}

