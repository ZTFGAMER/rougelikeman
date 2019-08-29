using System;

public class AI3012 : AIBase
{
    private bool Conditions() => 
        (base.GetIsAlive() && (base.m_Entity.m_HatredTarget != null));

    private ActionBasic.ActionBase GetActionAttackOne()
    {
        int waittime = 200;
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        sequence.AddAction(base.GetActionAttackWait(base.m_Entity.m_Data.WeaponID, waittime, waittime));
        sequence.AddAction(base.GetActionAttackWait(base.m_Entity.m_Data.WeaponID, waittime, waittime));
        sequence.AddAction(base.GetActionAttackWait(base.m_Entity.m_Data.WeaponID, waittime, waittime));
        sequence.AddAction(base.GetActionWaitRandom(string.Empty, 0x3e8, 0x578));
        sequence.AddAction(this.GetActionMoveOne());
        sequence.AddAction(this.GetActionMoveOne());
        return sequence;
    }

    private ActionBasic.ActionBase GetActionMoveOne()
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        sequence.AddAction(new AIMove1052(base.m_Entity, 3));
        sequence.AddAction(base.GetActionWait("actionwait1", 400));
        return sequence;
    }

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        if (base.m_Entity.IsElite)
        {
            int waittime = 200;
            AIBase.ActionSequence action = new AIBase.ActionSequence {
                name = "actionseq",
                m_Entity = base.m_Entity
            };
            action.AddAction(base.GetActionAttackWait(base.m_Entity.m_Data.WeaponID, waittime, waittime));
            action.AddAction(base.GetActionAttackWait(base.m_Entity.m_Data.WeaponID, waittime, waittime));
            action.AddAction(base.GetActionAttackWait(base.m_Entity.m_Data.WeaponID, waittime, waittime));
            action.AddAction(base.GetActionWaitRandom(string.Empty, 0x3e8, 0x578));
            action.AddAction(new AIMove1033(base.m_Entity, 0f, 0, false));
            base.AddAction(action);
        }
        else
        {
            AIBase.ActionChooseRandom action = new AIBase.ActionChooseRandom {
                name = "actionchooser",
                m_Entity = base.m_Entity
            };
            action.ConditionBase = new Func<bool>(this.GetIsAlive);
            action.AddAction(10, this.GetActionMoveOne());
            action.AddAction(10, this.GetActionAttackOne());
            base.AddAction(action);
        }
    }
}

