using System;
using TableTool;

public class GameModeLevel : GameModeBase
{
    public override DropManager.DropData GetDropData()
    {
        Stage_Level_stagechapter currentStageLevel = LocalModelManager.Instance.Stage_Level_stagechapter.GetCurrentStageLevel();
        return new DropManager.DropData { EquipProb = currentStageLevel.EquipProb };
    }

    public override int GetDropDataEquipExp(Soldier_soldier data) => 
        data.ScrollDropLevel;

    public override int GetDropDataGold(Soldier_soldier data) => 
        data.GoldDropLevel;

    public override string[] GetMapAttributes()
    {
        if ((GameLogic.Release.Mode == null) || (GameLogic.Release.Mode.RoomGenerate == null))
        {
            return new string[0];
        }
        int currentRoomID = GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID();
        int stage = GameLogic.Hold.BattleData.Level_CurrentStage;
        return LocalModelManager.Instance.Stage_Level_stagechapter.GetStageLevelMapAttributes(stage, currentRoomID);
    }

    public override long GetMapStandardDefence()
    {
        if ((GameLogic.Release.Mode == null) || (GameLogic.Release.Mode.RoomGenerate == null))
        {
            return 0L;
        }
        int currentRoomID = GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID();
        int stage = GameLogic.Hold.BattleData.Level_CurrentStage;
        return LocalModelManager.Instance.Stage_Level_stagechapter.GetStageLevelStandardDefence(stage, currentRoomID);
    }

    public override int GetMaxLayer()
    {
        if (GameLogic.Hold.BattleData.Challenge_ismainchallenge())
        {
            return GameLogic.Hold.BattleData.ActiveData.MaxLayer;
        }
        return LocalModelManager.Instance.Stage_Level_stagechapter.GetCurrentMaxLevel(GameLogic.Hold.BattleData.Level_CurrentStage);
    }

    public override string[] GetMonsterTmxAttributes()
    {
        if ((GameLogic.Release.Mode == null) || (GameLogic.Release.Mode.RoomGenerate == null))
        {
            return new string[0];
        }
        int currentRoomID = GameLogic.Release.Mode.RoomGenerate.GetCurrentRoomID();
        int stage = GameLogic.Hold.BattleData.Level_CurrentStage;
        return LocalModelManager.Instance.Stage_Level_chapter1.GetStageLevel_Attributes(stage, currentRoomID);
    }

    public override string[] GetTmxIds(int roomid, int roomcount)
    {
        int stage = GameLogic.Hold.BattleData.Level_CurrentStage;
        return LocalModelManager.Instance.Stage_Level_chapter1.GetStageLevel_RoomIds(stage, roomid, roomcount);
    }
}

