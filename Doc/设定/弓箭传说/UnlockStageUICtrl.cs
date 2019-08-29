using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class UnlockStageUICtrl : MediatorCtrlBase
{
    public ButtonCtrl Button_Close;
    public Text Text_Title;
    public CanvasGroup titlecanvas;
    public CanvasGroup levelcanvas;
    public CanvasGroup infocanvas;
    public CanvasGroup skillcanvas;
    public Text Text_Close;
    public Text Text_Info;
    public UnlockStageLevelCtrl mLevelCtrl;
    public UnlockStageSkillCtrl mSkillCtrl;
    private UnlockStageProxy.Transfer mTransfer;
    private Tweener t_close;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    private void InitUI()
    {
        <InitUI>c__AnonStorey0 storey = new <InitUI>c__AnonStorey0 {
            $this = this
        };
        this.mLevelCtrl.Init(this.mTransfer.StageID);
        object[] args = new object[] { this.mTransfer.StageID };
        this.Text_Info.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("ChapterInfo_{0}", args), Array.Empty<object>());
        this.Button_Close.gameObject.SetActive(false);
        this.Text_Close.gameObject.SetActive(false);
        this.titlecanvas.alpha = 0f;
        this.levelcanvas.alpha = 0f;
        this.infocanvas.alpha = 0f;
        this.skillcanvas.alpha = 0f;
        (this.titlecanvas.transform as RectTransform).anchoredPosition = new Vector2(0f, -700f);
        this.mLevelCtrl.transform.localPosition = Vector3.zero;
        this.infocanvas.transform.localPosition = Vector3.zero;
        this.mSkillCtrl.transform.localPosition = Vector3.zero;
        storey.time = 0.3f;
        float num = 0f;
        Sequence seq = DOTween.Sequence();
        TweenSettingsExtensions.AppendCallback(seq, new TweenCallback(storey, this.<>m__0));
        TweenSettingsExtensions.AppendInterval(seq, storey.time + num);
        TweenSettingsExtensions.AppendCallback(seq, new TweenCallback(storey, this.<>m__1));
        TweenSettingsExtensions.AppendInterval(seq, storey.time + num);
        TweenSettingsExtensions.AppendCallback(seq, new TweenCallback(storey, this.<>m__2));
        TweenSettingsExtensions.AppendInterval(seq, storey.time + num);
        if (this.mSkillCtrl.GetUnlockSkillCount(this.mTransfer.StageID) > 0)
        {
            TweenSettingsExtensions.AppendCallback(seq, new TweenCallback(storey, this.<>m__3));
            TweenSettingsExtensions.AppendInterval(seq, storey.time + num);
            this.mSkillCtrl.Init(seq, this.mTransfer.StageID);
            TweenSettingsExtensions.AppendInterval(seq, 0.5f);
        }
        TweenSettingsExtensions.AppendCallback(seq, new TweenCallback(storey, this.<>m__4));
    }

    protected override void OnClose()
    {
        this.mSkillCtrl.DeInit();
    }

    public override object OnGetEvent(string eventName) => 
        null;

    public override void OnHandleNotification(INotification notification)
    {
    }

    protected override void OnInit()
    {
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_UnlockStage);
        }
        this.Button_Close.onClick = <>f__am$cache0;
    }

    public override void OnLanguageChange()
    {
        this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("UnlockNewStage", Array.Empty<object>());
        this.Text_Close.text = GameLogic.Hold.Language.GetLanguageByTID("TapToClose", Array.Empty<object>());
    }

    protected override void OnOpen()
    {
        IProxy proxy = Facade.Instance.RetrieveProxy("UnlockStageProxy");
        if (proxy == null)
        {
            SdkManager.Bugly_Report("UnlockStageUICtrl", "OnOpen proxy is null.");
            this.Button_Close.onClick();
        }
        if (proxy.Data == null)
        {
            SdkManager.Bugly_Report("UnlockStageUICtrl", "OnOpen proxy.Data is null.");
            this.Button_Close.onClick();
        }
        if (!(proxy.Data is UnlockStageProxy.Transfer))
        {
            SdkManager.Bugly_Report("UnlockStageUICtrl", "OnOpen proxy.Data is not UnlockStageProxy.Transfer.");
            this.Button_Close.onClick();
        }
        this.mTransfer = proxy.Data as UnlockStageProxy.Transfer;
        this.InitUI();
    }

    [CompilerGenerated]
    private sealed class <InitUI>c__AnonStorey0
    {
        internal float time;
        internal UnlockStageUICtrl $this;

        internal void <>m__0()
        {
            Sequence sequence = DOTween.Sequence();
            TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.$this.titlecanvas, 1f, this.time));
            RectTransform transform = this.$this.titlecanvas.transform as RectTransform;
            TweenSettingsExtensions.Join(sequence, ShortcutExtensions46.DOAnchorPosY(transform, -600f, this.time, false));
        }

        internal void <>m__1()
        {
            Sequence sequence = DOTween.Sequence();
            TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.$this.levelcanvas, 1f, this.time));
            TweenSettingsExtensions.Join(sequence, ShortcutExtensions.DOLocalMoveY(this.$this.levelcanvas.transform, 100f, this.time, false));
        }

        internal void <>m__2()
        {
            Sequence sequence = DOTween.Sequence();
            TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.$this.infocanvas, 1f, this.time));
            TweenSettingsExtensions.Join(sequence, ShortcutExtensions.DOLocalMoveY(this.$this.infocanvas.transform, 100f, this.time, false));
        }

        internal void <>m__3()
        {
            Sequence sequence = DOTween.Sequence();
            TweenSettingsExtensions.Append(sequence, ShortcutExtensions46.DOFade(this.$this.skillcanvas, 1f, this.time));
            TweenSettingsExtensions.Join(sequence, ShortcutExtensions.DOLocalMoveY(this.$this.skillcanvas.transform, 100f, this.time, false));
        }

        internal void <>m__4()
        {
            this.$this.Text_Close.gameObject.SetActive(true);
            this.$this.Button_Close.gameObject.SetActive(true);
        }
    }
}

