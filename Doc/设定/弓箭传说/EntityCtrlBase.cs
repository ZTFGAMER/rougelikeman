using System;
using System.Collections.Generic;

public abstract class EntityCtrlBase
{
    public List<EBattleAction> mActionsList = new List<EBattleAction>();
    protected EntityBase m_Entity;
    private bool bUseUpdate;

    protected EntityCtrlBase()
    {
    }

    public abstract void ExcuteCommend(EBattleAction id, object action);
    public virtual void OnRemove()
    {
    }

    public virtual void OnStart(List<EBattleAction> actIds)
    {
    }

    public virtual void OnUpdate(float delta)
    {
    }

    public void SetEntity(EntityBase entity)
    {
        this.m_Entity = entity;
    }

    public void SetUseUpdate()
    {
        this.bUseUpdate = true;
    }

    public bool UseUpdate =>
        this.bUseUpdate;
}

