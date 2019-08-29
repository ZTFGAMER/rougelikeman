using PureMVC.Interfaces;
using System;
using UnityEngine.UI;

public class MainUIPage3Ctrl : MediatorCtrlBase
{
    public Text Text_Soon;

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
    }

    public override void OnLanguageChange()
    {
        this.Text_Soon.text = GameLogic.Hold.Language.GetLanguageByTID("Main_CommingSoon", Array.Empty<object>());
    }

    protected override void OnOpen()
    {
        this.InitUI();
    }
}

