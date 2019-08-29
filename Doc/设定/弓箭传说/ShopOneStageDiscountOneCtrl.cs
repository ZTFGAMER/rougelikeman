using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopOneStageDiscountOneCtrl : MonoBehaviour
{
    public Text Text_Count;
    public Image Image_Icon;
    public Text Text_Content;
    private int type;
    private int count;

    public void Init(int type, int count)
    {
        this.type = type;
        this.count = count;
        this.Image_Icon.enabled = false;
        this.Text_Content.text = string.Empty;
        object[] args = new object[] { count };
        this.Text_Count.text = Utils.FormatString("x{0}", args);
        CurrencyType type2 = (CurrencyType) type;
        switch (type2)
        {
            case CurrencyType.Equip_Random_Quality_1:
            case CurrencyType.Equip_Random_Quality_2:
            case CurrencyType.Equip_Random_Quality_3:
            case CurrencyType.Equip_Random_Quality_4:
            case CurrencyType.Equip_Random_Quality_5:
            case CurrencyType.Equip_Random_Quality_6:
            case CurrencyType.Equip_Random_Quality_7:
            case CurrencyType.Equip_Random_Quality_8:
            case CurrencyType.Gold:
            case CurrencyType.Diamond:
            case CurrencyType.Key:
            case CurrencyType.DiamondBoxNormal:
            case CurrencyType.DiamondBoxLarge:
            case CurrencyType.EquipExp_Random:
            case CurrencyType.Free_Ad:
                this.Image_Icon.enabled = true;
                this.Image_Icon.set_sprite(SpriteManager.GetUICommonCurrency(type2));
                break;

            case CurrencyType.Reborn:
                this.Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("currency_reborn", Array.Empty<object>());
                break;
        }
    }

    public void OnLanguageUpdate()
    {
        this.Init(this.type, this.count);
    }
}

