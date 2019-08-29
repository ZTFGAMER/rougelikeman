using DG.Tweening;
using Dxx.Util;
using System;

public class AI5034 : AIBase
{
    private WeightRandomCount weight = new WeightRandomCount(2, 4);
    private float attackadd = 0.3f;
    private Sequence seq;

    private AIBase.ActionSequence GetSprint()
    {
        AIBase.ActionSequence sequence = new AIBase.ActionSequence {
            name = "actionseq",
            m_Entity = base.m_Entity
        };
        sequence.AddAction(base.GetActionDelegate(delegate {
            base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", -0.3f);
        }));
        sequence.AddAction(base.GetActionAttack(string.Empty, 0x13e2, true));
        sequence.AddAction(base.GetActionDelegate(delegate {
            base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", 0.3f);
        }));
        return sequence;
    }

    protected override void OnAIDeInit()
    {
        if (this.seq != null)
        {
            TweenExtensions.Kill(this.seq, false);
            this.seq = null;
        }
    }

    protected override void OnInit()
    {
        AIBase.ActionSequence sequence2;
        switch (this.weight.GetRandom())
        {
            case 0:
            {
                sequence2 = new AIBase.ActionSequence {
                    name = "actionseq",
                    m_Entity = base.m_Entity
                };
                AIBase.ActionSequence action = sequence2;
                action.AddAction(base.GetActionAttack(string.Empty, 0x13df, true));
                action.AddAction(base.GetActionWaitRandom("actionwait", 500, 0x3e8));
                base.AddAction(action);
                break;
            }
            case 1:
            {
                sequence2 = new AIBase.ActionSequence {
                    name = "actionseq",
                    m_Entity = base.m_Entity
                };
                AIBase.ActionSequence action = sequence2;
                action.AddAction(base.GetActionAttack(string.Empty, 0x13e0, true));
                action.AddAction(base.GetActionWaitRandom("actionwait", 600, 0x3e8));
                base.AddAction(action);
                break;
            }
            case 2:
                base.AddAction(this.GetSprint());
                base.AddAction(this.GetSprint());
                base.AddAction(base.GetActionWaitRandom("actionwait", 500, 0x3e8));
                break;

            case 3:
                base.AddAction(new AIMove1002(base.m_Entity, 700, 0x578));
                break;
        }
        base.bReRandom = true;
    }
}

