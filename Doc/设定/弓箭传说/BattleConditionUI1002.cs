using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class BattleConditionUI1002 : BattleConditionUIBase
{
    public Text Text_Content;

    protected override void OnInit()
    {
    }

    protected override void OnRefresh()
    {
        string str = Utils.GetSecond2String(GameLogic.Hold.BattleData.GetGameTime());
        object[] args = new object[] { str, base.mData.mCondition.GetBattleMaxString() };
        this.Text_Content.text = Utils.FormatString("{0}/{1}", args);
        bool flag = base.mData.mCondition.IsFinish();
        this.Text_Content.set_color(!flag ? Color.red : Color.green);
    }
}

