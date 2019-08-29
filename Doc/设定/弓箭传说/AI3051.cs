using System;

public class AI3051 : AIBase
{
    protected override void OnInit()
    {
        base.AddAction(new AIMove1034(base.m_Entity));
        base.AddAction(new AIMove1034(base.m_Entity));
        base.AddAction(new AIMove1034(base.m_Entity));
        base.AddAction(new AIMove1034(base.m_Entity));
        base.AddAction(base.GetActionWait(string.Empty, 500));
        base.AddAction(base.GetActionAttackSpecial(string.Empty, base.m_Entity.m_Data.WeaponID, false));
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

