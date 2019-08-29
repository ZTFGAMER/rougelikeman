using System;

public class AI3097 : AIBase
{
    private bool Conditions() => 
        (base.GetIsAlive() && (base.m_Entity.m_HatredTarget != null));

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        if (base.m_Entity.IsElite)
        {
            base.AddAction(base.GetActionWaitRandom("actionwaitr1", 400, 800));
            base.AddAction(base.GetActionAttackSpecial("actionattack", base.m_Entity.m_Data.WeaponID, true));
            base.AddAction(base.GetActionWaitRandom("actionwaitr2", 200, 400));
            base.AddAction(base.GetActionWaitRandom("actionwaitr3", 300, 600));
        }
        else
        {
            base.AddAction(base.GetActionWaitRandom("actionwaitr1", 600, 0x4b0));
            base.AddAction(base.GetActionAttackSpecial("actionattack", base.m_Entity.m_Data.WeaponID, true));
            base.AddAction(base.GetActionWaitRandom("actionwaitr2", 400, 800));
            base.AddAction(base.GetActionWaitRandom("actionwaitr3", 500, 0x3e8));
        }
    }

    protected override void OnInitOnce()
    {
        base.OnInitOnce();
    }
}

