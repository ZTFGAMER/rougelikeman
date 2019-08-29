using System;
using TableTool;

public class GameModeGold1 : GameModeBase
{
    public override DropManager.DropData GetDropData()
    {
        Stage_Level_activity activeData = GameLogic.Hold.BattleData.ActiveData;
        return new DropManager.DropData { EquipProb = activeData.EquipProb };
    }

    public override int GetDropDataEquipExp(Soldier_soldier data) => 
        data.ScrollDropLevel;

    public override int GetDropDataGold(Soldier_soldier data) => 
        data.GoldDropGold2;

    public override string[] GetMapAttributes() => 
        GameLogic.Hold.BattleData.ActiveLevelData.MapAttributes;

    public override long GetMapStandardDefence() => 
        GameLogic.Hold.BattleData.ActiveLevelData.StandardDefence;

    public override int GetMaxLayer() => 
        LocalModelManager.Instance.Stage_Level_activitylevel.GetMaxLayer();

    public override string[] GetMonsterTmxAttributes() => 
        GameLogic.Hold.BattleData.ActiveLevelData.Attributes;

    public override string[] GetTmxIds(int roomid, int roomcount) => 
        GameLogic.Hold.BattleData.GetActiveLevelData(roomid).RoomIDs;
}

