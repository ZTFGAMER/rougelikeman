namespace Com.Google.Android.Gms.Games.Stats
{
    using Com.Google.Android.Gms.Common.Api;
    using System;

    public interface Stats
    {
        PendingResult<Stats_LoadPlayerStatsResultObject> loadPlayerStats(GoogleApiClient arg_GoogleApiClient_1, bool arg_bool_2);
    }
}

