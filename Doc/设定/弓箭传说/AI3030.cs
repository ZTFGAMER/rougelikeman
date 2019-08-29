using System;

public class AI3030 : AIBase
{
    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        base.AddAction(new AIMove1022(base.m_Entity, 6f));
        base.AddAction(base.GetActionAttackWait(base.m_Entity.m_Data.WeaponID, 0x3e8, 0x3e8));
    }
}

