using DG.Tweening;
using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemDiamondBoxNormal : ShopItemDiamondBoxBase, AdsRequestHelper.AdsCallback
{
    public Image Image_Ad;
    private Box_SilverNormalBox mData;
    private bool bWatchEnd;

    private int get_price(int opencount)
    {
        if (opencount < this.mData.Price1.Length)
        {
            return this.mData.Price1[opencount];
        }
        return this.mData.Price1[this.mData.Price1.Length - 1];
    }

    protected override void OnAwake()
    {
        base.mTransfer.source = EquipSource.EDiamond_box_normal;
        base.mTransfer.boxtype = LocalSave.TimeBoxType.BoxChoose_DiamondNormal;
        base.mTransfer.retry_callback = () => this.onClickButtonInternal(base.mTransfer.count);
    }

    public void onClick(AdsRequestHelper.AdsDriver sender, string networkName)
    {
        AdsRequestHelper.DebugLog("DiamondNormal onClick");
    }

    protected override void OnClickButton()
    {
        LocalSave.Instance.mGuideData.SetIndex(2);
        base.mTransfer.ResetCount();
        this.onClickButtonInternal(base.mTransfer.count);
    }

    private void onClickButtonInternal(int count)
    {
        bool flag = false;
        bool flag2 = false;
        if (LocalSave.Instance.GetDiamondBoxFreeCount(base.mBoxType) > 0)
        {
            flag = true;
            flag2 = true;
        }
        else
        {
            count = MathDxx.Clamp(count, 0, base.mTransfer.diamonds.Length - 1);
            if (base.CheckCanOpen(2, this.get_price(count)))
            {
                flag2 = true;
            }
        }
        if (flag2)
        {
            if (LocalSave.Instance.GetTimeBoxCount(LocalSave.TimeBoxType.BoxChoose_DiamondNormal) > 0)
            {
                SdkManager.send_event_ad(ADSource.eDiamondNormal, "CLICK", 0, 0, string.Empty, string.Empty);
            }
            if (!NetManager.IsNetConnect)
            {
                CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError, Array.Empty<string>());
                if (LocalSave.Instance.GetTimeBoxCount(LocalSave.TimeBoxType.BoxChoose_DiamondNormal) > 0)
                {
                    SdkManager.send_event_ad(ADSource.eDiamondNormal, "IMPRESSION", 0, 0, "FAIL", "NET_ERROR");
                }
            }
            else if (LocalSave.Instance.GetTimeBoxCount(LocalSave.TimeBoxType.BoxChoose_DiamondNormal) > 0)
            {
                if (LocalSave.Instance.IsAdFree())
                {
                    this.send_get_box();
                }
                else if (!AdsRequestHelper.getRewardedAdapter().isLoaded())
                {
                    SdkManager.send_event_ad(ADSource.eDiamondNormal, "IMPRESSION", 0, 0, "FAIL", "AD_NOT_READY");
                    WindowUI.ShowAdInsideUI(AdInsideProxy.EnterSource.eGameTurn, delegate {
                        SdkManager.send_event_ad(ADSource.eDiamondNormal, "REWARD", 0, 0, "INSIDE", string.Empty);
                        this.send_get_box();
                    });
                }
                else
                {
                    AdsRequestHelper.getRewardedAdapter().Show(this);
                    SdkManager.send_event_ad(ADSource.eDiamondNormal, "IMPRESSION", 0, 0, "SUCCESS", string.Empty);
                }
            }
            else
            {
                this.send_get_box();
            }
        }
    }

    public void onClose(AdsRequestHelper.AdsDriver sender, string networkName)
    {
        AdsRequestHelper.DebugLog("DiamondNormal onClose");
        TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.1f), new TweenCallback(this, this.<onClose>m__2));
    }

    protected override void OnDeinit()
    {
        AdsRequestHelper.getRewardedAdapter().RemoveCallback(this);
    }

    public void onFail(AdsRequestHelper.AdsDriver sender, string msg)
    {
        AdsRequestHelper.DebugLog("DiamondNormal onFail");
    }

    protected override void OnInit()
    {
        this.bWatchEnd = false;
        LocalSave.Instance.mGuideData.CheckDiamondBox(base.NotFreeParent.transform as RectTransform, 1);
        this.mData = LocalModelManager.Instance.Box_SilverNormalBox.GetBeanById(LocalSave.Instance.Stage_GetStage());
        base.mTransfer.diamonds = this.mData.Price1;
        base.PerTime = this.mData.Time * 60;
        base.mGoldCtrl.SetValue(this.get_price(0));
        base.FreeShow(false);
        base.UpdateBox();
        if (LocalSave.Instance.IsAdFree())
        {
            this.Image_Ad.enabled = false;
            base.Text_Free.get_rectTransform().anchoredPosition = new Vector2(0f, base.Text_Free.get_rectTransform().anchoredPosition.y);
        }
        else
        {
            this.Image_Ad.enabled = true;
            base.Text_Free.get_rectTransform().anchoredPosition = new Vector2(base.Text_FreeX, base.Text_Free.get_rectTransform().anchoredPosition.y);
        }
        AdsRequestHelper.getRewardedAdapter().RemoveCallback(this);
        AdsRequestHelper.getRewardedAdapter().AddCallback(this);
    }

    protected override void OnLanguageChange()
    {
        base.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("shopui_diamondbox_title_normal", Array.Empty<object>());
        base.Text_BoxContent.text = GameLogic.Hold.Language.GetLanguageByTID("shopui_diamondbox_content_normal", Array.Empty<object>());
        base.Text_Free.text = GameLogic.Hold.Language.GetLanguageByTID("商店_免费抽取", Array.Empty<object>());
        base.Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("商店_抽一次", Array.Empty<object>());
    }

    public void onLoad(AdsRequestHelper.AdsDriver sender, string networkName)
    {
        AdsRequestHelper.DebugLog("DiamondNormal onLoad");
    }

    public void onOpen(AdsRequestHelper.AdsDriver sender, string networkName)
    {
        AdsRequestHelper.DebugLog("DiamondNormal onOpen");
    }

    public void onRequest(AdsRequestHelper.AdsDriver sender, string networkName)
    {
        AdsRequestHelper.DebugLog("DiamondNormal onRequest");
    }

    public void onReward(AdsRequestHelper.AdsDriver sender, string networkName)
    {
        this.bWatchEnd = true;
        SdkManager.send_event_ad(ADSource.eDiamondNormal, "REWARD", 0, 0, "BUSINESS", string.Empty);
        this.send_get_box();
    }

    private void send_get_box()
    {
        <send_get_box>c__AnonStorey0 storey = new <send_get_box>c__AnonStorey0 {
            $this = this,
            free = false
        };
        if (LocalSave.Instance.GetDiamondBoxFreeCount(base.mBoxType) > 0)
        {
            storey.free = true;
        }
        storey.list = LocalModelManager.Instance.Drop_Drop.GetDiamondBoxNormal();
        CReqItemPacket itemPacket = NetManager.GetItemPacket(storey.list, false);
        itemPacket.m_nPacketType = 2;
        storey.diamondup = this.get_price(base.mTransfer.count);
        itemPacket.m_nDiamondAmount = (uint) storey.diamondup;
        NetManager.SendInternal<CReqItemPacket>(itemPacket, SendType.eForceOnce, new Action<NetResponse>(storey.<>m__0));
    }

    [CompilerGenerated]
    private sealed class <send_get_box>c__AnonStorey0
    {
        internal List<Drop_DropModel.DropData> list;
        internal bool free;
        internal int diamondup;
        internal ShopItemDiamondBoxNormal $this;

        internal void <>m__0(NetResponse response)
        {
            if (response.IsSuccess)
            {
                if (LocalSave.Instance.GetTimeBoxCount(this.$this.mBoxType) > 0)
                {
                    LocalSave.Instance.Modify_TimeBoxCount(this.$this.mBoxType, -1, false);
                    this.$this.UpdateBox();
                }
                else if (LocalSave.Instance.GetDiamondExtraCount(this.$this.mBoxType) > 0)
                {
                    LocalSave.Instance.Modify_DiamondExtraCount(this.$this.mBoxType, -1);
                }
                this.$this.mTransfer.data = this.list[0];
                if (!this.free)
                {
                    this.$this.mTransfer.AddCount();
                }
                this.$this.update_red();
                Facade.Instance.RegisterProxy(new BoxOpenSingleProxy(this.$this.mTransfer));
                WindowUI.CloseWindow(WindowID.WindowID_BoxOpenSingle);
                WindowUI.ShowWindow(WindowID.WindowID_BoxOpenSingle);
                string purchase = !this.free ? "normalpurchasegems" : "normalpurchasefree";
                int gems = !this.free ? this.diamondup : 0;
                LocalSave.Instance.Modify_Diamond((long) -gems, true);
                SdkManager.send_event_shop(purchase, 0, gems, this.$this.mTransfer.data.id, this.$this.mTransfer.count);
            }
            else if (response.error != null)
            {
                CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 2);
            }
        }
    }
}

