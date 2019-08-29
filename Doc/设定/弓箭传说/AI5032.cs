using Dxx.Util;
using System;
using UnityEngine;

public class AI5032 : AIBase
{
    private WeightRandomCount weight = new WeightRandomCount(1, 4);
    private float attackadd = 0.3f;

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
                action.AddAction(base.GetActionAttack(string.Empty, 0x13d7, true));
                action.AddAction(base.GetActionWaitRandom("actionwait", 0x3e8, 0x5dc));
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
                action.AddAction(base.GetActionDelegate(delegate {
                    base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", this.attackadd);
                    base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", this.attackadd);
                }));
                action.AddAction(base.GetActionAttack(string.Empty, 0x13d8, true));
                action.AddAction(base.GetActionAttack(string.Empty, 0x13d9, true));
                action.AddAction(base.GetActionDelegate(delegate {
                    base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", -this.attackadd);
                    base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", -this.attackadd);
                }));
                action.AddAction(base.GetActionWaitRandom("actionwait", 0x3e8, 0x5dc));
                base.AddAction(action);
                break;
            }
            case 2:
            {
                sequence2 = new AIBase.ActionSequence {
                    name = "actionseq",
                    m_Entity = base.m_Entity
                };
                AIBase.ActionSequence action = sequence2;
                action.AddAction(base.GetActionDelegate(delegate {
                    base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", this.attackadd);
                    base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", this.attackadd);
                }));
                action.AddAction(base.GetActionAttack(string.Empty, 0x13d9, true));
                action.AddAction(base.GetActionAttack(string.Empty, 0x13d8, true));
                action.AddAction(base.GetActionDelegate(delegate {
                    base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", -this.attackadd);
                    base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", -this.attackadd);
                }));
                action.AddAction(base.GetActionWaitRandom("actionwait", 0x3e8, 0x5dc));
                base.AddAction(action);
                break;
            }
            case 3:
                base.AddAction(new AIMove1002(base.m_Entity, 500, 0x3e8));
                break;
        }
        base.bReRandom = true;
    }

    protected override void OnInitOnce()
    {
        bool flag = MathDxx.RandomBool();
        float rota = -45f;
        if (flag)
        {
            rota *= -1f;
        }
        GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x13da, base.m_Entity.position + new Vector3(0f, 1f, 0f), rota);
        GameLogic.Release.Bullet.CreateBullet(base.m_Entity, 0x13da, base.m_Entity.position + new Vector3(0f, 1f, 0f), rota + 180f);
    }
}

