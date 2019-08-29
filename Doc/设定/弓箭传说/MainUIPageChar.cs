using PureMVC.Interfaces;
using System;
using UnityEngine;

public class MainUIPageChar : UIBase
{
    private GameObject child;
    private MediatorCtrlBase ctrl;
    private ScrollRectBase mMainScroll;

    public MainUIPageChar(Transform parent, ScrollRectBase scroll) : base(parent)
    {
        this.mMainScroll = scroll;
    }

    protected override void OnClose()
    {
        if (this.ctrl != null)
        {
            this.ctrl.Close();
        }
    }

    protected override void OnDeInit()
    {
    }

    public override object OnGetEvent(string eventName)
    {
        if (this.ctrl != null)
        {
            return this.ctrl.OnGetEvent(eventName);
        }
        return null;
    }

    protected override void OnHandleNotification(INotification notification)
    {
        if (this.ctrl != null)
        {
            this.ctrl.OnHandleNotification(notification);
        }
    }

    protected override void OnInit()
    {
        base.mView = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("UIPanel/MainUI/1Char"));
        base.mView.SetParentNormal((this.child != null) ? this.child.transform : base.mParent);
        this.ctrl = base.mView.GetComponentInChildren<MediatorCtrlBase>();
        this.ctrl.SetArgs(this.mMainScroll);
        this.ctrl.Init();
    }

    protected override void OnInitBefore()
    {
        this.child = new GameObject("1_char");
        this.child.AddComponent<RectTransform>();
        this.child.transform.SetParentNormal(base.mParent);
    }

    public override void OnLanguageChange()
    {
        if (this.ctrl != null)
        {
            this.ctrl.OnLanguageChange();
        }
    }

    protected override void OnOpen()
    {
        if (this.ctrl != null)
        {
            this.ctrl.Open();
        }
    }
}

