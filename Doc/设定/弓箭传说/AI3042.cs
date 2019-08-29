using System;

public class AI3042 : AIBase
{
    protected override void OnInit()
    {
        base.AddAction(new AIMove1052(base.m_Entity, 3));
        base.AddAction(base.GetActionAttack(string.Empty, base.m_Entity.m_Data.WeaponID, true));
    }
}

