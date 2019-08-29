using DG.Tweening;
using Dxx.Util;
using PureMVC.Interfaces;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleMatchDefenceTimeCtrl : BattleLevelUICtrl
{
    public BattleMatchDefenceTime_DeadCtrl mDeadCtrl;
    private Transform parent;
    private BattleMatchDefenceTime_ConditionCtrl mCtrl;
    private SequencePool mPool = new SequencePool();
    [CompilerGenerated]
    private static Action <>f__am$cache0;

    protected override void OnClose()
    {
        this.mPool.Clear();
        if (this.parent != null)
        {
            this.parent.gameObject.SetActive(false);
        }
        base.OnClose();
    }

    public override object OnGetEvent(string eventName) => 
        base.OnGetEvent(eventName);

    public override void OnHandleNotification(INotification notification)
    {
        base.OnHandleNotification(notification);
        string name = notification.Name;
        object body = notification.Body;
        if (name != null)
        {
            if (name != "BATTLE_GET_GOLD")
            {
                if ((name != "BATTLE_ROOM_TYPE") && (name == "MatchDefenceTime_me_dead"))
                {
                    TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(this.mPool.Get(), 0.6f), new TweenCallback(this, this.<OnHandleNotification>m__0));
                }
            }
            else
            {
                int gold = (int) GameLogic.Hold.BattleData.GetGold();
                GameLogic.Hold.BattleData.Challenge_SendEvent("MatchDefenceTime_me_updatescore", gold);
                Singleton<MatchDefenceTimeSocketCtrl>.Instance.Send(MatchMessageType.eScoreUpdate, gold);
            }
        }
    }

    protected override void OnInit()
    {
        base.OnInit();
        GameObject child = Object.Instantiate<GameObject>(ResourceManager.Load<GameObject>("UIPanel/BattleUI/BattleMatchDefenceTime_Condition"));
        child.SetParentNormal(GameNode.m_InGame2);
        RectTransform transform = child.transform as RectTransform;
        transform.anchoredPosition = new Vector2(0f, PlatformHelper.GetFringeHeight());
        this.parent = transform;
        this.mCtrl = this.parent.GetComponent<BattleMatchDefenceTime_ConditionCtrl>();
        base.mExpCtrl.SetFringe();
        this.mDeadCtrl.Show(false);
    }

    public override void OnLanguageChange()
    {
        base.OnLanguageChange();
    }

    protected override void OnOpen()
    {
        this.mPool.Clear();
        if (base.mExpCtrl != null)
        {
            base.mExpCtrl.SetDropExp(GameLogic.Hold.BattleData.Challenge_DropExp());
        }
        if (this.parent != null)
        {
            this.parent.gameObject.SetActive(true);
        }
        base.OnOpen();
        GameLogic.Hold.BattleData.Challenge_SetUIParent(this.parent);
    }
}

