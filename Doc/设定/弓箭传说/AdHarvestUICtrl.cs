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

public class AdHarvestUICtrl : MediatorCtrlBase
{
    public UILineCtrl mTitleCtrl;
    public Text Text_Gold;
    public Text Text_EquipExp;
    public ButtonCtrl Button_Close;
    public ButtonCtrl Button_Shadow;
    public UILineCtrl mUILineCtrl;
    public Text Text_reward1;
    public Text Text_reward2;
    public ButtonCtrl Button_Harvest;
    public ScrollRectBase mScrollRect;
    private const int LineCount = 6;
    private const float WidthOne = 140f;
    private const float HeightOne = 140f;
    private AdHarvestBattleCtrl _battlectrl;
    private GameObject _harvestitem;
    private LocalUnityObjctPool mPool;
    private SequencePool mSeqPool = new SequencePool();
    private List<Drop_DropModel.DropData> mDataList = new List<Drop_DropModel.DropData>();
    private string adharvest_time;
    private int reward_interval;
    private float scrollwidth;
    private LoadSyncCtrl mLoadCtrl;
    private bool bCanReward;
    private long time;
    private long waittime;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    private void InitUI()
    {
        this.Update();
        this.mSeqPool.Clear();
        this.mPool.Collect<PropOneEquip>();
        LocalSave.Instance.mHarvest.refresh_rewards();
        this.mDataList = LocalSave.Instance.mHarvest.GetList();
        Sequence sequence = this.mSeqPool.Get();
        int count = this.mDataList.Count;
        for (int i = 0; i < count; i++)
        {
            <InitUI>c__AnonStorey1 storey = new <InitUI>c__AnonStorey1 {
                $this = this,
                index = i
            };
            TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(storey, this.<>m__0));
            TweenSettingsExtensions.AppendInterval(sequence, 0.03f);
        }
        int num3 = MathDxx.CeilBig(((float) count) / 6f);
        this.mScrollRect.get_content().sizeDelta = new Vector2(this.mScrollRect.get_content().sizeDelta.x, 140f * num3);
        this.mScrollRect.enabled = count > 12;
    }

    protected override void OnClose()
    {
        if (this.mLoadCtrl != null)
        {
            this.mLoadCtrl.DeInit();
        }
        this.mSeqPool.Clear();
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
        this.reward_interval = 0xe10;
        this.mPool = LocalUnityObjctPool.Create(base.gameObject);
        this.mPool.CreateCache<PropOneEquip>(this.harvestitem);
        this.scrollwidth = (this.mScrollRect.transform.parent as RectTransform).sizeDelta.x;
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_AdHarvest);
        }
        this.Button_Close.onClick = <>f__am$cache0;
        this.Button_Shadow.onClick = this.Button_Close.onClick;
        this.Button_Harvest.onClick = delegate {
            <OnInit>c__AnonStorey0 storey = new <OnInit>c__AnonStorey0 {
                $this = this
            };
            SdkManager.send_event_harvest("click", string.Empty, string.Empty, 0, 0);
            storey.list = LocalSave.Instance.mHarvest.GetList();
            CReqItemPacket itemPacket = NetManager.GetItemPacket(storey.list, false);
            itemPacket.m_nPacketType = 0x11;
            NetManager.SendInternal<CReqItemPacket>(itemPacket, SendType.eForceOnce, new Action<NetResponse>(storey.<>m__0));
        };
        this.OnLanguageChange();
    }

    public override void OnLanguageChange()
    {
        string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("adharvest_minute", Array.Empty<object>());
        int num = LocalSave.Instance.Card_GetHarvestGold();
        object[] args = new object[] { num, languageByTID };
        this.Text_Gold.text = Utils.FormatString("+{0}/{1}", args);
        this.Text_EquipExp.text = GameLogic.Hold.Language.GetLanguageByTID("adharvest_equipexp_drop", Array.Empty<object>());
        this.mTitleCtrl.SetText(GameLogic.Hold.Language.GetLanguageByTID("adharvest_title", Array.Empty<object>()));
        this.adharvest_time = GameLogic.Hold.Language.GetLanguageByTID("adharvest_time", Array.Empty<object>());
        this.Text_reward2.text = GameLogic.Hold.Language.GetLanguageByTID("adharvest_reward2", Array.Empty<object>());
    }

    protected override void OnOpen()
    {
        SdkManager.send_event_harvest("show", string.Empty, string.Empty, 0, 0);
        this.InitUI();
    }

    private void Update()
    {
        this.time = LocalSave.Instance.mHarvest.get_harvest_time();
        this.waittime = this.reward_interval - this.time;
        object[] args = new object[] { this.adharvest_time, Utils.GetSecond3String(this.time) };
        this.mUILineCtrl.SetText(Utils.FormatString("{0} {1}", args));
        this.bCanReward = LocalSave.Instance.mHarvest.get_can_reward();
        this.Button_Harvest.SetEnable(this.bCanReward);
        this.Button_Harvest.enabled = this.bCanReward;
        if (this.bCanReward)
        {
            this.Text_reward1.text = GameLogic.Hold.Language.GetLanguageByTID("adharvest_button", Array.Empty<object>());
            this.Text_reward1.get_rectTransform().anchoredPosition = new Vector2(0f, 4f);
            this.Text_reward1.fontSize = 0x2d;
            this.Text_reward2.enabled = false;
        }
        else
        {
            this.Text_reward2.enabled = true;
            this.Text_reward1.fontSize = 0x23;
            object[] objArray2 = new object[] { Utils.GetSecond3String(this.waittime) };
            this.Text_reward1.text = GameLogic.Hold.Language.GetLanguageByTID("adharvest_reward1", objArray2);
            this.Text_reward2.text = GameLogic.Hold.Language.GetLanguageByTID("adharvest_reward2", Array.Empty<object>());
            this.Text_reward1.get_rectTransform().anchoredPosition = new Vector2(0f, 24f);
            this.Text_reward2.get_rectTransform().anchoredPosition = new Vector2(0f, -16f);
        }
        if (LocalSave.Instance.mHarvest.refresh_rewards())
        {
            this.InitUI();
        }
    }

    private void UpdateTime()
    {
    }

    private AdHarvestBattleCtrl mBattleCtrl
    {
        get
        {
            if (this._battlectrl == null)
            {
                GameObject obj2 = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("UIPanel/AdHarvestUI/harvest_battle"));
                obj2.transform.parent = null;
                obj2.transform.position = Vector3.zero;
                this._battlectrl = obj2.GetComponent<AdHarvestBattleCtrl>();
            }
            return this._battlectrl;
        }
    }

    private GameObject harvestitem
    {
        get
        {
            if (this._harvestitem == null)
            {
                this._harvestitem = CInstance<UIResourceCreator>.Instance.GetPropOneEquip(base.transform).gameObject;
                this._harvestitem.SetActive(false);
            }
            return this._harvestitem;
        }
    }

    [CompilerGenerated]
    private sealed class <InitUI>c__AnonStorey1
    {
        internal int index;
        internal AdHarvestUICtrl $this;

        internal void <>m__0()
        {
            PropOneEquip equip = this.$this.mPool.DeQueue<PropOneEquip>();
            RectTransform t = equip.transform as RectTransform;
            ((Transform) t).SetParentNormal((Transform) this.$this.mScrollRect.get_content());
            t.SetLeftTop();
            equip.InitProp(this.$this.mDataList[this.index]);
            float x = ((this.index % 6) * 140f) + 10f;
            float y = ((-this.index / 6) * 140f) - 10f;
            t.anchoredPosition = new Vector2(x, y);
        }
    }

    [CompilerGenerated]
    private sealed class <OnInit>c__AnonStorey0
    {
        internal List<Drop_DropModel.DropData> list;
        internal AdHarvestUICtrl $this;

        internal void <>m__0(NetResponse response)
        {
            if (response.IsSuccess)
            {
                int coins = 0;
                int gems = 0;
                int num3 = 0;
                int count = this.list.Count;
                while (num3 < count)
                {
                    Drop_DropModel.DropData data = this.list[num3];
                    if (data.type == PropType.eCurrency)
                    {
                        if (data.id == 1)
                        {
                            coins += data.count;
                        }
                    }
                    else if (data.is_equipexp)
                    {
                        gems += data.count;
                    }
                    num3++;
                }
                SdkManager.send_event_harvest("end", "success", string.Empty, coins, gems);
                LocalSave.Instance.mHarvest.Get_to_pack();
                this.$this.Button_Close.onClick();
            }
            else
            {
                SdkManager.send_event_harvest("end", "fail", "server_resp_error", 0, 0);
            }
        }
    }
}

