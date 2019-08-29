using Dxx.UI;
using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GoldTextCtrl : MonoBehaviour
{
    private bool _iconfront = true;
    private float _interval;
    private Text text;
    private Color textColor;
    private Color redColor = new Color(1f, 0.3019608f, 0.3019608f);
    private RectTransform imageRect;
    private Image image;
    private TextColor3Dxx text3;
    private Color topColor;
    private Color bottomColor;
    private float allwidth;
    private int gold;
    private CurrencyType type;
    private bool useTextRed;
    private bool bInit;

    private void Awake()
    {
        this.init();
    }

    public float GetWidth() => 
        this.allwidth;

    private void init()
    {
        if (!this.bInit)
        {
            this.bInit = true;
            this.text = base.transform.Find("Text").GetComponent<Text>();
            this.textColor = this.text.get_color();
            this.image = base.transform.Find("Image").GetComponent<Image>();
            this.imageRect = this.image.transform as RectTransform;
            this.text3 = this.text.GetComponent<TextColor3Dxx>();
            if (this.text3 != null)
            {
                this.topColor = this.text3.topColor;
                this.bottomColor = this.text3.bottomColor;
            }
        }
    }

    public void SetAdd(int value)
    {
        this.SetValueInternal(value.ToString(), "+");
    }

    public void SetButtonEnable(bool value)
    {
        if (value)
        {
            this.SetTextRed(LocalSave.Instance.GetGold() < this.gold);
        }
        else
        {
            this.SetTextRed(false);
        }
    }

    public void SetCurrencyType(CurrencyType type)
    {
        this.init();
        this.type = type;
        string name = string.Empty;
        switch (type)
        {
            case CurrencyType.Gold:
                name = "Currency_Gold";
                break;

            case CurrencyType.Diamond:
                name = "Currency_Diamond";
                break;

            case CurrencyType.Key:
                name = "Currency_Key";
                break;

            default:
            {
                object[] args = new object[] { type };
                SdkManager.Bugly_Report("GoldTextCtrl", Utils.FormatString("CurrencyType[{0}] is dont achieve!!!", args));
                break;
            }
        }
        this.image.set_sprite(SpriteManager.GetUICommon(name));
    }

    public void SetCurrencyType(int type)
    {
        this.SetCurrencyType((CurrencyType) type);
    }

    public void SetReduce(int value)
    {
        this.SetValueInternal(value.ToString(), "-");
    }

    public void SetTextRed(bool red)
    {
        if (this.text3 == null)
        {
            Color color = !red ? this.textColor : this.redColor;
            this.text.set_color(color);
        }
        else if (red)
        {
            this.text3.topColor = this.redColor;
            this.text3.bottomColor = this.redColor;
            this.text.set_color(this.redColor);
        }
        else
        {
            this.text3.topColor = this.topColor;
            this.text3.bottomColor = this.bottomColor;
            this.text.set_color(Color.white);
        }
    }

    public void SetValue(int value)
    {
        this.gold = value;
        this.SetValueInternal(value.ToString(), "x");
        this.UpdateTextRed();
    }

    public void SetValue(float value)
    {
        this.SetValueInternal(value.ToString(), string.Empty);
        this.UpdateTextRed();
    }

    public void SetValue(string value)
    {
        this.SetValueInternal(value.ToString(), string.Empty);
        this.UpdateTextRed();
    }

    private void SetValueInternal(string value, string before)
    {
        if (this.text == null)
        {
            this.init();
        }
        object[] args = new object[] { before, value };
        this.text.text = Utils.FormatString("{0}{1}", args);
        this.allwidth = (this.imageRect.sizeDelta.x + this.text.preferredWidth) + this.Interval;
        float x = 0f;
        float num2 = 0f;
        if (this.bIconFront)
        {
            x = -this.allwidth / 2f;
            num2 = (x + this.imageRect.sizeDelta.x) + this.Interval;
        }
        else
        {
            num2 = -this.allwidth / 2f;
            x = (num2 + this.text.preferredWidth) + this.Interval;
        }
        this.imageRect.anchoredPosition = new Vector2(x, this.imageRect.anchoredPosition.y);
        this.text.get_rectTransform().anchoredPosition = new Vector2(num2, this.text.get_rectTransform().anchoredPosition.y);
        RectTransform transform = base.transform as RectTransform;
        transform.sizeDelta = new Vector2(this.allwidth, this.imageRect.sizeDelta.y);
    }

    private void UpdateTextRed()
    {
        this.init();
        if (this.useTextRed)
        {
            if (this.type == CurrencyType.Gold)
            {
                this.SetTextRed(LocalSave.Instance.GetGold() < this.gold);
            }
            else if (this.type == CurrencyType.Diamond)
            {
                this.SetTextRed(false);
            }
        }
    }

    public void UseTextRed()
    {
        this.useTextRed = true;
        this.UpdateTextRed();
    }

    public bool bIconFront
    {
        get => 
            this._iconfront;
        set => 
            (this._iconfront = value);
    }

    public float Interval
    {
        get => 
            this._interval;
        set => 
            (this._interval = value);
    }
}

