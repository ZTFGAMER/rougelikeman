using System;
using UnityEngine;

public class Equip1000001 : EquipBase
{
    private Animator effect;

    protected override void AwakeInit()
    {
        this.effect = base.transform.Find("Equip_EffectShow").GetComponent<Animator>();
        base.AwakeInit();
    }

    protected override void Init()
    {
        this.effect.gameObject.SetActive(false);
    }

    protected override void OnAbsorb()
    {
    }

    protected override void OnDropEnd()
    {
        this.effect.gameObject.SetActive(true);
        this.effect.Play("Equip_Show");
        GameNode.CameraShake(CameraShakeType.EquipDrop);
    }
}

