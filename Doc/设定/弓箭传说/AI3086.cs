using System;

public class AI3086 : AIBase
{
    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        base.AddAction(new AIMove1006(base.m_Entity, 0x708, 0x9c4));
    }
}

