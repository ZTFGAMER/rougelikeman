using System;

public class BuffAlone1008 : BuffAloneBase
{
    protected override void OnRemove()
    {
    }

    protected override void OnStart()
    {
        GameLogic.Hold.Sound.PlayBattleSpecial(0x4c4b4b, base.m_Entity.position);
    }
}

