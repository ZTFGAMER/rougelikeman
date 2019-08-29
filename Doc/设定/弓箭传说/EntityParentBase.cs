using System;
using UnityEngine;

public class EntityParentBase : MonoBehaviour
{
    protected EntityBase m_Entity;

    public EntityBase GetEntityParent() => 
        this.m_Entity;

    public bool IsSelf(EntityBase entity) => 
        ((this.m_Entity != null) && (this.m_Entity == entity));

    public void SetEntityParent(EntityBase entity)
    {
        this.m_Entity = entity;
    }
}

