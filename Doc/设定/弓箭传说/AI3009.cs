﻿using System;

public class AI3009 : AIBase
{
    public const int DivideCount = 2;

    private bool Conditions() => 
        (base.GetIsAlive() && (base.m_Entity.m_HatredTarget != null));

    protected override void OnAIDeInit()
    {
    }

    protected override void OnDeadBefore()
    {
        base.Divide(0xbc2, 2);
    }

    protected override void OnInit()
    {
        base.AddAction(new AIMove1007(base.m_Entity));
    }
}

