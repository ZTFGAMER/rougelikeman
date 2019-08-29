using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LayerRewardOneCtrl : MonoBehaviour
{
    public Image Image_Icon;
    public Text Text_Count;
    public Text Text_Content;
    public int id;

    public void Init(int id, int count)
    {
        this.id = id;
        this.Text_Content.text = string.Empty;
        this.Image_Icon.enabled = false;
        object[] args = new object[] { count };
        this.Text_Count.text = Utils.FormatString("x{0}", args);
        CurrencyType type = (CurrencyType) id;
        if (type == CurrencyType.Reborn)
        {
            this.Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("currency_reborn", Array.Empty<object>());
        }
        else
        {
            this.Image_Icon.enabled = true;
            this.Image_Icon.set_sprite(SpriteManager.GetUICommonCurrency(type));
        }
    }
}

