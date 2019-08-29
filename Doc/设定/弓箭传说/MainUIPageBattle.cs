using PureMVC.Interfaces;
using System;
using UnityEngine;

public class MainUIPageBattle : UIBase
{
    private GameObject child;
    private MediatorCtrlBase mCtrl;

    public MainUIPageBattle(Transform parent) : base(parent)
    {
    }

    protected override void OnClose()
    {
        if (this.mCtrl != null)
        {
            this.mCtrl.Close();
        }
    }

    protected override void OnDeInit()
    {
    }

    public override object OnGetEvent(string eventName)
    {
        if (this.mCtrl != null)
        {
            return this.mCtrl.OnGetEvent(eventName);
        }
        return null;
    }

    protected override void OnHandleNotification(INotification notification)
    {
        if (this.mCtrl != null)
        {
            this.mCtrl.OnHandleNotification(notification);
        }
    }

    protected override void OnInit()
    {
        base.mView = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("UIPanel/MainUI/2Battle"));
        base.mView.SetParentNormal((this.child != null) ? this.child.transform : base.mParent);
        this.mCtrl = base.mView.transform.Find("offset").GetComponent<MediatorCtrlBase>();
        this.mCtrl.Init();
    }

    protected override void OnInitBefore()
    {
        this.child = new GameObject("2_battle");
        this.child.AddComponent<RectTransform>();
        this.child.transform.SetParentNormal(base.mParent);
    }

    public override void OnLanguageChange()
    {
        if (this.mCtrl != null)
        {
            this.mCtrl.OnLanguageChange();
        }
    }

    protected override void OnOpen()
    {
        if (this.mCtrl != null)
        {
            this.mCtrl.Open();
        }
    }
}

