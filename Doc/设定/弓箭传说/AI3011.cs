using System;

public class AI3011 : AIBase
{
    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        base.AddAction(new AIMove1011(base.m_Entity, 0, 0));
    }
}

