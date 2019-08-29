using PureMVC.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RateModuleMediator : WindowMediator, IMediator, INotifier
{
    private static Text Text_Title;
    private static ButtonCtrl Button_OK;
    private static ButtonCtrl Button_Refuse;
    private static Text Text_OK;
    private static Text Text_Cancel;
    private static Animator TitleAni;
    private static Animator OKAni;
    private static Animator RefuseAni;

    public RateModuleMediator() : base("RateUIPanel")
    {
    }

    private void CloseRate()
    {
        WindowUI.CloseWindow(WindowID.WindowID_Rate);
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
        Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("点赞标题", Array.Empty<object>());
        Text_OK.text = GameLogic.Hold.Language.GetLanguageByTID("去点赞", Array.Empty<object>());
        Text_Cancel.text = GameLogic.Hold.Language.GetLanguageByTID("点赞拒绝", Array.Empty<object>());
    }

    protected override void OnRegisterEvery()
    {
        GameLogic.SetPause(true);
        Button_OK.onClick = delegate {
            this.CloseRate();
            RateUrlManager.OpenAppUrl();
        };
        Button_Refuse.onClick = () => this.CloseRate();
        TitleAni.enabled = true;
        OKAni.enabled = true;
        RefuseAni.enabled = true;
        TitleAni.Play("RateUITitleShow");
        OKAni.Play("RateUIButtonShow");
        RefuseAni.Play("RateUIButtonShow");
    }

    protected override void OnRegisterOnce()
    {
        Text_Title = base._MonoView.transform.Find("Middle/Text_Title").GetComponent<Text>();
        Button_OK = base._MonoView.transform.Find("Middle/ok/Button_ok").GetComponent<ButtonCtrl>();
        Button_Refuse = base._MonoView.transform.Find("Middle/cancel/Button_Cancel").GetComponent<ButtonCtrl>();
        Text_OK = Button_OK.transform.Find("fg/Text").GetComponent<Text>();
        Text_Cancel = Button_Refuse.transform.Find("fg/Text").GetComponent<Text>();
        TitleAni = base._MonoView.transform.Find("Middle/Text_Title").GetComponent<Animator>();
        OKAni = base._MonoView.transform.Find("Middle/ok").GetComponent<Animator>();
        RefuseAni = base._MonoView.transform.Find("Middle/cancel").GetComponent<Animator>();
    }

    protected override void OnRemoveAfter()
    {
        TitleAni.enabled = false;
        OKAni.enabled = false;
        RefuseAni.enabled = false;
        GameLogic.SetPause(false);
        WindowUI.ShowWindow(WindowID.WindowID_GameOver);
    }

    public override List<string> OnListNotificationInterests =>
        new List<string>();
}

