using Dxx.Util;
using System;

public class AchieveCondition1005 : AchieveConditionBase
{
    private int entityid;

    protected override void OnExcute()
    {
        if (GameLogic.Hold.BattleData.GetKillBoss(0x1389) > 0)
        {
        }
    }

    protected override string OnGetConditionString()
    {
        object[] args = new object[] { this.entityid };
        string languageByTID = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("monstername{0}", args), Array.Empty<object>());
        object[] objArray2 = new object[] { base.mData.mData.CondType };
        object[] objArray3 = new object[] { languageByTID };
        return GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("成就_条件{0}", objArray2), objArray3);
    }

    protected override void OnInit()
    {
        if (base.mArgs.Length != 2)
        {
            object[] args = new object[] { base.mArgs.Length };
            SdkManager.Bugly_Report(base.GetType().ToString(), Utils.FormatString("Args.Length:{0} != 2   !!!", args));
        }
        if (!int.TryParse(base.mArgs[1], out this.entityid))
        {
            SdkManager.Bugly_Report(base.GetType().ToString(), Utils.FormatString("Args[1] is not a int type", Array.Empty<object>()));
        }
    }
}

