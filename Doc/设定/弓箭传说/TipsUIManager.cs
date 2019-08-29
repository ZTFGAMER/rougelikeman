using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TipsUIManager : CInstance<TipsUIManager>
{
    private Dictionary<string, TipsUICtrl> mList = new Dictionary<string, TipsUICtrl>();

    public void Cache(GameObject o)
    {
        o.gameObject.SetActive(false);
    }

    public void Clear()
    {
    }

    public void Show(string value)
    {
        this.ShowInternal(value, Color.white);
    }

    public void Show(ETips type, params string[] args)
    {
        this.Show(type, Color.white, args);
    }

    public void Show(string value, float y)
    {
        GameObject obj2 = GameLogic.EffectGet("Game/UI/TipsUIOne");
        obj2.transform.SetParent(GameNode.m_TipsUI);
        RectTransform transform = obj2.transform as RectTransform;
        transform.localScale = Vector3.one;
        transform.anchoredPosition = new Vector2(0f, 0f);
        TipsUICtrl component = obj2.GetComponent<TipsUICtrl>();
        component.InitNotAni(value);
        component.transform.position = new Vector3(((float) GameLogic.Width) / 2f, y, 0f);
    }

    public void Show(ETips type, Color color, params string[] args)
    {
        string languageByTID = GameLogic.Hold.Language.GetLanguageByTID(type.ToString(), args);
        this.ShowInternal(languageByTID, color);
    }

    public void ShowCode(ushort errorcode, int type = 0)
    {
        if (errorcode == 8)
        {
            if (type == 1)
            {
                this.Show(ETips.Tips_GoldNotEnough, Array.Empty<string>());
            }
            else if (type == 2)
            {
                this.Show(ETips.Tips_DiamondNotEnough, Array.Empty<string>());
            }
            else if (type == 3)
            {
                this.Show(ETips.Tips_KeyNotEnough, Array.Empty<string>());
            }
        }
        else if (errorcode == 10)
        {
            this.Show(ETips.Tips_EquipMaterialNotEnough, Array.Empty<string>());
        }
        else if (errorcode == 1)
        {
            if (type == 1)
            {
                this.Show(ETips.Tips_MailAlreadyGot, Array.Empty<string>());
            }
            else
            {
                this.Show(ETips.Tips_NetError, Array.Empty<string>());
            }
        }
        else
        {
            ETips tips = (ETips) ((short) errorcode);
            if (!int.TryParse(tips.ToString(), out _))
            {
                this.Show(tips, Array.Empty<string>());
            }
            else
            {
                this.Show(ETips.Tips_NetError, Array.Empty<string>());
            }
        }
    }

    private TipsUICtrl ShowInternal(string value, Color color)
    {
        if (this.mList.TryGetValue(value, out TipsUICtrl component))
        {
            component.gameObject.SetActive(true);
            component.Init();
            return component;
        }
        GameObject obj2 = GameLogic.EffectGet("Game/UI/TipsUIOne");
        obj2.transform.SetParent(GameNode.m_TipsUI);
        RectTransform transform = obj2.transform as RectTransform;
        transform.localScale = Vector3.one;
        transform.anchoredPosition = new Vector2(0f, 0f);
        component = obj2.GetComponent<TipsUICtrl>();
        this.mList.Add(value, component);
        component.Init(value, color);
        return component;
    }

    public void ShowPotion(int id, int quality)
    {
        object[] args = new object[] { id };
        string languageByTID = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("药水效果描述{0}", args), Array.Empty<object>());
        this.ShowInternal(languageByTID, LocalSave.QualityColors[quality]);
    }
}

