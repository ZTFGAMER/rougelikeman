using DG.Tweening;
using Dxx.Net;
using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Runtime.CompilerServices;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class MainUIPageBattleCtrl : MediatorCtrlBase
{
    public GameObject window;
    public ButtonCtrl Button_Start;
    public RectTransform ButtonParent;
    public Text Text_Start;
    public Text Text_BestStage;
    public Text Text_BestScore;
    public Text Text_ChapterIndex;
    public ButtonCtrl Button_Setting;
    public ButtonCtrl Button_Mail;
    public MainUILevelItem mStageItem;
    public MainUIBattleLayerCtrl mLayerCtrl;
    public RedNodeCtrl mMailRedCtrl;
    public RedNodeCtrl mHarvestRedCtrl;
    public GoldTextCtrl mKeyCtrl;
    public ButtonCtrl Button_ModeTest;
    public ButtonCtrl Button_Change;
    public Text Text_Change;
    public ActiveUICtrl mActiveCtrl;
    public GameObject battleui;
    public GameObject activeui;
    public MainChapterCtrl mChapterCtrl;
    public ButtonCtrl Button_Harvest;
    private int currentStage;
    private float maily;
    private int mKeyCount;
    private bool bOpened;
    [CompilerGenerated]
    private static Action <>f__am$cache0;
    [CompilerGenerated]
    private static Action <>f__am$cache1;
    [CompilerGenerated]
    private static Action <>f__am$cache2;
    [CompilerGenerated]
    private static Action <>f__am$cache3;
    [CompilerGenerated]
    private static Action <>f__am$cache4;
    [CompilerGenerated]
    private static Action <>f__am$cache5;

    private void CheckUnlockStage()
    {
        if (LocalSave.Instance.Stage_GetFirstIn())
        {
            this.UpdateLayer();
            GameLogic.Hold.BattleData.Level_CurrentStage = LocalSave.Instance.Stage_GetStage();
            LocalSave.Instance.Stage_SetFirstIn();
            UnlockStageProxy.Transfer data = new UnlockStageProxy.Transfer {
                StageID = GameLogic.Hold.BattleData.Level_CurrentStage
            };
            Facade.Instance.RegisterProxy(new UnlockStageProxy(data));
            WindowUI.ShowWindow(WindowID.WindowID_UnlockStage);
        }
    }

    private void InitUI()
    {
        this.mKeyCtrl.SetValue(GameConfig.GetModeLevelKey());
        LocalSave instance = LocalSave.Instance;
        instance.OnMaxLevelUpdate = (Action) Delegate.Combine(instance.OnMaxLevelUpdate, new Action(this.UpdateBest));
        this.OnChangeState();
        this.UpdateLayer();
        if (<>f__am$cache4 == null)
        {
            <>f__am$cache4 = () => WindowUI.ShowWindow(WindowID.WindowID_Setting);
        }
        this.Button_Setting.onClick = <>f__am$cache4;
        if (<>f__am$cache5 == null)
        {
            <>f__am$cache5 = () => WindowUI.ShowWindow(WindowID.WindowID_Mail);
        }
        this.Button_Mail.onClick = <>f__am$cache5;
        this.Button_Start.onClick = new Action(this.OnClickPlay);
        this.CheckUnlockStage();
        this.update_mail();
        this.UpdateNet();
        this.update_harvest();
    }

    private void OnChangeState()
    {
        this.Text_Change.text = (this.mState != eBattleState.eBattle) ? "主线" : "活动";
        switch (this.mState)
        {
            case eBattleState.eBattle:
                this.Text_Change.text = "活动";
                break;

            case eBattleState.eActive:
                this.Text_Change.text = "主线";
                break;
        }
        this.battleui.SetActive(this.mState == eBattleState.eBattle);
        this.activeui.SetActive(this.mState == eBattleState.eActive);
    }

    private void OnClickPlay()
    {
        int modeLevelKey = GameConfig.GetModeLevelKey();
        if (!NetManager.IsNetConnect && !LocalSave.Instance.TrustCount_Use((short) modeLevelKey))
        {
            CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NeedNet, Array.Empty<string>());
        }
        else if (LocalSave.Instance.GetKey() < this.mKeyCount)
        {
            KeyBuyUICtrl.SetSource(KeyBuySource.EMAIN_BATTLE);
            WindowUI.ShowWindow(WindowID.WindowID_KeyBuy);
        }
        else
        {
            Facade.Instance.SendNotification("UseCurrencyKey");
            WindowUI.ShowMask(true);
            TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.4f), new TweenCallback(this, this.<OnClickPlay>m__7));
        }
    }

    protected override void OnClose()
    {
        this.bOpened = false;
        LocalSave instance = LocalSave.Instance;
        instance.OnMaxLevelUpdate = (Action) Delegate.Remove(instance.OnMaxLevelUpdate, new Action(this.UpdateBest));
        this.mActiveCtrl.Close();
    }

    public override object OnGetEvent(string eventName)
    {
        object obj2 = this.mActiveCtrl.OnGetEvent(eventName);
        if (obj2 != null)
        {
            return obj2;
        }
        return null;
    }

    public override void OnHandleNotification(INotification notification)
    {
        this.mActiveCtrl.OnHandleNotification(notification);
        string name = notification.Name;
        object body = notification.Body;
        if (name != null)
        {
            if (name == "PUB_NETCONNECT_UPDATE")
            {
                this.UpdateNet();
            }
            else if (name == "MainUI_MailUpdate")
            {
                this.update_mail();
            }
            else if (name == "MainUI_LayerUpdate")
            {
                this.UpdateLayer();
                this.OnLanguageChange();
            }
            else if (name == "MainUI_HarvestUpdate")
            {
                this.update_harvest();
            }
        }
    }

    protected override void OnInit()
    {
        this.mKeyCount = GameConfig.GetModeLevelKey();
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = () => WindowUI.ShowWindow(WindowID.WindowID_StageList);
        }
        this.mStageItem.OnButtonClick = <>f__am$cache0;
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = () => WindowUI.ShowWindow(WindowID.WindowID_LayerBox);
        }
        this.mLayerCtrl.OnLayerClick = <>f__am$cache1;
        if (<>f__am$cache2 == null)
        {
            <>f__am$cache2 = delegate {
                if (NetManager.NetTime > 0L)
                {
                    WindowUI.ShowWindow(WindowID.WindowID_AdHarvest);
                }
                else
                {
                    CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError, Array.Empty<string>());
                }
            };
        }
        this.Button_Harvest.onClick = <>f__am$cache2;
        if (<>f__am$cache3 == null)
        {
            <>f__am$cache3 = delegate {
                GameLogic.Hold.BattleData.Challenge_UpdateMode(0xbb9);
                WindowUI.ShowWindow(WindowID.WindowID_Battle);
            };
        }
        this.Button_ModeTest.onClick = <>f__am$cache3;
        this.Button_Change.onClick = delegate {
            GameLogic.Hold.BattleData.Challenge_UpdateMode(0xbba);
            WindowUI.ShowWindow(WindowID.WindowID_Battle);
        };
        float fringeHeight = PlatformHelper.GetFringeHeight();
        RectTransform parent = this.Button_Mail.transform.parent.parent as RectTransform;
        this.maily = parent.anchoredPosition.y;
        parent.anchoredPosition = new Vector2(parent.anchoredPosition.x, this.maily + fringeHeight);
        RectTransform transform2 = this.Button_Setting.transform.parent as RectTransform;
        transform2.anchoredPosition = new Vector2(transform2.anchoredPosition.x, this.maily + fringeHeight);
        float bottomHeight = PlatformHelper.GetBottomHeight();
        RectTransform transform3 = this.Button_Start.transform.parent as RectTransform;
        transform3.anchoredPosition = new Vector2(transform3.anchoredPosition.x, transform3.anchoredPosition.y + bottomHeight);
        this.ButtonParent.anchoredPosition = new Vector2(0f, GameLogic.Height * 0.23f);
        RectTransform transform = base.transform as RectTransform;
        (this.battleui.transform as RectTransform).sizeDelta = transform.sizeDelta;
        (this.activeui.transform as RectTransform).sizeDelta = transform.sizeDelta;
        (this.activeui.transform as RectTransform).sizeDelta = transform.sizeDelta;
        this.mActiveCtrl.Init();
    }

    public override void OnLanguageChange()
    {
        this.Text_Start.text = GameLogic.Hold.Language.GetLanguageByTID("Main_StartGame", Array.Empty<object>());
        this.UpdateBest();
        this.mLayerCtrl.OnLanguageChange();
        this.mActiveCtrl.OnLanguageChange();
    }

    protected override void OnOpen()
    {
        this.bOpened = true;
        this.InitUI();
        this.update_harvest_show();
        this.mActiveCtrl.Open();
    }

    private void OnStageUpdate()
    {
        this.Text_ChapterIndex.text = LocalModelManager.Instance.Stage_Level_stagechapter.GetChapterFullName(GameLogic.Hold.BattleData.Level_CurrentStage);
    }

    public void PlayInternal()
    {
        GameLogic.PlayBattle_Main();
    }

    private void update_harvest()
    {
        this.mHarvestRedCtrl.SetType(RedNodeType.eWarning);
        this.mHarvestRedCtrl.Value = !LocalSave.Instance.mHarvest.get_can_reward() ? 0 : 1;
    }

    private void update_harvest_show()
    {
        this.Button_Harvest.gameObject.SetActive(LocalSave.Instance.Card_GetHarvestAvailable());
    }

    private void update_mail()
    {
        this.mMailRedCtrl.SetType(RedNodeType.eRedCount);
        this.mMailRedCtrl.Value = LocalSave.Instance.Mail.GetRedCount();
    }

    private void UpdateBest()
    {
        string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("最高得分", Array.Empty<object>());
        object[] args = new object[] { languageByTID, LocalSave.Instance.GetScore() };
        this.Text_BestScore.text = Utils.FormatString("{0} : {1}", args);
        if (GameLogic.Hold.BattleData.Level_CurrentStage < LocalSave.Instance.mStage.CurrentStage)
        {
            this.Text_BestStage.text = GameLogic.Hold.Language.GetLanguageByTID("Main_PassStage", Array.Empty<object>());
        }
        else
        {
            string str2 = GameLogic.Hold.Language.GetLanguageByTID("最高层数", Array.Empty<object>());
            int currentMaxLevel = LocalModelManager.Instance.Stage_Level_stagechapter.GetCurrentMaxLevel(LocalSave.Instance.mStage.CurrentStage);
            int num2 = LocalSave.Instance.mStage.GetCurrentMaxLevel();
            object[] objArray2 = new object[] { str2, num2, currentMaxLevel };
            this.Text_BestStage.text = Utils.FormatString("{0} : {1}/{2}", objArray2);
        }
        this.OnStageUpdate();
    }

    private void UpdateLayer()
    {
        this.currentStage = GameLogic.Hold.BattleData.Level_CurrentStage;
        this.mStageItem.Init(this.currentStage);
        int maxLevel = LocalSave.Instance.mStage.MaxLevel;
        int nextLevel = LocalModelManager.Instance.Box_ChapterBox.GetNextLevel(LocalSave.Instance.Stage_GetNextID());
        this.mLayerCtrl.SetLayer(maxLevel, nextLevel);
        this.UpdateBest();
    }

    private void UpdateNet()
    {
        this.mLayerCtrl.UpdateNet();
    }

    private eBattleState mState
    {
        get => 
            ((eBattleState) PlayerPrefsEncrypt.GetInt("mainui_battle_state", 0));
        set => 
            PlayerPrefsEncrypt.SetInt("mainui_battle_state", (int) value);
    }

    private enum eBattleState
    {
        eBattle,
        eActive
    }
}

