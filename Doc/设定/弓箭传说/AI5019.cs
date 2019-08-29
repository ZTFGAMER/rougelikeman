using System;

public class AI5019 : AIBase
{
    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        base.AddAction(new AIMove1039(base.m_Entity, 8));
        if (GameLogic.Random(0, 100) < 50)
        {
            base.AddAction(new AIMove1038(base.m_Entity));
        }
        else
        {
            base.AddAction(base.GetActionAttackWait(0x13b0, 0x3e8, 0x3e8));
        }
        base.bReRandom = true;
    }
}

