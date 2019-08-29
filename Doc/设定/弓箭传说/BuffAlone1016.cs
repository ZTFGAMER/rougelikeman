using Dxx.Util;
using System;
using UnityEngine;

public class BuffAlone1016 : BuffAloneBase
{
    private float range;
    private float hprecoverratio;
    private float attackratio;
    private int hit;
    private int hprecover;
    private GameObject line;
    private LifeLineCtrl mLineCtrl;
    private GameObject effect;
    private EntityBabyBase m_EntityBaby;

    private void CacheEvent()
    {
        this.line = null;
        this.mLineCtrl = null;
    }

    private void CreateLine(EntityBase entity)
    {
        EntityBase parent;
        if (this.line == null)
        {
            this.line = GameLogic.EffectGet("Effect/Attributes/LifeLine");
            this.line.transform.SetParent(GameNode.m_PoolParent.transform);
            this.mLineCtrl = this.line.GetComponent<LifeLineCtrl>();
            this.mLineCtrl.mCacheEvent = new Action(this.CacheEvent);
        }
        this.mLineCtrl.UpdateEntity(base.m_Entity, entity);
        this.hit = MathDxx.CeilToInt(this.attackratio * base.m_Entity.m_EntityData.attribute.AttackValue.ValueCount);
        entity.m_EntityData.ExcuteBuffs(base.m_Entity, base.BuffID, base.buff_data.Attribute, (float) -this.hit);
        if (this.m_EntityBaby != null)
        {
            parent = this.m_EntityBaby.GetParent();
        }
        else
        {
            parent = base.m_Entity;
        }
        this.hprecover = MathDxx.CeilToInt(this.hprecoverratio * parent.m_EntityData.attribute.GetHPBase());
        parent.m_EntityData.ExcuteBuffs(base.m_Entity, base.BuffID, base.buff_data.Attribute, (float) this.hprecover);
    }

    protected override void ExcuteBuff(BuffAloneBase.BuffData data)
    {
        EntityBase entity = GameLogic.Release.Entity.GetNearEntity(base.m_Entity, this.range, false);
        if (entity != null)
        {
            this.CreateLine(entity);
        }
        else
        {
            this.RemoveLine();
        }
    }

    protected override void OnRemove()
    {
    }

    protected override void OnStart()
    {
        this.range = base.buff_data.Args[0];
        this.hprecoverratio = base.buff_data.Args[1] / 100f;
        this.attackratio = base.buff_data.Args[2] / 100f;
        this.m_EntityBaby = base.m_Entity as EntityBabyBase;
    }

    private void RemoveLine()
    {
        if (this.line != null)
        {
            GameLogic.EffectCache(this.line);
            this.line = null;
            this.mLineCtrl.Cache();
        }
    }
}

