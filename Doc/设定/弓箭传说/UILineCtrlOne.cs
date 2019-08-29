using System;
using UnityEngine;
using UnityEngine.UI;

public class UILineCtrlOne : MonoBehaviour
{
    public RectTransform mParent;
    public Text text;
    public RectTransform Image_Left;
    public RectTransform Image_Right;
    private RectTransform rectTransform;
    private string mValue;
    private float interval = 15f;
    private float outinterval;

    private void Awake()
    {
        this.rectTransform = base.transform as RectTransform;
    }

    public void RefreshUI()
    {
        float x = this.mParent.sizeDelta.x;
        float preferredWidth = this.text.preferredWidth;
        float num3 = (((x - preferredWidth) - (this.interval * 2f)) - (this.outinterval * 2f)) / 2f;
        this.Image_Left.sizeDelta = new Vector2(num3, this.Image_Left.sizeDelta.y);
        this.Image_Right.sizeDelta = this.Image_Left.sizeDelta;
        this.Image_Left.anchoredPosition = new Vector2(((-x / 2f) + (num3 / 2f)) + this.outinterval, 0f);
        this.Image_Right.anchoredPosition = new Vector2((this.interval + (num3 / 2f)) + (preferredWidth / 2f), 0f);
    }

    public void SetColor(Color color)
    {
        this.text.set_color(color);
    }

    public void SetFontSize(int size)
    {
        this.text.fontSize = size;
        this.RefreshUI();
    }

    public void SetInterval(float interval)
    {
        this.interval = interval;
        this.SetText(this.mValue);
    }

    public void SetOutInterval(float outinterval)
    {
        this.outinterval = outinterval;
        this.SetText(this.mValue);
    }

    public void SetText(string value)
    {
        this.mValue = value;
        this.text.text = value;
        this.RefreshUI();
    }

    public void SetY(float y)
    {
        this.rectTransform.anchoredPosition = new Vector2(0f, y);
    }

    private void Start()
    {
        this.RefreshUI();
    }
}

