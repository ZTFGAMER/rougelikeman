using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class MainModuleMediator : WindowMediator, IMediator, INotifier
{
    private static RectTransform mScrollTransform;
    private static ScrollRectBase mScrollRect;
    private static GridLayoutGroup grid;
    private static RectTransform mButtonFront;
    private static PageData[] mPageDatas = new PageData[5];
    private static ButtonCtrl Button_Start;
    private static Transform MiddleTransform;
    private static Image Image_Sound;
    private static BoxRedAniCtrl mBoxCtrl;
    private static ButtonCtrl Button_Setting;
    private static ButtonCtrl Button_Set;
    private static GameObject Obj_Setting;
    private static ButtonCtrl Button_Rate;
    private static MainDownCtrl mDownCtrl;
    private ActionBasic action;
    private bool bSettingShow;
    private int currentPage;
    private float scrollpercentx;
    private bool bOnlyMain;
    private Action OnOnlyMainAction;
    [CompilerGenerated]
    private static TweenCallback <>f__am$cache0;

    public MainModuleMediator() : base("MainUIPanel")
    {
        this.action = new ActionBasic();
        this.currentPage = 2;
    }

    private void EndDragItem(int page)
    {
        this.TouchPage(page);
    }

    public override object GetEvent(string eventName)
    {
        for (int i = 0; i < 5; i++)
        {
            PageData data = mPageDatas[i];
            if ((data != null) && (data.PageCtrl != null))
            {
                object obj2 = mPageDatas[i].PageCtrl.OnGetEvent(eventName);
                if (obj2 != null)
                {
                    return obj2;
                }
            }
        }
        return null;
    }

    private void GoldUpdate(long allgold, long change)
    {
        this.UpdateCardRedCount();
    }

    private void Guide()
    {
        WindowUI.ShowWindow(WindowID.WindowID_Battle);
    }

    private void MiddleShow(bool show)
    {
        Button_Start.gameObject.SetActive(show);
    }

    private void OnButtonClick()
    {
    }

    public override void OnHandleNotification(INotification notification)
    {
        <OnHandleNotification>c__AnonStorey1 storey = new <OnHandleNotification>c__AnonStorey1 {
            $this = this
        };
        string name = notification.Name;
        storey.vo = notification.Body;
        if (name != null)
        {
            if (name == "MainUI_UpdatePage")
            {
                this.update_page();
            }
            else if (name == "MainUI_GotoShop")
            {
                if (this.currentPage != 0)
                {
                    this.TouchPage(0);
                }
            }
            else if (name == "MainUI_ShopRedCountUpdate")
            {
                this.UpdateShopRedCount();
            }
            else if (name == "MainUI_EquipRedCountUpdate")
            {
                this.UpdateEquipRedCount();
            }
            else if (name == "MainUI_CardRedCountUpdate")
            {
                this.UpdateCardRedCount();
            }
            else if (name == "MainUI_GetGold")
            {
                if (this.bOnlyMain)
                {
                    this.PlayGetGold(storey.vo);
                }
                else
                {
                    this.OnOnlyMainAction = (Action) Delegate.Combine(this.OnOnlyMainAction, new Action(storey.<>m__0));
                }
            }
        }
        for (int i = 0; i < 5; i++)
        {
            PageData data = mPageDatas[i];
            if ((data != null) && (data.PageCtrl != null))
            {
                mPageDatas[i].PageCtrl.HandleNotification(notification);
            }
        }
    }

    protected override void OnLanguageChange()
    {
        for (int i = 0; i < 5; i++)
        {
            PageData data = mPageDatas[i];
            if ((data != null) && (data.PageCtrl != null))
            {
                mPageDatas[i].PageCtrl.OnLanguageChange();
            }
        }
        mDownCtrl.OnLanguageChange();
    }

    private void OnOnlyMain()
    {
        bool flag = false;
        if (!flag && LocalSave.Instance.mGuideData.CheckDiamondBox(mPageDatas[0].buttonctrl.transform as RectTransform, 0))
        {
        }
        if (this.OnOnlyMainAction != null)
        {
            this.OnOnlyMainAction();
            this.OnOnlyMainAction = null;
        }
    }

    protected override void OnRegisterEvery()
    {
        SdkManager.send_event_page_show(WindowID.WindowID_Main, "SHOW");
        ApplicationEvent.OnOnlyMain += new Action(this.OnOnlyMain);
        this.OnOnlyMainAction = null;
        this.bOnlyMain = true;
        GameLogic.ResetMaxResolution();
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = new TweenCallback(null, <OnRegisterEvery>m__0);
        }
        TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.3f), <>f__am$cache0);
        WindowUI.ShowCurrency(WindowID.WindowID_Currency);
        GameLogic.SetInGame(false);
        LocalSave.GoldUpdateEvent = (Action<long, long>) Delegate.Combine(LocalSave.GoldUpdateEvent, new Action<long, long>(this.GoldUpdate));
        LocalSave.CardUpdateEvent = (Action) Delegate.Combine(LocalSave.CardUpdateEvent, new Action(this.UpdateCardRedCount));
        for (int i = 0; i < 5; i++)
        {
            <OnRegisterEvery>c__AnonStorey0 storey = new <OnRegisterEvery>c__AnonStorey0 {
                $this = this,
                index = i
            };
            mPageDatas[i].AddButtonClick(new Action(storey.<>m__0));
        }
        GameLogic.Hold.Guide.mEquip.GoNext(0, mPageDatas[1].buttonctrl.transform as RectTransform);
        GameLogic.Hold.Guide.mCard.GoNext(0, mPageDatas[3].buttonctrl.transform as RectTransform);
        mScrollRect.ValueChanged = new Action<Vector2>(this.OnValueChanged);
        mScrollRect.EndDragItem = new Action<int>(this.EndDragItem);
        this.currentPage = 2;
        mPageDatas[this.currentPage].In();
        mScrollRect.SetPage(this.currentPage, false, null);
        for (int j = 0; j < 5; j++)
        {
            object[] args = new object[] { j };
            mPageDatas[j].Play(Utils.FormatString("Button_Init{0}", args));
        }
        this.update_page();
    }

    protected override void OnRegisterOnce()
    {
        Transform parent = base._MonoView.transform.Find("ScrollView/ScrollView/Viewport/Content");
        mScrollRect = base._MonoView.transform.Find("ScrollView/ScrollView").GetComponent<ScrollRectBase>();
        grid = mScrollRect.get_content().GetComponent<GridLayoutGroup>();
        mDownCtrl = base._MonoView.transform.Find("Down").GetComponent<MainDownCtrl>();
        mDownCtrl.SetScrollRect(mScrollRect);
        mPageDatas[0] = new PageData(0, base._MonoView.transform.Find("Down/Button_0"), new MainUIPageShop(parent, mScrollRect));
        mPageDatas[0].init();
        mPageDatas[1] = new PageData(1, base._MonoView.transform.Find("Down/Button_1"), new MainUIPageChar(parent, mScrollRect));
        mPageDatas[2] = new PageData(2, base._MonoView.transform.Find("Down/Button_2"), new MainUIPageBattle(parent));
        mPageDatas[3] = new PageData(3, base._MonoView.transform.Find("Down/Button_3"), new MainUIPage3(parent));
        mPageDatas[4] = new PageData(4, base._MonoView.transform.Find("Down/Button_4"), new MainUIPage4(parent));
        mButtonFront = base._MonoView.transform.Find("Down/Front") as RectTransform;
        mButtonFront.parent.localScale = Vector3.one * GameLogic.WidthScaleAll;
        mScrollRect.SetWhole(grid, 5);
    }

    protected override void OnRemoveAfter()
    {
        ApplicationEvent.OnOnlyMain -= new Action(this.OnOnlyMain);
        WindowUI.CloseCurrency();
    }

    private void OnValueChanged(Vector2 value)
    {
        this.scrollpercentx = value.x;
        mButtonFront.anchoredPosition = new Vector2((this.scrollpercentx * 480f) - 240f, mButtonFront.anchoredPosition.y);
    }

    private void PlayGetGold(object o)
    {
        int num = (int) o;
        if (num > 0)
        {
            CurrencyFlyCtrl.PlayGet(CurrencyType.Gold, (long) num, null, null, true);
        }
    }

    private void TouchPage(int nextpage)
    {
        int currentPage = this.currentPage;
        this.currentPage = nextpage;
        if (this.currentPage != currentPage)
        {
            mScrollRect.SetPage(this.currentPage, true, null);
            bool flag = this.currentPage < currentPage;
            string str = !flag ? "Right" : "Left";
            List<int> list = new List<int>();
            if (flag)
            {
                for (int j = currentPage; j > this.currentPage; j--)
                {
                    list.Add(j);
                }
            }
            else
            {
                for (int j = currentPage; j < this.currentPage; j++)
                {
                    list.Add(j);
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                int index = list[i];
                mPageDatas[index].Out();
                if (index == currentPage)
                {
                    object[] objArray1 = new object[] { str, index };
                    mPageDatas[index].Play(Utils.FormatString("Button_{0}Out{1}", objArray1));
                }
                else
                {
                    object[] objArray2 = new object[] { str, index };
                    mPageDatas[index].Play(Utils.FormatString("Button_{0}OutMove{1}", objArray2));
                }
            }
            mPageDatas[this.currentPage].In();
            object[] args = new object[] { str, this.currentPage };
            mPageDatas[this.currentPage].Play(Utils.FormatString("Button_{0}In{1}", args));
        }
    }

    private void update_page()
    {
        mDownCtrl.UpdateUI();
        this.UpdateShopRedCount();
        this.UpdateEquipRedCount();
        this.UpdateCardRedCount();
    }

    private void UpdateCardRedCount()
    {
        mDownCtrl.UpdateLock(3);
        int count = 0;
        if (((GameLogic.Hold.Guide.mCard.process > 0) && !LocalSave.Instance.Card_GetAllMax()) && (LocalSave.Instance.Card_GetRandomGold() <= LocalSave.Instance.GetGold()))
        {
            int num2 = LocalSave.Instance.Card_GetNeedLevel();
            if (LocalSave.Instance.GetLevel() >= num2)
            {
                count = 1;
            }
        }
        mDownCtrl.SetRedNodeType(3, RedNodeType.eGreenUp);
        mDownCtrl.SetRedCount(3, count);
    }

    private void UpdateEquipRedCount()
    {
        mDownCtrl.UpdateLock(1);
        if (GameLogic.Hold.Guide.mEquip.process == 0)
        {
            mDownCtrl.SetRedCount(1, 0);
        }
        else
        {
            int count = LocalSave.Instance.Equip_GetCanWearCount();
            if (count > 0)
            {
                mDownCtrl.SetRedNodeType(1, RedNodeType.eRedWear);
                mDownCtrl.SetRedCount(1, count);
            }
            else
            {
                int num2 = LocalSave.Instance.Equip_GetNewCount();
                if (num2 > 0)
                {
                    mDownCtrl.SetRedNodeType(1, RedNodeType.eRedNew);
                    mDownCtrl.SetRedCount(1, num2);
                }
                else
                {
                    mDownCtrl.SetRedCount(1, 0);
                    int num3 = LocalSave.Instance.Equip_GetCanUpCount();
                    mDownCtrl.SetRedNodeType(1, RedNodeType.eGreenCount);
                    mDownCtrl.SetRedCount(1, num3);
                }
            }
        }
    }

    private void UpdateGold()
    {
    }

    private void UpdateShopRedCount()
    {
        int count = 0;
        int diamondBoxFreeCount = LocalSave.Instance.GetDiamondBoxFreeCount(LocalSave.TimeBoxType.BoxChoose_DiamondLarge);
        int num3 = LocalSave.Instance.GetDiamondBoxFreeCount(LocalSave.TimeBoxType.BoxChoose_DiamondNormal);
        count += diamondBoxFreeCount;
        count += num3;
        mDownCtrl.SetRedNodeType(0, RedNodeType.eRedCount);
        mDownCtrl.SetRedCount(0, count);
    }

    public override List<string> OnListNotificationInterests =>
        new List<string> { 
            "PUB_UI_UPDATE_CURRENCY",
            "MainUI_GetGold",
            "MainUI_UpdateExp",
            "MainUI_GotoShop",
            "MainUI_MailUpdate",
            "MainUI_LayerUpdate",
            "MainUI_TimeBoxUpdate",
            "MainUI_ShopRedCountUpdate",
            "MainUI_EquipRedCountUpdate",
            "MainUI_CardRedCountUpdate",
            "MainUI_UpdatePage",
            "MainUI_HarvestUpdate",
            "ShopUI_Update"
        };

    [CompilerGenerated]
    private sealed class <OnHandleNotification>c__AnonStorey1
    {
        internal object vo;
        internal MainModuleMediator $this;

        internal void <>m__0()
        {
            this.$this.PlayGetGold(this.vo);
        }
    }

    [CompilerGenerated]
    private sealed class <OnRegisterEvery>c__AnonStorey0
    {
        internal int index;
        internal MainModuleMediator $this;

        internal void <>m__0()
        {
            if (this.index == 1)
            {
                GameLogic.Hold.Guide.mEquip.CurrentOver(0);
            }
            if (this.index == 3)
            {
                GameLogic.Hold.Guide.mCard.CurrentOver(0);
            }
            if (this.index == 0)
            {
                LocalSave.Instance.mGuideData.SetIndex(1);
            }
            if (this.$this.currentPage != this.index)
            {
                this.$this.TouchPage(this.index);
            }
        }
    }

    public class PageData
    {
        public int Page;
        public Transform self;
        public UIBase PageCtrl;
        public RectTransform buttonrect;
        public Animation animation;
        public ButtonCtrl buttonctrl;
        public RedNodeCtrl redctrl;
        private bool bInit;

        public PageData(int page, Transform self, UIBase ctrl)
        {
            this.Page = page;
            this.self = self;
            this.PageCtrl = ctrl;
            this.PageCtrl.InitBefore();
            this.animation = self.Find("child").GetComponent<Animation>();
            this.buttonctrl = self.Find("child/child/Button").GetComponent<ButtonCtrl>();
            this.buttonrect = this.buttonctrl.transform as RectTransform;
            this.redctrl = self.GetComponentInChildren<RedNodeCtrl>();
        }

        public void AddButtonClick(Action click)
        {
            this.buttonctrl.onClick = click;
        }

        public void In()
        {
            this.init();
            this.buttonrect.sizeDelta = new Vector2(240f, this.buttonrect.sizeDelta.y);
            this.PageCtrl.Open();
            this.PageCtrl.OnLanguageChange();
        }

        public void init()
        {
            if (!this.bInit)
            {
                this.bInit = true;
                this.PageCtrl.Init();
            }
        }

        public void Out()
        {
            this.buttonrect.sizeDelta = new Vector2(120f, this.buttonrect.sizeDelta.y);
            this.PageCtrl.Close();
        }

        public void Play(string name)
        {
            if (this.animation != null)
            {
                this.animation.Play(name);
            }
        }
    }
}

