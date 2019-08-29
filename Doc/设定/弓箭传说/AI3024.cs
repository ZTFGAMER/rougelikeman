using System;

public class AI3024 : AIBase
{
    private AIBase.ActionSequence GetBulletSeq()
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        sequence.AddAction(base.GetActionAttack(string.Empty, 0x13c6, true));
        sequence.AddAction(base.GetActionWait(string.Empty, 0x3e8));
        return sequence;
    }

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        AIBase.ActionSequence sequence2 = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        AIBase.ActionSequence action = sequence2;
        action.AddAction(new AIMove1015(base.m_Entity));
        action.AddAction(base.GetActionWaitRandom("actionwait", 800, 0x514));
        sequence2 = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        AIBase.ActionSequence sequence3 = sequence2;
        sequence3.AddAction(base.GetActionAttack("attack", base.m_Entity.m_Data.WeaponID, true));
        sequence3.AddAction(base.GetActionWaitRandom("actionwait", 450, 850));
        AIBase.ActionChooseRandom random = new AIBase.ActionChooseRandom {
            name = "actionchooser",
            m_Entity = base.m_Entity
        };
        random.ConditionBase = new Func<bool>(this.GetIsAlive);
        random.AddAction(10, action);
        if (base.m_Entity.IsElite)
        {
            random.AddAction(15, sequence3);
            random.AddAction(5, this.GetBulletSeq());
        }
        else
        {
            random.AddAction(20, sequence3);
        }
        base.AddAction(random);
    }
}

