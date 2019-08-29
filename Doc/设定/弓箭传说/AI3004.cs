using System;

public class AI3004 : AIBase
{
    private bool Conditions() => 
        (base.GetIsAlive() && (base.m_Entity.m_HatredTarget != null));

    protected override void OnAIDeInit()
    {
    }

    protected override void OnElite()
    {
    }

    protected override void OnInit()
    {
        if (GameLogic.Hold.Guide.GetFlowerAttack())
        {
            if (!base.m_Entity.IsElite)
            {
                base.AddAction(base.GetActionWaitRandom("actionwaitr1", 300, 600));
                base.AddAction(base.GetActionWaitRandom("actionwaitr1", 300, 600));
                base.AddAction(base.GetActionAttack("actionattack", base.m_Entity.m_Data.WeaponID, true));
                base.AddAction(base.GetActionWaitRandom("actionwaitr2", 100, 500));
            }
            else
            {
                base.AddAction(base.GetActionWaitRandom("actionwaitr1", 200, 400));
                base.AddAction(base.GetActionAttack("actionattack", base.m_Entity.m_Data.WeaponID, true));
                base.AddAction(base.GetActionWaitRandom("actionwaitr2", 100, 300));
            }
        }
    }
}

