using System;

public class AI3075 : AIBase
{
    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        base.AddAction(new AIMove1006(base.m_Entity, 800, 0x3e8));
    }
}

