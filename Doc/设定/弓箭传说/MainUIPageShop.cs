using PureMVC.Interfaces;
using System;
using UnityEngine;

public class MainUIPageShop : UIBase
{
    private GameObject child;
    private MediatorCtrlBase mCtrl;
    private ScrollRectBase mMainScroll;

    public MainUIPageShop(Transform parent, ScrollRectBase scroll) : base(parent)
    {
        this.mMainScroll = scroll;
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

    protected override void OnHandleNotification(INotification notification)
    {
        if (this.mCtrl != null)
        {
            this.mCtrl.OnHandleNotification(notification);
        }
    }

    protected override void OnInit()
    {
        base.mView = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("UIPanel/MainUI/0Shop"));
        base.mView.SetParentNormal((this.child != null) ? this.child.transform : base.mParent);
        this.mCtrl = base.mView.GetComponentInChildren<MediatorCtrlBase>();
        this.mCtrl.SetArgs(this.mMainScroll);
        this.mCtrl.Init();
    }

    protected override void OnInitBefore()
    {
        this.child = new GameObject("0_shop");
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

