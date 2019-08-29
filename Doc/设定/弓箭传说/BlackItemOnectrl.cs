using DG.Tweening;
using Dxx.Util;
using System;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class BlackItemOnectrl : MonoBehaviour
{
    public ButtonCtrl Button_Buy;
    public GameObject equipparent;
    public Image Image_Buy;
    public GoldTextCtrl mGoldCtrl;
    public Text Text_Name;
    public Text Text_Sold;
    public GameObject buyparent;
    public GameObject notbuyparent;
    public Action<BlackItemOnectrl> OnClickButton;
    public Shop_MysticShop mData;
    private PropOneEquip mItem;
    private Equip_equip equipdata;
    private LocalSave.EquipOne mEquipOne = new LocalSave.EquipOne();
    public int mIndex;
    private bool bBuy;

    private void Awake()
    {
        this.Button_Buy.onClick = delegate {
            if (this.bBuy)
            {
                CInstance<TipsUIManager>.Instance.Show(ETips.Tips_AlreadyBuy, Array.Empty<string>());
            }
            else if (this.OnClickButton != null)
            {
                this.OnClickButton(this);
            }
        };
    }

    public void Buy()
    {
        this.bBuy = true;
        this.SetBuy(this.bBuy);
    }

    public void Init(int index, Shop_MysticShop data)
    {
        if (this.mItem == null)
        {
            this.mItem = CInstance<UIResourceCreator>.Instance.GetPropOneEquip(this.equipparent.transform);
            this.mItem.SetButtonEnable(false);
        }
        Drop_DropModel.DropData data2 = new Drop_DropModel.DropData {
            type = PropType.eEquip,
            id = data.ProductId,
            count = data.ProductNum
        };
        this.mEquipOne.Clear();
        this.mEquipOne.EquipID = data.ProductId;
        this.mItem.InitProp(data2);
        this.mIndex = index;
        this.bBuy = false;
        this.mData = data;
        this.equipdata = LocalModelManager.Instance.Equip_equip.GetBeanById(this.mData.ProductId);
        if (this.equipdata == null)
        {
            object[] args = new object[] { data.ID };
            SdkManager.Bugly_Report("BlackItemOneCtrl", Utils.FormatString("Init Equip_equip:{0} is null", args));
        }
        else
        {
            this.SetBuy(this.bBuy);
            this.mGoldCtrl.UseTextRed();
            this.mGoldCtrl.SetCurrencyType(data.PriceType);
            this.mGoldCtrl.SetValue(data.Price);
            this.Text_Sold.text = GameLogic.Hold.Language.GetLanguageByTID("blackshop_sold", Array.Empty<object>());
            this.Image_Buy.transform.localScale = Vector3.one;
            this.Text_Name.text = this.mEquipOne.NameOnlyString;
            this.Text_Name.set_color(this.mEquipOne.qualityColor);
        }
    }

    private void SetBuy(bool buy)
    {
        if (this.buyparent != null)
        {
            this.buyparent.SetActive(buy);
        }
        if (this.notbuyparent != null)
        {
            this.notbuyparent.SetActive(!buy);
        }
        if (buy && (this.Image_Buy != null))
        {
            this.Image_Buy.transform.localScale = Vector3.zero;
            TweenSettingsExtensions.SetUpdate<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.Image_Buy.transform, 1f, 0.5f), 0x1b), true);
        }
    }

    public void SetCurrencyShow(bool value)
    {
        this.mGoldCtrl.gameObject.SetActive(value);
    }

    public void UpdateCurrency()
    {
        this.mGoldCtrl.SetValue(this.mData.Price);
    }
}

