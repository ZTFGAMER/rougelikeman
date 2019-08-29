using System;

public class AI3071 : AIBase
{
    protected override void OnInit()
    {
        AIBase.ActionSequence sequence2 = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        AIBase.ActionSequence action = sequence2;
        action.AddAction(new AIMove1045(base.m_Entity, 8f));
        action.AddAction(base.GetActionWaitRandom("actionwait", 300, 600));
        sequence2 = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        AIBase.ActionSequence sequence3 = sequence2;
        sequence3.AddAction(base.GetActionAttack("attack", base.m_Entity.m_Data.WeaponID, true));
        sequence3.AddAction(base.GetActionWaitRandom("actionwait", 300, 600));
        AIBase.ActionChooseRandom random = new AIBase.ActionChooseRandom {
            name = "actionchooser",
            m_Entity = base.m_Entity
        };
        random.ConditionBase = new Func<bool>(this.GetIsAlive);
        random.AddAction(20, action);
        random.AddAction(10, sequence3);
        base.AddAction(random);
    }
}

