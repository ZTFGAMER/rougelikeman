namespace GooglePlayGames.BasicApi.SavedGame
{
    using System;

    public enum ConflictResolutionStrategy
    {
        UseLongestPlaytime,
        UseOriginal,
        UseUnmerged,
        UseManual,
        UseLastKnownGood,
        UseMostRecentlySaved
    }
}

