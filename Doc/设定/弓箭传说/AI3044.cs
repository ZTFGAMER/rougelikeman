using System;

public class AI3044 : AIGroundBase
{
    private bool Conditions() => 
        (base.GetIsAlive() && (base.m_Entity.m_HatredTarget != null));

    private ActionBasic.ActionBase GetActionAttacks(int attackid, int attacktime, int attackmaxtime)
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq1016",
            m_Entity = base.m_Entity
        };
        sequence.AddAction(base.GetActionAttack("attacksss", attackid, true));
        sequence.AddAction(base.GetActionWaitRandom("waitsss", attacktime, attackmaxtime));
        return sequence;
    }

    protected override void OnInit()
    {
        if (base.m_Entity.IsElite)
        {
            base.AddAction(this.GetActionAttacks(base.m_Entity.m_Data.WeaponID, 0, 0));
            base.AddAction(this.GetActionAttacks(0x44a, 0, 0));
        }
        else
        {
            base.AddAction(this.GetActionAttacks(base.m_Entity.m_Data.WeaponID, 0x3e8, 0x3e8));
        }
        base.AddAction(new AIMove1026(base.m_Entity, 4));
    }
}

