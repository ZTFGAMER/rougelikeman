using PureMVC.Interfaces;
using System;

public class GuideUICtrl : MediatorCtrlBase
{
    protected override void OnClose()
    {
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
        base.OnHandleNotification(notification);
    }

    protected override void OnInit()
    {
    }

    public override void OnLanguageChange()
    {
    }

    protected override void OnOpen()
    {
    }
}

