using System;

public class AI3050 : AIBase
{
    protected override void OnInit()
    {
        base.AddAction(new AIMove1018(base.m_Entity, 500, 0x5dc));
        base.AddAction(base.GetActionAttackWait(base.m_Entity.m_Data.WeaponID, 500, -1));
    }
}

