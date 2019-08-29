using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemEquipExp : MonoBehaviour
{
    public static Dictionary<int, int> mRewards;
    public Text Text_Title;
    public ButtonCtrl Button_Get;
    public Transform itemparent;
    public GoldTextCtrl mGoldCtrl;
    public Action<int, ShopItemEquipExp> OnClickButton;
    private EquipOneCtrl mEquipItem;
    private Shop_Shop shopdata;
    private int mIndex;

    static ShopItemEquipExp()
    {
        Dictionary<int, int> dictionary = new Dictionary<int, int> {
            { 
                0,
                4
            },
            { 
                1,
                8
            },
            { 
                2,
                0x10
            }
        };
        mRewards = dictionary;
    }

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
        this.shopdata.ProductNum;

    public void Init(int index)
    {
        if (this.mEquipItem == null)
        {
            this.mEquipItem = CInstance<UIResourceCreator>.Instance.GetEquip(this.itemparent);
        }
        int num = LocalModelManager.Instance.Equip_equip.RandomEquipExp();
        int num2 = GameLogic.Random(5, 15);
        LocalSave.EquipOne equip = new LocalSave.EquipOne {
            EquipID = num,
            Count = num2
        };
        this.mEquipItem.Init(equip);
        this.mIndex = index;
        this.shopdata = LocalModelManager.Instance.Shop_Shop.GetBeanById(0x65 + index);
        object[] args = new object[] { mRewards[this.mIndex] };
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("shopui_equipexp_reward", args);
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

