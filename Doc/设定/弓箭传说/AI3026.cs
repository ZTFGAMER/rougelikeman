using System;

public class AI3026 : AIBase
{
    private ActionBasic.ActionBase GetActionMoveOne(int waittime, int waitmaxtime)
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        sequence.AddAction(new AIMove1018(base.m_Entity, waittime, waitmaxtime));
        sequence.AddAction(base.GetActionWaitRandom("actionwait1", 200, 400));
        return sequence;
    }

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        AIBase.ActionSequence action = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        action.AddAction(base.GetActionAttackSpecial("attack", base.m_Entity.m_Data.WeaponID, true));
        action.AddAction(base.GetActionWaitRandom("actionwait", 0x6a4, 0xce4));
        action.AddAction(base.GetActionRemoveAttack());
        action.AddAction(this.GetActionMoveOne(700, 0x6a4));
        AIBase.ActionChooseRandom random = new AIBase.ActionChooseRandom {
            name = "actionchooser",
            m_Entity = base.m_Entity
        };
        random.ConditionBase = new Func<bool>(this.GetIsAlive);
        random.AddAction(10, action);
        base.AddAction(random);
    }
}

