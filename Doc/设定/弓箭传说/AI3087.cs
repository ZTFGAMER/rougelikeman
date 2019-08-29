using System;

public class AI3087 : AIBase
{
    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        base.AddAction(new AIMove1006(base.m_Entity, 0x708, 0x9c4));
        base.AddAction(base.GetActionAttack(string.Empty, base.m_Entity.m_Data.WeaponID, true));
    }
}

