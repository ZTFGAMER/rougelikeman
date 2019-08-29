using System;
using System.Collections.Generic;

public class DropGold : DropBase
{
    private List<BattleDropData> mHittedList = new List<BattleDropData>();
    private int listcount;
    private int allcount;

    protected override List<BattleDropData> OnGetDropDead()
    {
        GameLogic.Hold.Drop.GetRandomLevel(ref this.mList, base.m_Data);
        return base.mList;
    }

    protected override List<BattleDropData> OnGetHittedList(long hit)
    {
        this.mHittedList.Clear();
        base.currentHP += hit;
        float num = ((float) base.currentHP) / ((float) base.MaxHP);
        int num2 = (int) (num * this.listcount);
        int num3 = this.allcount - num2;
        this.allcount = num2;
        for (int i = 0; i < num3; i++)
        {
            this.mHittedList.Add(base.mList[0]);
            base.mList.RemoveAt(0);
        }
        return this.mHittedList;
    }

    protected override void OnInit()
    {
        GameLogic.Hold.Drop.GetRandomGoldHitted(ref this.mList, base.m_Data);
        this.listcount = base.mList.Count;
        this.allcount = this.listcount;
    }
}

