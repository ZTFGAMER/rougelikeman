using System;
using TableTool;

public abstract class AIMoveBase : ActionBasic.ActionUIBase
{
    protected Operation_move Data;
    public string ClassName;
    public int ClassID;
    protected JoyData m_MoveData = new JoyData();

    public AIMoveBase(EntityBase entity)
    {
        this.m_MoveData.name = "MoveJoy";
        this.m_MoveData.action = "Run";
        this.ClassName = base.GetType().ToString();
        int.TryParse(this.ClassName.Substring(this.ClassName.Length - 4, 4), out this.ClassID);
        this.Data = LocalModelManager.Instance.Operation_move.GetBeanById(this.ClassID);
        base.name = this.ClassName;
        base.m_Entity = entity;
    }

    public static ConditionBase GetConditionRandomTime(int min, int max) => 
        new ConditionTime { time = GameLogic.Random((float) (((float) min) / 1000f), (float) (((float) max) / 1000f)) };

    public static ConditionBase GetConditionTime(int time) => 
        new ConditionTime { time = ((float) time) / 1000f };

    private void OnDizzy(bool value)
    {
        if (value)
        {
            base.End();
        }
    }

    protected override void OnEnd1()
    {
        base.m_Entity.OnDizzy = (Action<bool>) Delegate.Remove(base.m_Entity.OnDizzy, new Action<bool>(this.OnDizzy));
    }

    protected override void OnForceEnd()
    {
    }

    protected sealed override void OnInit()
    {
        if (base.m_Entity == null)
        {
            base.End();
        }
        else
        {
            base.m_Entity.OnDizzy = (Action<bool>) Delegate.Combine(base.m_Entity.OnDizzy, new Action<bool>(this.OnDizzy));
            this.OnInitBase();
        }
    }

    protected abstract void OnInitBase();
}

