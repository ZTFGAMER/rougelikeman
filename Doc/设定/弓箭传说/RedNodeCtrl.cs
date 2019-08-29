using System;
using UnityEngine;

public class RedNodeCtrl : MonoBehaviour
{
    private Transform child;
    private RedNodeOneCtrl _redctrl;

    private void Awake()
    {
        this.child = base.transform.Find("child");
    }

    private void child_show(bool show)
    {
        if (this.child != null)
        {
            this.child.gameObject.SetActive(show);
        }
    }

    public void DestroyChild()
    {
        if (this._redctrl != null)
        {
            Object.DestroyImmediate(this._redctrl.gameObject);
        }
    }

    private void SetText(string value)
    {
        this.mRedCtrl.SetText(value);
    }

    public void SetType(RedNodeType type)
    {
        this.mRedCtrl.SetType(type);
    }

    private RedNodeOneCtrl mRedCtrl
    {
        get
        {
            if ((this._redctrl == null) && (this.child != null))
            {
                GameObject child = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("UIPanel/ACommon/RedNodeOne"));
                child.SetParentNormal(this.child);
                this._redctrl = child.GetComponent<RedNodeOneCtrl>();
            }
            return this._redctrl;
        }
    }

    public int Value
    {
        set
        {
            if (value > 0)
            {
                this.child_show(true);
                this.mRedCtrl.Value = value;
            }
            else
            {
                this.child_show(false);
            }
        }
    }
}

