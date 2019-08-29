using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BattleConditionUI1001 : BattleConditionUIBase
{
    public Text Text_Content;

    protected override void OnInit()
    {
    }

    protected override void OnRefresh()
    {
        string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("成就_受伤次数", Array.Empty<object>());
        object[] args = new object[] { languageByTID, GameLogic.Hold.BattleData.GetHittedCount(), base.mData.mCondition.GetBattleMaxString() };
        this.Text_Content.text = Utils.FormatString("{0}:{1}/{2}", args);
        bool flag = base.mData.mCondition.IsFinish();
        this.Text_Content.set_color(!flag ? Color.red : Color.green);
    }
}

