using Dxx.Util;
using System;
using UnityEngine;

public class Goods2001 : GoodsBase
{
    private Vector3 startpos;
    private Vector3 endpos;
    private Vector3 dir;
    private float dis;
    private int state;
    private float speed = 3f;
    private Transform child;

    protected override void AwakeInit()
    {
    }

    public override void ChildTriggerEnter(GameObject o)
    {
        if (((GameLogic.Self != null) && (o == GameLogic.Self.gameObject)) && !GameLogic.Self.GetFlying())
        {
            long beforehit = -GameConfig.MapGood.GetTrapHit();
            GameLogic.SendHit_Trap(GameLogic.Self, beforehit);
        }
    }

    public override void ChildTriggetExit(GameObject o)
    {
    }

    protected override void Init()
    {
        this.child = base.transform.Find("trapEntity");
    }

    public void SetEndPosition(Vector3 endpos)
    {
        this.startpos = base.transform.localPosition;
        this.endpos = endpos;
        Vector3 vector = endpos - this.startpos;
        this.dis = vector.magnitude;
        Vector3 vector2 = endpos - this.startpos;
        this.dir = vector2.normalized;
    }

    protected override void UpdateProcess()
    {
        if (this.state == 0)
        {
            Vector3 vector = base.transform.localPosition - this.startpos;
            float num = vector.magnitude / this.dis;
            if (num < 1f)
            {
                Transform transform = base.transform;
                transform.localPosition += (this.dir * this.speed) * Updater.delta;
            }
            else
            {
                this.state = 1;
                this.child.localRotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }
        else
        {
            Vector3 vector2 = base.transform.localPosition - this.endpos;
            float num2 = vector2.magnitude / this.dis;
            if (num2 < 1f)
            {
                Transform transform = base.transform;
                transform.localPosition += (-this.dir * this.speed) * Updater.delta;
            }
            else
            {
                this.state = 0;
                this.child.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
    }
}

