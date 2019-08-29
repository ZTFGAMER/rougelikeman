using PureMVC.Interfaces;
using System;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;

public class AchieveUICtrl : MediatorCtrlBase
{
    public GameObject copyitems;
    public ButtonCtrl Button_Close;
    public ScrollIntAchieveCtrl mScrollInt;
    public Transform mScrollChild;
    public GameObject copyitem;
    public GameObject copyone;
    private int showCount = 10;
    private int count = 40;
    private float allWidth;
    private float itemWidth;
    private float offsetx = 360f;
    private float lastscrollpos;
    private float lastspeed;
    private int mCurrentIndex;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    private void InitUI()
    {
        this.mScrollInt.Init(LocalModelManager.Instance.Stage_Level_stagechapter.GetMaxChapter_Hero());
    }

    private void OnBeginDrag()
    {
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
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_Achieve);
        }
        this.Button_Close.onClick = <>f__am$cache0;
        this.mScrollInt.copyItem = this.copyitem;
        this.mScrollInt.mScrollChild = this.mScrollChild;
        this.mScrollInt.OnUpdateOne = new Action<int, AchieveItemCtrl>(this.UpdateActiveOne);
        this.mScrollInt.OnUpdateSize = new Action<int, AchieveItemCtrl>(this.UpdateActiveSize);
        this.mScrollInt.OnBeginDragEvent = new Action(this.OnBeginDrag);
        this.mScrollInt.OnScrollEnd = new Action<int, AchieveItemCtrl>(this.OnScrollEnd);
        this.copyitems.SetActive(false);
    }

    public override void OnLanguageChange()
    {
    }

    protected override void OnOpen()
    {
        this.InitUI();
    }

    private void OnScrollEnd(int index, AchieveItemCtrl one)
    {
    }

    private void UpdateActiveOne(int index, AchieveItemCtrl one)
    {
        one.Init(index + 1);
    }

    private void UpdateActiveSize(int index, AchieveItemCtrl one)
    {
    }
}

