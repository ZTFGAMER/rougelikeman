using System;
using TableTool;

public abstract class GameModeBase
{
    protected GameModeBase()
    {
    }

    public abstract DropManager.DropData GetDropData();
    public abstract int GetDropDataEquipExp(Soldier_soldier data);
    public abstract int GetDropDataGold(Soldier_soldier data);
    public abstract string[] GetMapAttributes();
    public abstract long GetMapStandardDefence();
    public abstract int GetMaxLayer();
    public abstract string[] GetMonsterTmxAttributes();
    public abstract string[] GetTmxIds(int roomid, int roomcount);
}

