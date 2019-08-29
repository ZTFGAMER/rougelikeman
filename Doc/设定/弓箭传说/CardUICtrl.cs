using DG.Tweening;
using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class CardUICtrl : MediatorCtrlBase
{
    private const int LineCount = 3;
    private const int CardWidth = 210;
    private const int CardHeight = 210;
    public GameObject window;
    public Text Text_Title;
    public Text Text_Content;
    public Text Text_Count;
    public ButtonGoldCtrl Button_Upgrade;
    public ButtonCtrl Button_BG;
    public RectTransform cardstitle;
    public Transform cardparent;
    public RectTransform randomobj;
    public CardInfoCtrl mInfoCtrl;
    public CardUpgradeCtrl mUpgradeCtrl;
    private List<LocalSave.CardOne> cards;
    private List<CardOneCtrl> mCardList = new List<CardOneCtrl>();
    private LocalUnityObjctPool mPool;
    private Sequence s;
    private Sequence s_random;
    private int gold;
    private GameObject _carditem;
    private float cardparenty;
    private bool bInitOver;
    private const int SpeedDownCount = 20;
    private int lastrandomindex = -1;
    private int currentcount;
    private int currentrandomid = -1;
    private AnimationCurve curve;
    private LocalSave.CardOne randomcard;
    private bool bOpened;

    private int GetCardIndex(LocalSave.CardOne one)
    {
        int num = 0;
        int count = this.cards.Count;
        while (num < count)
        {
            if (this.cards[num].CardID == one.CardID)
            {
                return num;
            }
            num++;
        }
        object[] args = new object[] { one.CardID };
        SdkManager.Bugly_Report("CardUICtrl", Utils.FormatString("GetCardIndex[{0}] is not found.", args));
        return 0;
    }

    private void InitUI()
    {
        GameLogic.Hold.Guide.mCard.GoNext(1, this.Button_Upgrade.transform as RectTransform);
        this.OnClickBG();
        this.bInitOver = false;
        this.ResetRandom();
        this.randomcard = null;
        this.randomobj.gameObject.SetActive(false);
        this.mCardList.Clear();
        this.UpdateButton();
        this.mPool.Collect<CardOneCtrl>();
        this.cards = LocalSave.Instance.GetCardsList();
        this.cards.Sort(new Comparison<LocalSave.CardOne>(this.Sort));
        int num = MathDxx.Clamp(this.cards.Count, 0, 0x10);
        TweenExtensions.Kill(this.s, false);
        this.s = DOTween.Sequence();
        for (int i = 0; i < num; i++)
        {
            <InitUI>c__AnonStorey0 storey = new <InitUI>c__AnonStorey0 {
                $this = this,
                index = i
            };
            TweenSettingsExtensions.AppendCallback(this.s, new TweenCallback(storey, this.<>m__0));
            TweenSettingsExtensions.AppendInterval(this.s, 0.02f);
        }
        TweenSettingsExtensions.AppendCallback(this.s, new TweenCallback(this, this.<InitUI>m__0));
    }

    private void OnClickBG()
    {
        this.mInfoCtrl.Show(false);
    }

    private void OnClickCard(CardOneCtrl one)
    {
        this.mInfoCtrl.Show(true);
        this.mInfoCtrl.Init(one);
    }

    private void OnClickUpgrade()
    {
        <OnClickUpgrade>c__AnonStorey1 storey = new <OnClickUpgrade>c__AnonStorey1 {
            $this = this
        };
        this.OnClickBG();
        if (this.bInitOver)
        {
            long num = LocalSave.Instance.GetGold() - this.gold;
            if (num < 0L)
            {
                PurchaseManager.Instance.SetOpenSource(ShopOpenSource.ETALENT);
                WindowUI.ShowGoldBuy(CoinExchangeSource.ETALENT, -num, new Action<int>(this.OnGoldBuyCallback));
            }
            else
            {
                GameLogic.Hold.Guide.mCard.CurrentOver(1);
                WindowUI.ShowMask(true);
                storey.drop = LocalSave.Instance.Card_GetRandomOnly();
                this.currentrandomid = storey.drop.id;
                CReqObtainTreasure packet = new CReqObtainTreasure {
                    m_nTransID = LocalSave.Instance.SaveExtra.GetTransID(),
                    m_nCoin = (uint) this.gold,
                    m_stTreasureItems = new CEquipmentItem()
                };
                packet.m_stTreasureItems.m_nUniqueID = Utils.GenerateUUID();
                packet.m_stTreasureItems.m_nEquipID = (uint) storey.drop.id;
                packet.m_stTreasureItems.m_nLevel = 1;
                packet.m_stTreasureItems.m_nFragment = 1;
                NetManager.SendInternal<CReqObtainTreasure>(packet, SendType.eForceOnce, new Action<NetResponse>(storey.<>m__0));
            }
        }
    }

    protected override void OnClose()
    {
        this.mPool.Collect<CardOneCtrl>();
        this.OnClickBG();
        if (this.s != null)
        {
            TweenExtensions.Kill(this.s, false);
        }
        if (this.s_random != null)
        {
            TweenExtensions.Kill(this.s_random, false);
            this.s_random = null;
        }
        this.bOpened = false;
    }

    public override object OnGetEvent(string eventName) => 
        base.OnGetEvent(eventName);

    private void OnGoldBuyCallback(int diamond)
    {
        this.OnClickUpgrade();
    }

    public override void OnHandleNotification(INotification notification)
    {
        string name = notification.Name;
        object body = notification.Body;
        if ((name != null) && (name == "PUB_UI_UPDATE_CURRENCY"))
        {
            this.UpdateButton();
        }
    }

    protected override void OnInit()
    {
        this.curve = LocalModelManager.Instance.Curve_curve.GetCurve(0x186b6);
        this.mPool = LocalUnityObjctPool.Create(base.gameObject);
        this.mPool.CreateCache<CardOneCtrl>(this.carditem);
        this.Button_Upgrade.onClick = new Action(this.OnClickUpgrade);
        this.Button_BG.onClick = new Action(this.OnClickBG);
        this.cardparenty = this.cardparent.localPosition.y;
        this.window.SetActive(false);
    }

    public override void OnLanguageChange()
    {
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("CardUI_Title", Array.Empty<object>());
        this.Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("CardUI_Content", Array.Empty<object>());
        this.mUpgradeCtrl.OnLanguageChange();
    }

    protected override void OnOpen()
    {
        this.bOpened = true;
        if (!this.window.activeSelf)
        {
            this.window.SetActive(true);
        }
        float fringeHeight = PlatformHelper.GetFringeHeight();
        this.cardstitle.localPosition = new Vector3(0f, fringeHeight, 0f);
        this.cardparent.localPosition = new Vector3(0f, this.cardparenty + fringeHeight, 0f);
        this.InitUI();
    }

    private void PlayRandom()
    {
        this.s_random = DOTween.Sequence();
        float num = 0f;
        int num2 = 8;
        if (this.currentcount < (20 - num2))
        {
            num = 0.07f;
        }
        else
        {
            num = (this.curve.Evaluate(((float) (this.currentcount - (20 - num2))) / ((float) num2)) * 0.2f) + 0.07f;
            num = MathDxx.Clamp(num, 0.07f, num);
        }
        TweenSettingsExtensions.AppendInterval(this.s_random, num);
        TweenSettingsExtensions.AppendCallback(this.s_random, new TweenCallback(this, this.<PlayRandom>m__1));
    }

    private void ResetRandom()
    {
        this.currentcount = 0;
        this.lastrandomindex = -1;
    }

    private void SetRandomPosition()
    {
        int num = GameLogic.Random(0, this.cards.Count);
        while (num == this.lastrandomindex)
        {
            num = GameLogic.Random(0, this.cards.Count);
        }
        this.randomobj.anchoredPosition = (this.mCardList[num].transform as RectTransform).anchoredPosition;
        GameLogic.Hold.Sound.PlayUI(0xf4245);
    }

    private int Sort(LocalSave.CardOne a, LocalSave.CardOne b)
    {
        if (a.CardID < b.CardID)
        {
            return -1;
        }
        return 1;
    }

    private void StartPlayRandom()
    {
        this.randomobj.gameObject.SetActive(true);
        this.randomobj.SetAsLastSibling();
        this.SetRandomPosition();
        this.PlayRandom();
    }

    private void UpdateButton()
    {
        this.mUpgradeCtrl.UpdateUpgrade();
        this.gold = LocalSave.Instance.Card_GetRandomGold();
    }

    private void UpdateOne(int index)
    {
        CardOneCtrl item = this.mPool.DeQueue<CardOneCtrl>();
        LocalSave.CardOne carddata = this.cards[index];
        item.InitCard(carddata);
        item.SetButtonEnable(true);
        item.Event_Click = new Action<CardOneCtrl>(this.OnClickCard);
        item.gameObject.SetParentNormal(this.cardparent);
        RectTransform transform = item.transform as RectTransform;
        float num = 420f;
        float x = ((index % 3) * 210) - (num / 2f);
        float y = (((-index / 3) * 210) - 105f) + 420f;
        transform.anchoredPosition = new Vector2(x, y);
        this.mCardList.Add(item);
    }

    private GameObject carditem
    {
        get
        {
            if (this._carditem == null)
            {
                GameObject child = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("UIPanel/CardUI/CardOne"));
                this._carditem = child;
                child.SetParentNormal(base.transform);
                child.SetActive(false);
            }
            return this._carditem;
        }
    }

    [CompilerGenerated]
    private sealed class <InitUI>c__AnonStorey0
    {
        internal int index;
        internal CardUICtrl $this;

        internal void <>m__0()
        {
            this.$this.UpdateOne(this.index);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickUpgrade>c__AnonStorey1
    {
        internal Drop_DropModel.DropData drop;
        internal CardUICtrl $this;

        internal void <>m__0(NetResponse response)
        {
            if (response.IsSuccess)
            {
                LocalSave.Instance.Modify_Gold((long) -this.$this.gold, true);
                LocalSave.CardOne one = LocalSave.Instance.Card_ReceiveCard(this.drop);
                this.$this.randomcard = LocalSave.Instance.GetCardByID(this.$this.currentrandomid);
                this.$this.StartPlayRandom();
                LocalSave.Instance.GetCardSucceed();
                int num = LocalSave.Instance.Card_GetRandomCount();
                SdkManager.send_event_talent("UPGRADE", this.$this.randomcard.CardID, num, this.$this.gold);
            }
            else
            {
                if (response.error != null)
                {
                    CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 1);
                }
                WindowUI.ShowMask(false);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <PlayRandom>c__AnonStorey2
    {
        internal int index;
        internal CardUICtrl $this;

        internal void <>m__0()
        {
            this.index++;
            this.$this.randomobj.gameObject.SetActive(!this.$this.randomobj.gameObject.activeSelf);
            if (this.index == 6)
            {
                this.$this.s_random = DOTween.Sequence();
                TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(this.$this.s_random, 0.5f), new TweenCallback(this, this.<>m__1));
            }
        }

        internal void <>m__1()
        {
            this.$this.randomobj.gameObject.SetActive(false);
            this.$this.mCardList[this.$this.GetCardIndex(this.$this.randomcard)].InitCard(this.$this.randomcard);
            CardLevelUpProxy proxy = new CardLevelUpProxy(this.$this.randomcard) {
                Event_Para0 = new Action(this.$this.UpdateButton)
            };
            Facade.Instance.RegisterProxy(proxy);
            WindowUI.ShowWindow(WindowID.WindowID_CardLevelUp);
            WindowUI.ShowMask(false);
        }
    }
}

