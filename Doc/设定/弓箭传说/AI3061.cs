using System;

public class AI3061 : AIBase
{
    public const int DivideCount = 3;
    private int randomcount;

    protected override void OnDeadBefore()
    {
        base.Divide(0xbf6, 3);
    }

    protected override void OnInit()
    {
        if (this.randomcount == 0)
        {
            base.AddAction(base.GetActionWait(string.Empty, 0x3e8));
        }
        base.AddAction(new AIMove1034(base.m_Entity));
        base.AddAction(new AIMove1034(base.m_Entity));
        base.AddAction(new AIMove1034(base.m_Entity));
        base.AddAction(new AIMove1034(base.m_Entity));
        base.AddAction(base.GetActionWait(string.Empty, 500));
        base.AddAction(base.GetActionAttackSpecial(string.Empty, 0x13ae, false));
        this.randomcount++;
        base.bReRandom = true;
    }
}

