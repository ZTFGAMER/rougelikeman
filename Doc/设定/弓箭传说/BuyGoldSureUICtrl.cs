using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine.UI;

public class BuyGoldSureUICtrl : MediatorCtrlBase
{
    public Text Text_Title;
    public GoldTextCtrl mGoldCtrl;
    public Image Image_Icon;
    public Text Text_Count;
    public ButtonCtrl Button_Sure;
    public ButtonCtrl Button_Refuse;
    public ButtonCtrl Button_Shadow;
    private BuyGoldSureProxy.Transfer mTransfer;
    private Shop_Shop shopdata;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    private void InitUI()
    {
        object[] args = new object[] { this.mTransfer.index + 1 };
        this.Image_Icon.set_sprite(SpriteManager.GetMain(Utils.FormatString("ic_coin_{0}", args)));
        this.shopdata = LocalModelManager.Instance.Shop_Shop.GetBeanById(0x65 + this.mTransfer.index);
        this.Text_Count.text = this.mTransfer.item.GetGold().ToString();
        this.mGoldCtrl.SetCurrencyType(CurrencyType.Diamond);
        this.mGoldCtrl.SetValue(this.shopdata.Price);
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
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_BuyGoldSure);
        }
        this.Button_Refuse.onClick = <>f__am$cache0;
        this.Button_Shadow.onClick = this.Button_Refuse.onClick;
        this.Button_Sure.onClick = delegate {
            this.Button_Refuse.onClick();
            if ((this.mTransfer != null) && (this.mTransfer.callback != null))
            {
                this.mTransfer.callback(this.mTransfer.index, this.mTransfer.item);
            }
        };
    }

    public override void OnLanguageChange()
    {
        object[] args = new object[] { this.shopdata.ID };
        string languageByTID = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("商店_金币{0}", args), Array.Empty<object>());
        object[] objArray2 = new object[] { languageByTID };
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("buygoldui_title", objArray2);
    }

    protected override void OnOpen()
    {
        IProxy proxy = Facade.Instance.RetrieveProxy("BuyGoldSureProxy");
        if (proxy == null)
        {
            SdkManager.Bugly_Report("BuyGoldSureUICtrl", "OnOpen BuyGoldSureProxy is null");
        }
        else if (proxy.Data == null)
        {
            SdkManager.Bugly_Report("BuyGoldSureUICtrl", "OnOpen BuyGoldSureProxy.Data is null");
        }
        else
        {
            this.mTransfer = proxy.Data as BuyGoldSureProxy.Transfer;
            if (this.mTransfer == null)
            {
                SdkManager.Bugly_Report("BuyGoldSureUICtrl", "OnOpen BuyGoldSureProxy.Data is null");
            }
            else
            {
                this.InitUI();
            }
        }
    }
}

