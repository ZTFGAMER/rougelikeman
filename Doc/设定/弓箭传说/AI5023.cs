using Dxx.Util;
using System;

public class AI5023 : AIBase
{
    private WeightRandomCount mWeightRandom = new WeightRandomCount(2, 3);

    private void AddAttack5054()
    {
        base.AddAction(base.GetActionDelegate(() => base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", -0.6f)));
        base.AddAction(base.GetActionAttack(string.Empty, 0x13be, false));
        base.AddAction(base.GetActionDelegate(() => base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 0.6f)));
    }

    private void AddAttack5056()
    {
        base.AddAction(base.GetActionDelegate(() => base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", -0.4f)));
        base.AddAction(base.GetActionAttack(string.Empty, 0x13c0, false));
        base.AddAction(base.GetActionDelegate(() => base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 0.4f)));
    }

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        switch (this.mWeightRandom.GetRandom())
        {
            case 0:
                this.AddAttack5054();
                break;

            case 1:
                base.AddAction(base.GetActionAttack(string.Empty, 0x13bf, false));
                break;

            case 2:
                this.AddAttack5056();
                break;
        }
        base.AddAction(base.GetActionWaitRandom(string.Empty, 0x3e8, 0x7d0));
        base.bReRandom = true;
    }

    protected override void OnInitOnce()
    {
        base.OnInitOnce();
    }
}

