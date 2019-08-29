using System;
using UnityEngine;

public abstract class ShopOneBase : MonoBehaviour
{
    private RectTransform _rectt;

    protected ShopOneBase()
    {
    }

    private void Awake()
    {
        this.OnAwake();
    }

    public void Deinit()
    {
        this.OnDeinit();
    }

    public void Init()
    {
        this.OnInit();
    }

    protected virtual void OnAwake()
    {
    }

    protected abstract void OnDeinit();
    protected abstract void OnInit();
    public abstract void OnLanguageChange();
    public abstract void UpdateNet();

    public RectTransform mRectTransform
    {
        get
        {
            if (this._rectt == null)
            {
                this._rectt = base.transform as RectTransform;
            }
            return this._rectt;
        }
    }
}

