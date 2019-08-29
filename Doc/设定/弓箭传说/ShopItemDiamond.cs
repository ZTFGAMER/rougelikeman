using DG.Tweening;
using Dxx.Util;
using GameProtocol;
using System;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemDiamond : MonoBehaviour
{
    public Text Text_Title;
    public ButtonCtrl Button_Get;
    public Image Image_Icon;
    public Text Text_Count;
    public Text Text_Money;
    private Shop_Shop shopdata;
    private int mIndex;

    private void Awake()
    {
        if (this.Button_Get != null)
        {
            this.Button_Get.SetDepondNet(true);
            this.Button_Get.onClick = () => this.OnClickButtonInternal(PurchaseManager.Instance.GetProductID(this.mIndex));
        }
    }

    public int GetDiamond() => 
        this.shopdata.ProductNum;

    public void Init(int index)
    {
        this.mIndex = index;
        this.shopdata = LocalModelManager.Instance.Shop_Shop.GetBeanById(0xc9 + index);
        object[] args = new object[] { index + 1 };
        this.Image_Icon.set_sprite(SpriteManager.GetMain(Utils.FormatString("ic_gem_{0}", args)));
        object[] objArray2 = new object[] { this.shopdata.ID };
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("商店_钻石{0}", objArray2), Array.Empty<object>());
        this.Text_Count.text = this.shopdata.ProductNum.ToString();
        this.Text_Money.text = PurchaseManager.Instance.GetProduct_localpricestring(index);
    }

    private void OnClickButtonInternal(string productID)
    {
        if (PurchaseManager.Instance != null)
        {
            SdkManager.send_event_iap("CLICK", PurchaseManager.Instance.GetOpenSource(), productID, string.Empty, string.Empty);
            PurchaseManager.Instance.OnPurchaseClicked(productID, (success, data) => PurchaseFly(data.product_id, (this == null) ? null : base.transform));
        }
    }

    public void OnLanguageChange()
    {
        this.Init(this.mIndex);
    }

    public static void PurchaseFly(string id, Transform t)
    {
        <PurchaseFly>c__AnonStorey0 storey = new <PurchaseFly>c__AnonStorey0 {
            id = id,
            t = t
        };
        TweenSettingsExtensions.SetUpdate<Sequence>(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 1f), new TweenCallback(storey, this.<>m__0)), true);
    }

    private static void PurchaseFlyInternal(string id, Transform t)
    {
        long gold = 0L;
        long diamond = 0L;
        if (id != null)
        {
            if (id != "com.habby.archero_d1")
            {
                if (id == "com.habby.archero_d2")
                {
                    diamond = LocalModelManager.Instance.Shop_Shop.GetBeanById(0xca).ProductNum;
                }
                else if (id == "com.habby.archero_d3")
                {
                    diamond = LocalModelManager.Instance.Shop_Shop.GetBeanById(0xcb).ProductNum;
                }
                else if (id == "com.habby.archero_d4")
                {
                    diamond = LocalModelManager.Instance.Shop_Shop.GetBeanById(0xcc).ProductNum;
                }
                else if (id == "com.habby.archero_d5")
                {
                    diamond = LocalModelManager.Instance.Shop_Shop.GetBeanById(0xcd).ProductNum;
                }
                else if (id == "com.habby.archero_d6")
                {
                    diamond = LocalModelManager.Instance.Shop_Shop.GetBeanById(0xce).ProductNum;
                }
            }
            else
            {
                diamond = LocalModelManager.Instance.Shop_Shop.GetBeanById(0xc9).ProductNum;
            }
        }
        if (diamond > 0L)
        {
            if (t != null)
            {
                LocalSave.Instance.Modify_Diamond(diamond, false);
                CurrencyFlyCtrl.PlayGet(CurrencyType.Diamond, diamond, t.position, null, null, true);
            }
            else
            {
                LocalSave.Instance.Modify_Diamond(diamond, true);
            }
        }
        if (gold > 0L)
        {
            if (t != null)
            {
                LocalSave.Instance.Modify_Gold(gold, false);
                CurrencyFlyCtrl.PlayGet(CurrencyType.Gold, gold, t.position, null, null, true);
            }
            else
            {
                LocalSave.Instance.Modify_Gold(gold, true);
            }
        }
    }

    public void UpdateNet()
    {
    }

    [CompilerGenerated]
    private sealed class <PurchaseFly>c__AnonStorey0
    {
        internal string id;
        internal Transform t;

        internal void <>m__0()
        {
            ShopItemDiamond.PurchaseFlyInternal(this.id, this.t);
        }
    }
}

