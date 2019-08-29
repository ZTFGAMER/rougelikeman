using Dxx.Util;
using PureMVC.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameOverModuleMediator : WindowMediator, IMediator, INotifier
{
    public const string NAME = "GameOverModuleMediator";
    private static Dictionary<GameMode, MediatorCtrlBase> mModeCtrlList = new Dictionary<GameMode, MediatorCtrlBase>();
    private static MediatorCtrlBase mCurrentModeCtrl;

    public GameOverModuleMediator() : base("GameOverUI")
    {
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
        object[] objArray2;
        GameMode key = GameLogic.Hold.BattleData.GetMode();
        mCurrentModeCtrl = null;
        mModeCtrlList.TryGetValue(key, out mCurrentModeCtrl);
        if (mCurrentModeCtrl != null)
        {
            goto Label_01C7;
        }
        string str = string.Empty;
        if (GameLogic.Hold.BattleData.isEnterSourceMain())
        {
            str = "GameOverLevel";
        }
        else if (GameLogic.Hold.BattleData.isEnterSourceMatch())
        {
            str = "GameOverMatchDefenceTime";
        }
        else
        {
            switch (key)
            {
                case GameMode.eChallenge101:
                case GameMode.eChallenge102:
                case GameMode.eChallenge103:
                case GameMode.eChallenge104:
                    str = "GameOverChallenge";
                    goto Label_014D;

                case GameMode.eBomberman:
                case GameMode.eBombDodge:
                case GameMode.eFlyDodge:
                case GameMode.eLevel:
                    str = "GameOverLevel";
                    goto Label_014D;

                case GameMode.eGold1:
                    str = "GameOverLevel";
                    goto Label_014D;

                case GameMode.eChest1:
                    str = "GameOverLevel";
                    goto Label_014D;

                case GameMode.eMatchDefenceTime:
                    str = "GameOverMatchDefenceTime";
                    goto Label_014D;
            }
            object[] args = new object[] { base.GetType().ToString(), key };
            SdkManager.Bugly_Report("GameOverModuleMediator", Utils.FormatString("OnRegisterOnce In {0} the GameNode.{1} is not achieve!", args));
        }
    Label_014D:
        objArray2 = new object[] { str };
        GameObject child = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>(Utils.FormatString("UIPanel/GameOverUI/{0}", objArray2)));
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
    Label_01C7:
        GameLogic.SetPause(true);
        mCurrentModeCtrl.Open();
        mCurrentModeCtrl.OnLanguageChange();
        LocalSave.LocalSaveExtra saveExtra = LocalSave.Instance.SaveExtra;
        saveExtra.overopencount++;
        LocalSave.Instance.SaveExtra.Refresh();
    }

    protected override void OnRegisterOnce()
    {
    }

    protected override void OnRemoveAfter()
    {
        GameLogic.Hold.BattleData.ActiveID = 0;
        GameLogic.Hold.BattleData.Challenge_DeInit();
        GameLogic.SetInGame(false);
        GameLogic.SetPause(false);
        if (mCurrentModeCtrl != null)
        {
            mCurrentModeCtrl.Close();
        }
    }

    public override List<string> OnListNotificationInterests =>
        new List<string>();
}

