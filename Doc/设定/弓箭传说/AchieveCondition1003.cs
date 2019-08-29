using Dxx.Util;
using System;

public class AchieveCondition1003 : AchieveConditionBase
{
    private int count;

    protected override void OnExcute()
    {
        int killMonsters = GameLogic.Hold.BattleData.GetKillMonsters();
        LocalSave.Instance.Achieve_AddProgress(base.ID, killMonsters);
    }

    protected override string OnGetConditionString()
    {
        object[] args = new object[] { base.mData.mData.CondType };
        object[] objArray2 = new object[] { this.count.ToString() };
        return GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("成就_条件{0}", args), objArray2);
    }

    protected override void OnInit()
    {
        this.count = base.mData.maxcount;
    }
}

