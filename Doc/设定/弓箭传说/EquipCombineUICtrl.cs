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

public class EquipCombineUICtrl : MediatorCtrlBase
{
    public RectTransform titletransform;
    public GameObject titlecombine;
    public Text Text_Title;
    public ButtonCtrl Button_Close;
    public Text Text_Guide;
    public GameObject copyitems;
    public GameObject copyone;
    public ScrollRectBase mScrollRect;
    public EquipCombineInfinity mInfinity;
    public RectTransform mScrollChild;
    public EquipCombineParent mCombineParent;
    public ButtonCtrl Button_Combine;
    public Text Text_Combine;
    public GameObject mMaskparent;
    private int leftpadding = 10;
    private int width = 140;
    private int height = 140;
    private int LineCount = 5;
    private int BottomHeight = 200;
    private Vector2 scrollsize;
    private bool bLock;
    private EquipCombineOne mPlayOne;
    private EquipCombineOne mChoose;
    private LocalUnityObjctPool mPool;
    private MutiCachePool<EquipCombineOne> mCachePool = new MutiCachePool<EquipCombineOne>();
    private List<LocalSave.EquipOne> mList;
    private List<EquipCombineOne> mItemList = new List<EquipCombineOne>();
    private SequencePool mSeqPool = new SequencePool();
    private Action onClose;
    [CompilerGenerated]
    private static Action <>f__am$cache0;
    [CompilerGenerated]
    private static Comparison<LocalSave.EquipOne> <>f__am$cache1;

    private void InitUI()
    {
        this.mScrollRect.verticalNormalizedPosition = 1f;
        this.bLock = false;
        this.mMaskparent.SetActive(false);
        this.show_combine_button(false);
        this.mCombineParent.Show(false);
        this.mList = LocalSave.Instance.GetHaveEquips(true);
        if (this.mList.Count > 0)
        {
            for (int i = this.mList.Count - 1; i >= 0; i--)
            {
                if (!this.mList[i].QualityCanUp)
                {
                    this.mList.RemoveAt(i);
                }
            }
        }
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = delegate (LocalSave.EquipOne a, LocalSave.EquipOne b) {
                bool canCombine = a.CanCombine;
                bool flag2 = b.CanCombine;
                if (canCombine && !flag2)
                {
                    return -1;
                }
                if (canCombine || !flag2)
                {
                    if (a.Position < b.Position)
                    {
                        return -1;
                    }
                    if (a.Position > b.Position)
                    {
                        return 1;
                    }
                    if (a.IconBase < b.IconBase)
                    {
                        return -1;
                    }
                    if (a.IconBase > b.IconBase)
                    {
                        return 1;
                    }
                    if (a.Quality > b.Quality)
                    {
                        return -1;
                    }
                    if (a.Quality < b.Quality)
                    {
                        return 1;
                    }
                    if (a.Level > b.Level)
                    {
                        return -1;
                    }
                    if (a.Level < b.Level)
                    {
                        return 1;
                    }
                }
                return 1;
            };
        }
        this.mList.Sort(<>f__am$cache1);
        this.update_scroll_height();
        this.mChoose = null;
        this.mCachePool.clear();
        this.mCachePool.hold(this.mList.Count);
        this.mPool.Collect<EquipCombineOne>();
        this.mPlayOne = this.mPool.DeQueue<EquipCombineOne>();
        this.mPlayOne.transform.SetParentNormal(this.mMaskparent);
        this.mPlayOne.gameObject.SetActive(false);
        this.mItemList.Clear();
        this.mInfinity.SetItemCount(this.mList.Count);
        this.mInfinity.Refresh();
        this.set_guide_info(0);
    }

    private void miss_combine_parent()
    {
        this.mCombineParent.Show(false);
        this.mChoose.SetChoose(null);
        this.set_all_equips_lock(false);
        this.mChoose = null;
    }

    private void OnClickOne(EquipCombineOne one)
    {
        <OnClickOne>c__AnonStorey2 storey = new <OnClickOne>c__AnonStorey2 {
            one = one,
            $this = this
        };
        if (storey.one.mChoose != null)
        {
            this.OnCombineDown(storey.one.mChoose);
            this.set_equip_lock(storey.one, false);
        }
        else if (this.mChoose == null)
        {
            <OnClickOne>c__AnonStorey1 storey2 = new <OnClickOne>c__AnonStorey1 {
                <>f__ref$2 = storey,
                count = storey.one.mData.data.BreakNeed
            };
            if (storey2.count > 0)
            {
                this.mChoose = storey.one;
                this.set_guide_info(1);
                this.mCombineParent.init_data(storey2.count, storey.one);
                this.set_all_equips_lock(true);
                this.play_combine(0, storey.one, new Action(storey2.<>m__0));
            }
        }
        else
        {
            int index = this.mCombineParent.FindEmpty();
            if ((index >= 0) && this.mCombineParent.can_choose(storey.one))
            {
                this.play_combine(index, storey.one, new Action(storey.<>m__0));
            }
        }
    }

    protected override void OnClose()
    {
        this.mSeqPool.Clear();
        if (this.onClose != null)
        {
            this.onClose();
        }
    }

    private void OnCombineDown(EquipCombineChooseOne one)
    {
        if (one.mIndex == 0)
        {
            this.miss_combine_parent();
            this.set_guide_info(0);
        }
        else
        {
            this.mCombineParent.down_one(one.mIndex);
            if (one.mEquipChoose != null)
            {
                one.mEquipChoose.PlayAni(true);
            }
            this.set_guide_info(1);
            one.Clear();
        }
        this.show_combine_button(false);
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
        this.mCachePool.Init(base.gameObject, this.copyone);
        this.mPool = LocalUnityObjctPool.Create(base.gameObject);
        this.mPool.CreateCache<EquipCombineOne>(this.copyone);
        this.copyitems.SetActive(false);
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_EquipCombine);
        }
        this.Button_Close.onClick = <>f__am$cache0;
        this.Button_Combine.onClick = delegate {
            <OnInit>c__AnonStorey0 storey = new <OnInit>c__AnonStorey0 {
                $this = this
            };
            SdkManager.send_event_equip_combine("click", 0, string.Empty, string.Empty);
            storey.data = new CEquipCompositeTrans();
            storey.data.m_nTransID = LocalSave.Instance.SaveExtra.GetTransID();
            storey.data.m_arrCompositeInfo = new CEquipmentItem[this.mChoose.mData.BreakNeed];
            string str = string.Empty;
            bool flag = false;
            bool flag2 = true;
            int equipID = 0;
            for (int i = 0; i < storey.data.m_arrCompositeInfo.Length; i++)
            {
                int index = this.mCombineParent.GetIndex(i);
                LocalSave.EquipOne one = this.mList[index];
                CEquipmentItem item = new CEquipmentItem {
                    m_nUniqueID = one.UniqueID,
                    m_nRowID = one.RowID
                };
                if (equipID == 0)
                {
                    equipID = one.EquipID;
                }
                else if (equipID != one.EquipID)
                {
                    flag2 = false;
                }
                str = str + one.RowID + ".";
                if (one.RowID == 0L)
                {
                    flag = true;
                }
                storey.data.m_arrCompositeInfo[i] = item;
            }
            if (flag)
            {
                CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError, Array.Empty<string>());
                object[] args = new object[] { str };
                SdkManager.Bugly_Report("EquipCombineUICtrl", Utils.FormatString("rowid=0 : {0}", args));
                SdkManager.send_event_equip_combine("end", 0, "fail", "rowid=0");
            }
            else if (!flag2)
            {
                CInstance<TipsUIManager>.Instance.Show(ETips.Tips_CombineError, Array.Empty<string>());
                object[] args = new object[] { str };
                SdkManager.Bugly_Report("EquipCombineUICtrl", Utils.FormatString("combinerror:{0}", args));
                SdkManager.send_event_equip_combine("end", 0, "fail", "combineerror");
            }
            else
            {
                NetManager.SendInternal<CEquipCompositeTrans>(storey.data, SendType.eForceOnce, 3, 10, new Action<NetResponse>(storey.<>m__0));
            }
        };
        this.mCombineParent.OnCombineDown = new Action<EquipCombineChooseOne>(this.OnCombineDown);
        float fringeHeight = PlatformHelper.GetFringeHeight();
        this.titletransform.anchoredPosition = new Vector2(this.titletransform.anchoredPosition.x, this.titletransform.anchoredPosition.y + fringeHeight);
        RectTransform transform = base.transform as RectTransform;
        RectTransform transform2 = this.mScrollRect.transform as RectTransform;
        float y = transform.sizeDelta.y;
        float num3 = (y <= 1280f) ? 0f : (y - 1280f);
        num3 += fringeHeight;
        transform2.sizeDelta = new Vector2(transform2.sizeDelta.x, transform2.sizeDelta.y + num3);
        this.scrollsize = transform2.sizeDelta;
        int num4 = MathDxx.CeilBig(this.scrollsize.y / ((float) this.height)) + 1;
        int itemcount = num4 * this.LineCount;
        this.mInfinity.copyItem = this.copyone;
        this.mInfinity.initDisplayCount = itemcount;
        this.mInfinity.Init(itemcount);
        this.mInfinity.updatecallback = new Action<int, EquipCombineOne>(this.UpdateChildCallBack);
    }

    public override void OnLanguageChange()
    {
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("EquipUI_CombineTitle", Array.Empty<object>());
        this.Text_Combine.text = GameLogic.Hold.Language.GetLanguageByTID("EquipUI_Combine", Array.Empty<object>());
    }

    protected override void OnOpen()
    {
        SdkManager.send_event_equip_combine("show", 0, string.Empty, string.Empty);
        IProxy proxy = Facade.Instance.RetrieveProxy("EquipCombineProxy");
        if ((proxy != null) && (proxy.Data is EquipCombineProxy.Transfer))
        {
            this.onClose = (proxy.Data as EquipCombineProxy.Transfer).onClose;
        }
        this.InitUI();
    }

    private void play_combine(int index, EquipCombineOne one, Action callback)
    {
        <play_combine>c__AnonStorey3 storey = new <play_combine>c__AnonStorey3 {
            callback = callback,
            $this = this
        };
        Sequence sequence = this.mSeqPool.Get();
        this.mPlayOne.gameObject.SetActive(true);
        this.mPlayOne.Init(index, one.mData);
        this.mPlayOne.SetButtonEnable(false);
        this.mPlayOne.transform.position = one.transform.position;
        this.mPlayOne.transform.localScale = Vector3.one * this.mCombineParent.GetScale(index);
        TweenSettingsExtensions.Append(sequence, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOMove(this.mPlayOne.transform, this.mCombineParent.GetPosition(index), 0.3f, false), 6));
        this.mMaskparent.SetActive(true);
        TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(storey, this.<>m__0));
    }

    private void set_all_equips_lock(bool value)
    {
        this.bLock = value;
        int num = 0;
        int count = this.mItemList.Count;
        while (num < count)
        {
            this.set_equip_lock(this.mItemList[num], value);
            num++;
        }
    }

    private void set_equip_lock(EquipCombineOne one, bool value)
    {
        if (this.mChoose != null)
        {
            int equipID = 0;
            int index = this.mCombineParent.GetIndex(0);
            if ((index >= 0) && (index < this.mList.Count))
            {
                equipID = this.mList[index].EquipID;
            }
            if (value)
            {
                int num3 = this.mCombineParent.get_choose_index(one);
                if (num3 >= 0)
                {
                    EquipCombineChooseOne choose = this.mCombineParent.GetChoose(num3);
                    if (choose != null)
                    {
                        one.SetChoose(choose);
                        choose.Set_Choose_Equip(one);
                    }
                    return;
                }
            }
            if (value)
            {
                if (one.mData.EquipID != equipID)
                {
                    one.SetLock(true);
                    one.PlayAni(false);
                }
                else
                {
                    one.SetLock(false);
                    one.PlayAni(true);
                }
            }
            else
            {
                one.SetLock(false);
                one.SetChoose(null);
                one.PlayAni(false);
            }
        }
    }

    private void set_guide_info(int index)
    {
        object[] args = new object[] { index };
        this.Text_Guide.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("combine_guide_{0}", args), Array.Empty<object>());
    }

    private void show_combine_button(bool value)
    {
        this.Button_Combine.transform.parent.gameObject.SetActive(value);
        if (value)
        {
            this.Button_Combine.transform.parent.localScale = Vector3.zero;
            TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.Button_Combine.transform.parent, Vector3.one, 0.3f), 0x1b);
        }
    }

    private void update_scroll_height()
    {
        int num2 = MathDxx.CeilBig(((float) this.mList.Count) / ((float) this.LineCount));
        this.mScrollRect.get_content().sizeDelta = new Vector2(this.mScrollRect.get_content().sizeDelta.x, (float) ((num2 * this.height) + this.BottomHeight));
    }

    private void UpdateChildCallBack(int index, EquipCombineOne one)
    {
        one.Init(index, this.mList[index]);
        one.PlayAni(false);
        one.OnButtonClick = new Action<EquipCombineOne>(this.OnClickOne);
        one.SetChoose(null);
        if (!this.mItemList.Contains(one))
        {
            this.mItemList.Add(one);
        }
        this.set_equip_lock(one, this.bLock);
    }

    [CompilerGenerated]
    private sealed class <OnClickOne>c__AnonStorey1
    {
        internal int count;
        internal EquipCombineUICtrl.<OnClickOne>c__AnonStorey2 <>f__ref$2;

        internal void <>m__0()
        {
            this.<>f__ref$2.$this.mCombineParent.Init(this.count, this.<>f__ref$2.one);
            this.<>f__ref$2.$this.mChoose.SetChoose(this.<>f__ref$2.$this.mCombineParent.GetChoose(0));
            this.<>f__ref$2.$this.mCombineParent.Show(true);
        }
    }

    [CompilerGenerated]
    private sealed class <OnClickOne>c__AnonStorey2
    {
        internal EquipCombineOne one;
        internal EquipCombineUICtrl $this;

        internal void <>m__0()
        {
            this.one.SetChoose(this.$this.mCombineParent.ChooseOne(this.one));
            if (this.$this.mCombineParent.Is_Full())
            {
                this.$this.show_combine_button(true);
                this.$this.set_guide_info(2);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <OnInit>c__AnonStorey0
    {
        internal CEquipCompositeTrans data;
        internal EquipCombineUICtrl $this;

        internal void <>m__0(NetResponse response)
        {
            if (response.IsSuccess)
            {
                this.$this.mChoose.mData.QualityUp();
                SdkManager.send_event_equipment("GET", this.$this.mChoose.mData.EquipID, 1, this.$this.mChoose.mData.Level, EquipSource.EEquip_Combine, 0);
                SdkManager.send_event_equip_combine("end", this.$this.mChoose.mData.EquipID, "success", string.Empty);
                EquipCombineUpProxy.Transfer data = new EquipCombineUpProxy.Transfer {
                    equip = this.$this.mChoose.mData,
                    onClose = new Action(this.$this.InitUI)
                };
                for (int i = 1; i < this.data.m_arrCompositeInfo.Length; i++)
                {
                    data.AddMatUniqueID(this.data.m_arrCompositeInfo[i].m_nUniqueID);
                }
                Facade.Instance.RegisterProxy(new EquipCombineUpProxy(data));
                WindowUI.ShowWindow(WindowID.WindowID_EquipCombineUp);
                this.$this.InitUI();
            }
            else if ((response.error != null) && (response.error.m_nStatusCode == 2))
            {
                CInstance<TipsUIManager>.Instance.Show(ETips.Tips_CombineError, Array.Empty<string>());
                SdkManager.send_event_equip_combine("end", 0, "fail", "combineerror");
            }
            else
            {
                CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError, Array.Empty<string>());
                SdkManager.send_event_equip_combine("end", 0, "fail", "net_error");
            }
        }
    }

    [CompilerGenerated]
    private sealed class <play_combine>c__AnonStorey3
    {
        internal Action callback;
        internal EquipCombineUICtrl $this;

        internal void <>m__0()
        {
            this.$this.mMaskparent.SetActive(false);
            if (this.callback != null)
            {
                this.callback();
            }
        }
    }
}

