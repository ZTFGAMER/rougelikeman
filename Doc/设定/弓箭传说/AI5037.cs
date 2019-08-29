using Dxx.Util;
using System;

public class AI5037 : AIBase
{
    private WeightRandomCount weight = new WeightRandomCount(1, 3);

    private ActionBasic.ActionBase GetActionSequence(int attackId, int waitTime, int movetime)
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            m_Entity = base.m_Entity,
            ConditionBase = new Func<bool>(this.GetHaveHatred)
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
        switch (this.weight.GetRandom())
        {
            case 0:
                base.AddAction(this.GetActionSequence(0x13e3, 0x7d0, 0x3e8));
                break;

            case 1:
                base.AddAction(this.GetActionSequence(0x13e5, 0x7d0, 0x3e8));
                break;

            case 2:
                base.AddAction(this.GetActionSequence(0x13e6, 0x7d0, 0x3e8));
                break;
        }
        base.bReRandom = true;
    }

    protected override void OnInitOnce()
    {
        base.m_Entity.m_EntityData.ExcuteAttributes("ReboundWall", 1L);
        base.m_Entity.m_EntityData.ExcuteAttributes("ReboundWallMin", 1L);
        base.m_Entity.m_EntityData.ExcuteAttributes("ReboundWallMax", 1L);
    }
}

