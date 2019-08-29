using System;
using UnityEngine;

public class Goods1005 : GoodsBase
{
    private Goods1005EffectCtrl effect;

    protected override void Init()
    {
        base.Init();
        Transform transform = base.transform.Find("Goods1005");
        if (transform != null)
        {
            this.effect = transform.GetComponent<Goods1005EffectCtrl>();
        }
        if (this.effect != null)
        {
            this.effect.Event_TriggerEnter = new Action<EntityBase>(this.Trigger);
        }
    }

    protected override void StartInit()
    {
        base.StartInit();
    }

    private void Trigger(EntityBase entity)
    {
        GameLogic.SendHit_Trap(GameLogic.Self, -20L);
    }

    protected override void UpdateProcess()
    {
        if (this.effect != null)
        {
            this.effect.transform.rotation = Quaternion.Euler(0f, this.effect.transform.eulerAngles.y + 2f, 0f);
        }
    }
}

