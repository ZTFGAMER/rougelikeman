using System;

public class BuffAlone1002 : BuffAloneBase
{
    protected override void OnRemove()
    {
    }

    protected override void OnResetBuffTime()
    {
        GameLogic.Hold.Sound.PlayBattleSpecial(0x4c4b4c, base.m_Entity.position);
    }

    protected override void OnStart()
    {
    }
}

