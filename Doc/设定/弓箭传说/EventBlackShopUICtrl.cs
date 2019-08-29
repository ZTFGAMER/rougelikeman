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

public class EventBlackShopUICtrl : MediatorCtrlBase
{
    private const float width = 200f;
    private const float height = 240f;
    private const int LineCount = 4;
    public Text Text_Title;
    public Text Text_Content;
    public Text Text_Close;
    public ButtonCtrl Button_Close;
    public GameObject items;
    private GameObject _itemone;
    private List<BlackItemOnectrl> mList = new List<BlackItemOnectrl>();
    private LocalUnityObjctPool mPool;
    private List<Shop_MysticShop> mDataList = new List<Shop_MysticShop>();
    private List<int> buys = new List<int>();
    private int diamondforcoin;
    private List<int> shows = new List<int>();
    private int shoptype;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    private void add_equipexp(CEquipmentItem item)
    {
        if (item != null)
        {
            LocalSave.EquipOne one = new LocalSave.EquipOne {
                UniqueID = item.m_nUniqueID,
                EquipID = (int) item.m_nEquipID,
                Level = 1,
                Count = (int) item.m_nFragment
            };
            if (one.Overlying)
            {
                LocalSave.Instance.AddProp(item);
            }
        }
    }

    private CEquipTrans GetEquipTrans(Shop_MysticShop data, int gold, int diamond)
    {
        CEquipTrans trans = new CEquipTrans {
            m_nTransID = LocalSave.Instance.SaveExtra.GetTransID(),
            m_nType = 1,
            m_nCoins = (ushort) gold,
            m_nDiamonds = (ushort) diamond,
            m_stEquipItem = new CEquipmentItem()
        };
        trans.m_stEquipItem.m_nUniqueID = Utils.GenerateUUID();
        trans.m_stEquipItem.m_nEquipID = (uint) data.ProductId;
        trans.m_stEquipItem.m_nLevel = 1;
        trans.m_stEquipItem.m_nFragment = (uint) data.ProductNum;
        return trans;
    }

    private void InitUI()
    {
        this.buys.Clear();
        this.shoptype = LocalModelManager.Instance.Shop_MysticShop.GetRandomShopType();
        this.mDataList = LocalModelManager.Instance.Shop_MysticShop.GetListByStage(GameLogic.Hold.BattleData.Level_CurrentStage, this.shoptype);
        this.mList.Clear();
        int count = this.mDataList.Count;
        float num3 = (-(MathDxx.Clamp(count, 0, 4) - 1) * 200f) / 2f;
        for (int i = 0; i < count; i++)
        {
            BlackItemOnectrl item = this.mPool.DeQueue<BlackItemOnectrl>();
            item.Init(i, this.mDataList[i]);
            item.OnClickButton = new Action<BlackItemOnectrl>(this.OnClickBuy);
            RectTransform child = item.transform as RectTransform;
            child.SetParentNormal(this.items);
            child.anchoredPosition = new Vector2(num3 + ((i % 4) * 200f), 120f + ((i / 4) * -240f));
            this.mList.Add(item);
        }
        this.shows.Clear();
        int num5 = 0;
        int num6 = this.mDataList.Count;
        while (num5 < num6)
        {
            this.shows.Add(this.mDataList[num5].ProductId);
            num5++;
        }
        for (int j = this.shows.Count; j < 8; j++)
        {
            this.shows.Add(0);
        }
        if (this.shows.Count >= 8)
        {
            SdkManager.send_event_mysteries("SHOW", this.shoptype, 0, 0, this.shows[0], this.shows[1], this.shows[2], this.shows[3], this.shows[4], this.shows[5], this.shows[6], this.shows[7], 0, 0, string.Empty, string.Empty);
        }
    }

    private void OnClickBuy(BlackItemOnectrl one)
    {
        int coins = (one.mData.PriceType != 1) ? 0 : one.mData.Price;
        int gems = (one.mData.PriceType != 2) ? 0 : one.mData.Price;
        SdkManager.send_event_mysteries("CLICK", this.shoptype, one.mIndex, one.mData.ProductId, 0, 0, 0, 0, 0, 0, 0, 0, coins, gems, string.Empty, string.Empty);
        LocalSave.EquipOne one2 = new LocalSave.EquipOne {
            EquipID = one.mData.ProductId,
            bNew = false,
            Count = one.mData.ProductNum,
            Level = 1,
            WearIndex = -1
        };
        EquipInfoModuleProxy.Transfer data = new EquipInfoModuleProxy.Transfer {
            one = one2,
            type = EquipInfoModuleProxy.InfoType.eBuy,
            buy_itemone = one,
            buy_callback = new Action<BlackItemOnectrl>(this.OnClickBuyInternal)
        };
        EquipInfoModuleProxy proxy = new EquipInfoModuleProxy(data);
        Facade.Instance.RegisterProxy(proxy);
        WindowUI.ShowWindow(WindowID.WindowID_EquipInfo);
    }

    private void OnClickBuyInternal(BlackItemOnectrl one)
    {
        <OnClickBuyInternal>c__AnonStorey0 storey = new <OnClickBuyInternal>c__AnonStorey0 {
            one = one,
            $this = this
        };
        switch (((CurrencyType) storey.one.mData.PriceType))
        {
            case CurrencyType.Gold:
            {
                <OnClickBuyInternal>c__AnonStorey1 storey2 = new <OnClickBuyInternal>c__AnonStorey1 {
                    <>f__ref$0 = storey
                };
                long gold = LocalSave.Instance.GetGold();
                storey2.needgold = storey.one.mData.Price;
                if (gold < storey2.needgold)
                {
                    PurchaseManager.Instance.SetOpenSource(ShopOpenSource.EBLACK_SHOP);
                    WindowUI.ShowGoldBuy(CoinExchangeSource.EBLACK_SHOP, storey2.needgold - gold, new Action<int>(storey2.<>m__0));
                    return;
                }
                SdkManager.send_event_mysteries("CLICK_PURCHASE", this.shoptype, storey.one.mIndex, storey.one.mData.ProductId, 0, 0, 0, 0, 0, 0, 0, 0, storey2.needgold, 0, string.Empty, string.Empty);
                storey2.equipTrans = this.GetEquipTrans(storey.one.mData, storey2.needgold, 0);
                NetManager.SendInternal<CEquipTrans>(storey2.equipTrans, SendType.eForceOnce, new Action<NetResponse>(storey2.<>m__1));
                break;
            }
            case CurrencyType.Diamond:
            {
                <OnClickBuyInternal>c__AnonStorey2 storey3 = new <OnClickBuyInternal>c__AnonStorey2 {
                    <>f__ref$0 = storey
                };
                long diamond = LocalSave.Instance.GetDiamond();
                storey3.needdiamond = storey.one.mData.Price;
                if (diamond < storey3.needdiamond)
                {
                    PurchaseManager.Instance.SetOpenSource(ShopOpenSource.EBLACK_SHOP);
                    WindowUI.ShowShopSingle(ShopSingleProxy.SingleType.eDiamond, null);
                    return;
                }
                SdkManager.send_event_mysteries("CLICK_PURCHASE", this.shoptype, storey.one.mIndex, storey.one.mData.ProductId, 0, 0, 0, 0, 0, 0, 0, 0, 0, storey3.needdiamond, string.Empty, string.Empty);
                storey3.equipTrans = this.GetEquipTrans(storey.one.mData, 0, storey3.needdiamond);
                NetManager.SendInternal<CEquipTrans>(storey3.equipTrans, SendType.eForceOnce, new Action<NetResponse>(storey3.<>m__0));
                break;
            }
        }
    }

    protected override void OnClose()
    {
        GameLogic.SetPause(false);
        WindowUI.CloseCurrency();
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
        string name = notification.Name;
        object body = notification.Body;
        if ((name != null) && (name == "PUB_UI_UPDATE_CURRENCY"))
        {
            this.UpdateCurrency();
        }
    }

    protected override void OnInit()
    {
        this.mPool = LocalUnityObjctPool.Create(base.gameObject);
        this.mPool.CreateCache<BlackItemOnectrl>(this.itemone);
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_EventBlackShop);
        }
        this.Button_Close.onClick = <>f__am$cache0;
    }

    public override void OnLanguageChange()
    {
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("神秘商店标题", Array.Empty<object>());
        this.Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("神秘商店描述", Array.Empty<object>());
        this.Text_Close.text = GameLogic.Hold.Language.GetLanguageByTID("神秘商店关闭", Array.Empty<object>());
    }

    protected override void OnOpen()
    {
        GameLogic.SetPause(true);
        WindowUI.ShowCurrency(WindowID.WindowID_Currency);
        this.mPool.Collect<BlackItemOnectrl>();
        this.InitUI();
    }

    private void UpdateCurrency()
    {
        int num = 0;
        int count = this.mList.Count;
        while (num < count)
        {
            this.mList[num].UpdateCurrency();
            num++;
        }
    }

    private GameObject itemone
    {
        get
        {
            if (this._itemone == null)
            {
                this._itemone = CInstance<UIResourceCreator>.Instance.GetBlackShopOne(base.transform).gameObject;
                this._itemone.SetActive(false);
            }
            return this._itemone;
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickBuyInternal>c__AnonStorey0
    {
        internal BlackItemOnectrl one;
        internal EventBlackShopUICtrl $this;
    }

    [CompilerGenerated]
    private sealed class <OnClickBuyInternal>c__AnonStorey1
    {
        internal CEquipTrans equipTrans;
        internal int needgold;
        internal EventBlackShopUICtrl.<OnClickBuyInternal>c__AnonStorey0 <>f__ref$0;

        internal void <>m__0(int diamond)
        {
            this.<>f__ref$0.$this.diamondforcoin = diamond;
            this.<>f__ref$0.$this.OnClickBuyInternal(this.<>f__ref$0.one);
        }

        internal void <>m__1(NetResponse response)
        {
            if (response.IsSuccess)
            {
                this.<>f__ref$0.$this.add_equipexp(this.equipTrans.m_stEquipItem);
                LocalSave.Instance.Modify_Gold((long) -this.<>f__ref$0.one.mData.Price, true);
                this.<>f__ref$0.one.Buy();
                CInstance<TipsUIManager>.Instance.Show(ETips.Tips_BlackShopBuy, Array.Empty<string>());
                this.<>f__ref$0.$this.diamondforcoin = 0;
                this.<>f__ref$0.$this.buys.Add(this.<>f__ref$0.one.mData.ProductId);
                SdkManager.send_event_equipment("GET", this.<>f__ref$0.one.mData.ProductId, this.<>f__ref$0.one.mData.ProductNum, 1, EquipSource.EBlack_shop, 0);
                SdkManager.send_event_mysteries("FINISH", this.<>f__ref$0.$this.shoptype, this.<>f__ref$0.one.mIndex, this.<>f__ref$0.one.mData.ProductId, 0, 0, 0, 0, 0, 0, 0, 0, this.needgold, 0, "SUCCESS", string.Empty);
            }
            else if (response.error != null)
            {
                CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 1);
                SdkManager.send_event_mysteries("FINISH", this.<>f__ref$0.$this.shoptype, this.<>f__ref$0.one.mIndex, this.<>f__ref$0.one.mData.ProductId, 0, 0, 0, 0, 0, 0, 0, 0, this.needgold, 0, "FAIL", "CURRENCY_NOT_ENOUGH");
            }
            else
            {
                SdkManager.send_event_mysteries("FINISH", this.<>f__ref$0.$this.shoptype, this.<>f__ref$0.one.mIndex, this.<>f__ref$0.one.mData.ProductId, 0, 0, 0, 0, 0, 0, 0, 0, this.needgold, 0, "FAIL", "RESPONSE_ERROR_NULL");
            }
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickBuyInternal>c__AnonStorey2
    {
        internal CEquipTrans equipTrans;
        internal int needdiamond;
        internal EventBlackShopUICtrl.<OnClickBuyInternal>c__AnonStorey0 <>f__ref$0;

        internal void <>m__0(NetResponse response)
        {
            if (response.IsSuccess)
            {
                this.<>f__ref$0.$this.add_equipexp(this.equipTrans.m_stEquipItem);
                LocalSave.Instance.Modify_Diamond((long) -this.<>f__ref$0.one.mData.Price, true);
                this.<>f__ref$0.one.Buy();
                CInstance<TipsUIManager>.Instance.Show(ETips.Tips_BlackShopBuy, Array.Empty<string>());
                this.<>f__ref$0.$this.diamondforcoin = 0;
                this.<>f__ref$0.$this.buys.Add(this.<>f__ref$0.one.mData.ProductId);
                SdkManager.send_event_equipment("GET", this.<>f__ref$0.one.mData.ProductId, this.<>f__ref$0.one.mData.ProductNum, 1, EquipSource.EBlack_shop, 0);
                SdkManager.send_event_mysteries("FINISH", this.<>f__ref$0.$this.shoptype, this.<>f__ref$0.one.mIndex, this.<>f__ref$0.one.mData.ProductId, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.needdiamond, "SUCCESS", string.Empty);
            }
            else if (response.error != null)
            {
                CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 2);
                SdkManager.send_event_mysteries("FINISH", this.<>f__ref$0.$this.shoptype, this.<>f__ref$0.one.mIndex, this.<>f__ref$0.one.mData.ProductId, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.needdiamond, "FAIL", "DIAMOND_NOT_ENOUGH");
            }
            else
            {
                CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError, Array.Empty<string>());
                SdkManager.send_event_mysteries("FINISH", this.<>f__ref$0.$this.shoptype, this.<>f__ref$0.one.mIndex, this.<>f__ref$0.one.mData.ProductId, 0, 0, 0, 0, 0, 0, 0, 0, 0, this.needdiamond, "FAIL", "RESPONSE_ERROR_NULL");
            }
        }
    }
}

