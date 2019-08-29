using PureMVC.Interfaces;
using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine.UI;

public class EventFirstGoldUICtrl : MediatorCtrlBase
{
    public Text Text_Title;
    public ButtonCtrl Button_Start;
    public Text Text_Start;
    public GameTurnTableCtrl mTurnCtrl;
    private TurnTableType resultType;
    private int[] qualities = new int[] { 1, 1, 1, 3, 3, 4 };

    private void InitUI()
    {
        List<TurnTableData> list = new List<TurnTableData>();
        string[] goldTurn = LocalModelManager.Instance.Stage_Level_stagechapter.GetBeanByChapter(GameLogic.Hold.BattleData.Level_CurrentStage).GoldTurn;
        int index = 0;
        int length = goldTurn.Length;
        while ((index < length) && (index < 6))
        {
            TurnTableData item = new TurnTableData();
            char[] separator = new char[] { ',' };
            string[] strArray2 = goldTurn[index].Split(separator);
            int result = 0;
            int.TryParse(strArray2[0], out result);
            long num4 = 0L;
            long.TryParse(strArray2[1], out num4);
            if (num4 > 0L)
            {
                if (result == 1)
                {
                    item.type = TurnTableType.Gold;
                }
                else
                {
                    item.type = TurnTableType.Diamond;
                }
                item.value = num4;
            }
            else
            {
                item.type = TurnTableType.Empty;
            }
            item.quality = this.qualities[index];
            list.Add(item);
            index++;
        }
        for (int i = list.Count; i < 6; i++)
        {
            TurnTableData item = new TurnTableData {
                type = TurnTableType.Empty
            };
            list.Add(item);
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
            WindowUI.CloseWindow(WindowID.WindowID_EventFirstGold);
        };
    }

    public override void OnLanguageChange()
    {
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("GameTurn_Title", Array.Empty<object>());
        this.Text_Start.text = GameLogic.Hold.Language.GetLanguageByTID("开始", Array.Empty<object>());
    }

    protected override void OnOpen()
    {
        GameLogic.Hold.Sound.PlayUI(0xf4244);
        LocalSave.Instance.BattleIn_UpdateGoldTurn();
        GameLogic.SetPause(true);
        this.Button_Start.onClick = delegate {
            this.Button_Start.gameObject.SetActive(false);
            this.mTurnCtrl.Init();
        };
        this.Button_Start.gameObject.SetActive(true);
        this.InitUI();
    }
}

