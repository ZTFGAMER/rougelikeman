using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class RewardSimpleUICtrl : MediatorCtrlBase
{
    public RectTransform child;
    public Text Text_Title;
    public RectTransform itemparent;
    public TapToCloseCtrl mTapCloseCtrl;
    public RectTransform itembg;
    private const int LineCount = 4;
    private const float TitleHeight = 50f;
    private const float OneWidth = 140f;
    private const float OneHeight = 140f;
    private const float playTime = 0.03f;
    private RewardSimpleProxy.Transfer mTransfer;
    private SequencePool mSeqPool = new SequencePool();
    private LocalUnityObjctPool mPool;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    private void InitUI()
    {
        <InitUI>c__AnonStorey1 storey = new <InitUI>c__AnonStorey1 {
            $this = this
        };
        this.mPool.Collect<PropOneEquip>();
        this.mTapCloseCtrl.Show(false);
        int count = this.mTransfer.list.Count;
        storey.currentlinecount = 4;
        int num2 = MathDxx.CeilBig(((float) count) / 4f);
        this.child.anchoredPosition = new Vector2(0f, (140f * (num2 - 1)) / 2f);
        this.itembg.sizeDelta = new Vector2(this.itembg.sizeDelta.x, (num2 * 140f) + 50f);
        if (count < 4)
        {
            storey.currentlinecount = count;
        }
        Sequence sequence = this.mSeqPool.Get();
        for (int i = 0; i < count; i++)
        {
            <InitUI>c__AnonStorey0 storey2 = new <InitUI>c__AnonStorey0 {
                <>f__ref$1 = storey,
                index = i
            };
            TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(storey2, this.<>m__0));
            TweenSettingsExtensions.AppendInterval(sequence, 0.03f);
        }
        TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(storey, this.<>m__0));
    }

    protected override void OnClose()
    {
        this.mSeqPool.Clear();
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
        GameObject gameObject = CInstance<UIResourceCreator>.Instance.GetPropOneEquip(base.transform).gameObject;
        this.mPool = LocalUnityObjctPool.Create(base.gameObject);
        this.mPool.CreateCache<PropOneEquip>(gameObject);
        gameObject.SetActive(false);
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_RewardSimple);
        }
        this.mTapCloseCtrl.OnClose = <>f__am$cache0;
    }

    public override void OnLanguageChange()
    {
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("rewardsimple_title", Array.Empty<object>());
    }

    protected override void OnOpen()
    {
        IProxy proxy = Facade.Instance.RetrieveProxy("RewardSimpleProxy");
        if ((proxy != null) && (proxy.Data != null))
        {
            this.mTransfer = proxy.Data as RewardSimpleProxy.Transfer;
            if (this.mTransfer != null)
            {
                this.InitUI();
            }
        }
    }

    [CompilerGenerated]
    private sealed class <InitUI>c__AnonStorey0
    {
        internal int index;
        internal RewardSimpleUICtrl.<InitUI>c__AnonStorey1 <>f__ref$1;

        internal void <>m__0()
        {
            Drop_DropModel.DropData data = this.<>f__ref$1.$this.mTransfer.list[this.index];
            PropOneEquip equip = this.<>f__ref$1.$this.mPool.DeQueue<PropOneEquip>();
            RectTransform transform = equip.transform as RectTransform;
            ((Transform) transform).SetParentNormal((Transform) this.<>f__ref$1.$this.itemparent);
            equip.InitProp(data);
            float x = ((this.index % this.<>f__ref$1.currentlinecount) * 140f) - ((140f * (this.<>f__ref$1.currentlinecount - 1)) / 2f);
            float y = (this.index / this.<>f__ref$1.currentlinecount) * -140f;
            transform.anchoredPosition = new Vector2(x, y);
            transform.localScale = Vector3.one * 0.3f;
            ShortcutExtensions.DOScale(transform, Vector3.one, 0.2f);
        }
    }

    [CompilerGenerated]
    private sealed class <InitUI>c__AnonStorey1
    {
        internal int currentlinecount;
        internal RewardSimpleUICtrl $this;

        internal void <>m__0()
        {
            this.$this.mTapCloseCtrl.Show(true);
        }
    }
}

