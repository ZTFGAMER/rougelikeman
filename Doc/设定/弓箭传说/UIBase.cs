using PureMVC.Interfaces;
using System;
using UnityEngine;

public abstract class UIBase
{
    protected GameObject mView;
    protected Transform mParent;

    public UIBase(Transform parent)
    {
        this.mParent = parent;
    }

    public void Close()
    {
        this.OnClose();
    }

    public void DeInit()
    {
        this.OnDeInit();
    }

    public void HandleNotification(INotification notification)
    {
        this.OnHandleNotification(notification);
    }

    public void Init()
    {
        this.OnInit();
    }

    public void InitBefore()
    {
        this.OnInitBefore();
    }

    protected abstract void OnClose();
    protected abstract void OnDeInit();
    public virtual object OnGetEvent(string eventName) => 
        null;

    protected abstract void OnHandleNotification(INotification notification);
    protected abstract void OnInit();
    protected virtual void OnInitBefore()
    {
    }

    public abstract void OnLanguageChange();
    protected abstract void OnOpen();
    public void Open()
    {
        this.OnOpen();
    }
}

