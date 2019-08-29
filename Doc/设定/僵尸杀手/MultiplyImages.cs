using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="MultiplyImages")]
public class MultiplyImages : ScriptableObject
{
    public Sprite[] activeMultiplier;
    public Sprite[] inactiveMultiplier;
    public Sprite[] secretHat;
    public Sprite[] dailyPresent;
    public Sprite[] activeBoosters;
    public Sprite[] inactiveBoosters;
    public Sprite[] plashka;
    public List<ActiveInactive> perks;
    public List<ActiveInactive> upgrageButtons;
    public List<RewardedButtonX> rewardedButtons;
    public ActiveInactive popup;
    public ActiveInactive nextLocationButton;
    public ActiveInactive rateUsStar;

    public static int GetDailyPresentSpriteID(DailyBonusData.RewardType type)
    {
        switch (type)
        {
            case DailyBonusData.RewardType.Money:
                return 0;

            case DailyBonusData.RewardType.Multiplier:
                return 1;

            case DailyBonusData.RewardType.Booster:
                return 2;
        }
        Debug.LogWarning("Something wrong with daily present Sprites. Type: " + type);
        return 0;
    }

    public static int GetMultiplierSpriteID(int multiplier)
    {
        switch (multiplier)
        {
            case 1:
                return 0;

            case 2:
                return 0;

            case 3:
                return 1;

            case 4:
                return 2;

            case 5:
                return 3;
        }
        Debug.LogWarning("Something wrong in multiplier sprites. Multiplier: " + multiplier);
        return 0;
    }
}

