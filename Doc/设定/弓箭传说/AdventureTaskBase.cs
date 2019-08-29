using System;

public abstract class AdventureTaskBase
{
    public Action Event_GameFinish;
    public Action<int> Event_RecoverHP;
    public Action<EntityBase> Event_OnMiss;
    public Action<EntityBase, int> Event_OnCrit;
    public Action<EntityBase> Event_OnHitted;
    private Action<string> UIEvent_Update;

    protected virtual bool _IsTaskFinish() => 
        false;

    public abstract string GetShowTaskString();
    public bool IsTaskFinish() => 
        this._IsTaskFinish();

    public void SetUIUpdateEvent(Action<string> callback)
    {
        this.UIEvent_Update = callback;
    }

    protected void UpdateUI()
    {
        if (this.UIEvent_Update != null)
        {
            this.UIEvent_Update(this.GetShowTaskString());
        }
    }
}

