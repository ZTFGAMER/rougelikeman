using System;

public class AI5008 : AIBase
{
    protected override void OnInit()
    {
        base.AddAction(new AIMove1032(base.m_Entity, 1f, 0x3e8));
        base.AddAction(new AIMove1032(base.m_Entity, 1f, 0x3e8));
        int num = GameLogic.Random(1, 4);
        for (int i = 0; i < num; i++)
        {
            base.AddAction(new AIMove1032(base.m_Entity, 1f, 0x3e8));
        }
        int num3 = GameLogic.Random(1, 4);
        for (int j = 0; j < num3; j++)
        {
            base.AddAction(base.GetActionAttack(string.Empty, 0x1398 + GameLogic.Random(0, 2), true));
        }
        base.bReRandom = true;
    }
}

