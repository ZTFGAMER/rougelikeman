using System;

public class AI3028 : AIGroundBase
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
        int num = 1;
        for (int i = 0; i < num; i++)
        {
            base.AddAction(this.GetActionAttacks(base.m_Entity.m_Data.WeaponID, 400, 700));
        }
        base.AddAction(new AIMove1026(base.m_Entity, 4));
        base.bReRandom = true;
    }
}

