using Dxx.Util;
using System;
using UnityEngine;

public class AI5036 : AIBase
{
    private WeightRandomCount weightnear = new WeightRandomCount(1, 3);
    private WeightRandomCount weightfar = new WeightRandomCount(1, 3);

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        if (Vector3.Distance(GameLogic.Self.position, base.m_Entity.position) < 10f)
        {
            switch (this.weightnear.GetRandom())
            {
                case 0:
                    base.AddAction(base.GetActionDelegate(delegate {
                        base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", -0.5f);
                        base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", -0.3f);
                    }));
                    base.AddAction(base.GetActionAttack(string.Empty, 0x13e9, true));
                    base.AddAction(base.GetActionDelegate(delegate {
                        base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", 0.5f);
                        base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 0.3f);
                    }));
                    base.AddAction(base.GetActionWaitRandom(string.Empty, 100, 200));
                    break;

                case 1:
                    base.AddAction(base.GetActionDelegate(delegate {
                        base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", -0.3f);
                        base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 0f);
                    }));
                    base.AddAction(base.GetActionAttack(string.Empty, 0x13ea, true));
                    base.AddAction(base.GetActionDelegate(delegate {
                        base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", 0.3f);
                        base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 0f);
                    }));
                    base.AddAction(base.GetActionWaitRandom(string.Empty, 100, 200));
                    break;

                case 2:
                    base.AddAction(base.GetActionAttack(string.Empty, 0x13ec, true));
                    base.AddAction(base.GetActionWaitRandom(string.Empty, 100, 200));
                    break;
            }
        }
        else
        {
            switch (this.weightfar.GetRandom())
            {
                case 0:
                    base.AddAction(base.GetActionRotateToEntity(GameLogic.Self));
                    base.AddAction(new AIMove1055(base.m_Entity));
                    base.AddAction(base.GetActionWaitRandom(string.Empty, 600, 800));
                    break;

                case 1:
                    base.AddAction(base.GetActionAttack(string.Empty, 0x13e8, true));
                    base.AddAction(base.GetActionWaitRandom(string.Empty, 100, 200));
                    break;

                case 2:
                    base.AddAction(base.GetActionDelegate(delegate {
                        base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", -0.2f);
                        base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 0f);
                    }));
                    base.AddAction(base.GetActionAttack(string.Empty, 0x13eb, true));
                    base.AddAction(base.GetActionDelegate(delegate {
                        base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", 0.2f);
                        base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 0f);
                    }));
                    base.AddAction(base.GetActionWaitRandom(string.Empty, 100, 200));
                    break;
            }
        }
        base.bReRandom = true;
    }
}

