using Dxx.Util;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class FirstItemOnectrl : MonoBehaviour
{
    public ButtonGoldCtrl Button_Buy;
    public GameObject alreadybuy;
    public Image Image_Icon;
    public Text Text_Content;
    public Text Text_Value;
    public Action<FirstItemOnectrl> OnClickButton;
    public Shop_ReadyShop mData;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private int <mIndex>k__BackingField;
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
        this.Text_Value.gameObject.SetActive(false);
    }

    public void Buy()
    {
        this.bBuy = true;
        this.SetBuy(this.bBuy);
        GameLogic.Hold.BattleData.SetFirstShopBuy(this.mIndex);
        Shop_item beanById = LocalModelManager.Instance.Shop_item.GetBeanById(this.mData.ProductId);
        switch (beanById.EffectType)
        {
            case 1:
                this.GetOneItem(beanById);
                break;

            case 2:
            {
                WeightRandom random = new WeightRandom();
                int index = 0;
                int length = beanById.EffectArgs.Length;
                while (index < length)
                {
                    char[] separator = new char[] { ',' };
                    string[] strArray = beanById.EffectArgs[index].Split(separator);
                    int result = 0;
                    int.TryParse(strArray[0], out result);
                    int num5 = 0;
                    int.TryParse(strArray[1], out num5);
                    random.Add(num5, result);
                    index++;
                }
                int key = random.GetRandom();
                if (key > 0)
                {
                    beanById = LocalModelManager.Instance.Shop_item.GetBeanById(key);
                    this.GetOneItem(beanById);
                }
                break;
            }
        }
    }

    private void GetOneItem(Shop_item item)
    {
        GetOnePotion(item);
        LocalSave.Instance.BattleIn_UpdatePotions(item.ItemID);
        CInstance<TipsUIManager>.Instance.ShowPotion(item.ItemID, item.Quality);
    }

    public static void GetOnePotion(Shop_item item)
    {
        int length = item.EffectArgs.Length;
        if (length > 0)
        {
            for (int i = 0; i < length; i++)
            {
                GameLogic.Self.m_EntityData.ExcuteAttributes(item.EffectArgs[i]);
            }
        }
    }

    public void Init(int index, Shop_ReadyShop data, bool buy)
    {
        this.mIndex = index;
        this.bBuy = buy;
        this.mData = data;
        this.SetBuy(this.bBuy);
        this.Text_Value.text = this.mData.ProductId.ToString();
        object[] args = new object[] { this.mData.ProductId };
        this.Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("药水效果描述{0}", args), Array.Empty<object>());
        object[] objArray2 = new object[] { this.mData.ProductId };
        this.Image_Icon.set_sprite(SpriteManager.GetBattle(Utils.FormatString("ShopStart_{0}", objArray2)));
    }

    public void SetBuy(bool buy)
    {
        if (buy)
        {
            this.alreadybuy.SetActive(true);
            this.Button_Buy.gameObject.SetActive(false);
        }
        else
        {
            this.Button_Buy.gameObject.SetActive(true);
            this.Button_Buy.SetCurrency(this.mData.PriceType);
            this.Button_Buy.SetGold(this.mData.Price);
            this.alreadybuy.SetActive(false);
        }
    }

    public int mIndex { get; private set; }
}

