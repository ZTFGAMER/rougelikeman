using System;

public class AI3029 : AIGroundBase
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

    private void InitAI()
    {
        base.AddAction(new AIMove1027(base.m_Entity, 2));
        base.AddAction(this.GetActionAttacks(base.m_Entity.m_Data.WeaponID, 300, 600));
    }

    protected override void OnInit()
    {
        this.InitAI();
    }
}

