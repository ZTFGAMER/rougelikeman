using System;

[Serializable]
public class KilledBosses
{
    public string name;
    public int count;
    public bool rewarded;
    public int rewardedStage;

    public bool IsKilled() => 
        (this.count > 0);
}

