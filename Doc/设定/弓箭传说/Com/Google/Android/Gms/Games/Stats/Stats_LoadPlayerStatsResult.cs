namespace Com.Google.Android.Gms.Games.Stats
{
    using Com.Google.Android.Gms.Common.Api;

    public interface Stats_LoadPlayerStatsResult : Result
    {
        PlayerStats getPlayerStats();
    }
}

