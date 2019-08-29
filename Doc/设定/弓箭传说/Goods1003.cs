using Dxx.Util;
using System;
using UnityEngine;

public class Goods1003 : GoodsBase
{
    protected bool bStartTrap;
    protected float trapTime;
    private float delaytime = 4f;
    private float firetime = 2f;
    private float currenttime;
    private bool bStartFire;
    private float firecurrenttime;
    private int state;
    private BoxCollider box;

    private void BoxShow(bool show)
    {
        this.box.center = new Vector3(this.box.center.x, this.box.center.y, !show ? ((float) 0x2710) : ((float) 0));
    }

    public override void ChildTriggerEnter(GameObject o)
    {
        if ((GameLogic.Self != null) && (o == GameLogic.Self.gameObject))
        {
            GameLogic.SendHit_Trap(GameLogic.Self, -20L);
        }
    }

    private void CreateEffect()
    {
        GameLogic.Release.MapEffect.Get("Effect/Goods/Goods1003").transform.position = base.transform.position;
    }

    protected override void Init()
    {
        base.Init();
        this.box = base.GetComponent<BoxCollider>();
    }

    protected override void UpdateProcess()
    {
        this.currenttime += Updater.delta;
        if (this.currenttime >= this.delaytime)
        {
            this.currenttime -= this.delaytime;
            this.bStartFire = true;
            this.BoxShow(true);
            this.state = 1;
            this.CreateEffect();
        }
        if (this.bStartFire)
        {
            this.firecurrenttime += Updater.delta;
            if (this.firecurrenttime >= this.firetime)
            {
                this.firecurrenttime = 0f;
                this.state = 0;
                this.bStartFire = false;
                this.BoxShow(false);
            }
        }
    }
}

