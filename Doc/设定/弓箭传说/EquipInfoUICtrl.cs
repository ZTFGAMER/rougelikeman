using DG.Tweening;
using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class EquipInfoUICtrl : MediatorCtrlBase
{
    private const string AniMoveName = "CharEquipInfoMove";
    public ButtonCtrl Button_Close;
    public ButtonCtrl Button_Upgrade;
    public Transform equipparent;
    public Text Text_Info;
    public GoldTextCtrl mGoldTextCtrl;
    public ButtonCtrl Button_Mask;
    public RectTransform bg;
    public Text Text_IsMax;
    public UILineCtrl mTitleCtrl;
    public Text Text_Quality;
    public Text Text_Upgrade;
    public RectTransform Image_OutLine;
    public GameObject attributeParent;
    public Text Text_Attribute;
    public Animator mAni;
    public GameObject WearParent;
    public ButtonCtrl Button_Wear;
    public ButtonCtrl Button_Buy;
    public Text Text_Buy;
    public Text Text_Wear;
    public GoldTextCtrl mBuyGold;
    public EquipInfoNeedCtrl mNeedCtrl;
    public List<GameObject> typeparent;
    public GameObject equipattparent;
    public Text Text_EquipInfo;
    public Text Text_AttributeTitle;
    public Text Text_MaterialTitle;
    private EquipOneCtrl _equipctrl;
    private BlackItemOnectrl _itemone;
    private Vector2 wearbuttonstartpos;
    private RectTransform mRectTransform;
    private LocalSave.EquipOne mEquipData;
    private EquipInfoModuleProxy.Transfer mTransfer;
    private LocalUnityObjctPool mPool;
    private List<EquipInfoAttributeOne> mTexts = new List<EquipInfoAttributeOne>();
    private int diamondforcoin;
    private bool bGoldBuy;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    private string GetAttributeBase(int index)
    {
        List<Goods_goods.GoodShowData> equipShowAttrs = LocalModelManager.Instance.Equip_equip.GetEquipShowAttrs(this.mEquipData);
        List<string> equipAttributesNext = LocalModelManager.Instance.Equip_equip.GetEquipAttributesNext(this.mEquipData);
        string str = equipShowAttrs[index].ToString();
        if (this.mEquipData.IsMax)
        {
            return str;
        }
        if (!this.mEquipData.CountEnough)
        {
            return str;
        }
        object[] args = new object[] { str, equipAttributesNext[index] };
        return Utils.FormatString("{0} (<color=#00ff00ff>{1}</color>)", args);
    }

    private void InitAttribute()
    {
        this.mPool.Collect<EquipInfoAttributeOne>();
        this.mTexts.Clear();
        float y = 0f;
        string specialInfoString = this.mEquipData.SpecialInfoString;
        if (!string.IsNullOrEmpty(specialInfoString))
        {
            EquipInfoAttributeOne one = this.mPool.DeQueue<EquipInfoAttributeOne>();
            one.SetText(specialInfoString);
            RectTransform child = one.transform as RectTransform;
            child.SetParentNormal(this.attributeParent);
            child.anchoredPosition = new Vector2(0f, y);
            y -= one.GetTextHeight();
        }
        List<Goods_goods.GoodShowData> equipShowAttrs = LocalModelManager.Instance.Equip_equip.GetEquipShowAttrs(this.mEquipData);
        List<string> equipAttributesNext = LocalModelManager.Instance.Equip_equip.GetEquipAttributesNext(this.mEquipData);
        int index = 0;
        int count = equipShowAttrs.Count;
        while (index < count)
        {
            EquipInfoAttributeOne item = this.mPool.DeQueue<EquipInfoAttributeOne>();
            this.mTexts.Add(item);
            string attributeBase = this.GetAttributeBase(index);
            item.SetText(attributeBase);
            RectTransform transform = item.transform as RectTransform;
            transform.SetParentNormal(this.attributeParent);
            transform.anchoredPosition = new Vector2(0f, y);
            y -= item.GetTextHeight();
            index++;
        }
        List<string> equipShowAddAttributes = LocalModelManager.Instance.Equip_equip.GetEquipShowAddAttributes(this.mEquipData);
        int num4 = 0;
        int num5 = equipShowAddAttributes.Count;
        while (num4 < num5)
        {
            string str3 = equipShowAddAttributes[num4].ToString();
            EquipInfoAttributeOne one3 = this.mPool.DeQueue<EquipInfoAttributeOne>();
            one3.SetText(equipShowAddAttributes[num4]);
            RectTransform transform = one3.transform as RectTransform;
            transform.SetParentNormal(this.attributeParent);
            transform.anchoredPosition = new Vector2(0f, y);
            y -= one3.GetTextHeight();
            num4++;
        }
    }

    private void InitNormalButton()
    {
        if ((this.mTransfer != null) && (this.mTransfer.type == EquipInfoModuleProxy.InfoType.eNormal))
        {
            if (this.mEquipData.IsWear)
            {
                this.Text_Wear.text = GameLogic.Hold.Language.GetLanguageByTID("EquipUI_ButtonUnwear", Array.Empty<object>());
            }
            else
            {
                this.Text_Wear.text = GameLogic.Hold.Language.GetLanguageByTID("EquipUI_ButtonWear", Array.Empty<object>());
            }
            this.mGoldTextCtrl.SetCurrencyType(CurrencyType.Gold);
            this.mGoldTextCtrl.UseTextRed();
            this.mGoldTextCtrl.SetValue(this.mEquipData.NeedGold);
            if (this.mEquipData.IsMax)
            {
                this.Button_Upgrade.gameObject.SetActive(false);
                this.Text_IsMax.gameObject.SetActive(true);
                this.mNeedCtrl.gameObject.SetActive(false);
                (this.Button_Wear.transform.parent as RectTransform).anchoredPosition = new Vector2(0f, this.wearbuttonstartpos.y);
            }
            else
            {
                (this.Button_Wear.transform.parent as RectTransform).anchoredPosition = this.wearbuttonstartpos;
                this.Button_Upgrade.gameObject.SetActive(true);
                this.mNeedCtrl.gameObject.SetActive(true);
                this.UpdateButtonUpgrade();
                this.Text_IsMax.gameObject.SetActive(false);
            }
            this.mNeedCtrl.Init(this.mEquipData);
        }
    }

    private void OnClickUpgrade()
    {
        <OnClickUpgrade>c__AnonStorey0 storey = new <OnClickUpgrade>c__AnonStorey0 {
            $this = this
        };
        if (!this.mEquipData.CountEnough)
        {
            CInstance<TipsUIManager>.Instance.Show(ETips.Tips_EquipMaterialNotEnough, Array.Empty<string>());
        }
        else if (!this.mEquipData.GoldEnough)
        {
            long needgold = this.mEquipData.NeedGold - LocalSave.Instance.GetGold();
            PurchaseManager.Instance.SetOpenSource(ShopOpenSource.EEQUIP_UPGRADE);
            WindowUI.ShowGoldBuy(CoinExchangeSource.EEQUIP_UPGRADE, needgold, new Action<int>(this.OnGoldBuyCallback));
            this.diamondforcoin = (int) Formula.GetNeedDiamond(needgold);
        }
        else if (this.mEquipData.RowID == 0L)
        {
            CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError, Array.Empty<string>());
            SdkManager.Bugly_Report("EquipInfoUICtrl", "equipdata.RowID = 0");
        }
        else
        {
            storey.itemUpgrade = new CItemUpgarde();
            storey.itemUpgrade.m_nTransID = LocalSave.Instance.SaveExtra.GetTransID();
            storey.itemUpgrade.m_nRowID = this.mEquipData.RowID;
            storey.itemUpgrade.m_nCoins = (uint) this.mEquipData.NeedGold;
            storey.itemUpgrade.arrayItems = new CMaterialItem[] { new CMaterialItem() };
            storey.itemUpgrade.arrayItems[0].m_nEquipID = (uint) this.mEquipData.NeedMatID;
            storey.itemUpgrade.arrayItems[0].m_nMaterial = (uint) this.mEquipData.NeedMatCount;
            NetManager.SendInternal<CItemUpgarde>(storey.itemUpgrade, SendType.eForceOnce, new Action<NetResponse>(storey.<>m__0));
        }
    }

    protected override void OnClose()
    {
    }

    public override object OnGetEvent(string eventName) => 
        null;

    private void OnGoldBuyCallback(int diamond)
    {
        this.diamondforcoin = diamond;
        this.OnClickUpgrade();
    }

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
                this.InitNormalButton();
                this.InitAttribute();
            }
        }
    }

    protected override void OnInit()
    {
        this.mPool = LocalUnityObjctPool.Create(base.gameObject);
        this.mPool.CreateCache<EquipInfoAttributeOne>(this.Text_Attribute.gameObject);
        this.Text_Attribute.gameObject.SetActive(false);
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_EquipInfo);
        }
        this.Button_Close.onClick = <>f__am$cache0;
        this.Button_Mask.onClick = this.Button_Close.onClick;
        this.Button_Upgrade.onClick = new Action(this.OnClickUpgrade);
        this.mRectTransform = base.transform as RectTransform;
        this.Button_Wear.onClick = delegate {
            GameLogic.Hold.Guide.mEquip.CurrentOver(2);
            this.mTransfer.wearcallback();
            this.Button_Close.onClick();
        };
        this.Button_Buy.onClick = delegate {
            WindowUI.CloseWindow(WindowID.WindowID_EquipInfo);
            if ((this.mTransfer != null) && (this.mTransfer.buy_callback != null))
            {
                this.mTransfer.buy_callback(this.mTransfer.buy_itemone);
            }
        };
        this.wearbuttonstartpos = (this.Button_Wear.transform.parent as RectTransform).anchoredPosition;
    }

    public override void OnLanguageChange()
    {
        this.Text_Upgrade.text = GameLogic.Hold.Language.GetLanguageByTID("CardUI_Upgrade", Array.Empty<object>());
        this.Text_Buy.text = GameLogic.Hold.Language.GetLanguageByTID("EquipUI_Buy", Array.Empty<object>());
        this.Text_AttributeTitle.text = GameLogic.Hold.Language.GetLanguageByTID("EquipUI_AttributeTitle", Array.Empty<object>());
        this.Text_MaterialTitle.text = GameLogic.Hold.Language.GetLanguageByTID("EquipUI_MaterialTitle", Array.Empty<object>());
    }

    protected override void OnOpen()
    {
        this.Button_Buy.transform.parent.gameObject.SetActive(false);
        this.Button_Wear.transform.parent.gameObject.SetActive(false);
        this.Button_Upgrade.transform.parent.gameObject.SetActive(false);
        GameLogic.Hold.Sound.PlayUI(SoundUIType.ePopUI);
        IProxy proxy = Facade.Instance.RetrieveProxy("EquipInfoModuleProxy");
        if (proxy != null)
        {
            this.mTransfer = (EquipInfoModuleProxy.Transfer) proxy.Data;
            if (this.mTransfer != null)
            {
                this.mEquipData = this.mTransfer.one;
                switch (this.mTransfer.type)
                {
                    case EquipInfoModuleProxy.InfoType.eNormal:
                        this.Button_Wear.transform.parent.gameObject.SetActive(true);
                        this.Button_Upgrade.transform.parent.gameObject.SetActive(true);
                        this.Button_Buy.transform.parent.gameObject.SetActive(false);
                        this.mNeedCtrl.gameObject.SetActive(true);
                        this.type_show(1, false);
                        break;

                    case EquipInfoModuleProxy.InfoType.eBuy:
                        this.type_show(1, true);
                        this.mNeedCtrl.gameObject.SetActive(false);
                        this.Button_Wear.transform.parent.gameObject.SetActive(false);
                        this.Button_Upgrade.transform.parent.gameObject.SetActive(false);
                        this.Button_Buy.transform.parent.gameObject.SetActive(true);
                        this.mBuyGold.SetCurrencyType(this.mTransfer.buy_itemone.mData.PriceType);
                        this.mBuyGold.UseTextRed();
                        this.mBuyGold.SetValue(this.mTransfer.buy_itemone.mData.Price);
                        break;
                }
                this.UpdateUI();
            }
        }
    }

    private void PlayLevelUp()
    {
        float num = 0.24f;
        float num2 = 0.07f;
        int attributeAllCount = LocalModelManager.Instance.Equip_equip.GetAttributeAllCount(this.mEquipData.EquipID);
        Sequence sequence = DOTween.Sequence();
        TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.equipparent, 1.3f, num), 6));
        TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this, this.<PlayLevelUp>m__3));
        TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.equipparent, 1f, num2), 6));
        for (int i = 0; i < attributeAllCount; i++)
        {
            <PlayLevelUp>c__AnonStorey1 storey = new <PlayLevelUp>c__AnonStorey1 {
                $this = this,
                index = i
            };
            if (storey.index < this.mTexts.Count)
            {
                TweenSettingsExtensions.AppendInterval(sequence, 0.2f);
                TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.mTexts[storey.index].transform, 1.3f, num), 6));
                TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(storey, this.<>m__0));
                TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.mTexts[storey.index].transform, 1f, num2), 6));
            }
        }
    }

    private void type_show(int index, bool value)
    {
        this.typeparent[index].SetActive(value);
    }

    private void update_equipinfo()
    {
        this.mEquipCtrl.Init(this.mEquipData);
        this.mEquipCtrl.SetButtonEnable(false);
        this.mEquipCtrl.ShowLevel(false);
        this.mEquipCtrl.SetCountShow(false);
        EquipType propType = this.mEquipData.PropType;
        if (propType == EquipType.eEquip)
        {
            this.type_show(0, true);
            object[] args = new object[] { GameLogic.Hold.Language.Level, this.mEquipData.Level, this.mEquipData.CurrentMaxLevel };
            this.Text_EquipInfo.text = Utils.FormatString("{0}: {1}/{2}", args);
        }
        else if (propType == EquipType.eMaterial)
        {
            this.type_show(0, false);
            object[] args = new object[] { GameLogic.Hold.Language.Count, this.mEquipData.Count };
            this.Text_EquipInfo.text = Utils.FormatString("{0}: {1}", args);
        }
        else
        {
            object[] args = new object[] { this.mEquipData.EquipID, this.mEquipData.PropType };
            SdkManager.Bugly_Report("EquipInfoUICtrl", Utils.FormatString("update_equipinfo. Equip:{0} PropType:{1} is not achieve!", args));
        }
    }

    private void UpdateButtonUpgrade()
    {
        bool flag = NetManager.IsNetConnect && this.mEquipData.CountEnough;
        this.Button_Upgrade.SetGray(flag);
        if (flag)
        {
            this.mGoldTextCtrl.SetTextRed(!this.mEquipData.GoldEnough);
        }
        else
        {
            this.mGoldTextCtrl.SetTextRed(false);
        }
    }

    private void UpdateNet()
    {
        this.UpdateButtonUpgrade();
    }

    private void UpdateUI()
    {
        this.mRectTransform.anchoredPosition = Vector2.zero;
        this.equipparent.localScale = Vector3.one;
        Color color = LocalSave.QualityColors[this.mEquipData.Quality];
        this.mTitleCtrl.SetColor(color);
        this.mTitleCtrl.SetText(this.mEquipData.NameOnlyString);
        this.Text_Quality.set_color(color);
        this.Text_Quality.text = this.mEquipData.QualityString;
        this.Text_Info.text = this.mEquipData.InfoString;
        this.equipattparent.SetActive(false);
        EquipType propType = this.mEquipData.PropType;
        if (propType == EquipType.eEquip)
        {
            this.InitNormalButton();
            this.InitAttribute();
            switch (this.mTransfer.type)
            {
                case EquipInfoModuleProxy.InfoType.eNormal:
                    this.bg.sizeDelta = new Vector2(this.bg.sizeDelta.x, 1010f);
                    break;

                case EquipInfoModuleProxy.InfoType.eBuy:
                    this.bg.sizeDelta = new Vector2(this.bg.sizeDelta.x, 840f);
                    break;
            }
            this.equipattparent.SetActive(true);
        }
        else if (propType == EquipType.eMaterial)
        {
            this.bg.sizeDelta = new Vector2(this.bg.sizeDelta.x, 480f);
        }
        else
        {
            object[] args = new object[] { this.mEquipData.EquipID, this.mEquipData.PropType };
            SdkManager.Bugly_Report("EquipInfoUICtrl", Utils.FormatString("UpdateUI. Equip:{0} PropType:{1} is not achieve!", args));
        }
        this.update_equipinfo();
        GameLogic.Hold.Guide.mEquip.GoNext(2, this.Button_Wear.transform as RectTransform);
    }

    private EquipOneCtrl mEquipCtrl
    {
        get
        {
            if (this._equipctrl == null)
            {
                this._equipctrl = CInstance<UIResourceCreator>.Instance.GetEquip(this.equipparent);
                this._equipctrl.ShowAniEnable(false);
                this._equipctrl.SetButtonEnable(false);
            }
            return this._equipctrl;
        }
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

    [CompilerGenerated]
    private sealed class <OnClickUpgrade>c__AnonStorey0
    {
        internal CItemUpgarde itemUpgrade;
        internal EquipInfoUICtrl $this;

        internal void <>m__0(NetResponse response)
        {
            if (response.IsSuccess)
            {
                LocalSave.Instance.Modify_Gold((long) -this.$this.mEquipData.NeedGold, true);
                LocalSave.EquipOne propByID = LocalSave.Instance.GetPropByID(this.$this.mEquipData.NeedMatID);
                if (propByID != null)
                {
                    propByID.Count -= this.$this.mEquipData.NeedMatCount;
                }
                LocalSave.Instance.EquipLevelUp(this.$this.mEquipData);
                if (this.$this.mTransfer.updatecallback != null)
                {
                    this.$this.mTransfer.updatecallback();
                }
                this.$this.InitNormalButton();
                this.$this.PlayLevelUp();
                SdkManager.send_event_equipment("UPGRADE", this.$this.mEquipData.EquipID, 0, this.$this.mEquipData.Level, EquipSource.EEquip_page, (int) this.itemUpgrade.m_nCoins);
                this.$this.bGoldBuy = false;
                this.$this.diamondforcoin = 0;
            }
            else if (response.error != null)
            {
                if (response.error.m_nStatusCode == 8)
                {
                    CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 1);
                    SdkManager.Bugly_Report("EquipInfoUICtrl", " gold not enough");
                }
                else if (response.error.m_nStatusCode == 10)
                {
                    CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 0);
                    SdkManager.Bugly_Report("EquipInfoUICtrl", " scroll not enough");
                }
            }
            else
            {
                CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError, Array.Empty<string>());
                SdkManager.Bugly_Report("EquipInfoUICtrl", " error == null");
            }
        }
    }

    [CompilerGenerated]
    private sealed class <PlayLevelUp>c__AnonStorey1
    {
        internal int index;
        internal EquipInfoUICtrl $this;

        internal void <>m__0()
        {
            if (this.index < this.$this.mTexts.Count)
            {
                this.$this.mTexts[this.index].SetText(this.$this.GetAttributeBase(this.index));
                this.$this.mAni.Play("CharEquipInfoMove");
            }
        }
    }
}

