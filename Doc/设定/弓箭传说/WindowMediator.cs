using DG.Tweening;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class WindowMediator : Mediator
{
    public static Dictionary<string, WindowCacheData> mCacheUIPanel = new Dictionary<string, WindowCacheData>();
    public static Dictionary<string, FrontEventCtrl> mFrontEventList = new Dictionary<string, FrontEventCtrl>();
    public static Dictionary<string, Sequence> mSequences = new Dictionary<string, Sequence>();
    protected string UIPath;
    private GameObject _popupparent;
    private FrontEventCtrl mFrontEventCtrl;
    protected GameObject _MonoView;

    public WindowMediator(string path)
    {
        base.m_mediatorName = base.GetType().ToString();
        this.UIPath = path;
    }

    private void ClearSeq()
    {
        Sequence sequence = null;
        if (mSequences.TryGetValue(base.m_mediatorName, out sequence) && (sequence != null))
        {
            TweenExtensions.Kill(sequence, false);
            sequence = null;
        }
    }

    public static Transform GetParent(string name)
    {
        switch (UIResourceDefine.GetWindowLayerType(name))
        {
            case LayerType.eRoot:
                return GameNode.m_UIMain;

            case LayerType.eInGame:
                return GameNode.m_InGame;

            case LayerType.eFront:
                return GameNode.m_Front;

            case LayerType.eFrontEvent:
                return GameNode.m_FrontEvent;

            case LayerType.eFront2:
                return GameNode.m_Front2;

            case LayerType.eFront3:
                return GameNode.m_Front3;

            case LayerType.eFrontMask:
                return GameNode.m_FrontMask;

            case LayerType.eFrontNet:
                return GameNode.m_FrontNet;
        }
        return null;
    }

    private Sequence GetSeq()
    {
        Sequence sequence = null;
        if (mSequences.TryGetValue(base.m_mediatorName, out sequence))
        {
            if (sequence != null)
            {
                TweenExtensions.Kill(sequence, false);
                sequence = null;
            }
            sequence = DOTween.Sequence();
            mSequences[base.m_mediatorName] = sequence;
            return sequence;
        }
        sequence = DOTween.Sequence();
        mSequences.Add(base.m_mediatorName, sequence);
        return sequence;
    }

    public sealed override void HandleNotification(INotification notification)
    {
        string name = notification.Name;
        object body = notification.Body;
        if ((name != null) && (name == "PUB_LANGUAGE_UPDATE"))
        {
            this.OnLanguageChange();
        }
        this.OnHandleNotification(notification);
    }

    public virtual void OnHandleNotification(INotification notification)
    {
    }

    protected abstract void OnLanguageChange();
    public override void OnRegister()
    {
        this.ClearSeq();
        this.OnRegisterBefore();
        GameObject obj2 = null;
        bool flag = mCacheUIPanel.TryGetValue(base.m_mediatorName, out WindowCacheData data);
        if (data != null)
        {
            obj2 = data.obj;
        }
        if (obj2 == null)
        {
            this._MonoView = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("UIPanel/" + this.UIPath));
            if (UIResourceDefine.GetWindowPop(base.m_mediatorName))
            {
                this._popupparent = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("UIPanel/ACommon/PopUpParent"));
                this._popupparent.name = base.m_mediatorName;
                RectTransform transform = this._popupparent.transform as RectTransform;
                transform.SetParent(GetParent(base.m_mediatorName));
                transform.localPosition = Vector3.zero;
                transform.offsetMin = Vector2.zero;
                transform.offsetMax = Vector2.zero;
                this.mFrontEventCtrl = this._popupparent.GetComponent<FrontEventCtrl>();
                mFrontEventList.Add(base.m_mediatorName, this.mFrontEventCtrl);
                this.mFrontEventCtrl.Play(true);
                this._MonoView.SetParentNormal(this._popupparent.transform);
            }
            else
            {
                this._MonoView.SetParentNormal(GetParent(base.m_mediatorName));
            }
            base.ViewComponent = this._MonoView;
            if (!flag)
            {
                WindowCacheData data2 = new WindowCacheData {
                    name = base.m_mediatorName,
                    obj = this._MonoView,
                    lasttime = Time.realtimeSinceStartup
                };
                mCacheUIPanel.Add(base.m_mediatorName, data2);
            }
            else
            {
                mCacheUIPanel[base.m_mediatorName].obj = this._MonoView;
                mCacheUIPanel[base.m_mediatorName].lasttime = Time.realtimeSinceStartup;
            }
            this.OnRegisterOnce();
        }
        else
        {
            mCacheUIPanel[base.m_mediatorName].lasttime = Time.realtimeSinceStartup;
            this._MonoView = obj2;
            this._MonoView.SetActive(true);
            this._MonoView.transform.SetAsLastSibling();
            mFrontEventList.TryGetValue(base.m_mediatorName, out this.mFrontEventCtrl);
            if (this.mFrontEventCtrl != null)
            {
                this.mFrontEventCtrl.gameObject.SetActive(true);
                this.mFrontEventCtrl.transform.SetAsLastSibling();
                this.mFrontEventCtrl.Play(true);
            }
        }
        this.OnRegisterEvery();
        this.OnLanguageChange();
    }

    protected virtual void OnRegisterBefore()
    {
    }

    protected virtual void OnRegisterEvery()
    {
    }

    protected virtual void OnRegisterOnce()
    {
    }

    public sealed override void OnRemove()
    {
        if (UIResourceDefine.GetWindowPop(base.m_mediatorName) && (this.mFrontEventCtrl != null))
        {
            this.mFrontEventCtrl.Play(false);
            TweenSettingsExtensions.SetUpdate<Sequence>(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(this.GetSeq(), 0.15f), new TweenCallback(this, this.<OnRemove>m__0)), true);
        }
        else
        {
            if (this._MonoView != null)
            {
                this._MonoView.SetActive(false);
            }
            WindowUI.PopClose();
        }
        this.OnRemoveAfter();
    }

    protected virtual void OnRemoveAfter()
    {
    }

    public virtual void OnShowWindow()
    {
    }

    public static void RemoveCache(string name)
    {
        if (mCacheUIPanel.TryGetValue(name, out WindowCacheData data))
        {
            mFrontEventList.TryGetValue(name, out FrontEventCtrl ctrl);
            if (ctrl != null)
            {
                Object.Destroy(ctrl.gameObject);
                mFrontEventList.Remove(name);
            }
            else if (data.obj != null)
            {
                Object.Destroy(data.obj);
                data.obj = null;
            }
            mCacheUIPanel.Remove(name);
        }
        MediatorBase.Remove(name);
    }

    public void WindowShowUI(bool show)
    {
        if (this._MonoView != null)
        {
            this._MonoView.SetActive(show);
            if (this.mFrontEventCtrl != null)
            {
                if (show)
                {
                    this.mFrontEventCtrl.gameObject.SetActive(true);
                    this.mFrontEventCtrl.Play(true);
                }
                else
                {
                    this.mFrontEventCtrl.Play(false);
                    TweenSettingsExtensions.SetUpdate<Sequence>(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.15f), new TweenCallback(this, this.<WindowShowUI>m__1)), true);
                }
            }
        }
    }

    public sealed override IEnumerable<string> ListNotificationInterests
    {
        get
        {
            List<string> list = new List<string> { 
                "PUB_LANGUAGE_UPDATE",
                "PUB_UI_UPDATE_CURRENCY",
                "PUB_NETCONNECT_UPDATE",
                "PUB_UI_UPDATE_PING"
            };
            List<string> onListNotificationInterests = this.OnListNotificationInterests;
            int num = 0;
            int count = onListNotificationInterests.Count;
            while (num < count)
            {
                list.Add(onListNotificationInterests[num]);
                num++;
            }
            return list;
        }
    }

    public virtual List<string> OnListNotificationInterests =>
        new List<string>();

    public enum LayerType
    {
        eRoot,
        eInGame,
        eFront,
        eFrontEvent,
        eFront2,
        eFront3,
        eFrontMask,
        eFrontNet
    }

    public class WindowCacheData
    {
        public string name;
        public GameObject obj;
        public float lasttime;
    }
}

