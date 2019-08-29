using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class EquipBuyInfoUICtrl : MediatorCtrlBase
{
    private const string AniMoveName = "CharEquipInfoMove";
    public ButtonCtrl Button_Close;
    public ButtonCtrl Button_Buy;
    public Transform equipparent;
    public Text Text_Info;
    public GoldTextCtrl mGoldTextCtrl;
    public ButtonCtrl Button_Mask;
    public Text Text_SkillName;
    public Text Text_Buy;
    public GameObject attributeParent;
    public Text Text_Attribute;
    private BlackItemOnectrl _itemone;
    private EquipBuyInfoProxy.Transfer mTransfer;
    private LocalUnityObjctPool mPool;
    private LocalSave.EquipOne mEquipData;
    private List<Text> mTexts = new List<Text>();
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    private void InitAttribute()
    {
        this.mPool.Collect<Text>();
        this.mTexts.Clear();
        float y = 0f;
        List<Goods_goods.GoodShowData> equipShowAttrs = LocalModelManager.Instance.Equip_equip.GetEquipShowAttrs(this.mEquipData);
        List<string> equipAttributesNext = LocalModelManager.Instance.Equip_equip.GetEquipAttributesNext(this.mEquipData);
        int num2 = 0;
        int count = equipShowAttrs.Count;
        while (num2 < count)
        {
            string str = equipShowAttrs[num2].ToString();
            Text item = this.mPool.DeQueue<Text>();
            this.mTexts.Add(item);
            item.text = str;
            RectTransform child = item.transform as RectTransform;
            child.SetParentNormal(this.attributeParent);
            child.anchoredPosition = new Vector2(0f, y);
            y -= item.preferredHeight;
            num2++;
        }
        List<string> equipShowAddAttributes = LocalModelManager.Instance.Equip_equip.GetEquipShowAddAttributes(this.mEquipData);
        int num4 = 0;
        int num5 = equipShowAddAttributes.Count;
        while (num4 < num5)
        {
            Text item = this.mPool.DeQueue<Text>();
            this.mTexts.Add(item);
            item.text = equipShowAddAttributes[num4];
            RectTransform transform = item.transform as RectTransform;
            transform.SetParentNormal(this.attributeParent);
            transform.anchoredPosition = new Vector2(0f, y);
            y -= item.preferredHeight;
            num4++;
        }
    }

    private void InitButton()
    {
        this.mGoldTextCtrl.SetCurrencyType(this.mTransfer.itemone.mData.PriceType);
        this.mGoldTextCtrl.UseTextRed();
        this.mGoldTextCtrl.SetValue(this.mTransfer.itemone.mData.Price);
    }

    protected override void OnClose()
    {
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
        string name = notification.Name;
        object body = notification.Body;
        if (name != null)
        {
            if (name == "PUB_NETCONNECT_UPDATE")
            {
                this.UpdateNet();
            }
            else if (name == "PUB_UI_UPDATE_CURRENCY")
            {
                this.InitButton();
                this.InitAttribute();
            }
        }
    }

    protected override void OnInit()
    {
        this.mPool = LocalUnityObjctPool.Create(base.gameObject);
        this.mPool.CreateCache<Text>(this.Text_Attribute.gameObject);
        this.Text_Attribute.gameObject.SetActive(false);
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_EquipBuyInfo);
        }
        this.Button_Close.onClick = <>f__am$cache0;
        this.Button_Mask.onClick = this.Button_Close.onClick;
        this.Button_Buy.onClick = delegate {
            WindowUI.CloseWindow(WindowID.WindowID_EquipBuyInfo);
            if ((this.mTransfer != null) && (this.mTransfer.callback != null))
            {
                this.mTransfer.callback(this.mTransfer.itemone);
            }
        };
    }

    public override void OnLanguageChange()
    {
        this.Text_Buy.text = GameLogic.Hold.Language.GetLanguageByTID("EquipUI_Buy", Array.Empty<object>());
    }

    protected override void OnOpen()
    {
        GameLogic.Hold.Sound.PlayUI(SoundUIType.ePopUI);
        IProxy proxy = Facade.Instance.RetrieveProxy("EquipBuyInfoProxy");
        if (proxy != null)
        {
            this.mTransfer = (EquipBuyInfoProxy.Transfer) proxy.Data;
            if (this.mTransfer != null)
            {
                LocalSave.EquipOne one = new LocalSave.EquipOne {
                    EquipID = this.mTransfer.itemone.mData.ProductId,
                    Level = 1,
                    Count = 0,
                    bNew = false,
                    WearIndex = -1
                };
                this.mEquipData = one;
                this.itemone.Init(0, this.mTransfer.itemone.mData);
                this.itemone.Button_Buy.enabled = false;
                this.UpdateUI();
            }
        }
    }

    private void UpdateNet()
    {
    }

    private void UpdateUI()
    {
        this.equipparent.localScale = Vector3.one;
        this.Text_SkillName.text = this.mEquipData.NameString;
        this.Text_Info.text = this.mEquipData.InfoString;
        this.InitButton();
        this.InitAttribute();
    }

    private BlackItemOnectrl itemone
    {
        get
        {
            if (this._itemone == null)
            {
                this._itemone = CInstance<UIResourceCreator>.Instance.GetBlackShopOne(this.equipparent);
                this._itemone.SetCurrencyShow(false);
            }
            return this._itemone;
        }
    }
}

