using Dxx.Util;
using System;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemGold : MonoBehaviour
{
    public Text Text_Title;
    public ButtonCtrl Button_Get;
    public Image Image_Icon;
    public Text Text_Count;
    public GoldTextCtrl mGoldCtrl;
    public Action<int, ShopItemGold> OnClickButton;
    private Shop_Shop shopdata;
    private int mIndex;

    private void Awake()
    {
        this.Button_Get.SetDepondNet(true);
        this.Button_Get.onClick = delegate {
            if (this.OnClickButton != null)
            {
                this.OnClickButton(this.mIndex, this);
            }
        };
    }

    public int GetDiamond() => 
        ((int) this.shopdata.Price);

    public int GetGold() => 
        LocalSave.Instance.mShop.get_buy_golds(this.mIndex);

    public void Init(int index)
    {
        this.mIndex = index;
        this.shopdata = LocalModelManager.Instance.Shop_Shop.GetBeanById(0x65 + index);
        object[] args = new object[] { index + 1 };
        this.Image_Icon.set_sprite(SpriteManager.GetMain(Utils.FormatString("ic_coin_{0}", args)));
        if (LocalSave.Instance.Card_GetHarvestLevel() == 0)
        {
            object[] objArray2 = new object[] { this.shopdata.ID };
            this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("商店_金币{0}", objArray2), Array.Empty<object>());
        }
        else
        {
            object[] objArray3 = new object[] { LocalSave.Instance.mShop.get_gold_time(index) };
            this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("shopui_equipexp_reward", objArray3);
        }
        this.Text_Count.text = this.GetGold().ToString();
        this.mGoldCtrl.SetValue(this.shopdata.Price);
    }

    public void OnLanguageChange()
    {
        this.Init(this.mIndex);
    }

    public void UpdateNet()
    {
    }
}

