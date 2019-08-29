using PureMVC.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.UI;

public class EventRewardTurnUICtrl : MediatorCtrlBase
{
    public Text Text_Title;
    public ButtonCtrl Button_Start;
    public ButtonCtrl Button_Close;
    public GoldTextCtrl mGoldCtrl;
    public GameTurnTableCtrl mTurnCtrl;
    private TurnTableType resultType;
    private int[] qualities = new int[] { 1, 1, 1, 2, 2, 3 };
    private TurnTableType[] types = new TurnTableType[] { TurnTableType.Reward_Gold2 };
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    private void InitUI()
    {
        List<TurnTableData> list = new List<TurnTableData>();
        int index = 0;
        int length = this.qualities.Length;
        while (index < length)
        {
            TurnTableData item = new TurnTableData {
                type = this.types[index],
                quality = this.qualities[index]
            };
            list.Add(item);
            index++;
        }
        this.mTurnCtrl.InitGood(list);
    }

    protected override void OnClose()
    {
        GameLogic.SetPause(false);
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
        this.mTurnCtrl.TurnEnd = delegate (TurnTableData data) {
            this.resultType = data.type;
            WindowUI.CloseWindow(WindowID.WindowID_EventRewardTurn);
        };
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_EventRewardTurn);
        }
        this.Button_Close.onClick = <>f__am$cache0;
    }

    public override void OnLanguageChange()
    {
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("GameTurn_Title", Array.Empty<object>());
    }

    protected override void OnOpen()
    {
        this.mGoldCtrl.SetCurrencyType(CurrencyType.Diamond);
        this.mGoldCtrl.SetValue(50);
        GameLogic.Hold.Sound.PlayUI(0xf4244);
        GameLogic.SetPause(true);
        this.Button_Start.onClick = delegate {
            this.Button_Start.gameObject.SetActive(false);
            this.mTurnCtrl.Init();
        };
        this.Button_Start.gameObject.SetActive(true);
        this.InitUI();
    }
}

