using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class StaticConstants
{
    public static readonly string PlayerSaveDataPath = (Application.persistentDataPath + "/PlayerData.dat");
    public static readonly string SaltKey = "Salt";
    public static readonly string PasswordKey = "Password";
    public static readonly string DailyRewardKey = "Daily";
    public static readonly string DailyMultiplierTime = "DailyMultiplierTime";
    public static readonly string MoneyBoxKey = "MoneyBox";
    public static readonly string MusicMuted = "MusicMuted";
    public static readonly string MusicVolume = "MusicVolume";
    public static readonly string SoundMuted = "SoundMuted";
    public static readonly string SoundVolume = "SoundVolume";
    public static readonly string LastOnlineTime = "LastOnlineTime";
    public static readonly string TutorialCompleted = "GameplayTutorialComplete";
    public static readonly string AbilityTutorialCompleted = "AbilityTutorialComplete";
    public static readonly string UpgradeTutorialCompleted = "UpgradeTutorialComplete";
    public static readonly string GoToNextWorldPopUpShowed = "GoToNextWorldPopUpShowed";
    public static readonly string currentWorld = "CurrentWorld";
    public static readonly string interstitialAdsKey = "interstitialAdsKey";
    public static readonly string infinityMultiplierPurchased = "infinityMultiplierPurchased";
    public static readonly string starterPackPurchased = "starterPackPurchased";
    public static readonly string firstEnterTime = "firstEnterTime";
    public static readonly string firstDailyClaim = "firstDailyClaim";
    public static readonly string lastSelectedLanguage = "lastselectedlanguage";
    public static readonly string allBossesRewardedkey = "allBossesRewarded";
    public static readonly string autoSignIn = "autoSignIn";
    public static readonly int MultiplierDurationInSeconds = 0x5460;
    public static readonly int StarterPackHoursDuration = 0x30;
    public static readonly string MultiplierKey = "CurrentMultiplier";
    public static readonly float InGameGoldConst = 1f;
    public static readonly float IdleGoldConst = 0.45f;
    public static readonly float InGameExpConst = 0.3f;
    public static readonly float OfflineGoldConst = 0.002f;
    public static readonly float MinOfflineMinutes = 30f;
    public static readonly float MaxOfflineMinutes = 480f;
    public static readonly float DailyCoinMultiplier = 1.5f;
    public static readonly float DailyCoinMultiplier2 = 100f;
    public static readonly bool NeedInternetConnection = true;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static byte[] <Salt>k__BackingField;
    [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static string <Password>k__BackingField;
    public static readonly byte[] CsvSalt = new byte[] { 
        0xe5, 140, 0xb1, 100, 0x89, 0x51, 0xe1, 0xe7, 0x12, 0x34, 0xc2, 0x33, 0x91, 0x93, 0xbf, 0x88,
        0x85, 0x5f, 0x4c, 0x2d, 0x3f, 150, 0x9a, 0x20, 0x38, 0x38, 0x63, 170, 0x71, 0xb5, 12, 0xa5
    };
    public static readonly string CsvPass = "aca26bad-c47a-41a7-bf08-85565578d163";

    public static bool IsConnectedToInternet()
    {
        if (NeedInternetConnection && (Application.internetReachability == NetworkReachability.NotReachable))
        {
            return false;
        }
        return true;
    }

    public static byte[] Salt
    {
        [CompilerGenerated]
        get => 
            <Salt>k__BackingField;
        [CompilerGenerated]
        private set => 
            (<Salt>k__BackingField = value);
    }

    public static string Password
    {
        [CompilerGenerated]
        get => 
            <Password>k__BackingField;
        [CompilerGenerated]
        private set => 
            (<Password>k__BackingField = value);
    }
}

