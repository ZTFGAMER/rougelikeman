using System;

public abstract class SkillMasteryBase
{
    protected EntityBase m_Entity;
    protected string mData;

    protected SkillMasteryBase()
    {
    }

    public void Init(EntityBase entity, string data)
    {
        this.m_Entity = entity;
        this.mData = data;
        this.OnInit();
    }

    protected abstract void OnInit();
}

