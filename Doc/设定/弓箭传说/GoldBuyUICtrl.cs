using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.UI;

public class GoldBuyUICtrl : MediatorCtrlBase
{
    private static CoinExchangeSource mSource;
    public Text Text_Title;
    public Text Text_Content;
    public GoldTextCtrl mDiamondCtrl;
    public ButtonCtrl Button_Buy;
    public ButtonCtrl Button_Close;
    public ButtonCtrl Button_Shadow;
    private GoldBuyModuleProxy.Transfer mTransfer;
    private long needdiamond;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    public static CoinExchangeSource GetSource() => 
        mSource;

    private void InitUI()
    {
        IProxy proxy = Facade.Instance.RetrieveProxy("GoldBuy");
        this.mTransfer = proxy.Data as GoldBuyModuleProxy.Transfer;
        this.needdiamond = Formula.GetNeedDiamond(this.mTransfer.gold);
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("金币不足标题", Array.Empty<object>());
        this.Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("金币不足描述", Array.Empty<object>());
        this.mDiamondCtrl.SetCurrencyType(CurrencyType.Diamond);
        this.mDiamondCtrl.UseTextRed();
        this.mDiamondCtrl.SetValue((int) this.needdiamond);
        this.Button_Buy.onClick = delegate {
            if (LocalSave.Instance.GetDiamond() < this.needdiamond)
            {
                WindowUI.ShowShopSingle(ShopSingleProxy.SingleType.eDiamond, null);
            }
            else
            {
                <InitUI>c__AnonStorey0 storey = new <InitUI>c__AnonStorey0 {
                    $this = this,
                    diamondToCoin = new CDiamondToCoin()
                };
                storey.diamondToCoin.m_nTransID = LocalSave.Instance.SaveExtra.GetTransID();
                storey.diamondToCoin.m_nCoins = (uint) this.mTransfer.gold;
                storey.diamondToCoin.m_nDiamonds = (uint) this.needdiamond;
                Debugger.Log(string.Concat(new object[] { "Send DiamondToCoin Request ", storey.diamondToCoin.m_nCoins, " transid ", storey.diamondToCoin.m_nTransID }));
                NetManager.SendInternal<CDiamondToCoin>(storey.diamondToCoin, SendType.eForceOnce, new Action<NetResponse>(storey.<>m__0));
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
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_GoldBuy);
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
        GameLogic.Hold.Sound.PlayUI(SoundUIType.ePopUI);
        this.InitUI();
    }

    public static void SetSource(CoinExchangeSource source)
    {
        mSource = source;
    }

    [CompilerGenerated]
    private sealed class <InitUI>c__AnonStorey0
    {
        internal CDiamondToCoin diamondToCoin;
        internal GoldBuyUICtrl $this;

        internal void <>m__0(NetResponse response)
        {
            if ((response.IsSuccess && (response.data != null)) && (response.data is CRespDimaonToCoin))
            {
                SdkManager.send_event_exchange(GoldBuyUICtrl.GetSource(), (int) this.diamondToCoin.m_nCoins, (int) this.$this.needdiamond);
                CRespDimaonToCoin data = response.data as CRespDimaonToCoin;
                LocalSave.Instance.UserInfo_SetDiamond((int) data.m_nDiamonds);
                LocalSave.Instance.UserInfo_SetGold((int) data.m_nCoins);
                if (this.$this.mTransfer.callback != null)
                {
                    this.$this.mTransfer.callback((int) this.$this.needdiamond);
                }
                this.$this.Button_Close.onClick();
            }
            else if (response.error != null)
            {
                CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 2);
            }
        }
    }
}

