using Dxx.Util;
using System;
using UnityEngine;

public class ProgressCtrl : MonoBehaviour
{
    public ProgressDirection direction;
    private RectTransform fill;
    private RectTransform tran;
    private float width;
    private float height;
    private float _Value;

    private void Awake()
    {
        this.InitFill();
        this.tran = base.transform as RectTransform;
        this.OnAwake();
    }

    private void InitFill()
    {
        if (this.fill == null)
        {
            this.fill = base.transform.Find("Slider/Fill") as RectTransform;
            if (this.fill != null)
            {
                this.width = this.fill.sizeDelta.x;
                this.height = this.fill.sizeDelta.y;
            }
        }
    }

    protected virtual void OnAwake()
    {
    }

    private void RefreshSize()
    {
        if (((this.width == 0f) && (this.fill != null)) && (this.fill.parent != null))
        {
            RectTransform parent = this.fill.parent as RectTransform;
            this.width = parent.sizeDelta.x;
            this.height = parent.sizeDelta.y;
        }
    }

    protected void UpdateFill()
    {
        this.InitFill();
        this.RefreshSize();
        if (this.fill != null)
        {
            this.fill.sizeDelta = new Vector2(this.Value * this.width, this.height);
        }
    }

    public float Value
    {
        get => 
            this._Value;
        set
        {
            value = MathDxx.Clamp01(value);
            this._Value = value;
            this.UpdateFill();
        }
    }

    public enum ProgressDirection
    {
        LeftToRight
    }
}

