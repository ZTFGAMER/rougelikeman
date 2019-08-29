using Dxx.Util;
using System;

public class AchieveCondition1006 : AchieveConditionBase
{
    private int entityid;
    private int count;

    protected override void OnExcute()
    {
        int killMonsters = GameLogic.Hold.BattleData.GetKillMonsters(this.entityid);
        LocalSave.Instance.Achieve_AddProgress(base.ID, killMonsters);
    }

    protected override string OnGetConditionString()
    {
        object[] args = new object[] { this.entityid };
        string languageByTID = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("monstername{0}", args), Array.Empty<object>());
        object[] objArray2 = new object[] { base.mData.mData.CondType };
        object[] objArray3 = new object[] { languageByTID, this.count };
        return GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("成就_条件{0}", objArray2), objArray3);
    }

    protected override void OnInit()
    {
        if (base.mArgs.Length != 2)
        {
            object[] args = new object[] { base.mArgs.Length };
            SdkManager.Bugly_Report(base.GetType().ToString(), Utils.FormatString("Args.Length:{0} != 2   !!!", args));
        }
        this.count = base.mData.maxcount;
        if (!int.TryParse(base.mArgs[1], out this.entityid))
        {
            SdkManager.Bugly_Report(base.GetType().ToString(), Utils.FormatString("Args[0] is not a int type", Array.Empty<object>()));
        }
    }
}

