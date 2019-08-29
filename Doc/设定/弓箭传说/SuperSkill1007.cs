using System;

public class SuperSkill1007 : SuperSkillBase
{
    protected override void OnDeInit()
    {
    }

    protected override void OnInit()
    {
    }

    protected override void OnUseSkill()
    {
        GameLogic.SendBuff(base.m_Entity, 0x402, Array.Empty<float>());
    }
}

