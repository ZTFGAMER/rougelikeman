using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ShopOneGold : ShopOneBase
{
    public const float itemwidth = 235f;
    public Text Text_Title;
    public GameObject goldparent;
    private List<ShopItemGold> mList = new List<ShopItemGold>();
    private GameObject _itemgold;
    private LocalUnityObjctPool mPool;

    protected override void OnAwake()
    {
        if (this.mPool == null)
        {
            this.mPool = LocalUnityObjctPool.Create(base.gameObject);
            this.mPool.CreateCache<ShopItemGold>(this.itemgold);
            this.itemgold.SetActive(false);
        }
    }

    private void OnClickGold(int index, ShopItemGold item)
    {
        <OnClickGold>c__AnonStorey0 storey = new <OnClickGold>c__AnonStorey0 {
            item = item,
            index = index
        };
        storey.diamond = storey.item.GetDiamond();
        if (LocalSave.Instance.GetDiamond() < storey.diamond)
        {
            CInstance<TipsUIManager>.Instance.Show(ETips.Tips_DiamondNotEnoughBuy, Array.Empty<string>());
            Facade.Instance.SendNotification("MainUI_GotoShop", "ShopOneDiamond");
        }
        else
        {
            <OnClickGold>c__AnonStorey1 storey2 = new <OnClickGold>c__AnonStorey1 {
                <>f__ref$0 = storey,
                gold = storey.item.GetGold()
            };
            CDiamondToCoin packet = new CDiamondToCoin {
                m_nTransID = LocalSave.Instance.SaveExtra.GetTransID(),
                m_nCoins = (uint) storey2.gold,
                m_nDiamonds = (uint) storey.diamond
            };
            NetManager.SendInternal<CDiamondToCoin>(packet, SendType.eForceOnce, new Action<NetResponse>(storey2.<>m__0));
        }
    }

    protected override void OnDeinit()
    {
    }

    protected override void OnInit()
    {
        this.mPool.Collect<ShopItemGold>();
        this.mList.Clear();
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("商店_金币标题", Array.Empty<object>()), Array.Empty<object>());
        int num = 3;
        float num2 = (num - 1) * 235f;
        for (int i = 0; i < num; i++)
        {
            ShopItemGold item = this.mPool.DeQueue<ShopItemGold>();
            item.gameObject.SetParentNormal(this.goldparent);
            RectTransform transform = item.transform as RectTransform;
            transform.anchoredPosition = new Vector2((-num2 / 2f) + (235f * i), 0f);
            item.Init(i);
            item.OnClickButton = new Action<int, ShopItemGold>(this.OnOpenWindowSure);
            this.mList.Add(item);
        }
    }

    public override void OnLanguageChange()
    {
        this.OnInit();
    }

    private void OnOpenWindowSure(int index, ShopItemGold item)
    {
        BuyGoldSureProxy.Transfer data = new BuyGoldSureProxy.Transfer {
            index = index,
            item = item,
            callback = new Action<int, ShopItemGold>(this.OnClickGold)
        };
        BuyGoldSureProxy proxy = new BuyGoldSureProxy(data);
        Facade.Instance.RegisterProxy(proxy);
        WindowUI.ShowWindow(WindowID.WindowID_BuyGoldSure);
    }

    public override void UpdateNet()
    {
        int num = 0;
        int count = this.mList.Count;
        while (num < count)
        {
            this.mList[num].UpdateNet();
            num++;
        }
    }

    private GameObject itemgold
    {
        get
        {
            if (this._itemgold == null)
            {
                this._itemgold = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("UIPanel/ShopUI/ShopItemGoldOne"));
                this._itemgold.SetParentNormal(this.goldparent);
            }
            return this._itemgold;
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickGold>c__AnonStorey0
    {
        internal ShopItemGold item;
        internal int index;
        internal int diamond;
    }

    [CompilerGenerated]
    private sealed class <OnClickGold>c__AnonStorey1
    {
        internal int gold;
        internal ShopOneGold.<OnClickGold>c__AnonStorey0 <>f__ref$0;

        internal void <>m__0(NetResponse response)
        {
            if ((response.IsSuccess && (response.data != null)) && (response.data is CRespDimaonToCoin))
            {
                CRespDimaonToCoin data = response.data as CRespDimaonToCoin;
                if (data != null)
                {
                    LocalSave.Instance.UserInfo_SetDiamond((int) data.m_nDiamonds);
                    long gold = LocalSave.Instance.GetGold();
                    long nCoins = data.m_nCoins;
                    if (gold < nCoins)
                    {
                        long num3 = nCoins - gold;
                        LocalSave.Instance.UserInfo_SetGold((int) gold);
                        LocalSave.Instance.Modify_Gold(num3, false);
                        if (this.<>f__ref$0.item != null)
                        {
                            CurrencyFlyCtrl.PlayGet(CurrencyType.Gold, (long) this.gold, this.<>f__ref$0.item.transform.position, null, null, true);
                        }
                        else
                        {
                            CurrencyFlyCtrl.PlayGet(CurrencyType.Gold, (long) this.gold, null, null, true);
                        }
                    }
                    else
                    {
                        LocalSave.Instance.UserInfo_SetGold((int) data.m_nCoins);
                    }
                    object[] args = new object[] { this.<>f__ref$0.index + 1 };
                    SdkManager.send_event_shop(Utils.FormatString("Gold{0}", args), this.gold, this.<>f__ref$0.diamond, 0, 0);
                }
            }
            else if (response.error != null)
            {
                CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 2);
            }
        }
    }
}

