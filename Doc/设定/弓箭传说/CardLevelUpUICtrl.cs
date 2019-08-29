using DG.Tweening;
using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class CardLevelUpUICtrl : MediatorCtrlBase
{
    public Text Text_CardName;
    public TapToCloseCtrl mCloseCtrl;
    public Transform CardParent;
    public Transform AttributeParent;
    private CardOneCtrl _cardctrl;
    private List<CardLevelUpAttCtrl> mAttList = new List<CardLevelUpAttCtrl>();
    private List<CardLevelUpAtt2Ctrl> mAtt2List = new List<CardLevelUpAtt2Ctrl>();
    private LocalSave.CardOne mData;
    private Action onEventClose;
    [CompilerGenerated]
    private static Action<NetResponse> <>f__am$cache0;

    private CardLevelUpAtt2Ctrl GetAtt2One(int index)
    {
        if (this.mAtt2List.Count > index)
        {
            this.mAtt2List[index].gameObject.SetActive(true);
            return this.mAtt2List[index];
        }
        GameObject obj2 = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("UIPanel/CardLevelUpUI/AttributeUnLock"));
        Transform transform = obj2.transform;
        transform.SetParent(this.AttributeParent);
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        this.mAtt2List.Add(obj2.GetComponent<CardLevelUpAtt2Ctrl>());
        return this.mAtt2List[this.mAtt2List.Count - 1];
    }

    private CardLevelUpAttCtrl GetAttOne(int index)
    {
        if (this.mAttList.Count > index)
        {
            this.mAttList[index].gameObject.SetActive(true);
            return this.mAttList[index];
        }
        GameObject obj2 = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("UIPanel/CardLevelUpUI/AttributeOne"));
        Transform transform = obj2.transform;
        transform.SetParent(this.AttributeParent);
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        this.mAttList.Add(obj2.GetComponent<CardLevelUpAttCtrl>());
        return this.mAttList[this.mAttList.Count - 1];
    }

    private void OnClickClose()
    {
        if (this.onEventClose != null)
        {
            this.onEventClose();
        }
        WindowUI.CloseWindow(WindowID.WindowID_CardLevelUp);
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
    }

    public override void OnLanguageChange()
    {
    }

    protected override void OnOpen()
    {
        IProxy proxy = Facade.Instance.RetrieveProxy("CardLevelUpProxy");
        if (proxy != null)
        {
            this.onEventClose = proxy.Event_Para0;
            this.mData = proxy.Data as LocalSave.CardOne;
            object[] args = new object[] { this.mData.CardID };
            this.Text_CardName.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("宝物名称{0}", args), Array.Empty<object>());
            this.Text_CardName.transform.localPosition = Vector3.zero;
            this.mCloseCtrl.OnClose = new Action(this.OnClickClose);
            if ((this.mData.CardID == LocalSave.Instance.Card_GetHarvestID()) && (LocalSave.Instance.Card_GetHarvestLevel() == 1))
            {
                LocalSave.Instance.mHarvest.Unlock();
                CReqItemPacket itemPacket = NetManager.GetItemPacket(null, false);
                itemPacket.m_nPacketType = 0x11;
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = delegate (NetResponse response) {
                        if (response.IsSuccess)
                        {
                            LocalSave.Instance.mHarvest.init_last_time(Utils.GetTimeStamp());
                            SdkManager.send_event_harvest("unlock", string.Empty, string.Empty, 0, 0);
                        }
                    };
                }
                NetManager.SendInternal<CReqItemPacket>(itemPacket, SendType.eCache, <>f__am$cache0);
            }
            this.mCardCtrl.InitCard(this.mData);
            this.mCardCtrl.SetNameShow(false);
            this.mCardCtrl.transform.localPosition = Vector3.zero;
            this.mCardCtrl.SetAlpha(0f);
            this.UpdateUI();
        }
    }

    private Sequence UpdateAttribute()
    {
        Sequence sequence = DOTween.Sequence();
        if (this.mData.level == 1)
        {
            int num = this.mData.data.BaseAttributes.Length;
            for (int k = 0; k < num; k++)
            {
                CardLevelUpAtt2Ctrl ctrl = this.GetAtt2One(k);
                ctrl.transform.localPosition = new Vector3(0f, (float) (k * -150), 0f);
                ctrl.transform.localScale = Vector3.zero;
                ctrl.UpdateUI(this.mData, k);
                TweenSettingsExtensions.Append(sequence, ctrl.GetTweener());
            }
            for (int m = num; m < this.mAtt2List.Count; m++)
            {
                this.mAtt2List[m].gameObject.SetActive(false);
            }
            return sequence;
        }
        int length = this.mData.data.BaseAttributes.Length;
        for (int i = 0; i < length; i++)
        {
            CardLevelUpAttCtrl attOne = this.GetAttOne(i);
            attOne.transform.localPosition = new Vector3(0f, (float) (i * -150), 0f);
            attOne.transform.localScale = Vector3.zero;
            attOne.UpdateUI(this.mData, i);
            TweenSettingsExtensions.Append(sequence, attOne.GetTweener());
        }
        for (int j = length; j < this.mAttList.Count; j++)
        {
            this.mAttList[j].gameObject.SetActive(false);
        }
        return sequence;
    }

    private void UpdateUI()
    {
        this.mCloseCtrl.Show(false);
        for (int i = 0; i < this.mAttList.Count; i++)
        {
            this.mAttList[i].gameObject.SetActive(false);
        }
        for (int j = 0; j < this.mAtt2List.Count; j++)
        {
            this.mAtt2List[j].gameObject.SetActive(false);
        }
        Sequence sequence = DOTween.Sequence();
        Tweener tweener = ShortcutExtensions.DOLocalMoveY(this.Text_CardName.transform, 100f, 0.3f, false);
        TweenSettingsExtensions.Append(sequence, tweener);
        TweenSettingsExtensions.Append(sequence, this.mCardCtrl.PlayCanvas(0f, 1f, 0.3f));
        TweenSettingsExtensions.Join(sequence, ShortcutExtensions.DOLocalMoveY(this.mCardCtrl.transform, 100f, 0.3f, false));
        TweenSettingsExtensions.Append(sequence, this.UpdateAttribute());
        TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this, this.<UpdateUI>m__1));
    }

    private CardOneCtrl mCardCtrl
    {
        get
        {
            if (this._cardctrl == null)
            {
                GameObject obj2 = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("UIPanel/CardUI/CardOne"));
                this._cardctrl = obj2.GetComponent<CardOneCtrl>();
                Transform transform = obj2.transform;
                transform.SetParent(this.CardParent);
                transform.localPosition = Vector3.zero;
                transform.localScale = Vector3.one;
                transform.localRotation = Quaternion.identity;
            }
            return this._cardctrl;
        }
    }
}

