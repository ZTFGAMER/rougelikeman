using System;
using System.Collections.Generic;
using TableTool;

public abstract class DropBase
{
    protected Soldier_soldier m_Data;
    protected List<BattleDropData> mList = new List<BattleDropData>();
    protected long MaxHP;
    protected long currentHP;

    protected DropBase()
    {
    }

    public List<BattleDropData> GetDropDead() => 
        this.OnGetDropDead();

    public List<BattleDropData> GetHittedList(long hit) => 
        this.OnGetHittedList(hit);

    public void Init(Soldier_soldier data, long hp)
    {
        this.m_Data = data;
        this.MaxHP = hp;
        this.currentHP = hp;
        this.OnInit();
    }

    protected abstract List<BattleDropData> OnGetDropDead();
    protected abstract List<BattleDropData> OnGetHittedList(long hit);
    protected abstract void OnInit();
}

