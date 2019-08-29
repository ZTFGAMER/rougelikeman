using System;
using System.Collections.Generic;

public class DropChallenge101 : DropBase
{
    protected override List<BattleDropData> OnGetDropDead() => 
        base.mList;

    protected override List<BattleDropData> OnGetHittedList(long hit) => 
        null;

    protected override void OnInit()
    {
    }
}

