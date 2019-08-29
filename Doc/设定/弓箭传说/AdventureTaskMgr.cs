using Dxx.Util;
using System;

public class AdventureTaskMgr : CInstance<AdventureTaskMgr>
{
    private AdventureTaskBase mAdventureTask;
    private int mTaskID = -1;

    private int GetRandomTaskID() => 
        0x3e9;

    public string GetShowTaskString()
    {
        if (this.mAdventureTask != null)
        {
            return this.mAdventureTask.GetShowTaskString();
        }
        object[] args = new object[] { this.mTaskID };
        return Utils.FormatString("ID:{0} is error.", args);
    }

    public void InitAdventureTask()
    {
        this.mTaskID = this.GetRandomTaskID();
        object[] args = new object[] { this.mTaskID };
        string typeName = Utils.FormatString("AdventureTask{0}", args);
        Type type = Type.GetType(typeName);
        this.mAdventureTask = type.Assembly.CreateInstance(typeName) as AdventureTaskBase;
    }

    public bool IsTaskFinish()
    {
        if (this.mAdventureTask != null)
        {
            return this.mAdventureTask.IsTaskFinish();
        }
        return true;
    }

    public void OnHitted(EntityBase source)
    {
        if ((this.mAdventureTask != null) && (this.mAdventureTask.Event_OnHitted != null))
        {
            this.mAdventureTask.Event_OnHitted(source);
        }
    }

    public void SetUIUpdateEvent(Action<string> callback)
    {
        if (this.mAdventureTask != null)
        {
            this.mAdventureTask.SetUIUpdateEvent(callback);
        }
    }
}

