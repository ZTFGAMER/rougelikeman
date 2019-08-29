using DG.Tweening;
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

public class KeyBuyUICtrl : MediatorCtrlBase, AdsRequestHelper.AdsCallback
{
    private static KeyBuySource mSource;
    public Text Text_Title;
    public Text Text_Content;
    public Text Text_DiamondCount;
    public Text Text_AdCount;
    public Text Text_AdLast;
    public Text Text_AdGet;
    public Image Image_Ad;
    public GameObject freeparent;
    public GameObject notfreeparent;
    public Text Text_NotFreeCount;
    public GoldTextCtrl mAdCtrl;
    public GoldTextCtrl mDiamondCtrl;
    public GoldTextCtrl mNotFreeDiamondCtrl;
    public ButtonCtrl Button_Buy;
    public ButtonCtrl Button_Close;
    public ButtonCtrl Button_Shadow;
    public ButtonCtrl Button_Ad;
    public ButtonCtrl Button_BuyNotFree;
    private float Text_AdGetX;
    private int KeyCount;
    private int adCount;
    private long needdiamond;
    private bool bAdReward;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    public static KeyBuySource GetSource() => 
        mSource;

    private void InitUI()
    {
        this.needdiamond = GameConfig.GetKeyBuyDiamond();
        object[] args = new object[] { this.KeyCount.ToString() };
        this.Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("key_buy_content", args);
        object[] objArray2 = new object[] { this.KeyCount };
        this.Text_DiamondCount.text = Utils.FormatString("x{0}", objArray2);
        object[] objArray3 = new object[] { this.KeyCount };
        this.Text_NotFreeCount.text = Utils.FormatString("x{0}", objArray3);
        object[] objArray4 = new object[] { this.adCount };
        this.Text_AdCount.text = Utils.FormatString("x{0}", objArray4);
        this.update_ad_count();
        this.mDiamondCtrl.SetCurrencyType(CurrencyType.Diamond);
        this.mDiamondCtrl.UseTextRed();
        this.mDiamondCtrl.SetValue((int) this.needdiamond);
        this.mNotFreeDiamondCtrl.SetCurrencyType(CurrencyType.Diamond);
        this.mNotFreeDiamondCtrl.UseTextRed();
        this.mNotFreeDiamondCtrl.SetValue((int) this.needdiamond);
        this.Button_Buy.onClick = delegate {
            if (LocalSave.Instance.GetDiamond() < this.needdiamond)
            {
                WindowUI.ShowShopSingle(ShopSingleProxy.SingleType.eDiamond, null);
            }
            else
            {
                SdkManager.send_event_strength("PURCHASE_CLICK", GetSource(), string.Empty, string.Empty, 0);
                CLifeTransPacket packet = new CLifeTransPacket {
                    m_nTransID = LocalSave.Instance.SaveExtra.GetTransID(),
                    m_nType = 2,
                    m_nMaterial = (ushort) this.needdiamond
                };
                Debugger.Log("buy key needdiamond = " + this.needdiamond);
                NetManager.SendInternal<CLifeTransPacket>(packet, SendType.eForceOnce, delegate (NetResponse response) {
                    if (response.IsSuccess)
                    {
                        LocalSave.Instance.Modify_Diamond(-this.needdiamond, true);
                        CurrencyFlyCtrl.PlayGet(CurrencyType.Key, (long) this.KeyCount, null, null, true);
                        this.Button_Close.onClick();
                        SdkManager.send_event_strength("FINISH", GetSource(), "SUCCESS", string.Empty, (int) this.needdiamond);
                    }
                    else if (response.error != null)
                    {
                        SdkManager.send_event_strength("FINISH", GetSource(), "FAIL", "DIAMOND_NOT_ENOUGH", 0);
                        CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 2);
                    }
                    else
                    {
                        SdkManager.send_event_strength("FINISH", GetSource(), "FAIL", "RESPONSE_ERROR_NULL", 0);
                    }
                });
            }
        };
        this.Button_BuyNotFree.onClick = this.Button_Buy.onClick;
    }

    public void onClick(AdsRequestHelper.AdsDriver sender, string networkName)
    {
        AdsRequestHelper.DebugLog("Key onClick");
    }

    public void onClose(AdsRequestHelper.AdsDriver sender, string networkName)
    {
        AdsRequestHelper.DebugLog("Key onClose");
        TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.1f), new TweenCallback(this, this.<onClose>m__3));
    }

    protected override void OnClose()
    {
        AdsRequestHelper.getRewardedAdapter().RemoveCallback(this);
    }

    public void onFail(AdsRequestHelper.AdsDriver sender, string msg)
    {
        AdsRequestHelper.DebugLog("Key onFail");
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
        this.Text_AdGetX = this.Text_AdGet.get_rectTransform().anchoredPosition.x;
        this.KeyCount = GameConfig.GetMaxKeyCount();
        this.adCount = GameConfig.GetAdKeyCount();
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_KeyBuy);
        }
        this.Button_Close.onClick = <>f__am$cache0;
        this.Button_Ad.SetDepondNet(true);
        this.Button_Ad.onClick = delegate {
            this.bAdReward = false;
            SdkManager.send_event_ad(ADSource.eKey, "CLICK", 0, 0, string.Empty, string.Empty);
            if (LocalSave.Instance.UserInfo_GetAdKeyCount() > 0)
            {
                if (!NetManager.IsNetConnect)
                {
                    CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError, Array.Empty<string>());
                    SdkManager.send_event_ad(ADSource.eKey, "IMPRESSION", 0, 0, "FAIL", "NET_ERROR");
                }
                else if (LocalSave.Instance.IsAdFree())
                {
                    this.onReward(null, null);
                }
                else if (!AdsRequestHelper.getRewardedAdapter().isLoaded())
                {
                    SdkManager.send_event_ad(ADSource.eKey, "IMPRESSION", 0, 0, "FAIL", "AD_NOT_READY");
                    CInstance<TipsUIManager>.Instance.Show(ETips.Tips_AdNotReady, Array.Empty<string>());
                }
                else
                {
                    AdsRequestHelper.getRewardedAdapter().Show(this);
                    SdkManager.send_event_ad(ADSource.eKey, "IMPRESSION", 0, 0, "SUCCESS", string.Empty);
                }
            }
        };
        this.Button_Shadow.onClick = this.Button_Close.onClick;
    }

    public override void OnLanguageChange()
    {
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("key_buy_title", Array.Empty<object>());
    }

    public void onLoad(AdsRequestHelper.AdsDriver sender, string networkName)
    {
        AdsRequestHelper.DebugLog("Key onLoad");
    }

    public void onOpen(AdsRequestHelper.AdsDriver sender, string networkName)
    {
        AdsRequestHelper.DebugLog("Key onOpen");
    }

    protected override void OnOpen()
    {
        AdsRequestHelper.getRewardedAdapter().AddCallback(this);
        SdkManager.send_event_strength("CLICK", GetSource(), string.Empty, string.Empty, 0);
        GameLogic.Hold.Sound.PlayUI(SoundUIType.ePopUI);
        this.InitUI();
    }

    public void onRequest(AdsRequestHelper.AdsDriver sender, string networkName)
    {
        AdsRequestHelper.DebugLog("Key onRequest");
    }

    public void onReward(AdsRequestHelper.AdsDriver sender, string networkName)
    {
        this.bAdReward = true;
        if (LocalSave.Instance.UserInfo_GetAdKeyCount() > 0)
        {
            LocalSave.Instance.UserInfo_UseAdKeyCount();
            this.update_ad_count();
        }
        object[] args = new object[] { this.adCount };
        AdsRequestHelper.DebugLog(Utils.FormatString("Key Reward adCount : {0}", args));
        List<Drop_DropModel.DropData> list = new List<Drop_DropModel.DropData>();
        Drop_DropModel.DropData item = new Drop_DropModel.DropData {
            type = PropType.eCurrency,
            id = 3,
            count = this.adCount
        };
        list.Add(item);
        CReqItemPacket itemPacket = NetManager.GetItemPacket(list, false);
        itemPacket.m_nPacketType = 0x12;
        NetManager.SendInternal<CReqItemPacket>(itemPacket, SendType.eUDP, delegate (NetResponse response) {
            if (response.IsSuccess)
            {
                SdkManager.send_event_ad(ADSource.eKey, "REWARD", 0, 0, string.Empty, string.Empty);
                object[] objArray1 = new object[] { this.adCount };
                AdsRequestHelper.DebugLog(Utils.FormatString("Key Reward adCount : {0} success", objArray1));
            }
        });
        CurrencyFlyCtrl.PlayGet(CurrencyType.Key, (long) this.adCount, null, null, true);
        this.Button_Close.onClick();
    }

    public static void SetSource(KeyBuySource source)
    {
        mSource = source;
    }

    private void update_ad_count()
    {
        string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("key_ad_count", Array.Empty<object>());
        int num = LocalSave.Instance.UserInfo_GetAdKeyCount();
        object[] args = new object[] { languageByTID, num };
        this.Text_AdLast.text = Utils.FormatString("{0}: {1}", args);
        string str2 = GameLogic.Hold.Language.GetLanguageByTID("key_ad_get", Array.Empty<object>());
        if (!LocalSave.Instance.IsAdFree())
        {
            this.Image_Ad.enabled = true;
            this.mAdCtrl.Interval = 10f;
            this.mAdCtrl.SetValue(str2);
        }
        else
        {
            this.Image_Ad.enabled = false;
            this.Text_AdGet.get_rectTransform().anchoredPosition = new Vector2(0f, this.Text_AdGet.get_rectTransform().anchoredPosition.y);
        }
        if (num > 0)
        {
            this.freeparent.SetActive(true);
            this.notfreeparent.SetActive(false);
        }
        else
        {
            this.freeparent.SetActive(false);
            this.notfreeparent.SetActive(true);
        }
    }
}

