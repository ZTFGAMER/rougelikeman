using System;

public class AI3047 : AIBase
{
    protected override void OnInit()
    {
        base.AddAction(new AIMove1052(base.m_Entity, 3));
        base.AddAction(base.GetActionAttackWait(base.m_Entity.m_Data.WeaponID, 0x5dc, 0x5dc));
    }
}

