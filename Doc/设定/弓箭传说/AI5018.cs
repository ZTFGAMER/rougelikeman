using System;

public class AI5018 : AIBase
{
    public const int DivideCount = 2;

    protected override void OnDeadBefore()
    {
        base.Divide(0xbf5, 2);
    }

    protected override void OnInit()
    {
        base.AddAction(new AIMove1034(base.m_Entity));
        base.AddAction(new AIMove1034(base.m_Entity));
        base.AddAction(new AIMove1034(base.m_Entity));
        base.AddAction(new AIMove1034(base.m_Entity));
        base.AddAction(base.GetActionWait(string.Empty, 500));
        base.AddAction(base.GetActionAttackSpecial(string.Empty, 0x13ae, false));
    }
}

