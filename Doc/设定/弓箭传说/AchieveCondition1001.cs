using Dxx.Util;

public class AchieveCondition1001 : AchieveConditionBase
{
    private int maxlevel;
    private int hittedcount;

    protected override void OnExcute()
    {
        if ((GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID() >= this.maxlevel) && (GameLogic.Hold.BattleData.GetHittedCount(this.maxlevel) <= this.hittedcount))
        {
            LocalSave.Instance.Achieve_AddProgress(base.ID, 1);
        }
    }

    protected override string OnGetBattleMaxString() => 
        this.hittedcount.ToString();

    protected override string OnGetConditionString()
    {
        object[] args = new object[] { base.mData.mData.CondType };
        object[] objArray2 = new object[] { this.maxlevel.ToString(), this.hittedcount.ToString() };
        return GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("成就_条件{0}", args), objArray2);
    }

    protected override void OnInit()
    {
        if (base.mArgs.Length != 3)
        {
            object[] args = new object[] { base.mArgs.Length };
            SdkManager.Bugly_Report("AchieveCondition1001", Utils.FormatString("Args.Length:{0} != 3   !!!", args));
        }
        if (!int.TryParse(base.mArgs[1], out this.maxlevel))
        {
            SdkManager.Bugly_Report("AchieveCondition1001", Utils.FormatString("Args[1] is not a int type", Array.Empty<object>()));
        }
        if (!int.TryParse(base.mArgs[2], out this.hittedcount))
        {
            SdkManager.Bugly_Report("AchieveCondition1001", Utils.FormatString("Args[2] is not a int type", Array.Empty<object>()));
        }
    }

    protected override bool OnIsFinish() => 
        (GameLogic.Hold.BattleData.GetHittedCount() <= this.hittedcount);
}

