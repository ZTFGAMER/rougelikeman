using System;

public class AI3069 : AIBase
{
    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        base.AddAction(new AIMove1044(base.m_Entity));
        base.AddAction(base.GetActionAttack(string.Empty, base.m_Entity.m_Data.WeaponID, true));
    }
}

