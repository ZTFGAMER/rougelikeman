using Dxx.Util;
using PureMVC.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleModuleMediator : WindowMediator, IMediator, INotifier
{
    public const string Event_GetGoldPosition = "Event_GetGoldPosition";
    public const string NAME = "BattleModuleMediator";
    private static Dictionary<GameMode, MediatorCtrlBase> mModeCtrlList = new Dictionary<GameMode, MediatorCtrlBase>();
    private static MediatorCtrlBase mCurrentModeCtrl;

    public BattleModuleMediator() : base("BattleUI")
    {
    }

    public override object GetEvent(string eventName)
    {
        if (mCurrentModeCtrl != null)
        {
            return mCurrentModeCtrl.OnGetEvent(eventName);
        }
        return null;
    }

    public override void OnHandleNotification(INotification notification)
    {
        if (mCurrentModeCtrl != null)
        {
            mCurrentModeCtrl.OnHandleNotification(notification);
        }
    }

    protected override void OnLanguageChange()
    {
        if (mCurrentModeCtrl != null)
        {
            mCurrentModeCtrl.OnLanguageChange();
        }
    }

    protected override void OnRegisterEvery()
    {
        GameMode key = GameLogic.Hold.BattleData.GetMode();
        mCurrentModeCtrl = null;
        mModeCtrlList.TryGetValue(key, out mCurrentModeCtrl);
        if (mCurrentModeCtrl != null)
        {
            mCurrentModeCtrl.gameObject.SetActive(true);
            mCurrentModeCtrl.transform.SetAsLastSibling();
        }
        else
        {
            string str;
            switch (key)
            {
                case GameMode.eChallenge101:
                case GameMode.eChallenge102:
                case GameMode.eChallenge103:
                case GameMode.eChallenge104:
                    str = "BattleLevelPanel";
                    break;

                case GameMode.eBomberman:
                    str = "BattleLevelPanel";
                    break;

                case GameMode.eBombDodge:
                    str = "BattleChallengePanel101";
                    break;

                case GameMode.eFlyDodge:
                    str = "BattleLevelPanel";
                    break;

                case GameMode.eLevel:
                    str = "BattleLevelPanel";
                    break;

                case GameMode.eGold1:
                    str = "BattleLevelPanel";
                    break;

                case GameMode.eChest1:
                    str = "BattleLevelPanel";
                    break;

                case GameMode.eMatchDefenceTime:
                    str = "BattleMatchDefenceTime";
                    break;

                default:
                {
                    object[] objArray1 = new object[] { base.GetType().ToString(), key };
                    throw new Exception(Utils.FormatString("In {0} the GameNode.{1} is not achieve!", objArray1));
                }
            }
            object[] args = new object[] { str };
            GameObject child = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>(Utils.FormatString("UIPanel/BattleUI/{0}", args)));
            child.SetParentNormal(base._MonoView.transform);
            mCurrentModeCtrl = child.GetComponentInChildren<MediatorCtrlBase>();
            mCurrentModeCtrl.Init();
            if (mModeCtrlList.ContainsKey(key))
            {
                mModeCtrlList[key] = mCurrentModeCtrl;
            }
            else
            {
                mModeCtrlList.Add(key, mCurrentModeCtrl);
            }
        }
        GameLogic.Release.Mode.Init();
        CameraControlM.Instance.ResetCameraSize();
        mCurrentModeCtrl.Open();
        mCurrentModeCtrl.OnLanguageChange();
        GameLogic.Hold.BattleData.Challenge_Start();
    }

    protected override void OnRegisterOnce()
    {
    }

    protected override void OnRemoveAfter()
    {
        if (mCurrentModeCtrl != null)
        {
            mCurrentModeCtrl.Close();
            mCurrentModeCtrl.gameObject.SetActive(false);
        }
    }

    public override List<string> OnListNotificationInterests =>
        new List<string> { 
            "BATTLE_GAMEOVER",
            "BATTLE_UI_BOSSHP_UPDATE",
            "BATTLE_LEVEL_UP",
            "BATTLE_EXP_UP",
            "BATTLE_CHOOSESKILL_TO_BATTLE_CLOSE",
            "BATTLE_ROOM_TYPE",
            "PUB_UI_UPDATE_CURRENCY",
            "Mode_Greedy_CurrentWaveOver",
            "Mode_Greedy_UpdateCurrentWave",
            "CurrentWaveOver",
            "UpdateCurrentWave",
            "Mode_Adventure_CurrentWaveOver",
            "Mode_Adventure_UpdateCurrentWave",
            "Currency_BattleKey",
            "UpdateWave",
            "BATTLE_GET_GOLD",
            "MatchDefenceTime_me_dead",
            "BattleUI_level_wave_update"
        };
}

