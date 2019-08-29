using System;

public class AI5001 : AIBase
{
    private ActionBasic.ActionBase GetActionSequence(int attackId, int waitTime, int movetime)
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            m_Entity = base.m_Entity
        };
        sequence.AddAction(base.GetActionAttack("actionattack", attackId, true));
        sequence.AddAction(base.GetActionWait("actionwait", waitTime));
        sequence.AddAction(new AIMove1018(base.m_Entity, movetime, 0));
        return sequence;
    }

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        AIBase.ActionChooseRandom action = new AIBase.ActionChooseRandom {
            name = "actionchooserandom",
            m_Entity = base.m_Entity
        };
        action.ConditionBase = new Func<bool>(this.GetHaveHatred);
        action.AddAction(20, this.GetActionSequence(0x1389, 0x7d0, 0x3e8));
        action.AddAction(20, this.GetActionSequence(0x138a, 0x7d0, 0x3e8));
        action.AddAction(20, this.GetActionSequence(0x138b, 0x9c4, 0x3e8));
        action.AddAction(20, this.GetActionSequence(0x138c, 0x9c4, 0x3e8));
        base.AddAction(base.GetActionWaitRandom("actionwait1", 200, 500));
        base.AddAction(action);
    }
}

