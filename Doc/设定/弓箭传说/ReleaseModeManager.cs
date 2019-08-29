using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ReleaseModeManager
{
    private GameMode mMode;
    private DeadGoodMgr mDeadGoodMgr = new DeadGoodMgr();
    private Transform goodsParent;
    private GameObject MoveJoy;
    private float mStartTime;
    private RoomGenerateBase _RoomGenerate;
    public Action<RoomGenerateBase.Room> OnGotoNextRoom;
    [CompilerGenerated]
    private static Action<RoomGenerateBase.Room> <>f__mg$cache0;

    public void CreateGoods(Vector3 pos, List<BattleDropData> goodslist, int radius)
    {
        GameLogic.Hold.Sound.PlayBattleSpecial(0x4c4b49, pos);
        this.mDeadGoodMgr.StartDrop(pos, goodslist, radius, this.goodsParent);
    }

    private void CreateJoy()
    {
        this.MoveJoy = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("Game/UI/MoveJoy"));
        this.MoveJoy.transform.SetParent(GameNode.m_Joy);
        this.MoveJoy.transform.localPosition = Vector3.zero;
        this.MoveJoy.transform.localScale = Vector3.one;
        this.MoveJoy.SetActive(false);
    }

    private void CreatePlayer()
    {
        GameObject obj2 = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("Game/Player/PlayerNode"));
        obj2.transform.parent = GameNode.m_Battle.transform;
        int id = 0x3e9;
        GameLogic.SelfAttribute.Init();
        EntityHero component = obj2.GetComponent<EntityHero>();
        component.Init(id);
        component.transform.position = new Vector3(0f, 1000f, 0f);
        GameLogic.SelfAttribute.InitBabies();
        CameraControlM.Instance.ResetCameraPosition();
    }

    public void DeInit()
    {
        SkillAloneAttrGoodBase.DeInitData();
        this.OnGotoNextRoom = (Action<RoomGenerateBase.Room>) Delegate.Remove(this.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(this.OnGotoNextRoomEvent));
        this.mDeadGoodMgr.DeInit();
        if (this._RoomGenerate != null)
        {
            this._RoomGenerate.DeInit();
            this._RoomGenerate = null;
        }
    }

    public void EnterDoor()
    {
        this.RoomGenerate.EnterDoor();
    }

    public int GetCurrentRoomID()
    {
        if (this.RoomGenerate != null)
        {
            return this.RoomGenerate.GetCurrentRoomID();
        }
        return 0;
    }

    public GameMode GetMode() => 
        this.mMode;

    public GameObject GetMoveJoy() => 
        this.MoveJoy;

    private void GuideEndAction()
    {
        this.RoomGenerate.DeInit();
        this.SwitchModeNotGuide();
        this.RoomGenerate.Init();
        this.RoomGenerate.StartGame();
        this.RoomGenerate.EnterDoor();
    }

    public void Init()
    {
        GameLogic.SetInGame(true);
        Updater.GetUpdater().Init();
        GameLogic.Hold.BattleDataReset();
        GameLogic.SelfAttribute.ClearBattle();
        GameLogic.Release.Form.InitData();
        this.OnGotoNextRoom = (Action<RoomGenerateBase.Room>) Delegate.Combine(this.OnGotoNextRoom, new Action<RoomGenerateBase.Room>(this.OnGotoNextRoomEvent));
        if (<>f__mg$cache0 == null)
        {
            <>f__mg$cache0 = new Action<RoomGenerateBase.Room>(SkillAloneAttrGoodBase.OnGotoNextRoom);
        }
        this.OnGotoNextRoom = (Action<RoomGenerateBase.Room>) Delegate.Combine(this.OnGotoNextRoom, <>f__mg$cache0);
        SkillAloneAttrGoodBase.InitData();
        this.mStartTime = Time.unscaledTime;
        this.mMode = GameLogic.Hold.BattleData.GetMode();
        this.mDeadGoodMgr.Init();
        this.CreatePlayer();
        LocalSave.Instance.BattleIn_Restore();
        this.CreateJoy();
        this.SwitchMode();
        this.startdrop();
    }

    private void OnGotoNextRoomEvent(RoomGenerateBase.Room room)
    {
        GameLogic.Release.MapEffect.MapCache();
        this.mDeadGoodMgr.DeInit();
    }

    public void PlayerDead()
    {
        this.RoomGenerate.PlayerDead();
    }

    public void SetGoodsParent(Transform parent)
    {
        this.goodsParent = parent;
    }

    private void startdrop()
    {
        HeroDropCtrl ctrl = new HeroDropCtrl();
        ctrl.Init();
        ctrl.StartDrop();
    }

    private void SwitchMode()
    {
        if (GameLogic.Hold.Guide.GetNeedGuide())
        {
            this._RoomGenerate = new RoomGenerateLevelGuide();
        }
        else
        {
            this.SwitchModeNotGuide();
        }
        this.RoomGenerate.Init();
        this.RoomGenerate.StartGame();
        this.RoomGenerate.SetGuideEndAction(new Action(this.GuideEndAction));
    }

    private void SwitchModeNotGuide()
    {
        switch (this.mMode)
        {
            case GameMode.eChallenge101:
                this._RoomGenerate = new RoomGenerateChallenge101();
                break;

            case GameMode.eChallenge102:
                this._RoomGenerate = new RoomGenerateChallenge102();
                break;

            case GameMode.eChallenge103:
                this._RoomGenerate = new RoomGenerateChallenge103();
                break;

            case GameMode.eChallenge104:
                this._RoomGenerate = new RoomGenerateChallenge104();
                break;

            case GameMode.eBomberman:
                this._RoomGenerate = new RoomGenerateChallenge101();
                break;

            case GameMode.eBombDodge:
                this._RoomGenerate = new RoomGenerateBombDodge();
                GameLogic.Self.SetCollidersScale(0.7f);
                break;

            case GameMode.eFlyDodge:
                this._RoomGenerate = new RoomGenerateFlyDodge();
                break;

            case GameMode.eLevel:
                this._RoomGenerate = new RoomGenerateLevel();
                break;

            case GameMode.eGold1:
                this._RoomGenerate = new RoomGenerateGold1();
                break;

            case GameMode.eChest1:
                this._RoomGenerate = new RoomGenerateChest1();
                break;

            case GameMode.eMatchDefenceTime:
                this._RoomGenerate = new RoomGenerateMatchDefenceTime();
                break;

            default:
            {
                object[] args = new object[] { base.GetType().ToString(), this.mMode };
                SdkManager.Bugly_Report("ReleaseModeManager.cs", Utils.FormatString("SwitchModeNotGuide In {0} the GameNode.{1} is not achieve!", args));
                break;
            }
        }
    }

    public RoomGenerateBase RoomGenerate =>
        this._RoomGenerate;
}

