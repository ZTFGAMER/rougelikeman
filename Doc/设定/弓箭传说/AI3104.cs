using System;

public class AI3104 : AIBase
{
    protected override void OnInit()
    {
        base.AddAction(new AIMove1034(base.m_Entity));
        base.AddAction(new AIMove1034(base.m_Entity));
        base.AddAction(new AIMove1034(base.m_Entity));
        base.AddAction(new AIMove1034(base.m_Entity));
        base.AddAction(base.GetActionWait(string.Empty, 0x3e8));
    }

    protected override void OnInitOnce()
    {
        base.OnInitOnce();
        if (base.m_Entity.IsElite)
        {
            base.m_Entity.AddSkill(0x10c8ee, Array.Empty<object>());
        }
    }
}

