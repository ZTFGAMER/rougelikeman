using PureMVC.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ProducterUICtrl : MediatorCtrlBase
{
    public ButtonCtrl Button_Close;
    public List<ProducterOneCtrl> mList;
    private static List<string> mProducters;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    static ProducterUICtrl()
    {
        List<string> list = new List<string> { 
            "Sinan Zhu",
            "Han Zhang",
            "Jian Chen"
        };
        mProducters = list;
    }

    private void InitUI()
    {
        int num = 0;
        int count = this.mList.Count;
        while (num < count)
        {
            this.mList[num].Init(mProducters[num]);
            num++;
        }
    }

    protected override void OnClose()
    {
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
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_Producer);
        }
        this.Button_Close.onClick = <>f__am$cache0;
    }

    public override void OnLanguageChange()
    {
    }

    protected override void OnOpen()
    {
        this.InitUI();
    }

    private void UpdateChildCallBack(int index, ProducterOneCtrl one)
    {
    }

    private void UpdateList()
    {
    }
}

