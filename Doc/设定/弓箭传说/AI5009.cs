using Dxx.Util;
using System;

public class AI5009 : AIBase
{
    private WeightRandomCount mWeight = new WeightRandomCount(1, 3);
    private int ran;

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        this.ran = this.mWeight.GetRandom();
        switch (this.ran)
        {
            case 0:
                base.AddActionDelegate(delegate {
                    base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", -0.25f);
                    base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 1f);
                });
                base.AddAction(base.GetActionAttack(string.Empty, 0x13ce, true));
                base.AddActionDelegate(delegate {
                    base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", 0.25f);
                    base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", -1f);
                });
                base.AddActionDelegate(delegate {
                    base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", 1f);
                    base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 1f);
                });
                base.AddAction(base.GetActionAttack(string.Empty, 0x13cf, true));
                base.AddActionDelegate(delegate {
                    base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", -1f);
                    base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", -1f);
                });
                break;

            case 1:
                base.AddActionDelegate(() => base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", -0.3f));
                base.AddAction(base.GetActionAttack(string.Empty, 0x13d0, true));
                base.AddActionDelegate(() => base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", 0.3f));
                break;

            case 2:
                base.AddAction(new AIMove1050(base.m_Entity));
                break;
        }
        base.AddAction(base.GetActionWait(string.Empty, 300));
        base.bReRandom = true;
    }
}

