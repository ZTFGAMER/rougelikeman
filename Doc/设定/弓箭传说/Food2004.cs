using System;
using UnityEngine;

public class Food2004 : Food2001
{
    private Animator effect;

    protected override void OnAwakeInit()
    {
        this.effect = base.transform.Find("Equip_EffectShow").GetComponent<Animator>();
        base.OnAwakeInit();
    }

    protected override void OnDropEnd()
    {
        this.effect.gameObject.SetActive(true);
        this.effect.Play("Equip_Show");
        GameNode.CameraShake(CameraShakeType.EquipDrop);
    }

    protected override void OnEnables()
    {
        base.OnEnables();
        this.effect.gameObject.SetActive(false);
    }
}

