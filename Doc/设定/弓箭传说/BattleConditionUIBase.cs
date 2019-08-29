using System;
using UnityEngine;

public class BattleConditionUIBase : MonoBehaviour
{
    protected LocalSave.AchieveDataOne mData;

    public void Init(LocalSave.AchieveDataOne data)
    {
        this.mData = data;
        this.OnInit();
    }

    protected virtual void OnInit()
    {
    }

    protected virtual void OnRefresh()
    {
    }

    public void Refresh()
    {
        this.OnRefresh();
    }
}

