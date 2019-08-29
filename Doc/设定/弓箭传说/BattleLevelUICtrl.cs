using DG.Tweening;
using PureMVC.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleLevelUICtrl : MediatorCtrlBase
{
    public ButtonCtrl Button_Pause;
    public BattleExpCtrl mExpCtrl;
    public BattleBossHPCtrl mHPCtrl;
    public BattleGoldCtrl mGoldCtrl;
    public GameObject copyitems;
    public GameObject copyGold;
    public RectTransform Image_Gold;
    public BattleLevelAchieveCtrl mAchieveCtrl;
    public Transform challenge_parent;
    private ActionUpdateCtrl mActionUpdateCtrl;
    private BattleLevelWaveCtrl mLevelWaveCtrl;
    private Sequence seq_levelup;
    private int levelupCount;
    private Tweener tGold;
    private List<long> getgoldlist = new List<long>();
    private bool bGoldAniPlaying;
    private LocalUnityObjctPool mObjPool;
    [CompilerGenerated]
    private static TweenCallback <>f__am$cache0;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map7;

    private void CacheGoldText(MainUIGoldAddCtrl ctrl)
    {
        this.mObjPool.EnQueue<MainUIGoldAddCtrl>(ctrl.gameObject);
    }

    private void challenge_init()
    {
        this.mAchieveCtrl.Show(true);
        this.mGoldCtrl.gameObject.SetActive(false);
        GameLogic.Hold.BattleData.Challenge_SetUIParent(this.challenge_parent);
    }

    private void init_level_wave()
    {
        if (this.mLevelWaveCtrl == null)
        {
            GameObject child = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("UIPanel/BattleUI/waveparent"));
            child.SetParentNormal(this.mExpCtrl.transform.parent);
            this.mLevelWaveCtrl = child.GetComponent<BattleLevelWaveCtrl>();
            this.mLevelWaveCtrl.SetActive(false);
        }
    }

    private void InitUI()
    {
        this.mAchieveCtrl.Show(false);
        this.mGoldCtrl.gameObject.SetActive(true);
        WindowUI.CloseCurrency();
        if (this.Button_Pause != null)
        {
            this.Button_Pause.gameObject.SetActive(!GameLogic.Hold.Guide.GetNeedGuide());
            this.Button_Pause.onClick = new Action(this.OnClickPause);
        }
        ReleaseModeManager mode = GameLogic.Release.Mode;
        mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>) Delegate.Combine(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(this.OnGotoNextRoom));
        GameLogic.Hold.Sound.PlayBackgroundMusic(SoundManager.BackgroundMusicType.eBattle);
        CameraControlM.Instance.PlayStartAnimate(null);
        if (this.mExpCtrl != null)
        {
            this.mExpCtrl.Init();
        }
        if (this.mHPCtrl != null)
        {
            this.mHPCtrl.Init();
        }
        this.mActionUpdateCtrl = new ActionUpdateCtrl();
        this.mActionUpdateCtrl.Init(false);
        this.UpdateGold();
        this.ShowBossHP(false);
        this.StartGame();
    }

    private void OnClickPause()
    {
        WindowUI.ShowWindow(WindowID.WindowID_Pause);
    }

    protected override void OnClose()
    {
        if (this.mHPCtrl != null)
        {
            this.mHPCtrl.DeInit();
        }
        if (this.seq_levelup != null)
        {
            TweenExtensions.Kill(this.seq_levelup, false);
        }
        if (this.mExpCtrl != null)
        {
            this.mExpCtrl.DeInit();
        }
        this.mObjPool.Collect<MainUIGoldAddCtrl>();
        ReleaseModeManager mode = GameLogic.Release.Mode;
        mode.OnGotoNextRoom = (Action<RoomGenerateBase.Room>) Delegate.Remove(mode.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(this.OnGotoNextRoom));
        this.mActionUpdateCtrl.DeInit();
        GameLogic.SetGameState(GameLogic.EGameState.Over);
        GameLogic.Release.Release();
        if (this.mLevelWaveCtrl != null)
        {
            this.mLevelWaveCtrl.Deinit();
            Object.Destroy(this.mLevelWaveCtrl.gameObject);
        }
    }

    private void OnCloseLevelUpUI()
    {
        this.levelupCount--;
        if (this.levelupCount < 0)
        {
            this.levelupCount = 0;
        }
        if (this.levelupCount > 0)
        {
            this.OpenLevelUpUI();
        }
    }

    public override object OnGetEvent(string eventName)
    {
        if ((eventName != null) && (eventName == "Event_GetGoldPosition"))
        {
            return this.Image_Gold.position;
        }
        return null;
    }

    private void OnGotoNextRoom(RoomGenerateBase.Room room)
    {
        if ((GameLogic.Hold.Guide != null) && (this.Button_Pause != null))
        {
            this.Button_Pause.gameObject.SetActive(!GameLogic.Hold.Guide.GetNeedGuide());
        }
        else
        {
            this.Button_Pause.gameObject.SetActive(true);
        }
    }

    public override void OnHandleNotification(INotification notification)
    {
        string name = notification.Name;
        object body = notification.Body;
        if (name != null)
        {
            if (<>f__switch$map7 == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(9) {
                    { 
                        "BATTLE_GET_GOLD",
                        0
                    },
                    { 
                        "BATTLE_GAMEOVER",
                        1
                    },
                    { 
                        "BATTLE_UI_BOSSHP_UPDATE",
                        2
                    },
                    { 
                        "BATTLE_EXP_UP",
                        3
                    },
                    { 
                        "BATTLE_LEVEL_UP",
                        4
                    },
                    { 
                        "BATTLE_CHOOSESKILL_TO_BATTLE_CLOSE",
                        5
                    },
                    { 
                        "BATTLE_ROOM_TYPE",
                        6
                    },
                    { 
                        "Currency_BattleKey",
                        7
                    },
                    { 
                        "BattleUI_level_wave_update",
                        8
                    }
                };
                <>f__switch$map7 = dictionary;
            }
            if (<>f__switch$map7.TryGetValue(name, out int num))
            {
                switch (num)
                {
                    case 0:
                        this.UpdateGold();
                        break;

                    case 1:
                        if (!GameConfig.GetCanOpenRateUI())
                        {
                            WindowUI.ShowWindow(WindowID.WindowID_GameOver);
                            break;
                        }
                        WindowUI.ShowWindow(WindowID.WindowID_Rate);
                        break;

                    case 2:
                        if (this.mHPCtrl != null)
                        {
                            this.ShowBossHP(true);
                            this.mHPCtrl.UpdateBossHP((float) body);
                            if (!this.mHPCtrl.IsShow() && (this.mExpCtrl != null))
                            {
                                this.mExpCtrl.Show(true);
                            }
                        }
                        break;

                    case 3:
                        if (this.mExpCtrl != null)
                        {
                            this.mExpCtrl.ExpUP((ProgressAniManager) body);
                        }
                        break;

                    case 4:
                    {
                        GameLogic.Hold.Sound.PlayUI(0x4c4b47);
                        int level = GameLogic.Self.m_EntityData.GetLevel();
                        this.levelupCount++;
                        if (this.mExpCtrl != null)
                        {
                            this.mExpCtrl.SetLevel(level);
                        }
                        if (this.levelupCount == 1)
                        {
                            this.OpenLevelUpUI();
                        }
                        break;
                    }
                    case 5:
                        this.OnCloseLevelUpUI();
                        break;

                    case 6:
                    {
                        RoomGenerateBase.RoomType type = (RoomGenerateBase.RoomType) body;
                        if (type != RoomGenerateBase.RoomType.eBoss)
                        {
                            this.ShowBossHP(false);
                            break;
                        }
                        break;
                    }
                    case 7:
                        if (!((bool) body))
                        {
                            WindowUI.CloseCurrency();
                            break;
                        }
                        WindowUI.ShowCurrency(WindowID.WindowID_CurrencyBattleKey);
                        break;

                    case 8:
                    {
                        BattleLevelWaveData data = (BattleLevelWaveData) body;
                        this.init_level_wave();
                        this.mLevelWaveCtrl.SetInfo(data);
                        break;
                    }
                }
            }
        }
    }

    protected override void OnInit()
    {
        if (this.copyGold != null)
        {
            this.mObjPool = LocalUnityObjctPool.Create(base.gameObject);
            this.mObjPool.CreateCache<MainUIGoldAddCtrl>(this.copyGold);
        }
        this.copyitems.SetActive(false);
    }

    public override void OnLanguageChange()
    {
    }

    protected override void OnOpen()
    {
        SdkManager.send_event_game_start(BattleSource.eWorld, GameLogic.Hold.BattleData.Level_CurrentStage);
        this.InitUI();
    }

    private void OpenLevelUpUI()
    {
        GameLogic.Self.LevelUp();
        if (<>f__am$cache0 == null)
        {
            <>f__am$cache0 = new TweenCallback(null, <OpenLevelUpUI>m__0);
        }
        this.seq_levelup = TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(DOTween.Sequence(), 0.8f), <>f__am$cache0);
    }

    protected void ShowBossHP(bool show)
    {
        if (this.mHPCtrl != null)
        {
            this.mHPCtrl.Show(show);
        }
        if (this.mExpCtrl != null)
        {
            this.mExpCtrl.Show(!show);
        }
    }

    private void StartGame()
    {
        GameLogic.SetGameState(GameLogic.EGameState.Gaming);
    }

    private void UpdateGold()
    {
        if (this.mGoldCtrl != null)
        {
            float gold = GameLogic.Hold.BattleData.GetGold();
            this.mGoldCtrl.SetGold((long) gold);
        }
    }
}

