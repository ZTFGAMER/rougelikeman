using System;

public class AI3085 : AIBase
{
    private ActionBasic.ActionBase GetActionMoveOne(int waittime, int waitmaxtime)
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        sequence.AddAction(new AIMove1018(base.m_Entity, waittime, waitmaxtime));
        sequence.AddAction(base.GetActionWaitRandom("actionwait1", 200, 400));
        return sequence;
    }

    private ActionBasic.ActionBase GetLeftAttack()
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            m_Entity = base.m_Entity
        };
        sequence.AddAction(base.GetActionDelegate(delegate {
            base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", -0.5f);
            base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", -0.2f);
        }));
        sequence.AddAction(base.GetActionAttack(string.Empty, 0x443, true));
        sequence.AddAction(base.GetActionDelegate(delegate {
            base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", 0.5f);
            base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 0.2f);
        }));
        return sequence;
    }

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        AIBase.ActionChooseRandom random2;
        AIBase.ActionSequence action = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        if (base.m_Entity.IsElite)
        {
            random2 = new AIBase.ActionChooseRandom {
                m_Entity = base.m_Entity
            };
            AIBase.ActionChooseRandom random = random2;
            random.AddAction(10, this.GetLeftAttack());
            random.AddAction(10, base.GetActionAttack("attack", base.m_Entity.m_Data.WeaponID, true));
            action.AddAction(random);
            action.AddAction(base.GetActionWaitRandom("actionwait", 200, 400));
            action.AddAction(this.GetActionMoveOne(250, 750));
        }
        else
        {
            action.AddAction(base.GetActionAttack("attack", base.m_Entity.m_Data.WeaponID, true));
            action.AddAction(base.GetActionWaitRandom("actionwait", 0x3e8, 0x5dc));
            action.AddAction(this.GetActionMoveOne(500, 0x5dc));
        }
        random2 = new AIBase.ActionChooseRandom {
            name = "actionchooser",
            m_Entity = base.m_Entity
        };
        AIBase.ActionChooseRandom random3 = random2;
        random3.ConditionBase = new Func<bool>(this.GetIsAlive);
        random3.AddAction(10, action);
        base.AddAction(random3);
    }

    protected override void OnInitOnce()
    {
        base.OnInitOnce();
        if (base.m_Entity.IsElite)
        {
            base.m_Entity.m_EntityData.ExcuteAttributes("MoveSpeed%", 0x2710L);
            base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("Run", 0.5f);
        }
    }
}

