using Dxx.Util;
using System;
using UnityEngine;

public class Goods1004 : GoodsBase
{
    private Animator ani;
    private const string ShowAni = "spike_trap_show";
    private const string HideAni = "spike_trap_hide";
    private const string ShowPara = "Show";
    protected bool bStartTrap;
    protected float trapTime;
    protected int framecount;

    protected override void AwakeInit()
    {
    }

    public override void ChildTriggerEnter(GameObject o)
    {
        if ((((GameLogic.Self != null) && (o == GameLogic.Self.gameObject)) && (!GameLogic.Self.GetFlying() && (GameLogic.Self != null))) && ((!GameLogic.Self.GetIsDead() && GameLogic.Self.m_EntityData.GetCanTrapHit()) && (((Updater.AliveTime - this.trapTime) > 1f) || ((Time.frameCount - this.framecount) > 1))))
        {
            this.trapTime = Updater.AliveTime;
            long beforehit = -GameConfig.MapGood.GetTrapHit();
            GameLogic.SendHit_Trap(GameLogic.Self, beforehit);
        }
    }

    protected override void Init()
    {
        this.ani = base.GetComponent<Animator>();
        base.GetComponentInChildren<GoodsColliderBase>().SetGoods(this);
    }

    protected override void UpdateProcess()
    {
        this.framecount = Time.frameCount;
    }
}

