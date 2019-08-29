using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class MailUICtrl : MediatorCtrlBase
{
    public Text Text_Title;
    public Text Text_Empty;
    public ButtonCtrl Button_Close;
    public MailInfinity mInfinity;
    public RectTransform window;
    public GameObject copyitems;
    private List<CMailInfo> mList;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    private void EmptyShow(bool value)
    {
        if (this.Text_Empty != null)
        {
            this.Text_Empty.gameObject.SetActive(value);
        }
    }

    private void InitUI()
    {
        this.UpdateList();
    }

    private void OnClickOpen(int index, MailOneCtrl one)
    {
        MailInfoProxy.Transfer data = new MailInfoProxy.Transfer {
            data = this.mList[index],
            ctrl = one
        };
        Facade.Instance.RegisterProxy(new MailInfoProxy(data));
        WindowUI.ShowWindow(WindowID.WindowID_MailInfo);
    }

    protected override void OnClose()
    {
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
        string name = notification.Name;
        object body = notification.Body;
        if ((name != null) && (name == "MailUI_MailUpdate"))
        {
            this.UpdateList();
        }
    }

    protected override void OnInit()
    {
        RectTransform transform = base.transform as RectTransform;
        this.window.sizeDelta = new Vector2(this.window.sizeDelta.x, transform.sizeDelta.y + this.window.anchoredPosition.y);
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_Mail);
        }
        this.Button_Close.onClick = <>f__am$cache0;
        this.mInfinity.initDisplayCount = 10;
        this.mInfinity.Init(1);
        this.mInfinity.updatecallback = new Action<int, MailOneCtrl>(this.UpdateChildCallBack);
        this.copyitems.SetActive(false);
    }

    public override void OnLanguageChange()
    {
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("邮件标题", Array.Empty<object>());
        this.Text_Empty.text = GameLogic.Hold.Language.GetLanguageByTID("Main_MailEmpty", Array.Empty<object>());
    }

    protected override void OnOpen()
    {
        this.InitUI();
    }

    private void UpdateChildCallBack(int index, MailOneCtrl one)
    {
        one.Init(index, this.mList[index]);
        one.OnClickButton = new Action<int, MailOneCtrl>(this.OnClickOpen);
    }

    private void UpdateList()
    {
        this.mList = LocalSave.Instance.Mail.list;
        int count = this.mList.Count;
        this.EmptyShow(count == 0);
        this.mInfinity.SetItemCount(count);
        this.mInfinity.Refresh();
    }
}

