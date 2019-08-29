using PureMVC.Interfaces;
using System;
using UnityEngine;

public abstract class MediatorCtrlBase : MonoBehaviour
{
    protected bool bInitSize = true;
    protected WindowID mWindowID;

    protected MediatorCtrlBase()
    {
    }

    public void Close()
    {
        this.OnClose();
    }

    public void Init()
    {
        this.OnInitBefore();
        if (this.bInitSize)
        {
            RectTransform transform = base.transform as RectTransform;
            if (GameLogic.ScreenRatio > 1.777778f)
            {
                transform.sizeDelta = new Vector2((float) GameLogic.Width, (float) GameLogic.Height);
            }
            else
            {
                transform.sizeDelta = new Vector2((GameLogic.ScreenWidth * GameLogic.DesignHeight) / ((float) GameLogic.ScreenHeight), (float) GameLogic.DesignHeight);
            }
        }
        this.OnInit();
    }

    protected virtual void OnClose()
    {
    }

    public virtual object OnGetEvent(string eventName) => 
        null;

    public virtual void OnHandleNotification(INotification notification)
    {
    }

    protected virtual void OnInit()
    {
    }

    protected virtual void OnInitBefore()
    {
    }

    public abstract void OnLanguageChange();
    protected virtual void OnOpen()
    {
    }

    protected virtual void OnSetArgs(object o)
    {
    }

    public void Open()
    {
        this.OnOpen();
    }

    public void SetArgs(object o)
    {
        this.OnSetArgs(o);
    }
}

