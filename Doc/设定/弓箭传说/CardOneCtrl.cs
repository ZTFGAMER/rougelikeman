using DG.Tweening;
using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class CardOneCtrl : MonoBehaviour
{
    public ButtonCtrl button;
    public Text buttonText;
    public Image Image_Icon;
    public Image Image_Quality;
    public Text Text_LevelContent;
    public Text Text_Level;
    public Text Text_Name;
    public Image Image_Unknow;
    public CanvasGroup mCanvas;
    private Skill_slotout mData;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private LocalSave.CardOne <carddata>k__BackingField;
    public Action<CardOneCtrl> Event_Click;
    private static Dictionary<int, Color> mLevelContentColors;
    private static Dictionary<int, Color> mLevelColors;

    static CardOneCtrl()
    {
        Dictionary<int, Color> dictionary = new Dictionary<int, Color> {
            { 
                1,
                new Color(0.4196078f, 0.4196078f, 0.4196078f)
            },
            { 
                2,
                new Color(0.2039216f, 0.3647059f, 0.5137255f)
            },
            { 
                3,
                new Color(0.4235294f, 0.3176471f, 0.5176471f)
            }
        };
        mLevelContentColors = dictionary;
        dictionary = new Dictionary<int, Color> {
            { 
                1,
                new Color(0.7333333f, 0.7333333f, 0.7333333f)
            },
            { 
                2,
                new Color(0.2980392f, 0.6627451f, 0.9411765f)
            },
            { 
                3,
                new Color(0.7843137f, 0.4235294f, 1f)
            }
        };
        mLevelColors = dictionary;
    }

    private void Awake()
    {
        this.button.onClick = new Action(this.OnClick);
    }

    public void InitCard(LocalSave.CardOne carddata)
    {
        this.carddata = carddata;
        this.UpdateUI();
    }

    public void OnClick()
    {
        if (this.Event_Click != null)
        {
            this.Event_Click(this);
        }
    }

    public void OnLanguageChange()
    {
        this.Text_LevelContent.text = GameLogic.Hold.Language.GetLanguageByTID("CardUI_Level", Array.Empty<object>());
    }

    public Tweener PlayCanvas(float startalpha, float endalpha, float time)
    {
        this.SetAlpha(startalpha);
        return ShortcutExtensions46.DOFade(this.mCanvas, endalpha, time);
    }

    public void SetAlpha(float alpha)
    {
        this.mCanvas.alpha = alpha;
    }

    public void SetButtonEnable(bool value)
    {
        if (this.button != null)
        {
            this.button.enabled = value;
        }
        if (this.buttonText != null)
        {
            this.buttonText.enabled = value;
        }
    }

    public void SetLock(bool value)
    {
    }

    public void SetNameShow(bool value)
    {
        this.Text_Name.enabled = value;
    }

    public void SetTextShow(bool value)
    {
        if (this.Text_Level != null)
        {
            this.Text_Level.gameObject.SetActive(value);
        }
    }

    public void UpdateUI()
    {
        this.SetNameShow(true);
        this.SetAlpha(1f);
        this.mData = LocalModelManager.Instance.Skill_slotout.GetBeanById(this.carddata.CardID);
        Sprite card = SpriteManager.GetCard(this.carddata.data.GroupID);
        if (card != null)
        {
            this.Image_Icon.set_sprite(card);
        }
        else
        {
            this.Image_Icon.set_sprite(SpriteManager.GetCard(0x65));
        }
        object[] args = new object[] { this.mData.Quality };
        this.Image_Quality.set_sprite(SpriteManager.GetCard(Utils.FormatString("CardUI_Quality{0}", args)));
        object[] objArray2 = new object[] { this.mData.Quality };
        this.Image_Unknow.set_sprite(SpriteManager.GetCard(Utils.FormatString("CardUI_Unknow{0}", objArray2)));
        if (this.carddata.Unlock)
        {
            this.Text_LevelContent.enabled = true;
            this.Image_Unknow.enabled = false;
            this.Image_Icon.enabled = true;
        }
        else
        {
            this.Text_LevelContent.enabled = false;
            this.Image_Unknow.enabled = true;
            this.Image_Icon.enabled = false;
        }
        this.SetTextShow(true);
        if (!this.carddata.IsMaxLevel)
        {
            if (this.carddata.level > 0)
            {
                this.Text_Level.text = this.carddata.level.ToString();
            }
            else
            {
                this.Text_Level.text = string.Empty;
            }
        }
        else
        {
            this.Text_Level.text = "Max";
        }
        if (!this.carddata.Unlock)
        {
            this.Text_Name.text = GameLogic.Hold.Language.GetLanguageByTID("CardUI_Lock", Array.Empty<object>());
        }
        else
        {
            object[] objArray3 = new object[] { this.carddata.CardID };
            this.Text_Name.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("宝物名称{0}", objArray3), Array.Empty<object>());
        }
        this.Text_LevelContent.set_color(mLevelContentColors[this.carddata.data.Quality]);
        this.Text_Level.set_color(mLevelColors[this.carddata.data.Quality]);
    }

    public LocalSave.CardOne carddata { get; private set; }
}

