using System;

public class AI1801 : AIBase
{
    protected override void OnInit()
    {
        AIBase.ActionSequence action = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        ActionBasic.ActionBase base2 = base.GetActionAttackSpecial("attack", base.m_Entity.m_Data.WeaponID, true);
        base2.ConditionBase = () => base.m_Entity.m_HatredTarget != null;
        action.AddAction(base2);
        action.AddAction(base.GetActionWaitRandom("actionwait", 500, 500));
        base.AddAction(action);
    }
}

