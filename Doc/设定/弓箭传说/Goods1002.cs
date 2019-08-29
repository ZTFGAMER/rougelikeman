using Dxx.Util;
using System;
using UnityEngine;

public class Goods1002 : GoodsBase
{
    protected bool bStartTrap;
    protected float trapTime;

    public override void ChildTriggerEnter(GameObject o)
    {
        if (((GameLogic.Self != null) && (o == GameLogic.Self.gameObject)) && !GameLogic.Self.GetFlying())
        {
            this.bStartTrap = true;
            this.trapTime = -1f;
        }
    }

    public override void ChildTriggetExit(GameObject o)
    {
        if (o == GameLogic.Self.gameObject)
        {
            this.bStartTrap = false;
        }
    }

    protected override void UpdateProcess()
    {
        if ((this.bStartTrap && (GameLogic.Self != null)) && (!GameLogic.Self.GetIsDead() && ((Updater.AliveTime - this.trapTime) > 0.1f)))
        {
            this.trapTime = Updater.AliveTime;
            GameLogic.SendBuff(GameLogic.Self, 0x3fc, Array.Empty<float>());
        }
    }
}

