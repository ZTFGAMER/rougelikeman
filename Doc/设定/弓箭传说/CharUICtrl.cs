using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharUICtrl : MediatorCtrlBase
{
    public GameObject window;
    [Tooltip("标题")]
    public UILineCtrl mLineCtrl;
    public Text Text_MyCollections;
    public Text Text_Attribute;
    public RectTransform mCollectionsParent;
    public ButtonCtrl Button_Close;
    public List<EquipBGCtrl> mEquipBGList = new List<EquipBGCtrl>();
    public ScrollRectBase mScrollRectBase;
    public MainUIScrollRectInsideCtrl mInsideCtrl;
    public RectTransform board;
    public RectTransform bagParent;
    public GameObject copyitems;
    public CharUIHeroCtrl mHeroCtrl;
    public ButtonCtrl Button_Combine;
    public Text Text_Combine;
    public CharSortCtrl mSortCtrl;
    public UILineCtrl mMaterialLineCtrl;
    public ButtonCtrl Button_Light;
    public CharEquipChooseCtrl mChooseCtrl;
    [Header("穿戴时的装备显示位置")]
    public Transform wearctrlpos;
    public RedNodeCtrl mCombineRedCtrl;
    private GameObject _equipitem;
    private const int ColumnCount = 5;
    private const int EquipWidth = 140;
    private const int EquipHeight = 140;
    private const float BottomHeight = 250f;
    private float AllHeight;
    private List<EquipOneCtrl> mEquipItemList = new List<EquipOneCtrl>();
    private MutiCachePool<EquipOneCtrl> mCachePool = new MutiCachePool<EquipOneCtrl>();
    private Sequence seq;
    private float scrollendpos;
    private Vector2 collisionpos;
    private Vector2 bagparentpos;
    private EquipOneCtrl mClickEquip;
    private bool bGuide1;
    private EquipOneCtrl _WearCtrl;
    private float fringeHeight;
    private bool bOpened;
    private UIState state;
    private float lastframey;
    [CompilerGenerated]
    private static Action <>f__am$cache0;
    [CompilerGenerated]
    private static TweenCallback <>f__am$cache1;
    [CompilerGenerated]
    private static TweenCallback <>f__am$cache2;

    private void AddBags(List<LocalSave.EquipOne> list, float height, float startx, float starty)
    {
        <AddBags>c__AnonStorey0 storey = new <AddBags>c__AnonStorey0 {
            startx = startx,
            starty = starty,
            list = list,
            $this = this
        };
        int num = 0;
        int count = storey.list.Count;
        while (num < count)
        {
            <AddBags>c__AnonStorey1 storey2 = new <AddBags>c__AnonStorey1 {
                <>f__ref$0 = storey,
                index = num
            };
            TweenSettingsExtensions.AppendCallback(this.seq, new TweenCallback(storey2, this.<>m__0));
            TweenSettingsExtensions.AppendInterval(this.seq, 0.02f);
            num++;
        }
    }

    private void ChangeState(UIState state, bool force = false)
    {
        if (((this.state != state) && ((this.state != UIState.eWearing) || force)) && ((this.state != UIState.eEmptyWearing) || force))
        {
            this.state = state;
            if (state != UIState.eNormal)
            {
                if (state == UIState.eWear)
                {
                    this.ScrollPlayFade(false);
                    this.mScrollRectBase.verticalNormalizedPosition = 1f;
                    this.DoWearAction();
                }
                else if (state == UIState.eEmptyWearing)
                {
                    this.mScrollRectBase.verticalNormalizedPosition = 1f;
                    if (<>f__am$cache1 == null)
                    {
                        <>f__am$cache1 = new TweenCallback(null, <ChangeState>m__4);
                    }
                    if (<>f__am$cache2 == null)
                    {
                        <>f__am$cache2 = new TweenCallback(null, <ChangeState>m__5);
                    }
                    TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.Append(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.Append(DOTween.Sequence(), this.ScrollPlayFade(false)), <>f__am$cache1), this.ScrollPlayFade(true)), <>f__am$cache2);
                }
            }
            else
            {
                this.ScrollPlayFade(true);
                this.mWearCtrl.gameObject.SetActive(false);
                this.StopWearAction();
                this.MissAdd();
            }
        }
    }

    private void ChooseUIShow(bool show)
    {
        if (this.mChooseCtrl != null)
        {
            this.mChooseCtrl.Show(show);
        }
    }

    private void DoWearAction()
    {
        this.MissAdd();
        int num = 0;
        int count = this.mEquipBGList.Count;
        while (num < count)
        {
            this.mEquipBGList[num].SetButtonEnable(false);
            num++;
        }
        List<int> list = LocalSave.Instance.Equip_GetCanWears(this.mChooseCtrl.equipdata.data.Position);
        int num3 = 0;
        int num4 = list.Count;
        while (num3 < num4)
        {
            this.mEquipBGList[list[num3]].DoWear();
            this.mEquipBGList[list[num3]].SetButtonEnable(true);
            num3++;
        }
    }

    private float GetHeight(int count, int perheight)
    {
        if ((count % 5) == 0)
        {
            return (float) ((count / 5) * perheight);
        }
        return (float) (((count / 5) * perheight) + perheight);
    }

    private bool GetScrolling() => 
        (this.mScrollRectBase.get_velocity() != Vector2.zero);

    private void InitChooseCtrl()
    {
    }

    private void InitWears()
    {
        int num = 0;
        int count = this.mEquipBGList.Count;
        while (num < count)
        {
            this.mEquipBGList[num].SetClick(new Action<int>(this.OnClickWearAdd));
            num++;
        }
    }

    private void KillSequence()
    {
        if (this.seq != null)
        {
            TweenExtensions.Kill(this.seq, false);
            this.seq = null;
        }
    }

    private void MissAdd()
    {
        int num = 0;
        int count = this.mEquipBGList.Count;
        while (num < count)
        {
            this.mEquipBGList[num].MissAdd();
            num++;
        }
    }

    private void OnClickInfo()
    {
        if (!this.GetScrolling())
        {
            this.ChooseUIShow(false);
            EquipInfoModuleProxy.Transfer data = new EquipInfoModuleProxy.Transfer {
                one = this.mChooseCtrl.equipdata,
                updatecallback = new Action(this.UpgradeCallBack),
                wearcallback = new Action(this.WearCallBack)
            };
            EquipInfoModuleProxy proxy = new EquipInfoModuleProxy(data);
            Facade.Instance.RegisterProxy(proxy);
            WindowUI.ShowWindow(WindowID.WindowID_EquipInfo);
        }
    }

    private void OnClickLevel()
    {
        this.OnClickInfo();
    }

    private void OnClickScrollView()
    {
        this.ChooseUIShow(false);
        if (this.state == UIState.eWear)
        {
            this.ChangeState(UIState.eNormal, false);
        }
    }

    private void OnClickUnwear()
    {
        int index = this.mChooseCtrl.GetIndex();
        this.mEquipBGList[index].Unwear(this.wearctrlpos.position, delegate (LocalSave.EquipOne data) {
            LocalSave.Instance.EquipUnwear(data.UniqueID);
            TweenSettingsExtensions.Append(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.Append(DOTween.Sequence(), this.ScrollPlayFade(false)), new TweenCallback(this, this.<OnClickUnwear>m__9)), this.ScrollPlayFade(true));
        });
        this.mEquipBGList[index].MissAdd();
        this.ChooseUIShow(false);
        GameLogic.Hold.Sound.PlayUI(0xf4247);
    }

    private void OnClickWear()
    {
        <OnClickWear>c__AnonStorey3 storey = new <OnClickWear>c__AnonStorey3 {
            $this = this
        };
        this.mScrollRectBase.enabled = true;
        LocalSave.Instance.Equip_GetCanWearIndex(this.mChooseCtrl.equipdata, out storey.emptyindex);
        if (storey.emptyindex >= 0)
        {
            GameLogic.Hold.Sound.PlayUI(0xf4247);
            this.mEquipBGList[storey.emptyindex].Unwear(this.wearctrlpos.position, null);
            this.ChangeState(UIState.eEmptyWearing, false);
            this.mWearCtrl.gameObject.SetActive(true);
            this.mWearCtrl.Init(this.mChooseCtrl.equipdata);
            this.mWearCtrl.transform.localScale = Vector3.one;
            this.mWearCtrl.transform.position = this.mClickEquip.transform.position;
            TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.Join(TweenSettingsExtensions.Append(DOTween.Sequence(), ShortcutExtensions.DOMove(this.mWearCtrl.transform, this.mEquipBGList[storey.emptyindex].transform.position, 0.3f, false)), ShortcutExtensions.DOScale(this.mWearCtrl.transform.transform, Vector3.one * 1.1f, 0.3f)), new TweenCallback(storey, this.<>m__0));
            this.MissAdd();
            this.ChooseUIShow(false);
        }
        else
        {
            this.UpdateWear(this.mChooseCtrl.equipdata);
            this.ChooseUIShow(false);
        }
    }

    private void OnClickWearAdd(int index)
    {
        <OnClickWearAdd>c__AnonStorey2 storey = new <OnClickWearAdd>c__AnonStorey2 {
            index = index,
            $this = this
        };
        if (this.state == UIState.eWear)
        {
            GameLogic.Hold.Sound.PlayUI(0xf4247);
            this.ChangeState(UIState.eWearing, false);
            this.mEquipBGList[storey.index].Unwear(this.wearctrlpos.position, null);
            TweenSettingsExtensions.OnComplete<Sequence>(TweenSettingsExtensions.Join(TweenSettingsExtensions.Append(DOTween.Sequence(), ShortcutExtensions.DOMove(this.mWearCtrl.transform, this.mEquipBGList[storey.index].transform.position, 0.3f, false)), ShortcutExtensions.DOScale(this.mWearCtrl.transform, Vector3.one * 1.1f, 0.3f)), new TweenCallback(storey, this.<>m__0));
            this.MissAdd();
            this.StopWearAction();
        }
        else if (this.mEquipBGList[storey.index].GetIsWear() && (this.state == UIState.eNormal))
        {
            RectTransform transform = this.mChooseCtrl.transform as RectTransform;
            Vector3 position = this.mEquipBGList[storey.index].transform.position;
            this.UpdateChooseEquip(this.mEquipBGList[storey.index].ctrl);
            this.mChooseCtrl.SetIndex(storey.index);
        }
    }

    protected override void OnClose()
    {
        this.mCachePool.clear();
        this.bOpened = false;
        this.mHeroCtrl.Show(false);
        LocalSave.Instance.Equip_RemoveUpdateAction(new Action(this.UpdateAttribute));
        this.KillSequence();
    }

    private void OnDrag(PointerEventData eventData)
    {
    }

    private void OnDragBegin(PointerEventData eventData)
    {
        this.ChooseUIShow(false);
        Updater.RemoveUpdateUI(new Action<float>(this.OnUpdate));
    }

    private void OnDragEnd(PointerEventData eventData)
    {
        this.ChooseUIShow(false);
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
        string name = notification.Name;
        object body = notification.Body;
        if (name != null)
        {
            if (name == "PUB_NETCONNECT_UPDATE")
            {
                this.UpdateNet();
            }
            else if (name == "MainUI_EquipRedCountUpdate")
            {
            }
        }
    }

    protected override void OnInit()
    {
        this.collisionpos = this.mCollectionsParent.anchoredPosition;
        this.bagparentpos = this.bagParent.anchoredPosition;
        this.mScrollRectBase.bUseScrollEvent = false;
        this.window.SetActive(false);
        this.fringeHeight = PlatformHelper.GetFringeHeight();
        RectTransform transform = this.mScrollRectBase.transform as RectTransform;
        transform.anchoredPosition = new Vector3(0f, this.fringeHeight, 0f);
        transform.sizeDelta = new Vector2(transform.sizeDelta.x, (transform.sizeDelta.y / GameLogic.WidthScaleAll) + this.fringeHeight);
        this.AllHeight = transform.sizeDelta.y;
        this.mScrollRectBase.OnClick = new Action(this.OnClickScrollView);
        this.Button_Light.onClick = new Action(this.OnClickScrollView);
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_Char);
        }
        this.Button_Close.onClick = <>f__am$cache0;
        this.Button_Combine.onClick = delegate {
            EquipCombineProxy.Transfer data = new EquipCombineProxy.Transfer {
                onClose = () => this.UpdateEquipsList()
            };
            Facade.Instance.RegisterProxy(new EquipCombineProxy(data));
            WindowUI.ShowWindow(WindowID.WindowID_EquipCombine);
        };
        this.mScrollRectBase.BeginDrag = new Action<PointerEventData>(this.OnDragBegin);
        this.mScrollRectBase.Drag = new Action<PointerEventData>(this.OnDrag);
        this.mScrollRectBase.EndDrag = new Action<PointerEventData>(this.OnDragEnd);
        this.mCachePool.Init(base.gameObject, this.equipitem);
        this.copyitems.SetActive(false);
        this.InitWears();
        this.InitChooseCtrl();
        this.mSortCtrl.OnButtonClick = list => this.UpdateEquipsList();
        this.mCombineRedCtrl.SetType(RedNodeType.eWarning);
    }

    public override void OnLanguageChange()
    {
        this.mLineCtrl.SetText(GameLogic.Hold.Language.GetLanguageByTID("EquipUI_MyEquip", Array.Empty<object>()));
        this.Text_Combine.text = GameLogic.Hold.Language.GetLanguageByTID("EquipUI_Combine", Array.Empty<object>());
        this.Text_MyCollections.text = GameLogic.Hold.Language.GetLanguageByTID("EquipUI_MyCollections", Array.Empty<object>());
        this.mMaterialLineCtrl.SetText(GameLogic.Hold.Language.GetLanguageByTID("EquipUI_Materials", Array.Empty<object>()));
        this.mChooseCtrl.OnLanguageChange();
        this.mSortCtrl.OnLanguageChange();
    }

    protected override void OnOpen()
    {
        this.bOpened = true;
        this.mMaterialLineCtrl.gameObject.SetActive(false);
        if (!this.window.activeSelf)
        {
            this.window.SetActive(true);
        }
        LocalSave.Instance.Equip_AddUpdateAction(new Action(this.UpdateAttribute));
        this.UpdateAttribute();
        this.ChooseUIShow(false);
        this.UpdateNet();
        this.ChangeState(UIState.eNormal, false);
        this.mClickEquip = null;
        this.mChooseCtrl.transform.position = new Vector3(9999f, 0f, 0f);
        this.UpdateEquipsList();
        this.mScrollRectBase.verticalNormalizedPosition = 1f;
    }

    protected override void OnSetArgs(object o)
    {
        this.mInsideCtrl.anotherScrollRect = o as ScrollRectBase;
    }

    private void OnUpdate(float delta)
    {
        if (!this.GetScrolling())
        {
            this.mScrollRectBase.verticalNormalizedPosition = Mathf.Lerp(this.mScrollRectBase.verticalNormalizedPosition, this.scrollendpos, 0.2f);
            if (Mathf.Abs((float) (this.mChooseCtrl.transform.position.y - this.lastframey)) < 0.2f)
            {
                this.mScrollRectBase.verticalNormalizedPosition = this.scrollendpos;
                Updater.RemoveUpdateUI(new Action<float>(this.OnUpdate));
            }
            this.lastframey = this.mChooseCtrl.transform.position.y;
        }
    }

    private Sequence ScrollPlayFade(bool show)
    {
        Sequence sequence = DOTween.Sequence();
        if (!show)
        {
            TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this, this.<ScrollPlayFade>m__6));
            return sequence;
        }
        TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this, this.<ScrollPlayFade>m__7));
        return sequence;
    }

    private void SetScrollEnable(bool value)
    {
        this.mScrollRectBase.UseDrag = value;
        this.mInsideCtrl.anotherScrollRect.UseDrag = value;
        this.mInsideCtrl.enabled = value;
    }

    private void StopWearAction()
    {
        int num = 0;
        int count = this.mEquipBGList.Count;
        while (num < count)
        {
            this.mEquipBGList[num].WearOver();
            num++;
        }
        int num3 = 0;
        int num4 = this.mEquipBGList.Count;
        while (num3 < num4)
        {
            this.mEquipBGList[num3].UpdateButtonEnable();
            num3++;
        }
    }

    private void update_combine_rednode()
    {
        if (LocalSave.Instance.Equip_can_combine_count() > 0)
        {
            this.mCombineRedCtrl.Value = 1;
        }
        else
        {
            this.mCombineRedCtrl.Value = 0;
        }
    }

    private void UpdateAttribute()
    {
        SelfAttributeData selfAttributeShow = GameLogic.SelfAttributeShow;
        string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("Attr_Attack", Array.Empty<object>());
        string str2 = GameLogic.Hold.Language.GetLanguageByTID("Attr_HPMax", Array.Empty<object>());
        if (this.Text_Attribute != null)
        {
            object[] args = new object[] { languageByTID, selfAttributeShow.attribute.AttackValue.Value, str2, selfAttributeShow.attribute.HPValue.Value };
            this.Text_Attribute.text = Utils.FormatString("{0} {1}  {2} {3}", args);
        }
    }

    private void UpdateChooseCardScrollView()
    {
        float num = (this.mChooseCtrl.transform.position.y / ((float) GameLogic.Height)) * GameLogic.DesignHeight;
        float num2 = 350f;
        float num3 = 1080f;
        float num4 = 0f;
        if (num < num2)
        {
            num4 = num2 - num;
        }
        else if (num > num3)
        {
            num4 = num3 - num;
        }
        if (num4 != 0f)
        {
            float num5 = this.mScrollRectBase.get_content().sizeDelta.y - this.AllHeight;
            this.scrollendpos = this.mScrollRectBase.verticalNormalizedPosition - ((num4 / (GameLogic.WidthScale / GameLogic.HeightScale)) / num5);
            this.scrollendpos = MathDxx.Clamp01(this.scrollendpos);
            this.lastframey = num;
            Updater.AddUpdateUI(new Action<float>(this.OnUpdate), false);
        }
    }

    public void UpdateChooseEquip(EquipOneCtrl one)
    {
        GameLogic.Hold.Guide.mEquip.CurrentOver(1);
        this.mChooseCtrl.Init(one.equipdata);
        this.mClickEquip = one;
        this.OnClickInfo();
    }

    private void UpdateEquipsList()
    {
        this.mCachePool.clear();
        this.update_combine_rednode();
        LocalSave.Instance.Equip_SetRefresh();
        this.mEquipItemList.Clear();
        this.KillSequence();
        this.seq = DOTween.Sequence();
        List<LocalSave.EquipOne> haveEquips = LocalSave.Instance.GetHaveEquips(true);
        int num = 0;
        int count = this.mEquipBGList.Count;
        while (num < count)
        {
            this.mEquipBGList[num].Init(null);
            num++;
        }
        int num3 = 0;
        int num4 = haveEquips.Count;
        while (num3 < num4)
        {
            LocalSave.EquipOne equipdata = haveEquips[num3];
            if (equipdata.WearIndex >= 0)
            {
                this.mEquipBGList[equipdata.WearIndex].Init(equipdata);
            }
            num3++;
        }
        List<LocalSave.EquipOne> list = this.mSortCtrl.GetList(EquipType.eEquip);
        TweenSettingsExtensions.AppendInterval(this.seq, 0.2f);
        float height = 13f;
        float startx = -280f;
        float starty = -height - 70f;
        if (list.Count > 0)
        {
            this.AddBags(list, height, startx, starty);
            float num8 = this.GetHeight(list.Count, 140);
            starty -= num8;
            height += num8;
        }
        List<LocalSave.EquipOne> list3 = this.mSortCtrl.GetList(EquipType.eMaterial);
        if (list3.Count > 0)
        {
            this.mMaterialLineCtrl.gameObject.SetActive(true);
            this.mMaterialLineCtrl.SetY(starty + 70f);
            starty -= 50f;
            this.AddBags(list3, height, startx, starty);
            float num9 = this.GetHeight(list3.Count, 140);
            starty -= num9;
            height += num9;
        }
        this.mCachePool.hold(list.Count + list3.Count);
        this.mScrollRectBase.get_content().sizeDelta = new Vector2(this.mScrollRectBase.get_content().sizeDelta.x, (((height + -this.board.anchoredPosition.y) + this.board.sizeDelta.y) + 250f) - this.fringeHeight);
        this.UpdateHero();
    }

    private void UpdateHero()
    {
        int weaponid = LocalSave.Instance.Equip_GetWeapon();
        this.mHeroCtrl.InitWeapon(weaponid);
        this.mHeroCtrl.InitCloth(LocalSave.Instance.Equip_GetCloth());
        this.mHeroCtrl.InitPet(0, LocalSave.Instance.Equip_GetPet(0));
        this.mHeroCtrl.InitPet(1, LocalSave.Instance.Equip_GetPet(1));
        this.mHeroCtrl.Show(true);
    }

    private void UpdateNet()
    {
        this.mChooseCtrl.UpdateNet();
    }

    private void UpdateWear(LocalSave.EquipOne equipdata)
    {
        this.mWearCtrl.gameObject.SetActive(true);
        this.mWearCtrl.Init(equipdata);
        this.mWearCtrl.transform.position = this.mClickEquip.transform.position;
        this.mWearCtrl.transform.localScale = Vector3.one;
        this.ChangeState(UIState.eWear, false);
        ShortcutExtensions.DOMove(this.mWearCtrl.transform, this.wearctrlpos.position, 0.5f, false);
    }

    private void UpgradeCallBack()
    {
        if (this.mClickEquip != null)
        {
            this.mClickEquip.Init();
            if (this.mClickEquip.equipdata.IsWear)
            {
                this.mClickEquip.SetButtonEnable(false);
            }
        }
        this.UpdateEquipsList();
    }

    private void WearCallBack()
    {
        if (this.mClickEquip != null)
        {
            if (this.mClickEquip.equipdata.IsWear)
            {
                this.OnClickUnwear();
            }
            else
            {
                this.OnClickWear();
            }
        }
    }

    private GameObject equipitem
    {
        get
        {
            if (this._equipitem == null)
            {
                this._equipitem = CInstance<UIResourceCreator>.Instance.GetEquip(this.copyitems.transform).gameObject;
            }
            return this._equipitem;
        }
    }

    private EquipOneCtrl mWearCtrl
    {
        get
        {
            if (this._WearCtrl == null)
            {
                this._WearCtrl = CInstance<UIResourceCreator>.Instance.GetEquip(this.bagParent.parent);
                this._WearCtrl.SetButtonEnable(false);
                this._WearCtrl.ShowAniEnable(false);
                this._WearCtrl.transform.localPosition = new Vector3(10000f, 0f);
            }
            return this._WearCtrl;
        }
    }

    [CompilerGenerated]
    private sealed class <AddBags>c__AnonStorey0
    {
        internal float startx;
        internal float starty;
        internal List<LocalSave.EquipOne> list;
        internal CharUICtrl $this;
    }

    [CompilerGenerated]
    private sealed class <AddBags>c__AnonStorey1
    {
        internal int index;
        internal CharUICtrl.<AddBags>c__AnonStorey0 <>f__ref$0;

        internal void <>m__0()
        {
            EquipOneCtrl item = this.<>f__ref$0.$this.mCachePool.get();
            RectTransform t = item.transform as RectTransform;
            ((Transform) t).SetParentNormal((Transform) this.<>f__ref$0.$this.bagParent);
            t.anchoredPosition = new Vector2(this.<>f__ref$0.startx + ((this.index % 5) * 140), this.<>f__ref$0.starty - ((this.index / 5) * 140));
            item.Init(this.<>f__ref$0.list[this.index]);
            item.UpdateRedShow();
            item.OnClickEvent = new Action<EquipOneCtrl>(this.<>f__ref$0.$this.UpdateChooseEquip);
            this.<>f__ref$0.$this.mEquipItemList.Add(item);
            if (!this.<>f__ref$0.$this.bGuide1 && !item.equipdata.Overlying)
            {
                GameLogic.Hold.Guide.mEquip.GoNext(1, t);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickWear>c__AnonStorey3
    {
        internal int emptyindex;
        internal CharUICtrl $this;

        internal void <>m__0()
        {
            this.$this.mEquipBGList[this.emptyindex].Init(this.$this.mChooseCtrl.equipdata);
            LocalSave.Instance.EquipWear(this.$this.mChooseCtrl.equipdata, this.emptyindex);
            this.$this.ChangeState(CharUICtrl.UIState.eNormal, true);
            this.$this.UpdateEquipsList();
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickWearAdd>c__AnonStorey2
    {
        internal int index;
        internal CharUICtrl $this;

        internal void <>m__0()
        {
            this.$this.mEquipBGList[this.index].Init(this.$this.mWearCtrl.equipdata);
            LocalSave.Instance.EquipWear(this.$this.mWearCtrl.equipdata, this.index);
            this.$this.ChangeState(CharUICtrl.UIState.eNormal, true);
            this.$this.UpdateEquipsList();
        }
    }

    public enum UIState
    {
        eNormal,
        eWear,
        eWearing,
        eEmptyWearing
    }
}

