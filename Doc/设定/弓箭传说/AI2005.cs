using System;

public class AI2005 : AIBabyBase
{
    protected override void OnInit()
    {
        base.m_Entity.AddSkill(0x10c8ec, Array.Empty<object>());
        base.OnInit();
    }
}

