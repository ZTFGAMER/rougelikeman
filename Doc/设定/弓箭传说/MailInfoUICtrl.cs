using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class MailInfoUICtrl : MediatorCtrlBase
{
    public ButtonCtrl Button_Close;
    public ButtonCtrl Button_Shadow;
    public Text Text_TitleTitle;
    public Text Text_Title;
    public Text Text_Time;
    public Text Text_Content;
    public GameObject rewardparent;
    public GameObject rewardchildparent;
    public Text Text_RewardContent;
    public ScrollRectBase mScrollRect;
    public RectTransform scrolltransform;
    public ButtonCtrl Button_Get;
    private const float RewardWidth = 130f;
    private const float RewardHeight = 80f;
    private const float OneWidth = 100f;
    private bool bHaveReward = true;
    private List<RewardData> mList = new List<RewardData>();
    private LocalUnityObjctPool mPool;
    private MailInfoProxy.Transfer mTranfer;
    private Vector3 mCoinPos;
    private Vector3 mDiamondPos;

    private void InitGet()
    {
        RewardData data;
        this.mList.Clear();
        if (this.mTranfer.data.m_nDiamond > 0)
        {
            data = new RewardData {
                type = 1,
                id = 2,
                count = this.mTranfer.data.m_nDiamond
            };
            this.mList.Add(data);
        }
        if (this.mTranfer.data.m_nCoins > 0)
        {
            data = new RewardData {
                type = 1,
                id = 1,
                count = this.mTranfer.data.m_nCoins
            };
            this.mList.Add(data);
        }
        this.bHaveReward = (this.mList.Count > 0) && !this.mTranfer.data.IsGot;
        this.rewardparent.SetActive(this.bHaveReward);
        for (int i = 0; i < this.mList.Count; i++)
        {
            int num2 = i;
            RewardData data2 = this.mList[i];
            PropOneEquip equip = this.mPool.DeQueue<PropOneEquip>();
            equip.transform.SetLeft();
            equip.gameObject.SetParentNormal(this.rewardchildparent);
            equip.transform.localScale = Vector3.one * 0.6f;
            equip.transform.localPosition = new Vector3((num2 % 5) * 100f, (num2 / 5) * -100f);
            switch (((PropType) data2.type))
            {
                case PropType.eCurrency:
                    equip.InitCurrency(data2.id, (long) data2.count);
                    if (data2.id == 1)
                    {
                        this.mCoinPos = equip.GetMiddlePosition();
                    }
                    else if (data2.id == 2)
                    {
                        this.mDiamondPos = equip.GetMiddlePosition();
                    }
                    break;

                case PropType.eEquip:
                    equip.InitEquip(data2.id, data2.count);
                    break;
            }
        }
        this.RefreshGot();
    }

    private void InitUI()
    {
        this.Text_Content.text = this.mTranfer.data.m_strContent;
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("邮件详情", Array.Empty<object>());
        this.Text_Time.text = Utils.GetTimeGo((double) this.mTranfer.data.m_i64PubTime);
        this.Text_TitleTitle.text = this.mTranfer.data.m_strTitle;
        this.mScrollRect.get_content().sizeDelta = new Vector2(this.mScrollRect.get_content().sizeDelta.x, this.Text_Content.preferredHeight);
        LocalSave.Instance.Mail.MailReaded(this.mTranfer.data);
        this.InitGet();
    }

    private void OnClickClose()
    {
        WindowUI.CloseWindow(WindowID.WindowID_MailInfo);
    }

    private void OnClickGet()
    {
        Drop_DropModel.DropData data;
        List<Drop_DropModel.DropData> list = new List<Drop_DropModel.DropData>();
        if (this.mTranfer.data.m_nCoins > 0)
        {
            data = new Drop_DropModel.DropData {
                type = PropType.eCurrency,
                id = 1,
                count = this.mTranfer.data.m_nCoins
            };
            list.Add(data);
        }
        if (this.mTranfer.data.m_nDiamond > 0)
        {
            data = new Drop_DropModel.DropData {
                type = PropType.eCurrency,
                id = 2,
                count = this.mTranfer.data.m_nDiamond
            };
            list.Add(data);
        }
        CReqItemPacket itemPacket = NetManager.GetItemPacket(list, false);
        itemPacket.m_nPacketType = 6;
        itemPacket.m_nExtraInfo = this.mTranfer.data.m_nMailID;
        NetManager.SendInternal<CReqItemPacket>(itemPacket, SendType.eForceOnce, delegate (NetResponse response) {
            if (response.IsSuccess)
            {
                LocalSave.Instance.Mail.MailGot(this.mTranfer.data);
                if (this.mTranfer.data.m_nCoins > 0)
                {
                    LocalSave.Instance.Modify_Gold((long) this.mTranfer.data.m_nCoins, false);
                    CurrencyFlyCtrl.PlayGet(CurrencyType.Gold, (long) this.mTranfer.data.m_nCoins, this.mCoinPos, null, null, true);
                }
                if (this.mTranfer.data.m_nDiamond > 0)
                {
                    LocalSave.Instance.Modify_Diamond((long) this.mTranfer.data.m_nDiamond, false);
                    CurrencyFlyCtrl.PlayGet(CurrencyType.Diamond, (long) this.mTranfer.data.m_nDiamond, this.mDiamondPos, null, null, true);
                }
                if (this.mTranfer.ctrl != null)
                {
                    this.mTranfer.ctrl.UpdateMail();
                }
                this.OnClickClose();
            }
            else if (response.error != null)
            {
                CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 1);
            }
        });
    }

    protected override void OnClose()
    {
        if (this.mTranfer.poptype == MailInfoProxy.EMailPopType.eMain)
        {
            LocalSave.Instance.Mail.CheckMainPop();
        }
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
        this.Button_Close.onClick = new Action(this.OnClickClose);
        this.Button_Shadow.onClick = new Action(this.OnClickClose);
        this.Button_Get.onClick = new Action(this.OnClickGet);
        GameObject gameObject = CInstance<UIResourceCreator>.Instance.GetPropOneEquip(base.transform).gameObject;
        gameObject.SetActive(false);
        this.mPool = LocalUnityObjctPool.Create(base.gameObject);
        this.mPool.CreateCache<PropOneEquip>(gameObject);
    }

    public override void OnLanguageChange()
    {
    }

    protected override void OnOpen()
    {
        this.mCoinPos = new Vector3(((float) GameLogic.Width) / 2f, ((float) GameLogic.Height) / 2f, 0f);
        this.mDiamondPos = new Vector3(((float) GameLogic.Width) / 2f, ((float) GameLogic.Height) / 2f, 0f);
        this.mPool.Collect<PropOneEquip>();
        IProxy proxy = Facade.Instance.RetrieveProxy("MailInfoProxy");
        if (proxy == null)
        {
            SdkManager.Bugly_Report("MailInfoUICtrl", "MailInfoProxy is null.");
            this.OnClickClose();
        }
        else if (proxy.Data == null)
        {
            SdkManager.Bugly_Report("MailInfoUICtrl", "MailInfoProxy.Data is null.");
            this.OnClickClose();
        }
        else
        {
            this.mTranfer = proxy.Data as MailInfoProxy.Transfer;
            if (this.mTranfer == null)
            {
                SdkManager.Bugly_Report("MailInfoUICtrl", "MailInfoProxy.Data is not a [MailInfoProxy.Transfer] type.");
                this.OnClickClose();
            }
            else
            {
                this.InitUI();
            }
        }
    }

    private void RefreshGot()
    {
        bool flag = (this.mTranfer.data.IsHaveReward && this.mTranfer.data.IsGot) || !this.mTranfer.data.IsHaveReward;
        this.rewardparent.SetActive(!flag);
        this.Button_Get.gameObject.SetActive(!flag);
    }

    public class RewardData
    {
        public int type;
        public int id;
        public int count;
    }
}

