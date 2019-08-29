using PureMVC.Interfaces;
using System;
using System.Collections.Generic;

public class MaskModuleMediator : WindowMediator, IMediator, INotifier
{
    public MaskModuleMediator() : base("MaskUIPanel")
    {
    }

    public override void OnHandleNotification(INotification notification)
    {
        string name = notification.Name;
        object body = notification.Body;
        if (name == null)
        {
        }
    }

    protected override void OnLanguageChange()
    {
    }

    protected override void OnRegisterEvery()
    {
        base._MonoView.transform.SetAsLastSibling();
    }

    protected override void OnRegisterOnce()
    {
    }

    protected override void OnRemoveAfter()
    {
    }

    public override List<string> OnListNotificationInterests =>
        new List<string>();
}

