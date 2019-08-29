using Dxx.Util;
using System;

public class AchieveCondition1004 : AchieveConditionBase
{
    private int count;

    protected override void OnExcute()
    {
        int killBoss = GameLogic.Hold.BattleData.GetKillBoss();
        LocalSave.Instance.Achieve_AddProgress(base.ID, killBoss);
    }

    protected override string OnGetConditionString()
    {
        object[] args = new object[] { base.mData.mData.CondType };
        object[] objArray2 = new object[] { this.count };
        return GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("成就_条件{0}", args), objArray2);
    }

    protected override void OnInit()
    {
        this.count = base.mData.maxcount;
    }
}

