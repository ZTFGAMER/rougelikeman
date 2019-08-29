using PureMVC.Interfaces;
using System;
using UnityEngine;

public class BattleChallengeUICtrl101 : BattleLevelUICtrl
{
    public Transform parent;
    private ChallengeHideCtrl mHideCtrl;

    protected override void OnClose()
    {
        base.OnClose();
        if (this.mHideCtrl != null)
        {
            this.mHideCtrl.DeInit();
            this.mHideCtrl.gameObject.SetActive(false);
        }
    }

    public override object OnGetEvent(string eventName) => 
        base.OnGetEvent(eventName);

    public override void OnHandleNotification(INotification notification)
    {
        base.OnHandleNotification(notification);
    }

    protected override void OnInit()
    {
        base.OnInit();
    }

    public override void OnLanguageChange()
    {
        base.OnLanguageChange();
    }

    protected override void OnOpen()
    {
        if (base.mExpCtrl != null)
        {
            base.mExpCtrl.SetDropExp(GameLogic.Hold.BattleData.Challenge_DropExp());
        }
        base.OnOpen();
        GameLogic.Hold.BattleData.Challenge_SetUIParent(this.parent);
        if (GameLogic.Hold.BattleData.Challenge_MonsterHide())
        {
            if (this.mHideCtrl == null)
            {
                this.mHideCtrl = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("UIPanel/BattleUI/ChallengeHide")).GetComponent<ChallengeHideCtrl>();
            }
            this.mHideCtrl.transform.SetParentNormal(GameNode.m_Front);
            this.mHideCtrl.gameObject.SetActive(true);
            this.mHideCtrl.Init();
        }
    }
}

