using Dxx.Util;
using System;

public class AI5020 : AIBase
{
    private WeightRandomCount weight = new WeightRandomCount(2, 4);
    private int ran;

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        this.ran = this.weight.GetRandom();
        switch (this.ran)
        {
            case 0:
                base.AddActionDelegate(() => base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", -0.86f));
                base.AddAction(base.GetActionAttack(string.Empty, 0x13b4, true));
                base.AddActionDelegate(() => base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 0.86f));
                break;

            case 1:
                base.AddAction(base.GetActionAttack(string.Empty, 0x13b1, true));
                base.AddAction(base.GetActionDelegate(() => base.m_Entity.m_AttackCtrl.SetCanRotate(false)));
                base.AddAction(base.GetActionAttack(string.Empty, 0x13b2, false));
                base.AddAction(base.GetActionAttack(string.Empty, 0x13b1, false));
                base.AddAction(base.GetActionDelegate(() => base.m_Entity.m_AttackCtrl.SetCanRotate(true)));
                base.AddAction(base.GetActionWait(string.Empty, 500));
                break;

            case 2:
                base.AddAction(new AIMove1040(base.m_Entity, 2));
                base.AddAction(base.GetActionWait(string.Empty, 200));
                break;

            case 3:
                base.AddActionDelegate(delegate {
                    base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", -0.55f);
                    base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", -0.5f);
                });
                base.AddAction(base.GetActionAttack(string.Empty, 0x13b5, true));
                base.AddActionDelegate(delegate {
                    base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackPrev", 0.55f);
                    base.m_Entity.mAniCtrlBase.UpdateAnimationSpeed("AttackEnd", 0.5f);
                });
                base.AddAction(base.GetActionWait(string.Empty, 200));
                break;
        }
        base.bReRandom = true;
    }
}

