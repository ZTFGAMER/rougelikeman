using PureMVC.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;

public class ActiveUICtrl : MediatorCtrlBase
{
    private const string Ani_Info_Show = "Info_Show";
    private const string Ani_Info_Hide = "Info_Hide";
    public GameObject copyitems;
    public ButtonCtrl Button_Close;
    public ScrollIntActiveCtrl mScrollInt;
    public Transform mScrollChild;
    public GameObject copyActive;
    public GameObject copyDiffcult;
    public ActiveInfoCtrl mInfoCtrl;
    public Animation mInfoAni;
    private int showCount = 10;
    private int count = 40;
    private float allWidth;
    private float itemWidth;
    private float offsetx = 360f;
    private float lastscrollpos;
    private float lastspeed;
    private int mCurrentIndex;
    private List<ActiveOneCtrl> mCaches = new List<ActiveOneCtrl>();
    private List<Stage_Level_activityModel.ActivityTypeData> mDataList;
    private int currentChoose;
    private ActiveOneCtrl mChooseActive;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    private void InitUI()
    {
        this.mDataList = LocalModelManager.Instance.Stage_Level_activity.GetDifficults();
        this.currentChoose = 0;
        this.mScrollInt.Init(this.mDataList.Count);
    }

    private void OnBeginDrag()
    {
        this.mInfoAni.Play("Info_Hide");
    }

    protected override void OnClose()
    {
        this.mScrollInt.DeInit();
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_Active);
        }
        this.Button_Close.onClick = <>f__am$cache0;
        this.mScrollInt.copyItem = this.copyActive;
        this.mScrollInt.mScrollChild = this.mScrollChild;
        this.mScrollInt.OnUpdateOne = new Action<int, ActiveOneCtrl>(this.UpdateActiveOne);
        this.mScrollInt.OnUpdateSize = new Action<int, ActiveOneCtrl>(this.UpdateActiveSize);
        this.mScrollInt.OnBeginDragEvent = new Action(this.OnBeginDrag);
        this.mScrollInt.OnScrollEnd = new Action<int, ActiveOneCtrl>(this.OnScrollEnd);
        this.copyitems.SetActive(false);
    }

    public override void OnLanguageChange()
    {
    }

    protected override void OnOpen()
    {
        this.InitUI();
    }

    private void OnScrollEnd(int index, ActiveOneCtrl one)
    {
        this.mInfoCtrl.Init(one.activedata);
        this.mInfoAni.Play("Info_Show");
        this.currentChoose = index;
        this.mChooseActive = one;
        this.UpdateUI();
    }

    private void UpdateActiveOne(int index, ActiveOneCtrl one)
    {
        one.Init(this.mDataList[index]);
        if ((index == 0) && (this.mChooseActive == null))
        {
            this.mChooseActive = one;
            this.mInfoCtrl.Init(one.activedata);
        }
    }

    private void UpdateActiveSize(int index, ActiveOneCtrl one)
    {
        Stage_Level_activityModel.ActivityTypeData data = this.mDataList[index];
    }

    private void UpdateUI()
    {
    }
}

