using PureMVC.Interfaces;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class TestNoticeUICtrl : MediatorCtrlBase
{
    public Text Text_Title;
    public Text Text_Content;
    public Text Text_Sure;
    public ScrollRectBase mScrolRect;
    public ButtonCtrl Button_Sure;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    private void InitUI()
    {
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("common_notice", Array.Empty<object>());
        this.Text_Sure.text = GameLogic.Hold.Language.GetLanguageByTID("popwindow_sure", Array.Empty<object>());
        this.Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("mail_first_test", Array.Empty<object>());
        this.mScrolRect.get_content().sizeDelta = new Vector2(this.mScrolRect.get_content().sizeDelta.x, this.Text_Content.preferredHeight);
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
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_TestNotice);
        }
        this.Button_Sure.onClick = <>f__am$cache0;
    }

    public override void OnLanguageChange()
    {
    }

    protected override void OnOpen()
    {
        this.InitUI();
    }
}

