using PureMVC.Interfaces;
using System;
using System.Collections.Generic;

public class MediatorBase : WindowMediator, IMediator, INotifier
{
    public static Dictionary<string, MediatorCtrlBase> mCtrlList = new Dictionary<string, MediatorCtrlBase>();
    private MediatorCtrlBase _ctrl;

    public MediatorBase(string path) : base(path)
    {
    }

    public override object GetEvent(string eventName) => 
        this.ctrl.OnGetEvent(eventName);

    public override void OnHandleNotification(INotification notification)
    {
        this.ctrl.OnHandleNotification(notification);
        string name = notification.Name;
        object body = notification.Body;
        if (name == null)
        {
        }
    }

    protected override void OnLanguageChange()
    {
        this.ctrl.OnLanguageChange();
    }

    protected override void OnRegisterEvery()
    {
        this.ctrl.Open();
    }

    protected override void OnRegisterOnce()
    {
    }

    protected override void OnRemoveAfter()
    {
        if (this.ctrl != null)
        {
            this.ctrl.Close();
        }
    }

    public static void Remove(string name)
    {
        if (mCtrlList.ContainsKey(name))
        {
            mCtrlList.Remove(name);
        }
    }

    private MediatorCtrlBase ctrl
    {
        get
        {
            if (!mCtrlList.TryGetValue(base.m_mediatorName, out this._ctrl) && (base._MonoView != null))
            {
                this._ctrl = base._MonoView.transform.Find("offset").GetComponent<MediatorCtrlBase>();
                this._ctrl.Init();
                mCtrlList.Add(base.m_mediatorName, this._ctrl);
            }
            return this._ctrl;
        }
    }
}

