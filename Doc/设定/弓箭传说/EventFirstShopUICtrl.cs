using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using PureMVC.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class EventFirstShopUICtrl : MediatorCtrlBase
{
    public const int FirstShopCount = 2;
    public Text Text_Title;
    public ButtonCtrl Button_Close;
    public ButtonCtrl Button_Shadow;
    public GameObject copyitems;
    public GameObject copyitem;
    public GameObject itemsparent;
    private List<bool> goodbuy;
    private LocalUnityObjctPool mPool;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    private void InitUI()
    {
        IList<Shop_ReadyShop> allBeans = LocalModelManager.Instance.Shop_ReadyShop.GetAllBeans();
        int count = allBeans.Count;
        count = MathDxx.Clamp(count, count, 2);
        this.goodbuy = LocalSave.Instance.BattleIn_GetFirstShop();
        for (int i = 0; i < count; i++)
        {
            FirstItemOnectrl onectrl = this.mPool.DeQueue<FirstItemOnectrl>();
            onectrl.Init(i, allBeans[i], GameLogic.Hold.BattleData.GetFirstShopBuy(i));
            onectrl.OnClickButton = new Action<FirstItemOnectrl>(this.OnClickBuy);
            onectrl.SetBuy(this.goodbuy[i]);
            RectTransform child = onectrl.transform as RectTransform;
            child.SetParentNormal(this.itemsparent);
            child.anchoredPosition = new Vector2(0f, (float) (i * -170));
        }
    }

    private void OnClickBuy(FirstItemOnectrl one)
    {
        <OnClickBuy>c__AnonStorey0 storey = new <OnClickBuy>c__AnonStorey0 {
            one = one,
            $this = this
        };
        switch (((CurrencyType) storey.one.mData.PriceType))
        {
            case CurrencyType.Gold:
            {
                long gold = LocalSave.Instance.GetGold();
                int price = storey.one.mData.Price;
                if (gold < price)
                {
                    PurchaseManager.Instance.SetOpenSource(ShopOpenSource.EFIRST_SHOP);
                    WindowUI.ShowGoldBuy(CoinExchangeSource.EFIRST_SHOP, price - gold, new Action<int>(storey.<>m__0));
                    return;
                }
                CLifeTransPacket packet = new CLifeTransPacket {
                    m_nTransID = LocalSave.Instance.SaveExtra.GetTransID(),
                    m_nType = 3,
                    m_nMaterial = (ushort) price
                };
                NetManager.SendInternal<CLifeTransPacket>(packet, SendType.eForceOnce, new Action<NetResponse>(storey.<>m__1));
                break;
            }
            case CurrencyType.Diamond:
            {
                long diamond = LocalSave.Instance.GetDiamond();
                int price = storey.one.mData.Price;
                if (diamond < price)
                {
                    PurchaseManager.Instance.SetOpenSource(ShopOpenSource.EFIRST_SHOP);
                    WindowUI.ShowShopSingle(ShopSingleProxy.SingleType.eDiamond, null);
                    return;
                }
                CLifeTransPacket packet2 = new CLifeTransPacket {
                    m_nTransID = LocalSave.Instance.SaveExtra.GetTransID(),
                    m_nType = 4,
                    m_nMaterial = (ushort) price
                };
                NetManager.SendInternal<CLifeTransPacket>(packet2, SendType.eForceOnce, new Action<NetResponse>(storey.<>m__2));
                break;
            }
        }
        this.goodbuy[storey.one.mIndex] = true;
        LocalSave.Instance.BattleIn_UpdateFirstShop(this.goodbuy);
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
    }

    protected override void OnInit()
    {
        this.mPool = LocalUnityObjctPool.Create(base.gameObject);
        this.mPool.CreateCache<FirstItemOnectrl>(this.copyitem);
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_EventFirstShop);
        }
        this.Button_Close.onClick = <>f__am$cache0;
        this.Button_Shadow.onClick = this.Button_Close.onClick;
        this.copyitems.SetActive(false);
    }

    public override void OnLanguageChange()
    {
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("初始商店标题", Array.Empty<object>());
    }

    protected override void OnOpen()
    {
        GameLogic.SetPause(true);
        WindowUI.ShowCurrency(WindowID.WindowID_Currency);
        this.mPool.Collect<FirstItemOnectrl>();
        this.InitUI();
    }

    [CompilerGenerated]
    private sealed class <OnClickBuy>c__AnonStorey0
    {
        internal FirstItemOnectrl one;
        internal EventFirstShopUICtrl $this;

        internal void <>m__0(int diamond)
        {
            this.$this.OnClickBuy(this.one);
        }

        internal void <>m__1(NetResponse response)
        {
            if (response.IsSuccess)
            {
                LocalSave.Instance.Modify_Gold((long) -this.one.mData.Price, true);
                this.one.Buy();
            }
            else if (response.error != null)
            {
                CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 2);
            }
        }

        internal void <>m__2(NetResponse response)
        {
            if (response.IsSuccess)
            {
                LocalSave.Instance.Modify_Diamond((long) -this.one.mData.Price, true);
                this.one.Buy();
            }
            else if (response.error != null)
            {
                CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 2);
            }
        }
    }
}

