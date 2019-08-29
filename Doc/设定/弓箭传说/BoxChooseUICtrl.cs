using PureMVC.Interfaces;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.UI;

public class BoxChooseUICtrl : MediatorCtrlBase
{
    public Text Text_Title;
    public ButtonCtrl Button_Close;
    public ButtonCtrl Button_Shadow;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    private void InitUI()
    {
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
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_BoxChoose);
        }
        this.Button_Close.onClick = <>f__am$cache0;
        this.Button_Shadow.onClick = this.Button_Close.onClick;
    }

    public override void OnLanguageChange()
    {
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("宝箱_标题", Array.Empty<object>());
    }

    protected override void OnOpen()
    {
        this.InitUI();
    }
}

