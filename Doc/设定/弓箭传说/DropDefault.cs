using System;
using System.Collections.Generic;

public class DropDefault : DropBase
{
    protected override List<BattleDropData> OnGetDropDead()
    {
        GameLogic.Hold.Drop.GetRandomLevel(ref this.mList, base.m_Data);
        return base.mList;
    }

    protected override List<BattleDropData> OnGetHittedList(long hit) => 
        null;

    protected override void OnInit()
    {
    }
}

