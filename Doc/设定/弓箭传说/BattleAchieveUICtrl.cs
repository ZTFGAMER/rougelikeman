using PureMVC.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class BattleAchieveUICtrl : MediatorCtrlBase
{
    public ScrollRectBase mScrollRect;
    public GridLayoutGroup mGrid;
    public GameObject copyitems;
    public GameObject copyachieve;
    public ButtonCtrl Button_Close;
    private List<BattleAchieveOneCtrl> mList = new List<BattleAchieveOneCtrl>();
    private LocalUnityObjctPool mPool;
    [CompilerGenerated]
    private static Action <>f__am$cache0;
    [CompilerGenerated]
    private static Comparison<int> <>f__am$cache1;
    [CompilerGenerated]
    private static Action<int> <>f__am$cache2;

    private void InitUI()
    {
        this.mPool.Collect<BattleAchieveOneCtrl>();
        this.mList.Clear();
        List<int> stageList = LocalModelManager.Instance.Achieve_Achieve.GetStageList(GameLogic.Hold.BattleData.Level_CurrentStage, false);
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = delegate (int a, int b) {
                bool flag = LocalSave.Instance.Achieve_IsFinish(a);
                bool flag2 = LocalSave.Instance.Achieve_IsFinish(b);
                if (flag == flag2)
                {
                    return (a >= b) ? 1 : -1;
                }
                return !flag ? -1 : 1;
            };
        }
        stageList.Sort(<>f__am$cache1);
        int num = 0;
        int count = stageList.Count;
        while (num < count)
        {
            BattleAchieveOneCtrl ctrl = this.mPool.DeQueue<BattleAchieveOneCtrl>();
            ctrl.gameObject.SetParentNormal(this.mScrollRect.get_content());
            ctrl.Init(stageList[num]);
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = delegate (int achieveid) {
                    <InitUI>c__AnonStorey0 storey = new <InitUI>c__AnonStorey0 {
                        achieveid = achieveid
                    };
                    LocalSave.Instance.BattleIn_DeInit();
                    WindowUI.ShowLoading(new Action(storey.<>m__0), null, null, BattleLoadProxy.LoadingType.eMiss);
                };
            }
            ctrl.OnButtonClick = <>f__am$cache2;
            num++;
        }
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
        this.mPool = LocalUnityObjctPool.Create(base.gameObject);
        this.mPool.CreateCache<BattleAchieveOneCtrl>(this.copyachieve);
        this.copyitems.SetActive(false);
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_BattleAchieve);
        }
        this.Button_Close.onClick = <>f__am$cache0;
    }

    public override void OnLanguageChange()
    {
    }

    protected override void OnOpen()
    {
        GameLogic.SetPause(true);
        this.InitUI();
    }

    [CompilerGenerated]
    private sealed class <InitUI>c__AnonStorey0
    {
        internal int achieveid;

        internal void <>m__0()
        {
            WindowUI.ShowWindow(WindowID.WindowID_Main);
            GameLogic.Hold.BattleData.Challenge_MainUpdateMode(this.achieveid);
            WindowUI.ShowWindow(WindowID.WindowID_Battle);
        }
    }
}

