using Dxx.Util;
using System;

public class AI5031 : AIBase
{
    private WeightRandomCount mWeightRandom = new WeightRandomCount(1, 4);
    private int ran;

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

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        AIBase.ActionSequence sequence;
        AIBase.ActionSequence sequence2;
        this.ran = this.mWeightRandom.GetRandom();
        switch (this.ran)
        {
            case 0:
                sequence2 = new AIBase.ActionSequence {
                    name = "actionseq",
                    m_Entity = base.m_Entity
                };
                sequence = sequence2;
                if (!MathDxx.RandomBool())
                {
                    sequence.AddAction(base.GetActionAttack("attack", 0x13d2, true));
                    break;
                }
                sequence.AddAction(base.GetActionAttack("attack", 0x13d1, true));
                break;

            case 1:
            {
                sequence2 = new AIBase.ActionSequence {
                    name = "actionseq",
                    m_Entity = base.m_Entity
                };
                AIBase.ActionSequence action = sequence2;
                action.AddAction(base.GetActionDelegate(delegate {
                    base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", 2f);
                    base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 2f);
                }));
                int num2 = GameLogic.Random(4, 9);
                int num3 = GameLogic.Random(0, 2);
                for (int i = 0; i < num2; i++)
                {
                    int attackId = 0x13d3 + num3;
                    num3 = 1 - num3;
                    action.AddAction(base.GetActionAttack("attack", attackId, true));
                }
                action.AddAction(base.GetActionDelegate(delegate {
                    base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", -2f);
                    base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", -2f);
                }));
                action.AddAction(base.GetActionWaitRandom("actionwait", 500, 0x3e8));
                base.AddAction(action);
                goto Label_01F2;
            }
            case 2:
                base.AddAction(this.GetActionMoveOne(500, 0x5dc));
                goto Label_01F2;

            case 3:
                base.AddAction(new AIMove1054(base.m_Entity));
                base.AddAction(base.GetActionWaitRandom(string.Empty, 500, 800));
                base.AddAction(new AIMove1054(base.m_Entity));
                base.AddAction(base.GetActionWaitRandom(string.Empty, 500, 800));
                goto Label_01F2;

            default:
                goto Label_01F2;
        }
        sequence.AddAction(base.GetActionWaitRandom("actionwait", 500, 0x3e8));
        base.AddAction(sequence);
    Label_01F2:
        base.bReRandom = true;
    }
}

