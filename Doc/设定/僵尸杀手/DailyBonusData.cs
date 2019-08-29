using System;

[Serializable]
public class DailyBonusData
{
    public RewardType type;
    public SaveData.BoostersData.BoosterType boosterType;
    public int reward;

    public enum RewardType
    {
        Money,
        Multiplier,
        Booster
    }
}

