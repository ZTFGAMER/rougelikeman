using PureMVC.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.UI;

public class GameThreeMediator : WindowMediator, IMediator, INotifier
{
    private static GameThreeUICtrl ctrl;
    private static Text textcontent;
    private static Text textok;
    private static Text textok_shadow;
    private int count;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    public GameThreeMediator() : base("GameThreeUIPanel")
    {
        this.count = 3;
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
        textcontent.text = GameLogic.Hold.Language.GetLanguageByTID("猜一猜", Array.Empty<object>());
    }

    protected override void OnRegisterEvery()
    {
        GameLogic.SetPause(true);
    }

    protected override void OnRegisterOnce()
    {
        textcontent = base._MonoView.transform.Find("Title/Text_Content").GetComponent<Text>();
        ctrl = base._MonoView.GetComponent<GameThreeUICtrl>();
        ctrl.SetCount(this.count);
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_GameThree);
        }
        ctrl.SetEndCallback(<>f__am$cache0);
        ctrl.DoAllActions();
    }

    protected override void OnRemoveAfter()
    {
        GameLogic.SetPause(false);
    }

    public override List<string> OnListNotificationInterests =>
        new List<string>();
}

