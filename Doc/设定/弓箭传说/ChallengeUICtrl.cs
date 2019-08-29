using DG.Tweening;
using PureMVC.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeUICtrl : MediatorCtrlBase
{
    public Text Text_Title;
    public ButtonCtrl Button_Close;
    public ButtonCtrl Button_Shadow;
    public ChallengeInfoCtrl mInfoCtrl;
    public ScrollRectBase mScrollRect;
    public GridLayoutGroup scrollGroup;
    public GameObject copyitems;
    public GameObject copyitem;
    private LocalUnityObjctPool mPool;
    private List<Stage_Level_activity> mList;
    private int mCurrentID;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    private void InitUI()
    {
        this.mPool.Collect<ChallengeOneCtrl>();
        this.mCurrentID = LocalSave.Instance.Challenge_GetID();
        this.mInfoCtrl.transform.localScale = Vector3.zero;
        this.mList = LocalModelManager.Instance.Stage_Level_activity.GetChallengeList();
        int count = this.mList.Count;
        for (int i = 0; i < count; i++)
        {
            ChallengeOneCtrl ctrl = this.mPool.DeQueue<ChallengeOneCtrl>();
            ctrl.transform.SetParentNormal(this.mScrollRect.get_content());
            ctrl.Init(i, this.mList[i], count);
        }
        this.mScrollRect.SetWhole(this.scrollGroup, count);
        this.mScrollRect.UseDrag = false;
        if (LocalSave.Instance.Challenge_IsFirstIn())
        {
            <InitUI>c__AnonStorey0 storey = new <InitUI>c__AnonStorey0 {
                $this = this,
                beforeid = (this.mCurrentID - 0x835) - 1
            };
            this.mScrollRect.SetPage(storey.beforeid, false, null);
            this.mInfoCtrl.Init(this.mCurrentID - 1);
            this.PlayInfo(true);
            WindowUI.ShowMask(true);
            TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.5f), new TweenCallback(storey, this.<>m__0));
        }
        else
        {
            this.mScrollRect.SetPage(this.mCurrentID - 0x835, false, null);
            this.mInfoCtrl.Init(this.mCurrentID);
            this.PlayInfo(true);
        }
    }

    private void MoveToNext()
    {
        this.PlayInfo(true);
        this.mInfoCtrl.Init(this.mCurrentID);
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
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = delegate {
                GameLogic.Hold.BattleData.Challenge_DeInit();
                WindowUI.CloseWindow(WindowID.WindowID_Challenge);
            };
        }
        this.Button_Close.onClick = <>f__am$cache0;
        this.Button_Shadow.onClick = this.Button_Close.onClick;
        this.copyitems.SetActive(false);
        this.mPool = LocalUnityObjctPool.Create(base.gameObject);
        this.mPool.CreateCache<ChallengeOneCtrl>(this.copyitem);
    }

    public override void OnLanguageChange()
    {
    }

    protected override void OnOpen()
    {
        this.InitUI();
    }

    private void PlayInfo(bool show)
    {
        if (show)
        {
            this.mInfoCtrl.transform.localScale = Vector3.zero;
            TweenSettingsExtensions.Append(DOTween.Sequence(), TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.mInfoCtrl.transform, 1f, 0.25f), 0x1b));
        }
        else
        {
            this.mInfoCtrl.transform.localScale = Vector3.one;
            TweenSettingsExtensions.Append(DOTween.Sequence(), TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.mInfoCtrl.transform, 0f, 0.25f), 0x1a));
        }
    }

    [CompilerGenerated]
    private sealed class <InitUI>c__AnonStorey0
    {
        internal int beforeid;
        internal ChallengeUICtrl $this;

        internal void <>m__0()
        {
            LocalSave.Instance.Challenge_SetFirstIn();
            this.$this.PlayInfo(false);
            this.$this.mScrollRect.SetPage(this.beforeid + 1, true, new Action(this.$this.MoveToNext));
            WindowUI.ShowMask(false);
        }
    }
}

