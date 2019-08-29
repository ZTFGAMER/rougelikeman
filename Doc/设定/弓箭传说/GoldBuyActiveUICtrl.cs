using Dxx.Net;
using GameProtocol;
using PureMVC.Interfaces;
using System;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine.UI;

public class GoldBuyActiveUICtrl : MediatorCtrlBase
{
    public Text Text_Title;
    public Text Text_Content;
    public GoldTextCtrl mDiamondCtrl;
    public ButtonCtrl Button_Buy;
    public ButtonCtrl Button_Close;
    public ButtonCtrl Button_Shadow;
    private long gold;
    private long needdiamond;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    private void InitUI()
    {
        this.needdiamond = 50L;
        this.gold = this.needdiamond * LocalModelManager.Instance.Shop_Gold.GetDiamond2Gold();
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("金币购买标题", Array.Empty<object>());
        object[] args = new object[] { this.gold };
        this.Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("金币购买描述", args);
        this.mDiamondCtrl.SetCurrencyType(CurrencyType.Diamond);
        this.mDiamondCtrl.UseTextRed();
        this.mDiamondCtrl.SetValue((int) this.needdiamond);
        this.Button_Buy.onClick = delegate {
            if (LocalSave.Instance.GetDiamond() < this.needdiamond)
            {
                PurchaseManager.Instance.SetOpenSource(ShopOpenSource.EACTIVE);
                WindowUI.ShowShopSingle(ShopSingleProxy.SingleType.eDiamond, null);
            }
            else
            {
                CDiamondToCoin packet = new CDiamondToCoin {
                    m_nTransID = LocalSave.Instance.SaveExtra.GetTransID(),
                    m_nCoins = (uint) this.gold,
                    m_nDiamonds = (uint) this.needdiamond
                };
                Debugger.Log(string.Concat(new object[] { "Send DiamondToCoin Request ", packet.m_nCoins, " transid ", packet.m_nTransID }));
                NetManager.SendInternal<CDiamondToCoin>(packet, SendType.eForceOnce, delegate (NetResponse response) {
                    if ((response.IsSuccess && (response.data != null)) && (response.data is CRespDimaonToCoin))
                    {
                        CRespDimaonToCoin data = response.data as CRespDimaonToCoin;
                        LocalSave.Instance.UserInfo_SetDiamond((int) data.m_nDiamonds);
                        long gold = LocalSave.Instance.GetGold();
                        long nCoins = data.m_nCoins;
                        if (gold < nCoins)
                        {
                            long num3 = nCoins - gold;
                            LocalSave.Instance.UserInfo_SetGold((int) gold);
                            LocalSave.Instance.Modify_Gold(num3, false);
                            CurrencyFlyCtrl.PlayGet(CurrencyType.Gold, num3, null, null, true);
                        }
                        else
                        {
                            LocalSave.Instance.UserInfo_SetGold((int) data.m_nCoins);
                        }
                        this.Button_Close.onClick();
                    }
                    else if (response.error != null)
                    {
                        CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 2);
                    }
                });
            }
        };
    }

    protected override void OnClose()
    {
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_GoldBuyActive);
        }
        this.Button_Close.onClick = <>f__am$cache0;
        this.Button_Shadow.onClick = this.Button_Close.onClick;
        this.Button_Buy.SetDepondNet(true);
    }

    public override void OnLanguageChange()
    {
    }

    protected override void OnOpen()
    {
        this.InitUI();
    }
}

