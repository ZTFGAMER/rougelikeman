using System;

public class SuperSkill1002 : SuperSkillBase
{
    protected override void OnDeInit()
    {
    }

    protected override void OnInit()
    {
    }

    protected override void OnUseSkill()
    {
        GameLogic.SendBuff(base.m_Entity, 0x3e9, Array.Empty<float>());
    }
}

