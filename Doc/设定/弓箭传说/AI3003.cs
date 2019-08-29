using System;

public class AI3003 : AIBase
{
    private bool Conditions() => 
        (base.GetIsAlive() && (base.m_Entity.m_HatredTarget != null));

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        if (!base.m_Entity.IsElite)
        {
            base.AddAction(base.GetActionWaitRandom("actionwaitr1", 600, 0x3e8));
            base.AddAction(base.GetActionWaitRandom("actionwaitr1", 300, 700));
            base.AddAction(base.GetActionAttack("actionattack", base.m_Entity.m_Data.WeaponID, true));
            base.AddAction(base.GetActionWaitRandom("actionwaitr2", 400, 600));
        }
        else
        {
            base.AddAction(base.GetActionWaitRandom("actionwaitr1", 600, 0x3e8));
            base.AddAction(base.GetActionAttack("actionattack", base.m_Entity.m_Data.WeaponID, true));
            base.AddAction(base.GetActionWaitRandom("actionwaitr2", 200, 400));
        }
    }
}

