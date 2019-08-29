using Dxx.Util;
using System;
using UnityEngine;

public class GoodsCreate1102 : GoodsCreateBase
{
    private float speed = 15f;
    private float currentdis;
    private float maxdis = 100f;
    private float movex;
    private float movey;
    private Vector3 move;

    private void Caches()
    {
        GameLogic.Self.PlayEffect(0xf462c, base.transform.position);
        base.Cache();
    }

    protected override void OnAwake()
    {
    }

    protected override void OnInit()
    {
        this.movex = MathDxx.Sin(base.transform.eulerAngles.y);
        this.movey = MathDxx.Cos(base.transform.eulerAngles.y);
        this.move = new Vector3(this.movex, 0f, this.movey);
        this.currentdis = 0f;
    }

    protected override void OnStart()
    {
    }

    protected override void OnUpdate()
    {
        float num = this.speed * Updater.delta;
        this.currentdis += num;
        Transform transform = base.transform;
        transform.position += this.move * num;
        if (this.currentdis >= this.maxdis)
        {
            this.Caches();
        }
    }

    protected override void TriggerEnter(Collider o)
    {
        if ((o.gameObject.layer == LayerManager.MapOutWall) || (o.gameObject.layer == LayerManager.Stone))
        {
            this.Caches();
        }
        else if ((o.gameObject.layer == LayerManager.Player) && (o.gameObject == GameLogic.Self.gameObject))
        {
            this.Caches();
        }
    }
}

