using Dxx.Net;
using GameProtocol;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class LayerBoxUICtrl : MediatorCtrlBase
{
    private const string Ani_Info_Show = "Info_Show";
    private const string Ani_Info_Hide = "Info_Hide";
    public RectTransform window;
    public Text Text_Title;
    public Text Text_RewardsContent;
    public ButtonCtrl Button_Close;
    public ScrollIntLayerBoxCtrl mScrollInt;
    public Transform mScrollChild;
    public GameObject GoodsParent;
    public GameObject copyBox;
    public GameObject copyReward;
    public Text Text_Condition;
    public Text Text_Rewards;
    public ButtonCtrl Button_Get;
    public Text Text_Get;
    public Text Text_Target;
    public Text Text_Got;
    private int showCount = 10;
    private int count = 40;
    private float allWidth;
    private float itemWidth;
    private float offsetx = 360f;
    private float lastscrollpos;
    private float lastspeed;
    private int mCurrentIndex;
    private List<LayerRewardOneCtrl> mRewards = new List<LayerRewardOneCtrl>();
    private List<Box_ChapterBox> mDataList;
    private LayerBoxOneCtrl mChoose;
    private LocalUnityObjctPool mRewardPool;
    private int currentid;
    private bool bFirst;
    [CompilerGenerated]
    private static Action <>f__am$cache0;
    [CompilerGenerated]
    private static Comparison<Drop_DropModel.DropData> <>f__am$cache1;

    private Vector3 GetRewardPosition(int id)
    {
        int num = 0;
        int count = this.mRewards.Count;
        while (num < count)
        {
            if (this.mRewards[num].id == id)
            {
                return this.mRewards[num].transform.position;
            }
            num++;
        }
        return new Vector3(((float) GameLogic.Width) / 2f, ((float) GameLogic.Height) / 2f, 0f);
    }

    private void InitUI()
    {
        this.UpdateReward();
    }

    private void OnBeginDrag()
    {
    }

    protected override void OnClose()
    {
        WindowUI.CloseCurrency();
        this.mScrollInt.DeInit();
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
        float fringeHeight = PlatformHelper.GetFringeHeight();
        this.window.anchoredPosition = new Vector2(0f, fringeHeight);
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_LayerBox);
        }
        this.Button_Close.onClick = <>f__am$cache0;
        this.mRewardPool = LocalUnityObjctPool.Create(base.gameObject);
        this.mRewardPool.CreateCache<LayerRewardOneCtrl>(this.copyReward);
        this.mScrollInt.copyItem = this.copyBox;
        this.mScrollInt.mScrollChild = this.mScrollChild;
        this.mScrollInt.OnUpdateOne = new Action<int, LayerBoxOneCtrl>(this.UpdateOne);
        this.mScrollInt.OnUpdateSize = new Action<int, LayerBoxOneCtrl>(this.UpdateSize);
        this.mScrollInt.OnBeginDragEvent = new Action(this.OnBeginDrag);
        this.mScrollInt.OnScrollEnd = new Action<int, LayerBoxOneCtrl>(this.OnScrollEnd);
        this.copyBox.SetActive(false);
        this.copyReward.SetActive(false);
        this.Button_Get.onClick = () => this.SendLayer(this.currentid);
        this.mDataList = LocalModelManager.Instance.Box_ChapterBox.GetCurrentList();
        this.mScrollInt.DragDisableForce = true;
    }

    public override void OnLanguageChange()
    {
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("LayerBox_Title", Array.Empty<object>());
        this.Text_RewardsContent.text = GameLogic.Hold.Language.GetLanguageByTID("LayerBox_Rewards", Array.Empty<object>());
        this.Text_Got.text = GameLogic.Hold.Language.GetLanguageByTID("LayerBox_Got", Array.Empty<object>());
        this.Text_Get.text = GameLogic.Hold.Language.GetLanguageByTID("LayerBox_Get", Array.Empty<object>());
    }

    protected override void OnOpen()
    {
        WindowUI.ShowCurrency(WindowID.WindowID_Currency);
        this.bFirst = true;
        this.mScrollInt.Init(this.mDataList.Count);
        this.InitUI();
    }

    private void OnScrollEnd(int index, LayerBoxOneCtrl one)
    {
        this.mChoose = one;
        this.UpdateUI();
    }

    private void PlayRewards(List<Drop_DropModel.DropData> list)
    {
        <PlayRewards>c__AnonStorey1 storey = new <PlayRewards>c__AnonStorey1 {
            $this = this
        };
        this.mRewardPool.Collect<LayerRewardOneCtrl>();
        this.Button_Get.gameObject.SetActive(false);
        this.Text_Rewards.gameObject.SetActive(false);
        storey.is_fly = false;
        bool flag = false;
        int num = 0;
        int count = list.Count;
        while (num < count)
        {
            Drop_DropModel.DropData data = list[num];
            if (data.can_fly)
            {
                flag = true;
                CurrencyFlyCtrl.PlayGet((CurrencyType) data.id, (long) data.count, this.GetRewardPosition(data.id), null, new Action(storey.<>m__0), true);
            }
            num++;
        }
        if (!flag)
        {
            this.UpdateReward();
        }
        this.mRewards.Clear();
    }

    private void SendLayer(int id)
    {
        <SendLayer>c__AnonStorey0 storey = new <SendLayer>c__AnonStorey0 {
            $this = this,
            list = LocalModelManager.Instance.Box_ChapterBox.GetDrops(id)
        };
        storey.itemPacket = NetManager.GetItemPacket(storey.list, false);
        storey.itemPacket.m_nPacketType = 4;
        storey.itemPacket.m_nExtraInfo = (ushort) id;
        NetManager.SendInternal<CReqItemPacket>(storey.itemPacket, SendType.eForceOnce, new Action<NetResponse>(storey.<>m__0));
    }

    private void UpdateOne(int index, LayerBoxOneCtrl one)
    {
        one.Init(this.mDataList[index]);
        if ((index == 0) && (this.mChoose == null))
        {
            this.mChoose = one;
        }
    }

    private void UpdateReward()
    {
        this.Text_Rewards.gameObject.SetActive(true);
        this.currentid = LocalSave.Instance.Stage_GetNextID();
        int nextLevel = LocalModelManager.Instance.Box_ChapterBox.GetNextLevel(this.currentid);
        string stageLayer = GameLogic.Hold.Language.GetStageLayer(nextLevel);
        object[] args = new object[] { stageLayer };
        this.Text_Condition.text = GameLogic.Hold.Language.GetLanguageByTID("Main_StageCount", args);
        this.Text_Got.gameObject.SetActive(false);
        int currentid = this.currentid;
        if (this.currentid > this.mDataList.Count)
        {
            this.currentid = this.mDataList.Count;
        }
        this.mRewardPool.Collect<LayerRewardOneCtrl>();
        List<Drop_DropModel.DropData> drops = LocalModelManager.Instance.Box_ChapterBox.GetDrops(this.currentid);
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = delegate (Drop_DropModel.DropData a, Drop_DropModel.DropData b) {
                if (a.id != 3)
                {
                    if (b.id == 3)
                    {
                        return 1;
                    }
                    if (a.id == 1)
                    {
                        return -1;
                    }
                    if (b.id == 1)
                    {
                        return 1;
                    }
                }
                return -1;
            };
        }
        drops.Sort(<>f__am$cache1);
        this.mRewards.Clear();
        float num3 = 160f;
        float num4 = (((float) -(drops.Count - 1)) / 2f) * num3;
        int num5 = 0;
        int count = drops.Count;
        while (num5 < count)
        {
            LayerRewardOneCtrl item = this.mRewardPool.DeQueue<LayerRewardOneCtrl>();
            Drop_DropModel.DropData data = drops[num5];
            item.Init(data.id, data.count);
            RectTransform child = item.transform as RectTransform;
            child.SetParentNormal(this.GoodsParent);
            child.anchoredPosition = new Vector2(num4 + (num5 * num3), 0f);
            this.mRewards.Add(item);
            num5++;
        }
        int maxLevel = LocalSave.Instance.mStage.MaxLevel;
        int num8 = LocalModelManager.Instance.Box_ChapterBox.GetNextLevel(this.currentid);
        string str2 = GameLogic.Hold.Language.GetStageLayer(num8);
        object[] objArray2 = new object[] { str2 };
        this.Text_Target.text = GameLogic.Hold.Language.GetLanguageByTID("LayerBox_Target", objArray2);
        bool flag = maxLevel >= num8;
        this.Text_Target.gameObject.SetActive(!flag);
        this.Button_Get.gameObject.SetActive(flag);
        if (!this.bFirst)
        {
            this.mScrollInt.GotoInt(this.currentid - 1, true);
        }
        else
        {
            this.mScrollInt.GotoInt(this.currentid - 1, false);
        }
        if (currentid > this.mDataList.Count)
        {
            this.mRewardPool.Collect<LayerRewardOneCtrl>();
            this.mRewards.Clear();
            this.mScrollInt.GotoInt(this.mDataList.Count - 1, false);
            this.Text_Target.gameObject.SetActive(false);
            this.Text_Got.gameObject.SetActive(true);
            this.Button_Get.gameObject.SetActive(false);
        }
        this.bFirst = false;
    }

    private void UpdateSize(int index, LayerBoxOneCtrl one)
    {
        Box_ChapterBox box = this.mDataList[index];
    }

    private void UpdateUI()
    {
    }

    [CompilerGenerated]
    private sealed class <PlayRewards>c__AnonStorey1
    {
        internal bool is_fly;
        internal LayerBoxUICtrl $this;

        internal void <>m__0()
        {
            if (!this.is_fly)
            {
                this.$this.UpdateReward();
                this.is_fly = true;
            }
        }
    }

    [CompilerGenerated]
    private sealed class <SendLayer>c__AnonStorey0
    {
        internal CReqItemPacket itemPacket;
        internal List<Drop_DropModel.DropData> list;
        internal LayerBoxUICtrl $this;

        internal void <>m__0(NetResponse response)
        {
            if (response.IsSuccess)
            {
                if (this.itemPacket.m_nCoinAmount > 0)
                {
                    LocalSave.Instance.Modify_Gold((long) this.itemPacket.m_nCoinAmount, false);
                }
                if (this.itemPacket.m_nDiamondAmount > 0)
                {
                    LocalSave.Instance.Modify_Diamond((long) this.itemPacket.m_nDiamondAmount, false);
                }
                if (this.itemPacket.m_nNormalDiamondItem > 0)
                {
                    LocalSave.Instance.Modify_DiamondExtraCount(LocalSave.TimeBoxType.BoxChoose_DiamondNormal, this.itemPacket.m_nNormalDiamondItem);
                }
                if (this.itemPacket.m_nLargeDiamondItem > 0)
                {
                    LocalSave.Instance.Modify_DiamondExtraCount(LocalSave.TimeBoxType.BoxChoose_DiamondLarge, this.itemPacket.m_nLargeDiamondItem);
                }
                LocalSave.Instance.Stage_GetNextEnd();
                this.$this.PlayRewards(this.list);
                Facade.Instance.SendNotification("MainUI_LayerUpdate");
            }
            else if (response.error != null)
            {
                CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 0);
            }
        }
    }
}

