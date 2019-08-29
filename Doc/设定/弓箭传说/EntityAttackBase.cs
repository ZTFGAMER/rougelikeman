using Dxx.Util;
using System;

public abstract class EntityAttackBase
{
    protected int AttackID;
    protected bool bEnd = true;
    protected EntityBase m_Entity;
    protected JoyData m_AttackData = new JoyData();
    protected JoyData m_MoveData = new JoyData();
    private bool bInit;
    protected Action OnUnInstall;
    protected bool bRotate = true;
    private bool bAddActionEnd;

    protected EntityAttackBase()
    {
    }

    protected void AttackNotGo()
    {
        this.OnAttackActionEnd();
    }

    protected virtual void DeInit()
    {
    }

    public bool GetIsEnd() => 
        this.bEnd;

    public void Init(EntityBase entity, int AttackID)
    {
        this.m_Entity = entity;
        this.m_AttackData.name = "AttackJoy";
        this.m_MoveData.name = "MoveJoy";
        this.AttackID = AttackID;
        this.OnEnable();
        this.OnInit();
        if ((this.m_Entity != null) && !this.bAddActionEnd)
        {
            this.bAddActionEnd = true;
            Debugger.Log(this.m_Entity, "EntityAttackBase bAddActionEnd true  have weapon = " + (this.m_Entity.m_Weapon != null));
            if (this.m_Entity.m_Weapon != null)
            {
                this.m_Entity.m_Weapon.Event_EntityAttack_AttackEnd = (Action) Delegate.Combine(this.m_Entity.m_Weapon.Event_EntityAttack_AttackEnd, new Action(this.OnAttackActionEnd));
            }
        }
    }

    public abstract void Install();
    private void OnAttackActionEnd()
    {
        this.UnregistAttackEnd();
    }

    private void OnDisable()
    {
        if (this.bInit)
        {
            Updater.RemoveUpdate("EntityAttackBase", new Action<float>(this.UpdateProcess));
            this.bInit = false;
        }
    }

    private void OnEnable()
    {
        if (!this.bInit)
        {
            Updater.AddUpdate("EntityAttackBase", new Action<float>(this.UpdateProcess), false);
            this.bInit = true;
        }
    }

    protected abstract void OnInit();
    public virtual void SetData(params object[] args)
    {
    }

    public void SetIsEnd(bool isend)
    {
        this.bEnd = isend;
    }

    public void SetRotate(bool bRotate)
    {
        this.bRotate = bRotate;
    }

    public void UnInstall()
    {
        this.UnregistAttackEnd();
        this.OnDisable();
        if (this.OnUnInstall != null)
        {
            this.OnUnInstall();
        }
    }

    private void UnregistAttackEnd()
    {
        Debugger.Log(this.m_Entity, string.Concat(new object[] { "EntityAttackBase UnregistAttackEnd bAddActionEnd  ", this.bAddActionEnd, "  have weapon = ", this.m_Entity.m_Weapon != null }));
        if (this.bAddActionEnd)
        {
            if ((this.m_Entity != null) && (this.m_Entity.m_Weapon != null))
            {
                this.m_Entity.m_Weapon.Event_EntityAttack_AttackEnd = (Action) Delegate.Remove(this.m_Entity.m_Weapon.Event_EntityAttack_AttackEnd, new Action(this.OnAttackActionEnd));
            }
            this.bAddActionEnd = false;
        }
    }

    protected void UpdateAttackAngle()
    {
        if ((this.m_Entity != null) && (this.m_Entity.m_HatredTarget != null))
        {
            this.m_AttackData.direction.x = this.m_Entity.m_HatredTarget.position.x - this.m_Entity.position.x;
            this.m_AttackData.direction.z = this.m_Entity.m_HatredTarget.position.z - this.m_Entity.position.z;
            this.m_AttackData.direction = this.m_AttackData.direction.normalized;
            this.m_AttackData.angle = Utils.getAngle(this.m_AttackData.direction);
        }
    }

    protected virtual void UpdateProcess(float delta)
    {
    }
}

