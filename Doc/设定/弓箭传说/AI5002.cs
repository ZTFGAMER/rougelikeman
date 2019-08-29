using System;

public class AI5002 : AIBase
{
    private ActionBasic.ActionBase GetActionSequence(int attackId, int waitTime)
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            m_Entity = base.m_Entity
        };
        sequence.AddAction(base.GetActionAttack("actionattack", attackId, true));
        sequence.AddAction(base.GetActionWait("actionwait", waitTime));
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
        action.AddAction(20, this.GetActionSequence(0x138d, 0x5dc));
        action.AddAction(20, this.GetActionSequence(0x138e, 0x5dc));
        action.AddAction(20, this.GetActionSequence(0x138f, 0x5dc));
        base.AddAction(base.GetActionWaitRandom("actionwait1", 200, 500));
        base.AddAction(action);
    }
}

