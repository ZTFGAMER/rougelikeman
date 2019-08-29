using System;
using UnityEngine;

public class UILineCtrl : MonoBehaviour
{
    public UILineCtrlOne _one;
    private RectTransform rectTransform;

    private void Awake()
    {
        this.rectTransform = base.transform as RectTransform;
        if (this._one != null)
        {
            RectTransform transform = this._one.transform as RectTransform;
            transform.sizeDelta = (base.transform as RectTransform).sizeDelta;
        }
    }

    public void SetColor(Color color)
    {
        this.mOne.SetColor(color);
    }

    public void SetFontSize(int size)
    {
        this.mOne.SetFontSize(size);
    }

    public void SetInterval(float interval)
    {
        this.mOne.SetInterval(interval);
    }

    public void SetOutInterval(float outinterval)
    {
        this.mOne.SetOutInterval(outinterval);
    }

    public void SetText(string value)
    {
        this.mOne.SetText(value);
    }

    public void SetY(float y)
    {
        this.rectTransform.anchoredPosition = new Vector2(0f, y);
    }

    private UILineCtrlOne mOne
    {
        get
        {
            if (this._one == null)
            {
                this._one = CInstance<UIResourceCreator>.Instance.GetUILineOne(base.transform);
                RectTransform transform = this._one.transform as RectTransform;
                transform.sizeDelta = (base.transform as RectTransform).sizeDelta;
            }
            return this._one;
        }
    }
}

