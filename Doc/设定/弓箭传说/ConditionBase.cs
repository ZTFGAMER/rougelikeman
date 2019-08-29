using System;

public abstract class ConditionBase
{
    protected float starttime = Updater.AliveTime;

    public ConditionBase()
    {
        this.Init();
    }

    protected abstract void Init();
    public virtual bool IsEnd() => 
        true;
}

