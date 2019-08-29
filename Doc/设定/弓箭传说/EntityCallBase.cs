using Dxx.Util;
using System;

public class EntityCallBase : EntityBase
{
    protected EntityBase m_Parent;
    protected AIBase m_AIBase;

    public AIBase GetAI() => 
        this.m_AIBase;

    public EntityBase GetParent() => 
        this.m_Parent;

    protected override void OnCreateModel()
    {
        object[] args = new object[] { "AI", base.ClassID };
        Type type = Type.GetType(Utils.GetString(args));
        if (type != null)
        {
            object[] objArray2 = new object[] { "AI", base.ClassID };
            this.m_AIBase = type.Assembly.CreateInstance(Utils.GetString(objArray2)) as AIBase;
            this.m_AIBase.SetEntity(this);
            this.m_AIBase.Init(false);
        }
    }

    protected override void OnDeadBefore()
    {
        base.OnDeadBefore();
        this.SetCollider(false);
        if (this.m_AIBase != null)
        {
            this.m_AIBase.DeadBefore();
        }
    }

    protected override void OnDeInitLogic()
    {
        if ((this.m_Parent != null) && (this.m_Parent is EntityMonsterBase))
        {
            EntityMonsterBase parent = this.m_Parent as EntityMonsterBase;
            if ((parent != null) && (parent.m_AIBase != null))
            {
                RemoveCallData data = new RemoveCallData {
                    entityId = base.ClassID,
                    deadpos = base.position
                };
                parent.m_AIBase.RemoveCall(data);
            }
        }
        if (this.m_AIBase != null)
        {
            this.m_AIBase.DeInit();
            this.m_AIBase = null;
        }
    }

    public override void RemoveMove()
    {
        if (this.m_AIBase != null)
        {
            this.m_AIBase.RemoveMove();
        }
    }

    public void SetParent(EntityBase entity)
    {
        this.m_Parent = entity;
    }
}

