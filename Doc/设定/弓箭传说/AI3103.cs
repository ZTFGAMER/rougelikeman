using System;

public class AI3103 : AIBase
{
    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        base.AddAction(new AIMove1007(base.m_Entity));
    }
}

