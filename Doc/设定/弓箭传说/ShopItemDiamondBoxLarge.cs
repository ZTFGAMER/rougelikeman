using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TableTool;

public class ShopItemDiamondBoxLarge : ShopItemDiamondBoxBase
{
    private Box_SilverBox mData;

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
        base.mTransfer.source = EquipSource.EDiamond_box_large;
        base.mTransfer.boxtype = LocalSave.TimeBoxType.BoxChoose_DiamondLarge;
        base.mTransfer.retry_callback = () => this.onClickButtonInternal(base.mTransfer.count);
        base.mBoxType = LocalSave.TimeBoxType.BoxChoose_DiamondLarge;
    }

    protected override void OnClickButton()
    {
        base.mTransfer.ResetCount();
        this.onClickButtonInternal(base.mTransfer.count);
    }

    private void onClickButtonInternal(int count)
    {
        bool flag = false;
        if (LocalSave.Instance.GetDiamondBoxFreeCount(base.mBoxType) > 0)
        {
            flag = true;
        }
        else
        {
            count = MathDxx.Clamp(count, 0, base.mTransfer.diamonds.Length - 1);
            if (base.CheckCanOpen(2, this.get_price(count)))
            {
                flag = true;
            }
        }
        if (flag)
        {
            this.send_get_box();
        }
    }

    protected override void OnInit()
    {
        this.mData = LocalModelManager.Instance.Box_SilverBox.GetBeanById(LocalSave.Instance.Stage_GetStage());
        base.mTransfer.diamonds = this.mData.Price1;
        base.PerTime = this.mData.Time * 60;
        base.mGoldCtrl.SetValue(this.get_price(0));
        base.FreeShow(false);
        base.UpdateBox();
    }

    protected override void OnLanguageChange()
    {
        base.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("shopui_diamondbox_title_large", Array.Empty<object>());
        base.Text_BoxContent.text = GameLogic.Hold.Language.GetLanguageByTID("shopui_diamondbox_content_large", Array.Empty<object>());
        base.Text_Free.text = GameLogic.Hold.Language.GetLanguageByTID("商店_免费抽取", Array.Empty<object>());
        base.Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("商店_抽一次", Array.Empty<object>());
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
        storey.list = LocalModelManager.Instance.Drop_Drop.GetDiamondBoxLarge();
        CReqItemPacket itemPacket = NetManager.GetItemPacket(storey.list, false);
        itemPacket.m_nPacketType = 5;
        storey.diamondup = (ushort) this.get_price(base.mTransfer.count);
        itemPacket.m_nDiamondAmount = storey.diamondup;
        NetManager.SendInternal<CReqItemPacket>(itemPacket, SendType.eForceOnce, new Action<NetResponse>(storey.<>m__0));
    }

    [CompilerGenerated]
    private sealed class <send_get_box>c__AnonStorey0
    {
        internal List<Drop_DropModel.DropData> list;
        internal bool free;
        internal ushort diamondup;
        internal ShopItemDiamondBoxLarge $this;

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
                this.$this.update_red();
                this.$this.mTransfer.data = this.list[0];
                if (!this.free)
                {
                    this.$this.mTransfer.AddCount();
                }
                Facade.Instance.RegisterProxy(new BoxOpenSingleProxy(this.$this.mTransfer));
                WindowUI.CloseWindow(WindowID.WindowID_BoxOpenSingle);
                WindowUI.ShowWindow(WindowID.WindowID_BoxOpenSingle);
                string purchase = !this.free ? "largepurchasegems" : "largepurchasefree";
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

