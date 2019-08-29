using System;

public class AI3091 : AIBase
{
    private bool Conditions() => 
        (base.GetIsAlive() && (base.m_Entity.m_HatredTarget != null));

    private ActionBasic.ActionBase GetActionMoveOne(int movetime, int waittime)
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        sequence.AddAction(new AIMove1002(base.m_Entity, movetime, -1));
        sequence.AddAction(base.GetActionWait("actionwait1", waittime));
        return sequence;
    }

    private ActionBasic.ActionBase GetActionMoveTwo(int bulletid, int waitmin, int waitmax)
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq2",
            m_Entity = base.m_Entity
        };
        sequence.AddAction(base.GetActionAttack("actionattack2", bulletid, true));
        sequence.AddAction(base.GetActionWaitRandom("actionwaitrandom2", waitmin, waitmax));
        return sequence;
    }

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        AIBase.ActionChooseRandom action = new AIBase.ActionChooseRandom {
            name = "actionchooser",
            m_Entity = base.m_Entity
        };
        action.ConditionBase = new Func<bool>(this.GetIsAlive);
        if (base.m_Entity.IsElite)
        {
            action.AddAction(15, this.GetActionMoveOne(600, 200));
            action.AddAction(15, this.GetActionMoveTwo(0x449, 400, 300));
        }
        else
        {
            action.AddAction(15, this.GetActionMoveOne(600, 400));
            action.AddAction(15, this.GetActionMoveTwo(base.m_Entity.m_Data.WeaponID, 400, 600));
        }
        base.AddAction(action);
    }
}

