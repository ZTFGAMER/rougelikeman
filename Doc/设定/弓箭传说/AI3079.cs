using System;

public class AI3079 : AI3001
{
    protected override void OnInitOnce()
    {
        base.m_Entity.AddSkill(0x10c8ee, Array.Empty<object>());
        base.OnInitOnce();
    }
}

