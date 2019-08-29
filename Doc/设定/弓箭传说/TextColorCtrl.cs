using Dxx.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TextColorCtrl : MonoBehaviour
{
    private string mText;
    private int mFontSize = -1;
    public Color topColor1 = new Color(0.9490196f, 0.9882353f, 0.8901961f, 1f);
    public Color topColor2 = new Color(0.6392157f, 0.8823529f, 0.4039216f, 1f);
    public Color topoutlineColor = new Color(0f, 0f, 0f, 1f);
    public Color middleColor = new Color(0.3372549f, 0.4509804f, 0.2313726f, 1f);
    public Color middleoutlineColor = new Color(0f, 0f, 0f, 1f);
    public Color shadowColor = new Color(0.4352941f, 0.4352941f, 0.4352941f, 1f);
    private Text _text_top;
    private Text _text_middle;
    private Text _text_shadow;

    private void Awake()
    {
        TextColorDxx component = this.text_top.GetComponent<TextColorDxx>();
        component.topColor = this.topColor1;
        component.bottomColor = this.topColor2;
        this.text_top.GetComponent<Outline>().set_effectColor(this.topoutlineColor);
        this.text_middle.GetComponent<Outline>().set_effectColor(this.middleoutlineColor);
        this.text_shadow.set_color(this.shadowColor);
    }

    public string text
    {
        get => 
            this.mText;
        set
        {
            this.mText = value;
            this.text_top.text = value;
            this.text_middle.text = value;
            this.text_shadow.text = value;
        }
    }

    public int FontSize
    {
        get
        {
            if (this.mFontSize == -1)
            {
                this.mFontSize = this.text_top.fontSize;
            }
            return this.mFontSize;
        }
        set
        {
            this.mFontSize = value;
            this.text_top.fontSize = value;
            this.text_middle.fontSize = value;
            this.text_shadow.fontSize = value;
            this._text_middle.transform.localPosition = new Vector3(0f, ((float) -value) / 10f, 0f);
            this._text_shadow.transform.localPosition = new Vector3(0f, ((float) -value) / 5f, 0f);
        }
    }

    private Text text_top
    {
        get
        {
            if (this._text_top == null)
            {
                this._text_top = base.transform.Find("Text_Top").GetComponent<Text>();
            }
            return this._text_top;
        }
    }

    private Text text_middle
    {
        get
        {
            if (this._text_middle == null)
            {
                this._text_middle = base.transform.Find("Text_Middle").GetComponent<Text>();
            }
            return this._text_middle;
        }
    }

    private Text text_shadow
    {
        get
        {
            if (this._text_shadow == null)
            {
                this._text_shadow = base.transform.Find("Text_Shadow").GetComponent<Text>();
            }
            return this._text_shadow;
        }
    }
}

