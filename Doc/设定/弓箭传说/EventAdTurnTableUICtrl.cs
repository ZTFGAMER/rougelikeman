using PureMVC.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine.UI;

public class EventAdTurnTableUICtrl : MediatorCtrlBase, AdsRequestHelper.AdsCallback
{
    public Text Text_Title;
    public ButtonCtrl Button_Cancel;
    public ButtonCtrl Button_Ad;
    public GameTurnTableCtrl mTurnCtrl;
    public Text Text_Turn;
    private bool bStartTurn;
    private TurnTableType resultType;
    private int[] qualities = new int[] { 1, 1, 1, 2, 2, 3 };
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    private void InitUI()
    {
        List<TurnTableData> list = new List<TurnTableData>();
        string[] adTurn = LocalModelManager.Instance.Stage_Level_stagechapter.GetBeanByChapter(GameLogic.Hold.BattleData.Level_CurrentStage).AdTurn;
        int index = 0;
        int length = adTurn.Length;
        while ((index < length) && (index < 6))
        {
            TurnTableData item = new TurnTableData();
            char[] separator = new char[] { ',' };
            string[] strArray2 = adTurn[index].Split(separator);
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

    public void onClick(AdsRequestHelper.AdsDriver sender, string networkName)
    {
    }

    public void onClose(AdsRequestHelper.AdsDriver sender, string networkName)
    {
        if (!this.bStartTurn)
        {
            this.Button_Ad.gameObject.SetActive(true);
            this.Button_Cancel.gameObject.SetActive(true);
        }
    }

    protected override void OnClose()
    {
        GameLogic.SetPause(false);
    }

    public void onFail(AdsRequestHelper.AdsDriver sender, string msg)
    {
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
            WindowUI.CloseWindow(WindowID.WindowID_EventAdTurnTable);
        };
    }

    public override void OnLanguageChange()
    {
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("GameTurn_Title", Array.Empty<object>());
        this.Text_Turn.text = GameLogic.Hold.Language.GetLanguageByTID("event_ad_turntable_turn", Array.Empty<object>());
    }

    public void onLoad(AdsRequestHelper.AdsDriver sender, string networkName)
    {
    }

    public void onOpen(AdsRequestHelper.AdsDriver sender, string networkName)
    {
    }

    protected override void OnOpen()
    {
        this.bStartTurn = false;
        GameLogic.Hold.Sound.PlayUI(0xf4244);
        GameLogic.SetPause(true);
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_EventAdTurnTable);
        }
        this.Button_Cancel.onClick = <>f__am$cache0;
        this.Button_Ad.onClick = delegate {
            LocalSave.Instance.BattleAd_Use();
            this.Button_Ad.gameObject.SetActive(false);
            this.Button_Cancel.gameObject.SetActive(false);
            AdsRequestHelper.getRewardedAdapter().Show(this);
        };
        this.Button_Ad.gameObject.SetActive(true);
        this.Button_Cancel.gameObject.SetActive(true);
        this.InitUI();
    }

    public void onRequest(AdsRequestHelper.AdsDriver sender, string networkName)
    {
    }

    public void onReward(AdsRequestHelper.AdsDriver sender, string networkName)
    {
        this.mTurnCtrl.Init();
        this.bStartTurn = true;
        this.Button_Ad.gameObject.SetActive(false);
        this.Button_Cancel.gameObject.SetActive(false);
    }
}

