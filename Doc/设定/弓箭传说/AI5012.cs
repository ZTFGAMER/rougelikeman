using System;

public class AI5012 : AIBase
{
    private ActionBasic.ActionBase GetActionSequence(int attackId, int waitTime, int movetime)
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            m_Entity = base.m_Entity
        };
        sequence.AddAction(base.GetActionAttack("actionattack", attackId, true));
        sequence.AddAction(base.GetActionWait("actionwait", waitTime));
        sequence.AddAction(new AIMove1018(base.m_Entity, 0x3e8, 0));
        sequence.AddAction(base.GetActionWait("actionwait", movetime));
        return sequence;
    }

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        AIBase.ActionSequence action = new AIBase.ActionSequence {
            m_Entity = base.m_Entity
        };
        int num = GameLogic.Random(2, 5);
        for (int i = 0; i < num; i++)
        {
            action.AddAction(this.GetActionSequence(0x1389 + GameLogic.Random(0, 4), 0x7d0, 0x3e8));
        }
        action.AddAction(new AIMove1028(base.m_Entity));
        base.AddAction(base.GetActionWaitRandom("actionwait1", 200, 500));
        base.AddAction(action);
        base.bReRandom = true;
    }
}

