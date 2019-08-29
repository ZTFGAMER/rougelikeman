using System;
using TableTool;

public abstract class AchieveConditionBase
{
    private int _id;
    protected string[] mArgs;
    private bool m_isunlock;
    private bool m_isfinish;
    protected LocalSave.AchieveDataOne mData;

    protected AchieveConditionBase()
    {
    }

    public void Excute()
    {
        this.OnExcute();
    }

    public string GetBattleMaxString() => 
        this.OnGetBattleMaxString();

    public string GetConditionString() => 
        this.OnGetConditionString();

    public int GetCurrent() => 
        this.mData.currentcount;

    public int GetMax() => 
        this.mData.maxcount;

    public void Init(LocalSave.AchieveDataOne data)
    {
        this.mData = data;
        this._id = data.achieveid;
        this.mArgs = data.mData.CondTypeArgs;
        if (LocalModelManager.Instance.Achieve_Achieve.GetBeanById(this._id).UnlockType != 1)
        {
        }
        this.OnInit();
    }

    public bool IsFinish() => 
        (this.m_isfinish || this.OnIsFinish());

    protected abstract void OnExcute();
    protected virtual string OnGetBattleMaxString() => 
        string.Empty;

    protected abstract string OnGetConditionString();
    protected abstract void OnInit();
    protected virtual bool OnIsFinish() => 
        false;

    public void SetFinish()
    {
        this.m_isfinish = true;
    }

    public int ID =>
        this._id;

    public bool IsUnlock =>
        this.m_isunlock;
}

