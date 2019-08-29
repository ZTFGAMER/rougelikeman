using PureMVC.Interfaces;
using System;
using UnityEngine;

public class MainUIPage4 : UIBase
{
    private GameObject child;
    private MediatorCtrlBase ctrl;
    private ButtonCtrl mButtonStart;

    public MainUIPage4(Transform parent) : base(parent)
    {
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
        base.mView = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("UIPanel/MainUI/4"));
        base.mView.SetParentNormal((this.child != null) ? this.child.transform : base.mParent);
        this.ctrl = base.mView.GetComponentInChildren<MediatorCtrlBase>();
        this.ctrl.Init();
    }

    protected override void OnInitBefore()
    {
        this.child = new GameObject("4_setting");
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

