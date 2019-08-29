using Dxx.Util;
using System;

public class AI5021 : AIBase
{
    private WeightRandomCount weight = new WeightRandomCount(1, 4);
    private int ran;
    private int randomcount;

    protected override void OnAIDeInit()
    {
    }

    protected override void OnInit()
    {
        this.randomcount++;
        if (this.randomcount == 1)
        {
            base.AddActionWait(1f);
        }
        if (this.randomcount <= 2)
        {
            this.ran = this.weight.GetRandom();
            if (this.ran == 3)
            {
                this.ran = this.weight.GetRandom();
            }
        }
        else
        {
            this.ran = this.weight.GetRandom();
        }
        switch (this.ran)
        {
            case 0:
                base.AddAction(base.GetActionAttack(string.Empty, 0x13b9, true));
                break;

            case 1:
                base.AddAction(base.GetActionAttack(string.Empty, 0x13b7, true));
                break;

            case 2:
                base.AddAction(new AIMove1041(base.m_Entity));
                break;

            case 3:
                base.AddAction(new AIMove1042(base.m_Entity, 2));
                break;
        }
        base.AddAction(new AIMove1043(base.m_Entity, GameLogic.Random(0x5dc, 0x9c4)));
        base.bReRandom = true;
    }
}

