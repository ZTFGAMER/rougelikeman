using System;
using UnityEngine;

public class EntityMonsterStrenghBase : EntityMonsterBase
{
    private GameObject effect_strengh;

    protected override void OnDeInit()
    {
        base.OnDeInit();
        if (this.effect_strengh != null)
        {
            GameLogic.EffectCache(this.effect_strengh);
        }
    }

    protected override void StartInit()
    {
        base.StartInit();
        base.m_Body.SetStrengh();
        this.effect_strengh = GameLogic.EffectGet("Effect/Battle/effect_strengh");
        this.effect_strengh.SetParentNormal(base.m_Body.AnimatorBodyObj);
        this.effect_strengh.GetComponent<StrenghEffectCtrl>().InitMesh(this);
    }
}

