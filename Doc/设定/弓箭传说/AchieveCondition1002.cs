using Dxx.Util;
using System;

public class AchieveCondition1002 : AchieveConditionBase
{
    private int alltime;

    protected override void OnExcute()
    {
        if (GameLogic.Hold.BattleData.Win && (GameLogic.Hold.BattleData.GetGameTime() <= this.alltime))
        {
            LocalSave.Instance.Achieve_AddProgress(base.ID, 1);
        }
    }

    protected override string OnGetBattleMaxString() => 
        Utils.GetSecond2String(this.alltime);

    protected override string OnGetConditionString()
    {
        int num = this.alltime / 60;
        int num2 = this.alltime % 60;
        string languageByTID = string.Empty;
        if (num2 != 0)
        {
            object[] objArray1 = new object[] { num, num2 };
            languageByTID = GameLogic.Hold.Language.GetLanguageByTID("成就_时间分秒", objArray1);
        }
        else
        {
            object[] objArray2 = new object[] { num };
            languageByTID = GameLogic.Hold.Language.GetLanguageByTID("成就_时间分", objArray2);
        }
        object[] args = new object[] { base.mData.mData.CondType };
        object[] objArray4 = new object[] { languageByTID };
        return GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("成就_条件{0}", args), objArray4);
    }

    protected override void OnInit()
    {
        if (base.mArgs.Length != 2)
        {
            object[] args = new object[] { base.mArgs.Length };
            SdkManager.Bugly_Report(base.GetType().ToString(), Utils.FormatString("Args.Length:{0} != 2   !!!", args));
        }
        if (!int.TryParse(base.mArgs[1], out this.alltime))
        {
            SdkManager.Bugly_Report(base.GetType().ToString(), Utils.FormatString("Args[1] is not a int type", Array.Empty<object>()));
        }
    }

    protected override bool OnIsFinish() => 
        (GameLogic.Hold.BattleData.GetGameTime() <= this.alltime);
}

