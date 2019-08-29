using System;

public class AI5006 : AIBase
{
    private ActionBasic.ActionBase GetActionAttacks(int attackid, int attacktime, int attackmaxtime)
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        sequence.AddAction(base.GetActionAttackSpecial(string.Empty, attackid, true));
        sequence.AddAction(base.GetActionWaitRandom(string.Empty, attacktime, attackmaxtime));
        return sequence;
    }

    protected override void OnInit()
    {
        AIBase.ActionSequence action = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        int num = GameLogic.Random(1, 4);
        for (int i = 0; i < num; i++)
        {
            int attackid = GameLogic.Random(0x1394, 0x1397);
            action.AddAction(this.GetActionAttacks(attackid, 100, 100));
        }
        AIBase.ActionChooseRandom random = new AIBase.ActionChooseRandom {
            m_Entity = base.m_Entity
        };
        random.AddAction(10, new AIMove1023(base.m_Entity, 3));
        random.AddAction(10, new AIMove1024(base.m_Entity, 4));
        action.AddAction(random);
        base.AddAction(action);
        base.bReRandom = true;
    }
}

