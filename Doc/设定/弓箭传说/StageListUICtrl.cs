using PureMVC.Interfaces;
using System;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class StageListUICtrl : MediatorCtrlBase
{
    public RectTransform window;
    public Transform titleparent;
    public Animation titleani;
    public Text Text_Title;
    public GameObject title_normal;
    public GameObject title_hero;
    public ButtonCtrl Button_Close;
    public ScrollIntStageListCtrl mScrollInt;
    public Transform mScrollChild;
    public GameObject copyStage;
    public ButtonCtrl Button_Change;
    public Text Text_Change;
    public GameObject lockparent;
    public Text Text_Lock;
    private int mCurrentStage;
    private int mCurrentMaxStage;
    private int MaxStage;
    private bool m_bHeroModeUnlock;
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    private void InitUI()
    {
    }

    private void OnBeginDrag()
    {
        this.titleani.Play("Info_Hide");
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
            <>f__am$cache0 = () => WindowUI.CloseWindow(WindowID.WindowID_StageList);
        }
        this.Button_Close.onClick = <>f__am$cache0;
        this.copyStage.SetActive(false);
        this.mScrollInt.Speed = 2f;
        this.mScrollInt.copyItem = this.copyStage;
        this.mScrollInt.mScrollChild = this.mScrollChild;
        this.mScrollInt.OnUpdateOne = new Action<int, StageListOneCtrl>(this.UpdateOne);
        this.mScrollInt.OnUpdateSize = new Action<int, StageListOneCtrl>(this.UpdateSize);
        this.mScrollInt.OnBeginDragEvent = new Action(this.OnBeginDrag);
        this.mScrollInt.OnScrollEnd = new Action<int, StageListOneCtrl>(this.OnScrollEnd);
        this.Button_Change.onClick = delegate {
            GameLogic.Hold.BattleData.Level_CurrentStage = this.mCurrentStage;
            this.Button_Close.onClick();
        };
    }

    public override void OnLanguageChange()
    {
        this.Text_Change.text = GameLogic.Hold.Language.GetLanguageByTID("Chapter_Change", Array.Empty<object>());
    }

    protected override void OnOpen()
    {
        this.titleani.Play("Info_Show");
        this.m_bHeroModeUnlock = LocalSave.Instance.Stage_GetStage() > 10;
        if (!this.m_bHeroModeUnlock)
        {
            this.MaxStage = 10;
        }
        else
        {
            this.MaxStage = LocalModelManager.Instance.Stage_Level_stagechapter.GetMaxChapter();
        }
        this.mCurrentMaxStage = LocalSave.Instance.Stage_GetStage();
        WindowUI.ShowCurrency(WindowID.WindowID_Currency);
        this.mScrollInt.Init(this.MaxStage + 1);
        this.mCurrentStage = GameLogic.Hold.BattleData.Level_CurrentStage;
        this.mScrollInt.GotoInt(this.mCurrentStage - 1, false);
        this.InitUI();
    }

    private void OnScrollEnd(int index, StageListOneCtrl one)
    {
        this.titleani.Play("Info_Show");
        this.mCurrentStage = index + 1;
        bool flag = GameLogic.Hold.BattleData.IsHeroMode(this.mCurrentStage);
        this.title_normal.SetActive(!flag);
        this.title_hero.SetActive(flag);
        if (this.mCurrentStage <= this.MaxStage)
        {
            if (this.mCurrentStage > this.mCurrentMaxStage)
            {
                this.Button_Change.gameObject.SetActive(false);
                this.lockparent.SetActive(true);
                object[] args = new object[] { this.mCurrentStage - 1 };
                this.Text_Lock.text = GameLogic.Hold.Language.GetLanguageByTID("Chapter_UnlockGoal", args);
            }
            else
            {
                this.Button_Change.gameObject.SetActive(true);
                this.lockparent.SetActive(false);
            }
            this.Text_Title.text = LocalModelManager.Instance.Stage_Level_stagechapter.GetChapterFullName(this.mCurrentStage);
        }
        else
        {
            this.Button_Change.gameObject.SetActive(false);
            this.lockparent.SetActive(false);
            this.Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("stagelist_hero_title", Array.Empty<object>());
        }
        this.UpdateUI();
    }

    private void UpdateOne(int index, StageListOneCtrl one)
    {
        int stage = index + 1;
        one.Init(stage, stage <= this.mCurrentMaxStage);
    }

    private void UpdateSize(int index, StageListOneCtrl one)
    {
    }

    private void UpdateUI()
    {
    }
}

